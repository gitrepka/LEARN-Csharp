# Тема 2.8: Records (записи)

## Что ты узнаешь
- record class vs record struct
- Позиционные records, with-выражения, value equality
- Когда record vs class vs struct

---

## Объяснение

### ЗАЧЕМ?
Для данных, которые нужно СРАВНИВАТЬ по значению (не по ссылке) и КОПИРОВАТЬ с изменениями. Два игрока с одинаковыми характеристиками — равны? С record — да!

```csharp
// Позиционный record (автоматически создаёт свойства, конструктор, ToString, Equals)
record Point(int X, int Y);

var p1 = new Point(3, 4);
var p2 = new Point(3, 4);
Console.WriteLine(p1 == p2);  // True! (value equality, не reference)
Console.WriteLine(p1);        // "Point { X = 3, Y = 4 }" (автоматический ToString)

// with — копия с изменениями
var p3 = p1 with { X = 10 };
Console.WriteLine(p3); // "Point { X = 10, Y = 4 }"

// Деконструкция
var (x, y) = p1;
Console.WriteLine($"x={x}, y={y}");

// Наследование records
record Shape(string Color);
record Circle(string Color, double Radius) : Shape(Color);

// record struct (C# 10) — значимый тип + value equality + with
record struct Velocity(float X, float Y);
```

### Когда что?
| Тип | Для чего | Пример |
|-----|----------|--------|
| `class` | Объекты с идентичностью и поведением | Player, Enemy |
| `struct` | Маленькие значимые данные | Vector2, Color |
| `record class` | Иммутабельные данные, DTO | SaveData, Config |
| `record struct` | Маленькие данные + value equality | DamageInfo |

---

## Мини-упражнения
1. **⭐** Создай `record Point(int X, int Y)`, используй `with`.
2. **⭐** Покажи value equality: два одинаковых record равны.
3. **⭐** Создай `record struct Velocity(float X, float Y)`.

## Что дальше
Дальше — **кортежи** (2.9) и **collection expressions** (2.10).
