# Тема 6.4: Управление памятью

## Что ты узнаешь
- Garbage Collector: поколения, LOH, когда срабатывает
- IDisposable: Dispose pattern, using
- Span<T>, Memory<T>, ArrayPool<T>
- WeakReference<T>
- Профилирование: dotMemory, dotnet-counters

---

## Объяснение

### ЗАЧЕМ?
C# автоматически управляет памятью (GC). Но GC не бесплатен: паузы, лишние аллокации замедляют игру. Понимание памяти — разница между "работает" и "работает быстро".

### Garbage Collector (GC)

```
СТЕК (Stack)              КУЧА (Heap)
┌─────────────┐           ┌─────────────────┐
│ int x = 42  │           │ Gen 0 (молодые) │ ← GC собирает чаще
│ float y = 3.14│         │ Gen 1 (средние)  │
│ ref → [───]──┼──────────│ Gen 2 (старые)   │ ← GC собирает редко
│ ref → [───]──┼──────────│ LOH (> 85KB)     │ ← Large Object Heap
└─────────────┘           └─────────────────┘

Value types (int, struct) → стек (быстро)
Reference types (class, string) → куча (GC управляет)
```

### Поколения GC

```csharp
// Gen 0: новые объекты. GC проверяет часто.
var temp = new object(); // Gen 0

// Если объект пережил сборку Gen 0 → Gen 1
// Если пережил Gen 1 → Gen 2 (долгоживущие)

// LOH (Large Object Heap): объекты > 85KB
byte[] large = new byte[100000]; // LOH

// Принудительная сборка (обычно НЕ делай так!)
GC.Collect();
GC.Collect(0, GCCollectionMode.Forced); // только Gen 0

// Информация
Console.WriteLine(GC.GetTotalMemory(false)); // используемая память
Console.WriteLine(GC.CollectionCount(0));    // сколько раз собирался Gen 0
```

---

### IDisposable и Dispose pattern

Некоторые ресурсы GC **не** управляет: файлы, сетевые соединения, БД, потоки ОС. Нужно закрывать вручную.

```csharp
// using — автоматический Dispose
using FileStream fs = new("file.txt", FileMode.Open);
// fs.Dispose() вызовется автоматически

// await using для async
await using var connection = new SqlConnection(connStr);

// Свой IDisposable
class GameResourceManager : IDisposable
{
    private FileStream? _logFile;
    private bool _disposed;

    public GameResourceManager()
    {
        _logFile = File.OpenWrite("game.log");
    }

    public void Dispose()
    {
        if (_disposed) return;
        _logFile?.Dispose();
        _logFile = null;
        _disposed = true;
        GC.SuppressFinalize(this); // не вызывай финализатор
    }

    // Финализатор — "страховка" если забыли Dispose
    ~GameResourceManager()
    {
        Dispose();
    }
}
```

---

### Span<T> — работа без аллокаций

Span<T> — "окно" в массив. Не создаёт новый массив!

```csharp
int[] numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

// Обычный способ — создаёт НОВЫЙ массив
int[] slice1 = numbers[2..5]; // аллокация!

// Span — БЕЗ аллокации
Span<int> slice2 = numbers.AsSpan(2, 3); // {3, 4, 5}
slice2[0] = 99; // numbers[2] тоже стал 99!

// Строки
string text = "Hello, World!";
ReadOnlySpan<char> hello = text.AsSpan(0, 5); // "Hello" — без аллокации!

// stackalloc + Span (данные на стеке)
Span<int> stackData = stackalloc int[100]; // на стеке, не в куче!
stackData[0] = 42;

// ⚠️ Span нельзя сохранить в поле класса (только локальные переменные)
```

### Memory<T> — как Span, но можно хранить

```csharp
class DataProcessor
{
    private Memory<byte> _buffer; // ✅ Можно хранить в поле

    public DataProcessor(byte[] data)
    {
        _buffer = data.AsMemory();
    }

    public void Process()
    {
        Span<byte> span = _buffer.Span; // получить Span для работы
    }
}
```

### ArrayPool<T> — пул массивов

```csharp
using System.Buffers;

// Вместо new byte[1024] (аллокация, нагрузка на GC)
byte[] buffer = ArrayPool<byte>.Shared.Rent(1024); // берём из пула
try
{
    // работа с buffer
    // ⚠️ Размер может быть БОЛЬШЕ запрошенного!
}
finally
{
    ArrayPool<byte>.Shared.Return(buffer); // возвращаем в пул
}
```

### WeakReference<T>

Ссылка, которая **не мешает** GC собрать объект:

```csharp
// Кэш с WeakReference — объекты удаляются при нехватке памяти
class WeakCache<TKey, TValue> where TKey : notnull where TValue : class
{
    private readonly Dictionary<TKey, WeakReference<TValue>> _cache = new();

    public void Set(TKey key, TValue value)
    {
        _cache[key] = new WeakReference<TValue>(value);
    }

    public TValue? Get(TKey key)
    {
        if (_cache.TryGetValue(key, out var weakRef) && weakRef.TryGetTarget(out var value))
            return value;
        return null; // объект был собран GC
    }
}
```

---

### Профилирование

```bash
# dotnet-counters — мониторинг в реальном времени
dotnet tool install -g dotnet-counters
dotnet-counters monitor --process-id <PID>

# dotnet-trace — запись трейсов
dotnet tool install -g dotnet-trace
dotnet-trace collect --process-id <PID>
```

```csharp
// В коде
Console.WriteLine($"Память: {GC.GetTotalMemory(false) / 1024:N0} KB");
Console.WriteLine($"GC Gen0: {GC.CollectionCount(0)}");
Console.WriteLine($"GC Gen1: {GC.CollectionCount(1)}");
Console.WriteLine($"GC Gen2: {GC.CollectionCount(2)}");
```

---

## Советы для игр (Godot)

```csharp
// ❌ Создание объектов каждый кадр
public override void _Process(double delta)
{
    var pos = new Vector2(X, Y); // struct — OK (стек)
    var text = $"HP: {Health}";  // string — аллокация каждый кадр! ❌
}

// ✅ Кэшируй строки, используй StringBuilder
// ✅ Используй object pooling для часто создаваемых объектов (пули, эффекты)
// ✅ Избегай LINQ в _Process() (создаёт итераторы)
// ✅ Используй struct вместо class для маленьких данных
```

---

## Мини-упражнения
1. **⭐** Выведи информацию о GC: память, количество сборок по поколениям.
2. **⭐** Реализуй IDisposable для класса с файловым ресурсом.
3. **⭐⭐** Используй `Span<int>` для обработки среза массива без аллокации.
4. **⭐⭐** Используй `ArrayPool<byte>.Shared` вместо `new byte[]`.

---

## Контрольные вопросы перед Модулем 7
1. Что такое замыкание и какие проблемы оно может вызвать?
2. Чем рефлексия отличается от Source Generators?
3. Что такое race condition и как от неё защититься?
4. Чем Span<T> отличается от обычного массива?
5. Когда нужно реализовывать IDisposable?

## Что дальше
Модуль 6 завершён! Дальше — **Модуль 7: Отладка и инструменты**.
