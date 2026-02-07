# Тема 1.6: Массивы и диапазоны

## Что ты узнаешь
- Одномерные, многомерные и зубчатые массивы
- Методы Array (Sort, Find, Reverse...)
- Index (^) и Range (..) — современный доступ к элементам
- Span<T> — работа без копирования

---

## Объяснение

### ЗАЧЕМ?
Переменная хранит ОДНО значение. Но в игре 100 врагов, в приложении 1000 заметок. Массив — это пронумерованный список значений одного типа.

**Аналогия:** Массив — это шкафчик с ячейками. Каждая ячейка пронумерована (0, 1, 2...) и хранит одно значение.

### Одномерные массивы

```csharp
// Создание
int[] numbers = new int[5];           // 5 элементов, все = 0
int[] scores = { 100, 85, 90, 75 };  // сразу с данными
int[] scores2 = [100, 85, 90, 75];   // C# 12: collection expression

// Доступ по индексу (начинается с 0!)
scores[0] = 95;                // первый элемент
int last = scores[3];          // последний = 75
// scores[4] = 50;             // ❌ IndexOutOfRangeException! Только 0-3

// Длина
Console.WriteLine(scores.Length); // 4

// Перебор
for (int i = 0; i < scores.Length; i++)
    Console.Write($"{scores[i]} ");

foreach (int s in scores)
    Console.Write($"{s} ");
```

### Многомерные массивы

```csharp
// Двумерный [строки, столбцы] — прямоугольный
int[,] grid = new int[3, 4]; // 3 строки × 4 столбца
grid[0, 0] = 1;
grid[2, 3] = 99;

int[,] map =
{
    { 1, 0, 0, 1 },
    { 0, 1, 1, 0 },
    { 1, 1, 0, 1 }
};

// Перебор
for (int row = 0; row < map.GetLength(0); row++)
{
    for (int col = 0; col < map.GetLength(1); col++)
    {
        Console.Write(map[row, col] + " ");
    }
    Console.WriteLine();
}

// Зубчатый (jagged) — массив массивов, разная длина строк
int[][] jagged = new int[3][];
jagged[0] = [1, 2];
jagged[1] = [3, 4, 5, 6];
jagged[2] = [7];
```

### Методы Array

```csharp
int[] arr = { 5, 3, 8, 1, 9, 2 };

Array.Sort(arr);              // [1, 2, 3, 5, 8, 9]
Array.Reverse(arr);           // [9, 8, 5, 3, 2, 1]
int idx = Array.IndexOf(arr, 5); // 2

int found = Array.Find(arr, x => x > 4);       // 9 (первый > 4)
int[] all = Array.FindAll(arr, x => x > 4);     // [9, 8, 5]
bool exists = Array.Exists(arr, x => x == 3);   // true
bool allBig = Array.TrueForAll(arr, x => x > 0); // true

// Копирование
int[] copy = new int[6];
Array.Copy(arr, copy, arr.Length);
```

### Index и Range (C# 8+)

**Index — доступ с конца:**
```csharp
int[] nums = { 10, 20, 30, 40, 50 };

nums[^1]   // 50 — последний
nums[^2]   // 40 — предпоследний
nums[^5]   // 10 — первый (^Length)
// nums[^0] // ❌ IndexOutOfRangeException! ^0 = за пределами
```

**Range — срез массива:**
```csharp
int[] nums = { 10, 20, 30, 40, 50 };

int[] slice1 = nums[1..3];   // [20, 30] — от индекса 1 до 3 (не включая 3)
int[] slice2 = nums[..3];    // [10, 20, 30] — первые 3
int[] slice3 = nums[2..];    // [30, 40, 50] — с 3-го до конца
int[] slice4 = nums[1..^1];  // [20, 30, 40] — без первого и последнего
int[] all = nums[..];        // полная копия

// Для строк тоже работает!
string text = "Hello, World!";
string hello = text[..5];    // "Hello"
string world = text[7..^1];  // "World"
```

### Span<T> — работа без копирования

```csharp
int[] numbers = { 1, 2, 3, 4, 5 };

// Span — "окно" в массив без копирования данных
Span<int> span = numbers.AsSpan();
Span<int> middle = span[1..4]; // элементы 2, 3, 4

middle[0] = 99; // ИЗМЕНЯЕТ оригинальный массив!
Console.WriteLine(numbers[1]); // 99

// stackalloc — выделение на стеке (быстро, без GC)
Span<int> temp = stackalloc int[3];
temp[0] = 10;
temp[1] = 20;
temp[2] = 30;
```

---

## Частые ошибки

### 1. Index из-за границ
```csharp
int[] arr = { 1, 2, 3 };
arr[3] = 4; // ❌ IndexOutOfRangeException! Индексы: 0, 1, 2
```

### 2. Range создаёт КОПИЮ
```csharp
int[] original = { 1, 2, 3 };
int[] slice = original[..2]; // НОВЫЙ массив [1, 2]
slice[0] = 99;
Console.WriteLine(original[0]); // 1 — оригинал НЕ изменился

// Если нужно без копии — используй Span
Span<int> view = original.AsSpan(0, 2);
view[0] = 99;
Console.WriteLine(original[0]); // 99 — оригинал ИЗМЕНИЛСЯ
```

---

## Мини-упражнения

1. **⭐** Получи последние 3 элемента массива через `[^3..]`.
2. **⭐** Используй Range для получения подстроки без `Substring`.
3. **⭐** Создай Span из массива, измени элемент через Span — убедись что массив изменился.
4. **⭐** Найди максимальный элемент массива БЕЗ LINQ (циклом).
5. **⭐** "Что выведет?" — 3 примера с `^` и `..`.

---

## Практика Uno Platform ⭐⭐
**"Визуальный сортировщик"** — ввод чисел → столбиковая диаграмма → анимация сортировки.

## Практика Godot ⭐⭐
**"Memory Cards"** — 2D массив для расположения карт. Клик → переворот. Совпадение → остаются.

---

## Контрольные вопросы
1. Чем `[,]` отличается от `[][]`?
2. Что значит `^1`? А `^0`?
3. `nums[1..3]` включает элемент с индексом 3?
4. Чем Span отличается от обычного среза `[..]`?
5. Когда стоит использовать `stackalloc`?

## Что дальше
Дальше — **методы** (1.7): как организовать код в переиспользуемые блоки.
