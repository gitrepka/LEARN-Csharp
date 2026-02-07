# Тема 1.4: Управляющие конструкции (условия и циклы)

## Что ты узнаешь
- if/else, switch, switch expressions
- Циклы: for, foreach, while, do...while
- break, continue, return

---

## Объяснение

### ЗАЧЕМ?
Без условий и циклов программа — просто список инструкций сверху вниз. С ними — она умеет ДУМАТЬ и ПОВТОРЯТЬ. "Если враг рядом — атакуй. Иначе — патрулируй. Повторяй каждый кадр."

### Условные операторы

**if / else if / else:**
```csharp
int health = 30;

if (health > 75)
{
    Console.WriteLine("Полное здоровье");
}
else if (health > 25)
{
    Console.WriteLine("Ранен");
}
else
{
    Console.WriteLine("При смерти!"); // ← этот вариант
}
```

**switch (классический):**
```csharp
string weapon = "меч";

switch (weapon)
{
    case "меч":
        Console.WriteLine("Ближний бой");
        break;
    case "лук":
        Console.WriteLine("Дальний бой");
        break;
    case "посох":
        Console.WriteLine("Магия");
        break;
    default:
        Console.WriteLine("Кулаки");
        break;
}
```

**switch expression (C# 8+) — современный стиль:**
```csharp
string weapon = "меч";

string type = weapon switch
{
    "меч" or "топор" => "Ближний бой",
    "лук" or "арбалет" => "Дальний бой",
    "посох" or "жезл" => "Магия",
    _ => "Кулаки"  // _ = default (всё остальное)
};

Console.WriteLine(type); // "Ближний бой"
```

**Switch expression с условиями:**
```csharp
int score = 85;

string grade = score switch
{
    >= 90 => "Отлично",
    >= 75 => "Хорошо",
    >= 60 => "Удовлетворительно",
    _ => "Неудовлетворительно"
};
```

### Циклы

**for — когда знаешь сколько раз:**
```csharp
for (int i = 0; i < 10; i++)
{
    Console.Write($"{i} "); // 0 1 2 3 4 5 6 7 8 9
}
```

**foreach — для коллекций (списки, массивы):**
```csharp
string[] enemies = { "Гоблин", "Скелет", "Дракон" };

foreach (string enemy in enemies)
{
    Console.WriteLine($"Враг: {enemy}");
}
```

**while — пока условие true:**
```csharp
int health = 100;

while (health > 0)
{
    health -= 15; // каждую итерацию теряем 15 HP
    Console.WriteLine($"HP: {health}");
}
// Выведет: 85, 70, 55, 40, 25, 10, -5
```

**do...while — минимум одно выполнение:**
```csharp
int attempts = 0;
bool success;

do
{
    attempts++;
    success = Random.Shared.Next(10) == 0; // 10% шанс
    Console.WriteLine($"Попытка {attempts}...");
} while (!success);

Console.WriteLine($"Успех с {attempts} попытки!");
```

### Управление потоком

```csharp
// break — выйти из цикла
for (int i = 0; i < 100; i++)
{
    if (i == 5) break;    // выходим на 5
    Console.Write(i + " "); // 0 1 2 3 4
}

// continue — пропустить итерацию
for (int i = 0; i < 10; i++)
{
    if (i % 2 != 0) continue; // пропускаем нечётные
    Console.Write(i + " "); // 0 2 4 6 8
}

// return — выйти из метода
int FindFirst(int[] arr, int target)
{
    for (int i = 0; i < arr.Length; i++)
    {
        if (arr[i] == target) return i; // нашли — сразу возвращаем
    }
    return -1; // не нашли
}
```

---

## Примеры кода

### Пример 1: FizzBuzz через switch expression
```csharp
for (int i = 1; i <= 30; i++)
{
    string result = (i % 3, i % 5) switch
    {
        (0, 0) => "FizzBuzz",
        (0, _) => "Fizz",
        (_, 0) => "Buzz",
        _ => i.ToString()
    };
    Console.Write(result + " ");
}
```

### Пример 2: Вложенные циклы — таблица умножения
```csharp
for (int row = 1; row <= 10; row++)
{
    for (int col = 1; col <= 10; col++)
    {
        Console.Write($"{row * col,4}"); // ,4 = выравнивание по 4 символа
    }
    Console.WriteLine();
}
```

### Пример 3: Поиск с break
```csharp
string[] items = { "зелье", "меч", "щит", "лук", "зелье" };
int firstPotion = -1;

for (int i = 0; i < items.Length; i++)
{
    if (items[i] == "зелье")
    {
        firstPotion = i;
        break; // нашли первое — выходим
    }
}

Console.WriteLine($"Первое зелье на позиции: {firstPotion}"); // 0
```

---

## Частые ошибки

### 1. Бесконечный цикл
```csharp
// ❌ i никогда не увеличивается!
for (int i = 0; i < 10; )
{
    Console.WriteLine(i);
}

// ❌ while(true) без break
while (true)
{
    // забыл break — программа зависнет!
}
```

### 2. Off-by-one (ошибка на единицу)
```csharp
int[] arr = { 10, 20, 30 };

// ❌ arr.Length = 3, но последний индекс = 2!
for (int i = 0; i <= arr.Length; i++) // IndexOutOfRangeException на i=3!

// ✅ Строго < Length:
for (int i = 0; i < arr.Length; i++) // 0, 1, 2 — правильно
```

### 3. Забыл break в switch
```csharp
// В C# это ошибка компиляции (в отличие от C++), так что безопасно.
// Но в switch expression не нужен break вообще — он элегантнее.
```

---

## Мини-упражнения

1. **⭐** Перепиши цепочку `if/else if` (определение оценки по баллам) как switch expression.

2. **⭐** Цикл выводит только чётные числа от 0 до 100 — используй `continue`.

3. **⭐** Реши FizzBuzz через switch expression (как в примере выше, но напиши сам).

4. **⭐** "Найди ошибку" — почему этот код зависнет?
```csharp
int i = 10;
while (i > 0)
{
    Console.WriteLine(i);
    i++;  // ???
}
```

5. **⭐** Напиши цикл, который ищет первое число > 100 в массиве и выводит его.

---

## Практика Uno Platform ⭐⭐

**"Генератор таблицы умножения"**
- TextBox для ввода числа
- Кнопка "Сгенерировать"
- Grid заполняется результатами
- Чётные числа подсвечены другим цветом

---

## Практика Godot ⭐⭐

**AI врага**
- Массив точек патруля → `for` для обхода
- Обнаружение игрока: `if (distance < detectionRange)`
- Поведение через `switch` по состояниям: `Patrol`, `Chase`, `Attack`, `Flee`

```csharp
enum EnemyState { Patrol, Chase, Attack, Flee }

EnemyState _state = EnemyState.Patrol;

public override void _Process(double delta)
{
    switch (_state)
    {
        case EnemyState.Patrol:
            // двигаемся между точками
            break;
        case EnemyState.Chase:
            // бежим к игроку
            break;
        // ...
    }
}
```

---

## Контрольные вопросы

1. Чем switch expression отличается от обычного switch?
2. Когда использовать `for`, а когда `foreach`?
3. В чём разница между `break` и `continue`?
4. Что такое `_` (discard) в switch expression?
5. Почему `do...while` гарантирует хотя бы одно выполнение?

---

## Что дальше
Ты умеешь управлять потоком программы. Дальше — **строки** (тема 1.5): как работать с текстом, форматировать его и искать в нём.
