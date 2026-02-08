# Тема 4.7: Индексаторы и перегрузка операторов

## Что ты узнаешь
- Индексаторы: this[int index], this[string key]
- Перегрузка операторов: +, -, *, ==, !=
- Неявное и явное приведение: implicit, explicit
- Checked операторы (C# 11)

---

## Объяснение

### ЗАЧЕМ?
Хочешь обращаться к своему объекту как к массиву — `myObj[0]`? Или складывать два вектора `v1 + v2`? Индексаторы и перегрузка операторов делают код интуитивным.

### Индексаторы

```csharp
class Inventory
{
    private readonly Dictionary<string, int> _items = new();

    // Индексатор по строке
    public int this[string itemName]
    {
        get => _items.GetValueOrDefault(itemName, 0);
        set => _items[itemName] = value;
    }

    // Можно несколько индексаторов с разными параметрами
    public string this[int slot]
    {
        get => _items.ElementAtOrDefault(slot).Key ?? "Empty";
    }
}

Inventory inv = new();
inv["Меч"] = 1;
inv["Зелье"] = 5;
Console.WriteLine(inv["Меч"]);   // 1
Console.WriteLine(inv["Щит"]);   // 0 (не найден)
Console.WriteLine(inv[0]);       // "Меч"
```

### Readonly индексатор

```csharp
class Matrix
{
    private readonly double[,] _data;

    public Matrix(int rows, int cols)
    {
        _data = new double[rows, cols];
        Rows = rows;
        Cols = cols;
    }

    public int Rows { get; }
    public int Cols { get; }

    // Индексатор с двумя параметрами
    public double this[int row, int col]
    {
        get => _data[row, col];
        set => _data[row, col] = value;
    }
}

Matrix m = new(3, 3);
m[0, 0] = 1.0;
m[1, 1] = 1.0;
m[2, 2] = 1.0; // Единичная матрица
```

---

### Перегрузка операторов

```csharp
record struct Vector2(float X, float Y)
{
    // Бинарные операторы
    public static Vector2 operator +(Vector2 a, Vector2 b)
        => new(a.X + b.X, a.Y + b.Y);

    public static Vector2 operator -(Vector2 a, Vector2 b)
        => new(a.X - b.X, a.Y - b.Y);

    // Скалярное умножение
    public static Vector2 operator *(Vector2 v, float scalar)
        => new(v.X * scalar, v.Y * scalar);

    public static Vector2 operator *(float scalar, Vector2 v)
        => v * scalar; // коммутативность

    // Унарные операторы
    public static Vector2 operator -(Vector2 v)
        => new(-v.X, -v.Y);

    // Длина
    public float Length => MathF.Sqrt(X * X + Y * Y);

    // Нормализация
    public Vector2 Normalized => this * (1f / Length);
}

// Использование — как математика!
Vector2 pos = new(10, 20);
Vector2 velocity = new(5, -3);
Vector2 newPos = pos + velocity * 0.16f; // pos + (velocity × deltaTime)
Vector2 direction = (newPos - pos).Normalized;
```

### Операторы сравнения

```csharp
record struct Money(decimal Amount, string Currency) : IComparable<Money>
{
    // == и != автоматически от record struct

    public static bool operator >(Money a, Money b)
    {
        EnsureSameCurrency(a, b);
        return a.Amount > b.Amount;
    }

    public static bool operator <(Money a, Money b)
    {
        EnsureSameCurrency(a, b);
        return a.Amount < b.Amount;
    }

    public static bool operator >=(Money a, Money b) => !(a < b);
    public static bool operator <=(Money a, Money b) => !(a > b);

    // Арифметика
    public static Money operator +(Money a, Money b)
    {
        EnsureSameCurrency(a, b);
        return a with { Amount = a.Amount + b.Amount };
    }

    public int CompareTo(Money other)
    {
        EnsureSameCurrency(this, other);
        return Amount.CompareTo(other.Amount);
    }

    private static void EnsureSameCurrency(Money a, Money b)
    {
        if (a.Currency != b.Currency)
            throw new InvalidOperationException($"Нельзя сравнить {a.Currency} и {b.Currency}");
    }
}

Money price = new(100, "RUB");
Money tax = new(13, "RUB");
Money total = price + tax; // Money(113, "RUB")
bool expensive = total > new Money(50, "RUB"); // true
```

---

### Неявное и явное приведение

```csharp
record struct Celsius(double Value)
{
    // Неявное: Celsius → double (безопасно, без потери данных)
    public static implicit operator double(Celsius c) => c.Value;

    // Явное: double → Celsius (может быть неожиданным)
    public static explicit operator Celsius(double d) => new(d);

    // Неявное: int → Celsius
    public static implicit operator Celsius(int degrees) => new(degrees);
}

Celsius temp = 36;            // implicit: int → Celsius
double raw = temp;            // implicit: Celsius → double
Celsius back = (Celsius)98.6; // explicit: double → Celsius (нужен каст)

// Практический пример
record struct Percentage(double Value)
{
    public static implicit operator Percentage(double d) => new(d);
    public static implicit operator double(Percentage p) => p.Value;

    public static Percentage operator *(Percentage p, double factor) => new(p.Value * factor);

    public override string ToString() => $"{Value:F1}%";
}

Percentage discount = 15.0; // implicit
double result = 100.0 * (1 - discount / 100); // implicit → double
```

---

### Checked операторы (C# 11)

```csharp
record struct SafeInt(int Value)
{
    // Обычный оператор (unchecked)
    public static SafeInt operator +(SafeInt a, SafeInt b)
        => new(a.Value + b.Value);

    // Checked версия — бросает OverflowException
    public static SafeInt operator checked +(SafeInt a, SafeInt b)
        => new(checked(a.Value + b.Value));

    public static explicit operator SafeInt(int value) => new(value);
    public static explicit operator checked SafeInt(int value) => new(checked(value));
}

SafeInt a = new(int.MaxValue);
SafeInt b = new(1);

// unchecked (по умолчанию) — переполнение
SafeInt c = a + b; // int.MinValue (переполнение, без ошибки)

// checked — исключение
// SafeInt d = checked(a + b); // OverflowException!
```

---

## Правила перегрузки

- Если перегружаешь `==`, **обязательно** перегрузи `!=`
- Если перегружаешь `>`, **обязательно** перегрузи `<`
- `implicit` — только когда **нет потери данных**
- `explicit` — когда возможна потеря или неожиданное поведение
- Не перегружай `&&` и `||` напрямую — перегрузи `&`, `|`, `true`, `false`

---

## Мини-упражнения
1. **⭐** Создай `Vector2D` с перегруженными `+`, `-`, `* scalar`.
2. **⭐** Добавь `implicit operator` из `int` → `Vector2D` и `explicit` из `Vector2D` → `(int, int)`.
3. **⭐⭐** Создай класс `Matrix` с индексатором `this[int row, int col]`.

### Практика Uno Platform ⭐⭐
**Класс `Matrix`** с `+`, `-`, `*`, индексатором. UI для ввода и отображения матриц.

### Практика Godot ⭐⭐
**`GameCurrency`** с перегруженными операторами. Магазин: цены, покупка, проверка баланса. UI с отображением.

---

## Контрольные вопросы перед Модулем 5
1. Чем `Func<T>` отличается от `Action<T>`?
2. Что такое отложенное выполнение в LINQ?
3. Почему `throw;` лучше `throw ex;`?
4. Что такое deadlock с `.Result`?
5. Назови 3 вида паттернов в C#.
6. Зачем нужны checked операторы?

## Что дальше
Модуль 4 завершён! Дальше — **Модуль 5: Работа с файлами и данными**.
