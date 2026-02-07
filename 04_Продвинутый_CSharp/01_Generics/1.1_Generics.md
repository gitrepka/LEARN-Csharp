# Тема 4.1: Обобщения (Generics)

## Что ты узнаешь
- Обобщённые классы и методы
- Ограничения (constraints): where T : ...
- Ковариантность (out T) и контравариантность (in T)
- Func<T>, Action<T>, Predicate<T>

---

## Объяснение

### ЗАЧЕМ?
Представь: ты написал `IntList`, потом `StringList`, потом `PlayerList`... Один и тот же код, только тип разный. **Generics** — пиши один раз для ЛЮБОГО типа.

### Обобщённые методы

```csharp
// Без generics — нужен метод для каждого типа:
void PrintInt(int value) => Console.WriteLine(value);
void PrintString(string value) => Console.WriteLine(value);

// С generics — один метод для всех:
void Print<T>(T value) => Console.WriteLine(value);

Print(42);           // T = int (автоматический вывод)
Print("Hello");      // T = string
Print(3.14);         // T = double

// Generic метод с несколькими параметрами
TResult Convert<TInput, TResult>(TInput input, Func<TInput, TResult> converter)
{
    return converter(input);
}

string result = Convert(42, n => n.ToString()); // "42"
int length = Convert("Hello", s => s.Length);   // 5
```

### Обобщённые классы

```csharp
class Repository<T>
{
    private readonly List<T> _items = [];

    public void Add(T item) => _items.Add(item);
    public T Get(int index) => _items[index];
    public IReadOnlyList<T> GetAll() => _items;
    public int Count => _items.Count;
}

// Используем с разными типами:
Repository<string> names = new();
names.Add("Алиса");

Repository<int> scores = new();
scores.Add(100);

// Generic интерфейс
interface IRepository<T>
{
    void Add(T item);
    T? FindById(int id);
    IReadOnlyList<T> GetAll();
}
```

---

### Ограничения (Constraints)

Generics мощны, но иногда нужно гарантировать, что T умеет что-то конкретное.

```csharp
// where T : class — только ссылочные типы
void Process<T>(T item) where T : class
{
    // T гарантированно может быть null
}

// where T : struct — только значимые типы
void Calculate<T>(T value) where T : struct { }

// where T : new() — у T есть конструктор без параметров
T CreateInstance<T>() where T : new()
{
    return new T(); // можно вызвать new!
}

// where T : BaseClass — наследует от класса
void ProcessEnemy<T>(T enemy) where T : BaseEnemy
{
    enemy.TakeDamage(10); // доступны методы BaseEnemy
}

// where T : IInterface — реализует интерфейс
T Max<T>(T a, T b) where T : IComparable<T>
{
    return a.CompareTo(b) > 0 ? a : b;
}

// Несколько ограничений
class Manager<T> where T : class, IDisposable, new()
{
    public T Create() => new T();
}

// where T : notnull — не может быть null
void NotNull<T>(T value) where T : notnull { }

// where T : unmanaged — для unsafe-кода (примитивы, структуры без ссылок)

// allows ref struct (C# 13) — разрешает Span<T> и другие ref struct
void Process<T>(T value) where T : allows ref struct { }
```

### Generic Math (C# 11 / .NET 7+)

Раньше нельзя было написать `T + T`. Теперь можно!

```csharp
using System.Numerics;

// Работает с int, double, float, decimal — с ЛЮБЫМ числом
T Sum<T>(T[] numbers) where T : INumber<T>
{
    T result = T.Zero;
    foreach (T n in numbers)
        result += n;
    return result;
}

Console.WriteLine(Sum(new[] { 1, 2, 3 }));         // 6 (int)
Console.WriteLine(Sum(new[] { 1.5, 2.5, 3.0 }));   // 7.0 (double)

// Среднее
T Average<T>(T[] numbers) where T : INumber<T>
{
    T sum = Sum(numbers);
    return sum / T.CreateChecked(numbers.Length);
}
```

---

### Ковариантность и контравариантность

Это про **совместимость generic типов** в иерархиях наследования.

```csharp
// Ковариантность (out T) — "выдающий" тип
// Можно использовать более конкретный тип вместо базового

// IEnumerable<out T> — ковариантный
IEnumerable<string> strings = ["hello", "world"];
IEnumerable<object> objects = strings; // ✅ string → object — OK!

// Свой ковариантный интерфейс:
interface IProducer<out T>
{
    T Produce(); // T только на выходе (return)
}

class AnimalFactory : IProducer<Dog>
{
    public Dog Produce() => new Dog();
}

IProducer<Animal> factory = new AnimalFactory(); // Dog → Animal ✅

// Контравариантность (in T) — "принимающий" тип
// Можно использовать более базовый тип вместо конкретного

// Action<in T> — контравариантный
Action<object> printObj = obj => Console.WriteLine(obj);
Action<string> printStr = printObj; // ✅ Если умеет принимать object, то и string примет!
printStr("Hello");

// Свой контравариантный интерфейс:
interface IComparer<in T>
{
    int Compare(T x, T y); // T только на входе (параметры)
}
```

### Запоминалка
- **out** = ковариантность = "выдаёт T" = можно присвоить более конкретному → базовому
- **in** = контравариантность = "принимает T" = можно присвоить базовому → более конкретному

---

## Частые ошибки

```csharp
// ❌ Забыл ограничение — нельзя вызвать метод на T
void Process<T>(T item)
{
    item.Name; // ❌ Компилятор не знает что у T есть Name!
}
// ✅
void Process<T>(T item) where T : IHasName
{
    item.Name; // ✅ Гарантировано интерфейсом
}

// ❌ new T() без ограничения new()
T Create<T>() => new T(); // ❌ Ошибка компиляции!
// ✅
T Create<T>() where T : new() => new T();

// ❌ Путаница с вариантностью
// List<Dog> НЕ является List<Animal>!
// List<T> — инвариантный (нет out/in)
List<Dog> dogs = [new Dog()];
// List<Animal> animals = dogs; // ❌ Ошибка!

// ✅ Но IEnumerable<Dog> IS IEnumerable<Animal>:
IEnumerable<Animal> animals = dogs; // ✅ IEnumerable<out T>
```

---

## Мини-упражнения
1. **⭐** Создай `class Repository<T> where T : class, new()` с Add, Remove, GetAll.
2. **⭐** Создай generic метод `T Max<T>(T a, T b) where T : IComparable<T>`.
3. **⭐** Покажи ковариантность: `IEnumerable<Dog>` → `IEnumerable<Animal>`.
4. **⭐⭐** Напиши `T Sum<T>(params T[] numbers) where T : INumber<T>` с Generic Math.

### Практика Uno Platform ⭐⭐⭐
**Универсальный `DataGrid<T>`** — принимает `List<T>`, отображает свойства через рефлексию в виде таблицы, поддержка сортировки.

### Практика Godot ⭐⭐⭐
**`ObjectPool<T> where T : Node`** — пул объектов для пуль, врагов, эффектов. Методы `Get()`, `Return()`, `PreWarm(int count)`.

---

## Что дальше
Дальше — **делегаты и события** (4.2).
