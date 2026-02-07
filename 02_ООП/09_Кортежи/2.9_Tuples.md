# Тема 2.9: Кортежи (Tuples)

## Что ты узнаешь
- ValueTuple с именованными элементами
- Деконструкция
- Возврат нескольких значений из метода

---

## Объяснение

### ЗАЧЕМ?
Иногда метод должен вернуть ДВА значения (минимум и максимум, координаты X и Y). Создавать класс ради этого — перебор. Кортеж — лёгкое решение.

```csharp
// Создание
(int x, int y) point = (10, 20);
Console.WriteLine(point.x); // 10

// Возврат из метода
(int Min, int Max) FindMinMax(int[] numbers)
{
    return (numbers.Min(), numbers.Max());
}

var result = FindMinMax([5, 2, 8, 1, 9]);
Console.WriteLine($"Min: {result.Min}, Max: {result.Max}");

// Деконструкция — распаковка в отдельные переменные
var (min, max) = FindMinMax([5, 2, 8, 1, 9]);
Console.WriteLine($"Min: {min}, Max: {max}");

// Discard _ — если часть не нужна
var (_, maxOnly) = FindMinMax([5, 2, 8, 1, 9]);

// Кастомный Deconstruct для своих классов
class Player
{
    public string Name { get; set; }
    public int Level { get; set; }

    public void Deconstruct(out string name, out int level)
    {
        name = Name;
        level = Level;
    }
}

var player = new Player { Name = "Алиса", Level = 5 };
var (name, level) = player; // работает благодаря Deconstruct!
```

---

## Мини-упражнения
1. **⭐** Верни кортеж `(int min, int max)` из метода, деконструируй.
2. **⭐** Добавь `Deconstruct` к своему классу.

## Что дальше
Дальше — **collection expressions** (2.10) — последняя тема ООП-модуля!
