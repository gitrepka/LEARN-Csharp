# Тема 16.2: Оптимизации C#

## Что ты узнаешь
- Span<T> и Memory<T> — работа без аллокаций
- ArrayPool<T> — переиспользование массивов
- stackalloc — аллокация на стеке
- ValueTask — экономия на async
- Кэширование: MemoryCache, Lazy<T>
- FrozenDictionary, начальная capacity

---

## Объяснение

### Span<T> — "окно" в память без копирования

```csharp
// ❌ Substring создаёт НОВУЮ строку (аллокация)
string text = "Hello, World!";
string hello = text.Substring(0, 5); // новый объект в куче

// ✅ Span — просто "указывает" на часть существующей строки
ReadOnlySpan<char> helloSpan = text.AsSpan(0, 5); // ZERO allocation

// Парсинг без аллокаций
public static (int x, int y) ParseCoordinates(ReadOnlySpan<char> input)
{
    // input = "123,456"
    int commaIndex = input.IndexOf(',');
    int x = int.Parse(input[..commaIndex]);
    int y = int.Parse(input[(commaIndex + 1)..]);
    return (x, y);
}

// Span для массивов
int[] numbers = [1, 2, 3, 4, 5];
Span<int> slice = numbers.AsSpan(1, 3); // [2, 3, 4] — без копирования
slice[0] = 99; // numbers теперь [1, 99, 3, 4, 5] — тот же массив!

// ⚠️ Span нельзя хранить в полях класса (только в стеке)
// Для этого есть Memory<T>
```

### ArrayPool<T> — переиспользование массивов

```csharp
using System.Buffers;

// ❌ Плохо — каждый вызов создаёт новый массив
public byte[] ProcessData(Stream stream)
{
    byte[] buffer = new byte[4096]; // аллокация → GC
    stream.Read(buffer);
    return buffer;
}

// ✅ Хорошо — берём из пула, возвращаем обратно
public void ProcessData(Stream stream)
{
    byte[] buffer = ArrayPool<byte>.Shared.Rent(4096);
    try
    {
        int bytesRead = stream.Read(buffer);
        // используем buffer[..bytesRead]
    }
    finally
    {
        ArrayPool<byte>.Shared.Return(buffer, clearArray: true);
    }
}

// Пул возвращает массив >= запрошенного размера
// Rent(100) может вернуть массив длины 128
```

### stackalloc — аллокация на стеке

```csharp
// Стек: очень быстрый, но маленький (~1 MB)
// Куча: медленнее, но большая (гигабайты)

// Для небольших временных буферов:
public string BytesToHex(byte[] data)
{
    Span<char> buffer = stackalloc char[data.Length * 2];

    for (int i = 0; i < data.Length; i++)
    {
        buffer[i * 2] = "0123456789ABCDEF"[data[i] >> 4];
        buffer[i * 2 + 1] = "0123456789ABCDEF"[data[i] & 0xF];
    }

    return new string(buffer);
}

// ⚠️ Только для маленьких массивов (< 1KB)!
// Для больших — ArrayPool
Span<int> small = stackalloc int[64]; // ✅ OK
// Span<int> big = stackalloc int[1_000_000]; // ❌ StackOverflow!
```

### ValueTask — экономия для горячих путей

```csharp
// Task всегда аллоцирует объект в куче
// ValueTask — struct, аллокация только если нужна

// ❌ Метод обычно возвращает из кэша (синхронно)
public async Task<Player?> GetPlayerAsync(int id)
{
    if (_cache.TryGetValue(id, out var player))
        return player; // Task аллоцируется ЗРЯ

    return await _repo.GetByIdAsync(id);
}

// ✅ ValueTask — если результат уже готов, аллокации нет
public ValueTask<Player?> GetPlayerAsync(int id)
{
    if (_cache.TryGetValue(id, out var player))
        return ValueTask.FromResult(player); // без аллокации!

    return new ValueTask<Player?>(LoadFromDbAsync(id));
}

// ⚠️ ValueTask нельзя await дважды! Нельзя хранить.
```

### Кэширование

```csharp
using Microsoft.Extensions.Caching.Memory;

// MemoryCache — кэш в оперативной памяти
public class PlayerService
{
    private readonly IMemoryCache _cache;
    private readonly IPlayerRepository _repo;

    public async Task<Player?> GetPlayerAsync(int id)
    {
        string key = $"player:{id}";

        return await _cache.GetOrCreateAsync(key, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
            entry.SlidingExpiration = TimeSpan.FromMinutes(1);
            return await _repo.GetByIdAsync(id);
        });
    }
}

// Lazy<T> — вычисляется один раз, потом кэшируется
private readonly Lazy<ExpensiveData> _data = new(() =>
{
    // Вычисляется ТОЛЬКО при первом обращении к .Value
    return ComputeExpensiveData();
});
public ExpensiveData Data => _data.Value;
```

### Коллекции — capacity и FrozenDictionary

```csharp
// ❌ List растёт динамически: 4 → 8 → 16 → 32 → ...
var list = new List<int>();
for (int i = 0; i < 10000; i++)
    list.Add(i); // ~13 ресайзов + копирований!

// ✅ Указывай начальную capacity
var list = new List<int>(10000); // 0 ресайзов!

// То же для Dictionary
var dict = new Dictionary<string, int>(capacity: 1000);

// FrozenDictionary — для данных, которые НИКОГДА не меняются
// Медленнее создаётся, но БЫСТРЕЕ читается
FrozenDictionary<string, int> lookup = data.ToFrozenDictionary();
int value = lookup["key"]; // быстрее обычного Dictionary
```

### String оптимизации

```csharp
// ❌ Конкатенация в цикле — O(n²)
string result = "";
foreach (var item in items)
    result += item.ToString(); // новый объект каждый раз!

// ✅ StringBuilder
var sb = new StringBuilder(items.Count * 10); // примерная capacity
foreach (var item in items)
    sb.Append(item);
string result = sb.ToString();

// ✅ String.Join / String.Concat
string result = string.Join(", ", items);

// ✅ Interpolated string handler (.NET 6+)
// $"..." компилируется эффективно, но только для ОДНОЙ строки
// В цикле — всё равно StringBuilder
```

---

## Когда НЕ оптимизировать

```
❌ Не оптимизируй код, который вызывается 1 раз при старте
❌ Не заменяй LINQ на for, если коллекция < 1000 элементов
❌ Не используй Span везде — только в горячих путях
❌ Не жертвуй читаемостью ради наносекунд

✅ Оптимизируй:
- Код, который вызывается каждый кадр (60 раз/сек)
- Обработку большого объёма данных
- Горячие пути, найденные профайлером
- Аллокации, которые вызывают GC фризы
```

---

## Мини-упражнения

1. **⭐** BenchmarkDotNet: сравни `string.Substring()` vs `Span<char>` для парсинга CSV-строки.
2. **⭐⭐** Перепиши метод, который создаёт массив в цикле, на ArrayPool.
3. **⭐⭐** Добавь MemoryCache к методу, который "дорого" вычисляет результат.

## Что дальше
Дальше — **оптимизация Godot** (16.3).
