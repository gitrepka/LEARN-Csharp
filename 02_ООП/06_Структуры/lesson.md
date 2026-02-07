# Тема 2.6: Структуры (struct)

## Что ты узнаешь
- struct vs class: значимый тип, стек, копирование
- readonly struct, ref struct, record struct
- Когда struct, когда class

---

## Объяснение

### ЗАЧЕМ?
Класс хранит данные в куче (heap) и передаёт по ссылке. Для маленьких данных (координаты, цвет, урон) это накладные расходы. Struct хранит данные на стеке и копируется целиком — быстрее для маленьких объектов.

```csharp
// struct — значимый тип (копируется)
struct Vector2
{
    public float X;
    public float Y;

    public Vector2(float x, float y) { X = x; Y = y; }
    public float Length() => MathF.Sqrt(X * X + Y * Y);
}

Vector2 a = new(3, 4);
Vector2 b = a;      // КОПИЯ! b — отдельный объект
b.X = 100;
Console.WriteLine(a.X); // 3 — a НЕ изменился!

// readonly struct — гарантия неизменяемости
readonly struct Color
{
    public byte R { get; init; }
    public byte G { get; init; }
    public byte B { get; init; }
}

// record struct (C# 10) — struct с value equality и with
record struct DamageInfo(float Amount, DamageType Type, bool IsCritical);

var dmg = new DamageInfo(50, DamageType.Fire, true);
var reduced = dmg with { Amount = 30 }; // копия с изменением
```

### Когда struct, когда class?
| Критерий | struct | class |
|----------|--------|-------|
| Размер | Маленький (< 16 байт) | Любой |
| Семантика | Значение (копируется) | Ссылка (общий объект) |
| Примеры | Vector2, Color, DateTime | Player, Enemy, Window |
| Наследование | Нет (только интерфейсы) | Да |

---

## Мини-упражнения
1. **⭐** Создай `readonly struct Vector2D(float X, float Y)` с методом Length().
2. **⭐** Покажи разницу копирования struct vs class.

## Что дальше
Дальше — **перечисления (enum)** (2.7).
