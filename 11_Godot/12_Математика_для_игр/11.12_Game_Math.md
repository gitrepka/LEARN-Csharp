# Тема 11.12: Математика для игр

## Что ты узнаешь
- Vector2/Vector3: операции, нормализация, dot/cross product
- Lerp, Slerp — плавные переходы
- Тригонометрия для игр: углы, направления
- Mathf — утилиты Godot

---

## Объяснение

### Vector2 — основные операции

```csharp
Vector2 a = new(3, 4);
Vector2 b = new(1, 0);

float length = a.Length();           // 5 (sqrt(9+16))
Vector2 norm = a.Normalized();       // (0.6, 0.8) — единичный вектор
float dist = a.DistanceTo(b);       // расстояние между точками
float dot = a.Dot(b);               // скалярное произведение (3)
float angle = a.AngleTo(b);         // угол между векторами
Vector2 dir = a.DirectionTo(b);     // направление к точке

// Lerp — плавный переход
Vector2 current = new(0, 0);
Vector2 target = new(100, 100);
current = current.Lerp(target, 0.1f); // 10% к цели каждый кадр

// MoveToward — с фиксированной скоростью
current = current.MoveToward(target, speed * (float)delta);

// Поворот вектора
Vector2 rotated = Vector2.Right.Rotated(Mathf.DegToRad(45)); // 45°
```

### Mathf — утилиты

```csharp
float clamped = Mathf.Clamp(value, 0, 100);        // ограничить диапазон
float lerped = Mathf.Lerp(0f, 100f, 0.5f);         // 50
float smoothed = Mathf.Lerp(current, target, 1 - Mathf.Exp(-10 * delta)); // сглаживание
float mapped = Mathf.Remap(75, 0, 100, 0, 1);      // 0.75 (перевод диапазона)
float snapped = Mathf.Snapped(47, 10);              // 50 (привязка к сетке)
float wrapped = Mathf.Wrap(370, 0, 360);            // 10 (зацикливание)

// Тригонометрия
float rad = Mathf.DegToRad(90);    // π/2
float deg = Mathf.RadToDeg(Mathf.Pi); // 180

// Случайные числа
float r = (float)GD.RandRange(0, 100);
int ri = GD.RandRange(0, 10);
```

### Практические формулы

```csharp
// Расстояние до цели (без sqrt — для сравнения)
float distSq = Position.DistanceSquaredTo(target.Position);
if (distSq < attackRange * attackRange) Attack();

// Dot product — "смотрит ли враг на меня?"
Vector2 forward = Vector2.Right.Rotated(Rotation);
Vector2 toTarget = (target.Position - Position).Normalized();
float dot = forward.Dot(toTarget);
if (dot > 0.7f) // в конусе ~45° перед врагом
    GD.Print("Враг видит игрока!");
```

---

## Мини-упражнения
1. **⭐** Плавное следование за мышью через Lerp.
2. **⭐** Определи "видит ли враг игрока" через Dot product.
3. **⭐⭐** Движение по кругу через тригонометрию.

## Что дальше
Дальше — **шейдеры и VFX** (11.13).
