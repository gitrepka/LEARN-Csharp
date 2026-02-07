# Тема 19.5: Новые фичи C# 13 и C# 14

## Что ты узнаешь
- C# 13 (.NET 9): params collections, Lock, ref struct interfaces
- C# 14 (.NET 10): field keyword, extension members, partial constructors
- Как и когда применять новые возможности

---

## Объяснение

### C# 13 — Основные фичи

#### `params` для любых коллекций

```csharp
// Раньше: params работал только с массивами
void Old(params int[] numbers) { }

// C# 13: params работает с Span, List, IEnumerable и др.
void New(params ReadOnlySpan<int> numbers)
{
    foreach (var n in numbers)
        Console.Write($"{n} ");
}

void NewList(params List<string> items)
{
    items.Add("ещё один"); // можно модифицировать!
}

// Вызов не меняется
New(1, 2, 3, 4, 5);
NewList("a", "b", "c");

// Зачем? Span — без аллокации массива в куче!
```

#### `System.Threading.Lock` — новый тип для lock

```csharp
// Раньше: lock на object (monitor-based)
private readonly object _lockObj = new();
lock (_lockObj) { /* ... */ }

// C# 13: специальный тип Lock (быстрее)
private readonly Lock _lock = new();

public void ThreadSafeMethod()
{
    lock (_lock) // компилятор использует Lock.EnterScope()
    {
        // критическая секция
    }

    // Или вручную:
    using (_lock.EnterScope())
    {
        // критическая секция
    }
}

// Зачем? Lock тип оптимизирован лучше чем Monitor.Enter/Exit
```

#### `\e` escape-последовательность

```csharp
// Раньше: ANSI escape код
Console.Write("\x1b[31mRed text\x1b[0m");

// C# 13: \e = escape (0x1B)
Console.Write("\e[31mRed text\e[0m");
Console.Write("\e[1mBold\e[0m");
Console.Write("\e[32mGreen\e[0m");
```

#### `ref struct` может реализовывать интерфейсы

```csharp
// Раньше: ref struct не мог реализовывать интерфейсы
// C# 13: может!

public interface IParser
{
    bool TryParse(ReadOnlySpan<char> input, out int result);
}

public ref struct SpanParser : IParser
{
    public bool TryParse(ReadOnlySpan<char> input, out int result)
    {
        return int.TryParse(input, out result);
    }
}
```

#### Partial properties

```csharp
// C# 13: partial properties (для source generators)
public partial class Player
{
    // Декларация (в одном файле)
    public partial string Name { get; set; }
}

public partial class Player
{
    // Реализация (в другом файле, возможно сгенерированном)
    private string _name = "";
    public partial string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged(nameof(Name));
        }
    }
}
```

### C# 14 — Основные фичи (.NET 10)

#### `field` keyword — доступ к backing field

```csharp
// Раньше: нужно было объявлять поле явно
private string _name = "";
public string Name
{
    get => _name;
    set
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        _name = value;
    }
}

// C# 14: field — автоматическое backing field
public string Name
{
    get;
    set
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        field = value; // field = автоматическое backing поле
    }
}

// Ещё пример: ленивая инициализация
public List<Item> Items
{
    get => field ??= new List<Item>();
}

// Валидация в set:
public int Health
{
    get;
    set => field = Math.Clamp(value, 0, MaxHealth);
}
```

#### Extension members (расширенные расширения)

```csharp
// Раньше: только extension methods
public static class StringExtensions
{
    public static bool IsNullOrEmpty(this string? s) => string.IsNullOrEmpty(s);
}

// C# 14: extension properties, static methods и др.
public extension StringExtensions for string
{
    // Extension property
    public bool IsEmpty => string.IsNullOrEmpty(this);

    // Extension static method
    public static string CreateRepeated(char c, int count) => new(c, count);

    // Extension indexer (!)
    public char FromEnd(int index) => this[^index];
}

// Использование
string name = "Hello";
bool empty = name.IsEmpty;           // extension property
string dots = string.CreateRepeated('.', 10); // extension static
char last = name.FromEnd(1);         // extension indexer
```

#### Partial constructors

```csharp
// C# 14: partial constructors (для source generators)
public partial class ViewModel
{
    public partial ViewModel(INavigationService nav);
}

// Сгенерированная часть:
public partial class ViewModel
{
    private readonly INavigationService _nav;

    public partial ViewModel(INavigationService nav)
    {
        _nav = nav;
        Initialize();
    }
}
```

### Таблица: когда какую фичу использовать

```
Фича                 Когда использовать
─────────────────    ──────────────────────────────
params Span          Горячие пути без аллокаций
Lock                 Вместо lock (object) в новом коде
field keyword        Свойства с валидацией (вместо явного поля)
Extension members    Extension properties, static ext. methods
Partial properties   Source generators (MVVM, mapping)
```

---

## Мини-упражнения

1. **⭐** Используй `params ReadOnlySpan<int>` для метода Sum. Сравни с `params int[]`.
2. **⭐** Используй `field` keyword для свойства с валидацией (Age: 0-150).
3. **⭐⭐** Замени `lock (object)` на `Lock` в многопоточном коде.

## Что дальше
Дальше — **Polly и устойчивость** (19.6).
