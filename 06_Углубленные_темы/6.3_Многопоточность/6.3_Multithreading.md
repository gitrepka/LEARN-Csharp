# Тема 6.3: Потоки и многопоточность

## Что ты узнаешь
- Thread, ThreadPool
- Синхронизация: lock, Monitor, Semaphore, Interlocked
- TPL: Task.Run, Parallel.For/ForEach
- Channel<T> для producer/consumer
- Типичные проблемы: race conditions, deadlocks

---

## Объяснение

### ЗАЧЕМ?
Один поток — один ядро CPU. У тебя 8 ядер. Многопоточность позволяет использовать ВСЕ ядра. Но это опасно: два потока одновременно меняют одни данные → катастрофа.

### Разница: async vs потоки

- **async/await** — "не ждать" (I/O: сеть, диск). Один поток, но не блокируется.
- **Потоки/Task.Run** — "делать одновременно" (CPU: вычисления). Реально параллельно.

### Thread (низкоуровневый)

```csharp
// Создание потока
Thread thread = new(() =>
{
    for (int i = 0; i < 5; i++)
    {
        Console.WriteLine($"Поток: {i}");
        Thread.Sleep(100);
    }
});

thread.Start();        // запуск
thread.Join();         // ждать завершения
thread.IsBackground = true; // завершится при выходе из Main

// Thread.CurrentThread — текущий поток
Console.WriteLine($"ID потока: {Thread.CurrentThread.ManagedThreadId}");
```

### Race Condition — проблема

```csharp
int counter = 0;

// Два потока увеличивают один счётчик
Thread t1 = new(() => { for (int i = 0; i < 100000; i++) counter++; });
Thread t2 = new(() => { for (int i = 0; i < 100000; i++) counter++; });

t1.Start();
t2.Start();
t1.Join();
t2.Join();

Console.WriteLine(counter); // НЕ 200000! Может быть 150000, 180000 — каждый раз разное!
// Причина: counter++ = read → increment → write. Два потока могут read одновременно.
```

### Решение 1: lock

```csharp
int counter = 0;
object lockObj = new(); // объект для блокировки

Thread t1 = new(() =>
{
    for (int i = 0; i < 100000; i++)
    {
        lock (lockObj) // только один поток за раз
        {
            counter++;
        }
    }
});

// C# 13: System.Threading.Lock (быстрее object)
Lock lockObj2 = new();
lock (lockObj2)
{
    counter++;
}
```

### Решение 2: Interlocked (для простых операций)

```csharp
int counter = 0;

// Атомарные операции — без lock!
Interlocked.Increment(ref counter);
Interlocked.Decrement(ref counter);
Interlocked.Add(ref counter, 10);
Interlocked.Exchange(ref counter, 0); // установить значение
Interlocked.CompareExchange(ref counter, 100, 50); // если 50 → заменить на 100
```

### Semaphore — ограничение параллелизма

```csharp
// Максимум 3 одновременных операции
SemaphoreSlim semaphore = new(3);

async Task ProcessAsync(int id)
{
    await semaphore.WaitAsync(); // занять слот (ждать если все заняты)
    try
    {
        Console.WriteLine($"Начало {id}");
        await Task.Delay(1000); // работа
        Console.WriteLine($"Конец {id}");
    }
    finally
    {
        semaphore.Release(); // освободить слот
    }
}

// Запускаем 10 задач, но только 3 работают одновременно
var tasks = Enumerable.Range(0, 10).Select(ProcessAsync);
await Task.WhenAll(tasks);
```

### ReaderWriterLockSlim — для чтения/записи

```csharp
ReaderWriterLockSlim rwLock = new();
Dictionary<string, int> data = new();

// Чтение — несколько потоков одновременно
int Read(string key)
{
    rwLock.EnterReadLock();
    try { return data.GetValueOrDefault(key); }
    finally { rwLock.ExitReadLock(); }
}

// Запись — только один поток
void Write(string key, int value)
{
    rwLock.EnterWriteLock();
    try { data[key] = value; }
    finally { rwLock.ExitWriteLock(); }
}
```

---

### TPL: Parallel.For / ForEach

```csharp
// Параллельный цикл (CPU-bound)
Parallel.For(0, 1000, i =>
{
    // Каждая итерация может выполняться в своём потоке
    ProcessItem(i);
});

// Parallel.ForEach
string[] files = Directory.GetFiles("data", "*.txt");
Parallel.ForEach(files, file =>
{
    string content = File.ReadAllText(file);
    // обработка...
});

// Parallel.ForEachAsync (async, .NET 6+)
await Parallel.ForEachAsync(files,
    new ParallelOptions { MaxDegreeOfParallelism = 4 },
    async (file, ct) =>
    {
        string content = await File.ReadAllTextAsync(file, ct);
    });
```

### PeriodicTimer (.NET 6+)

```csharp
// Таймер для периодических задач (лучше чем Timer)
using PeriodicTimer timer = new(TimeSpan.FromSeconds(1));

while (await timer.WaitForNextTickAsync())
{
    Console.WriteLine($"Tick: {DateTime.Now:HH:mm:ss}");
}
```

---

### Deadlock

```csharp
// ❌ Классический deadlock
object lockA = new(), lockB = new();

Thread t1 = new(() =>
{
    lock (lockA)
    {
        Thread.Sleep(100);
        lock (lockB) { } // Ждёт lockB, который держит t2
    }
});

Thread t2 = new(() =>
{
    lock (lockB)
    {
        Thread.Sleep(100);
        lock (lockA) { } // Ждёт lockA, который держит t1
    }
});
// 💀 Оба потока ждут друг друга бесконечно!

// ✅ Решение: всегда захватывать lock-и в одном порядке
```

---

## Мини-упражнения
1. **⭐⭐** Создай race condition (два потока + общий счётчик), исправь через lock.
2. **⭐⭐** Используй `SemaphoreSlim(3)` для ограничения параллельных операций.
3. **⭐⭐** Создай producer-consumer через `Channel<T>` (из темы 3.3).

### Практика Uno Platform ⭐⭐⭐
**"Параллельный обработчик"** — обработка нескольких задач параллельно, SemaphoreSlim для лимита, прогресс каждой задачи.

### Практика Godot ⭐⭐⭐
**Процедурная генерация** — генерация чанков ландшафта в фоновых потоках. Channel<ChunkData> для передачи данных в основной поток.

---

## Что дальше
Дальше — **управление памятью** (6.4).
