# Тема 4.5: Асинхронное программирование (async/await)

## Что ты узнаешь
- Task, Task<T>, ValueTask — зачем и как
- async/await — синтаксис и правила
- Параллельное выполнение: Task.WhenAll, Task.WhenAny
- CancellationToken — отмена операций
- IAsyncEnumerable — асинхронные потоки
- Типичные ловушки: deadlocks, async void

---

## Объяснение

### ЗАЧЕМ?
Ты загружаешь файл из интернета. Это занимает 3 секунды. Без async — UI замерзает. С async — UI работает, загрузка идёт "в фоне". **Async — НЕ многопоточность.** Это про "не ждать впустую".

### Аналогия
Ты в кафе. Заказал кофе. **Синхронно** — стоишь и ждёшь. **Асинхронно** — сел за стол, читаешь книгу, официант принесёт когда будет готово.

---

### Основы async/await

```csharp
// Асинхронный метод
async Task<string> LoadDataAsync()
{
    // await "отпускает" поток, пока идёт ожидание
    await Task.Delay(2000); // имитация загрузки (2 секунды)
    return "Данные загружены!";
}

// Вызов
string data = await LoadDataAsync();
Console.WriteLine(data);

// Task — операция без результата
async Task SaveDataAsync(string data)
{
    await Task.Delay(1000);
    // сохранение...
}

// ValueTask — когда результат часто уже готов (кэш, быстрый путь)
ValueTask<int> GetCachedValueAsync(string key)
{
    if (_cache.TryGetValue(key, out int value))
        return ValueTask.FromResult(value); // мгновенно, без аллокации Task

    return new ValueTask<int>(LoadFromDbAsync(key)); // только при промахе кэша
}
```

### Правила async/await

```csharp
// 1. async метод ДОЛЖЕН содержать await
async Task DoWorkAsync()
{
    await Task.Delay(100); // ✅
}

// 2. Возвращай Task, Task<T> или ValueTask
async Task<int> GetNumberAsync() => await ComputeAsync();

// 3. Называй методы с суффиксом Async
async Task LoadDataAsync() { ... }  // ✅
async Task LoadData() { ... }       // ❌ плохое имя

// 4. НИКОГДА не используй async void (кроме event handlers!)
async void Bad() { ... }            // ❌ Исключение потеряется!
async Task Good() { ... }           // ✅
```

---

### Параллельное выполнение

```csharp
// Task.WhenAll — запустить все ПАРАЛЛЕЛЬНО, ждать ВСЕ
async Task LoadAllAsync()
{
    Task<string> task1 = LoadUserAsync();
    Task<string> task2 = LoadOrdersAsync();
    Task<string> task3 = LoadSettingsAsync();

    // Все три работают одновременно!
    string[] results = await Task.WhenAll(task1, task2, task3);

    Console.WriteLine($"User: {results[0]}");
    Console.WriteLine($"Orders: {results[1]}");
    Console.WriteLine($"Settings: {results[2]}");
}

// ❌ НЕ ТАК — это ПОСЛЕДОВАТЕЛЬНО:
string user = await LoadUserAsync();       // 2 сек
string orders = await LoadOrdersAsync();   // 2 сек
string settings = await LoadSettingsAsync(); // 2 сек
// Итого: 6 секунд

// ✅ С WhenAll — параллельно:
// Итого: 2 секунды (максимум из трёх)

// Task.WhenAny — ждать ПЕРВУЮ завершённую
Task<string> fastest = await Task.WhenAny(
    LoadFromServer1Async(),
    LoadFromServer2Async(),
    LoadFromServer3Async()
);
string result = await fastest;

// Task.Run — запустить CPU-bound работу в фоновом потоке
int result2 = await Task.Run(() =>
{
    // Тяжёлые вычисления (НЕ для I/O!)
    return HeavyCalculation();
});
```

---

### CancellationToken — отмена

