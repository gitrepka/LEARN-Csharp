# Тема 4.6: Паттерн-матчинг (Pattern Matching)

## Что ты узнаешь
- Типовой, константный, реляционный паттерны
- Логические паттерны: and, or, not
- Паттерны свойств и позиционные
- Паттерны списков (C# 11)
- Switch expressions с паттернами

---

## Объяснение

### ЗАЧЕМ?
Вместо длинных цепочек `if/else if` с приведениями типов — элегантные, читаемые выражения. Pattern matching — один из самых мощных инструментов C#.

### Типовой паттерн (Type pattern)

```csharp
object obj = "Hello";

// is + переменная
if (obj is string text)
{
    Console.WriteLine(text.Length); // text уже string, без приведения!
}

// В switch
string Describe(object value) => value switch
{
    int n => $"Число: {n}",
    string s => $"Строка: {s} (длина {s.Length})",
    bool b => b ? "Да" : "Нет",
    null => "Пусто",
    _ => $"Другое: {value.GetType().Name}"
};
```

### Константный паттерн

```csharp
string GetDay(int day) => day switch
{
    1 => "Понедельник",
    2 => "Вторник",
    3 => "Среда",
    4 => "Четверг",
    5 => "Пятница",
    6 => "Суббота",
    7 => "Воскресенье",
    _ => throw new ArgumentOutOfRangeException(nameof(day))
};

// null-проверка через паттерн
if (name is null) { /* ... */ }
if (name is not null) { /* ... */ }
```

### Реляционный паттерн (C# 9)

```csharp
string GetTemperature(int temp) => temp switch
{
    < -20 => "Экстремальный мороз",
    < 0 => "Мороз",
    0 => "Ноль",
    < 15 => "Прохладно",
    < 25 => "Тепло",
    < 35 => "Жарко",
    >= 35 => "Пекло"
};

// С логическими паттернами
string GetAge(int age) => age switch
{
    < 0 => throw new ArgumentException("Отрицательный возраст"),
    0 => "Новорождённый",
    > 0 and <= 12 => "Ребёнок",
    > 12 and <= 17 => "Подросток",
    >= 18 and < 65 => "Взрослый",
    >= 65 => "Пенсионер"
};
```

### Логические паттерны: and, or, not

```csharp
// and — оба условия
if (number is > 0 and < 100) { /* 1-99 */ }

// or — хотя бы одно
if (day is "Saturday" or "Sunday") { /* выходной */ }

// not — отрицание
if (obj is not null) { /* не null */ }
if (status is not "active" and not "pending") { /* ни то ни другое */ }

// Комбинация
string Classify(int n) => n switch
{
    > 0 and < 10 or > 90 and < 100 => "Крайний",
    >= 10 and <= 90 => "Средний",
    _ => "Вне диапазона"
};
```

### Паттерн свойств (Property pattern)

```csharp
record Person(string Name, int Age, Address? Address);
record Address(string City, string Country);

// Проверка свойств объекта
string Describe(Person p) => p switch
{
    { Age: < 0 } => "Невалидный возраст",
    { Age: 0, Name.Length: > 0 } => $"Новорождённый {p.Name}",
    { Address: null } => "Без адреса",
    { Address.Country: "Russia", Address.City: "Moscow" } => "Москвич",
    { Address.Country: "Russia" } => "Россиянин",
    { Age: >= 18 } => "Взрослый",
    _ => "Ребёнок"
};

// Вложенные свойства
if (person is { Address: { City: "Moscow" } })
{
    // Или короче (C# 10):
}
if (person is { Address.City: "Moscow" })
{
    Console.WriteLine("Москвич!");
}
```

### Позиционный паттерн (Positional pattern)

Для типов с деконструкцией (record, tuple):

```csharp
record Point(int X, int Y);

string GetQuadrant(Point p) => p switch
{
    (0, 0) => "Начало координат",
    (> 0, > 0) => "Первый квадрант",
    (< 0, > 0) => "Второй квадрант",
    (< 0, < 0) => "Третий квадрант",
    (> 0, < 0) => "Четвёртый квадрант",
    (0, _) => "На оси Y",
    (_, 0) => "На оси X"
};

// С кортежами
string RockPaperScissors(string p1, string p2) => (p1, p2) switch
{
    ("rock", "scissors") or ("scissors", "paper") or ("paper", "rock") => "Игрок 1 победил",
    _ when p1 == p2 => "Ничья",
    _ => "Игрок 2 победил"
};
```

### Паттерн списков (List pattern) — C# 11

```csharp
int[] numbers = [1, 2, 3, 4, 5];

// Точное совпадение
if (numbers is [1, 2, 3, 4, 5]) { /* точно эти элементы */ }

// С переменными
if (numbers is [var first, .., var last])
{
    Console.WriteLine($"Первый: {first}, Последний: {last}");
}

// С условиями
if (numbers is [> 0, _, _, _, < 10]) { /* первый > 0, последний < 10 */ }

// Discard (_) и rest (..)
string Describe(int[] arr) => arr switch
{
    [] => "Пустой",
    [var single] => $"Один элемент: {single}",
    [var a, var b] => $"Два: {a} и {b}",
    [var first, .., var last] => $"От {first} до {last} ({arr.Length} элементов)"
};

// Вложенные паттерны в списках
if (matrix is [[1, 0, ..], [0, 1, ..], ..])
{
    Console.WriteLine("Начинается как единичная матрица");
}
```

### Var-паттерн

Захватывает значение в переменную (всегда совпадает):

```csharp
// Полезно с when
string Process(int[] data) => data switch
{
    [var x, ..] when x < 0 => "Начинается с отрицательного",
    [var x, .. var rest] when rest.Length > 10 => "Длинный массив",
    _ => "Обычный"
};
```

---

## Практический пример: система урона в игре

```csharp
record DamageInfo(float Amount, DamageType Type, bool IsCritical, string Source);
enum DamageType { Physical, Fire, Ice, Lightning, Poison }

float CalculateFinalDamage(DamageInfo damage, ArmorInfo armor) => damage switch
{
    { Amount: <= 0 } => 0,
    { IsCritical: true, Type: DamageType.Fire } =>
        damage.Amount * 2.5f - armor.FireResistance,
    { IsCritical: true } =>
        damage.Amount * 2.0f - armor.PhysicalResistance,
    { Type: DamageType.Poison } =>
        damage.Amount, // яд игнорирует броню
    { Type: DamageType.Physical, Amount: var amt } =>
        Math.Max(0, amt - armor.PhysicalResistance),
    _ => Math.Max(0, damage.Amount - armor.GeneralResistance)
};
```

---

## Мини-упражнения
1. **⭐** Классифицируй число: отрицательное, ноль, положительное, > 100.
2. **⭐** Используй list pattern: `if (array is [var first, .., var last])`.
3. **⭐⭐** Паттерн свойств: `person is { Age: >= 18, Name.Length: > 0 }`.
4. **⭐⭐** Реализуй Rock-Paper-Scissors через tuple pattern matching.

### Практика Godot ⭐⭐⭐
**Система столкновений** — switch по типу body с property patterns. Разное поведение для Player, Enemy, Collectible, Hazard.

---

## Что дальше
Дальше — **индексаторы и операторы** (4.7).
