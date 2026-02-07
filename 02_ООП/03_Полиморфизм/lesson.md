# Тема 2.3: Полиморфизм

## Что ты узнаешь
- Полиморфизм через наследование и интерфейсы
- Приведение типов: is, as, typeof, GetType
- Полиморфные коллекции

---

## Объяснение

### ЗАЧЕМ?
**Аналогия:** Розетка принимает ЛЮБУЮ вилку стандарта. Ей неважно что подключено — лампа, телевизор, зарядка. Так и метод, принимающий `Shape`, работает с Circle, Rectangle, Triangle — без изменений.

### Через наследование

```csharp
abstract class Shape
{
    public abstract double Area();
}

class Circle : Shape
{
    public double Radius { get; init; }
    public override double Area() => Math.PI * Radius * Radius;
}

class Rectangle : Shape
{
    public double Width { get; init; }
    public double Height { get; init; }
    public override double Area() => Width * Height;
}

// Полиморфизм! Один массив — разные типы — одинаковый вызов
Shape[] shapes = [new Circle { Radius = 5 }, new Rectangle { Width = 3, Height = 4 }];

foreach (Shape s in shapes)
{
    Console.WriteLine($"Площадь: {s.Area():F2}"); // Каждый считает по-своему!
}
// Площадь: 78.54
// Площадь: 12.00
```

### Проверка и приведение типов

```csharp
Shape shape = new Circle { Radius = 5 };

// is — проверка типа (с pattern matching!)
if (shape is Circle circle)
{
    Console.WriteLine($"Радиус: {circle.Radius}");
}

// as — попытка приведения (вернёт null если не получится)
Circle? maybeCircle = shape as Circle;
if (maybeCircle != null)
    Console.WriteLine(maybeCircle.Radius);

// Прямое приведение (кинет InvalidCastException если неправильно)
Circle c = (Circle)shape; // опасно!

// typeof и GetType
Console.WriteLine(shape.GetType() == typeof(Circle)); // True
Console.WriteLine(shape is Shape);                     // True (Circle ЯВЛЯЕТСЯ Shape)
```

---

## Мини-упражнения
1. **⭐** Массив `Shape[]` с разными фигурами — вызови `Area()` в цикле.
2. **⭐** Используй `is Type variable` для безопасного приведения.
3. **⭐** Покажи разницу: `(Dog)animal` vs `animal as Dog` при невозможном приведении.

## Практика Uno ⭐⭐⭐
Расширь редактор: `IResizable`, `IRotatable`, `IDraggable` — не все фигуры реализуют всё.

## Практика Godot ⭐⭐⭐
`IInteractable.Interact()`: Chest, Door, NPC, Switch — игрок взаимодействует через полиморфизм.

## Контрольные вопросы
1. Чем полиморфизм через наследование отличается от полиморфизма через интерфейсы?
2. Когда `is` лучше чем `as`?
3. Что такое "полиморфная коллекция"?

## Что дальше
Дальше — **инкапсуляция** (2.4) и **интерфейсы** (2.5).