```csharp
async Task DownloadFileAsync(string url, CancellationToken ct)
{
    using var client = new HttpClient();

    // Передаём token — операция отменится если попросят
    var response = await client.GetAsync(url, ct);

    // Ручная проверка отмены
    ct.ThrowIfCancellationRequested();

    // ... обработка
}

// Использование
var cts = new CancellationTokenSource();

// Автоматическая отмена через 5 секунд
cts.CancelAfter(TimeSpan.FromSeconds(5));

try
{
    await DownloadFileAsync("https://example.com/bigfile", cts.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine("Загрузка отменена!");
}

// Ручная отмена (по кнопке)
cts.Cancel(); // отменяет все операции с этим token
```

---

### IAsyncEnumerable — асинхронные потоки

```csharp
// Генератор данных (приходят постепенно)
async IAsyncEnumerable<int> GenerateNumbersAsync(
    [EnumeratorCancellation] CancellationToken ct = default)
{
    for (int i = 0; i < 100; i++)
    {
        await Task.Delay(100, ct); // задержка между числами
        yield return i;
    }
}

// Потребление
await foreach (int number in GenerateNumbersAsync())
{
    Console.WriteLine(number);
    if (number > 10) break; // можно прервать
}
```

---

### Parallel.ForEachAsync — параллельная обработка

```csharp
string[] urls = ["url1", "url2", "url3", /* ... */];

await Parallel.ForEachAsync(urls,
    new ParallelOptions { MaxDegreeOfParallelism = 5 }, // максимум 5 одновременно
    async (url, ct) =>
    {
        string content = await DownloadAsync(url, ct);
        ProcessContent(content);
    });
```

---

### Под капотом

```csharp
// async/await — это "сахар". Компилятор создаёт state machine:
// 1. Метод разбивается на "куски" между await
// 2. Каждый await — точка приостановки
// 3. Когда Task завершается — продолжение выполняется

// ConfigureAwait(false) — НЕ возвращаться в UI-поток
async Task LibraryMethodAsync()
{
    // В библиотечном коде — не нужен UI-поток
    await Task.Delay(100).ConfigureAwait(false);
}

// В UI-коде — НЕ используй ConfigureAwait(false)!
// await LoadDataAsync(); — вернётся в UI-поток (для обновления UI)
```

---

## Типичные ловушки

```csharp
// ❌ DEADLOCK — .Result или .Wait() в UI-потоке
void Button_Click(object sender, EventArgs e)
{
    var data = LoadDataAsync().Result; // 💀 DEADLOCK!
    // async хочет вернуться в UI-поток, но он заблокирован .Result
}
// ✅
async void Button_Click(object sender, EventArgs e)
{
    var data = await LoadDataAsync(); // ✅
}

// ❌ async void — исключение потеряется
async void FireAndForget()
{
    await Task.Delay(100);
    throw new Exception("Упс!"); // 💀 Крашнет приложение!
}
// ✅ async Task

// ❌ Забыли await
async Task ProcessAsync()
{
    SaveDataAsync(); // ⚠️ Без await — fire-and-forget!
    // Метод может не завершиться, исключение потеряется
}
// ✅
async Task ProcessAsync()
{
    await SaveDataAsync();
}
```

---

## Мини-упражнения
1. **⭐** Напиши async метод с `Task.Delay`, вызови с await.
2. **⭐** Запусти 3 задачи параллельно через `Task.WhenAll`.
3. **⭐⭐** Реализуй CancellationToken: начни операцию, отмени через 2 секунды.
4. **⭐⭐** Создай `IAsyncEnumerable<int>`, потреби через `await foreach`.
5. **⭐⭐** Покажи deadlock с `.Result` и исправь.

### Практика Uno Platform ⭐⭐⭐
**"Загрузчик изображений"** — URL-ы → параллельная загрузка (HttpClient + Task.WhenAll), прогресс каждой загрузки, кнопка "Отменить" (CancellationToken).

### Практика Godot ⭐⭐⭐
**Процедурный генератор мира** — чанки генерируются в фоне (Task.Run), индикатор загрузки. CancellationToken при выходе из зоны. `await ToSignal()` для ожидания сигналов Godot.

---

## Что дальше
Дальше — **паттерн-матчинг** (4.6).
