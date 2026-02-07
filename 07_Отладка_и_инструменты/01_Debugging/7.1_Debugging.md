# Тема 7.1: Отладка (Debugging)

## Что ты узнаешь
- Breakpoints: обычные, условные, tracepoints
- Окна отладки: Watch, Locals, Immediate, Call Stack
- Step Over, Step Into, Step Out
- Debug vs Release
- Отладка Godot C# проектов

---

## Объяснение

### ЗАЧЕМ?
Отладка — это 50% времени разработки. Умение быстро находить баги отличает начинающего от профессионала. Отладчик — твой главный инструмент.

### Breakpoints (точки останова)

```csharp
void ProcessDamage(Player player, int damage)
{
    int armor = player.Armor;        // ← поставь breakpoint (F9 или клик по полоске)
    int reduced = damage - armor;
    if (reduced < 0) reduced = 0;
    player.Health -= reduced;
}
```

**Виды breakpoints:**

| Тип | Что делает | Когда |
|-----|-----------|-------|
| **Обычный** | Останавливает выполнение | Всегда |
| **Conditional** | Останавливается при условии | `i > 50` или `name == "bug"` |
| **Hit Count** | После N попаданий | "Остановись на 100-й итерации" |
| **Tracepoint (Logpoint)** | Пишет в Output без остановки | Логирование без Debug.WriteLine |
| **Exception** | При определённом исключении | NullReferenceException |

### Как поставить в Rider:
- **Обычный**: клик по левой полоске (или F9)
- **Conditional**: правый клик на breakpoint → Condition → `damage > 50`
- **Tracepoint**: правый клик → Log message → `Damage: {damage}, Armor: {armor}`

### Окна отладки

```
┌─── Watch ────────────────┐  ┌─── Call Stack ──────────┐
│ player.Health = 85       │  │ ProcessDamage()         │
│ damage = 30              │  │ ← CombatSystem.Attack() │
│ player.Name = "Алиса"    │  │ ← GameLoop.Update()     │
│ damage > player.Health   │  │ ← Main()                │
│ → false                  │  │                          │
└──────────────────────────┘  └──────────────────────────┘

┌─── Immediate Window ─────┐
│ > player.Skills.Count     │
│ 3                         │
│ > player.TakeDamage(10)   │  ← Можно вызывать методы!
│ > Math.Max(0, damage-50)  │
│ 0                         │
└──────────────────────────┘
```

**Watch** — добавь любое выражение, отслеживай его значение.
**Locals** — все локальные переменные текущего метода.
**Immediate Window** — выполняй код прямо во время отладки!
**Call Stack** — кто вызвал текущий метод (цепочка вызовов).
**Threads** — список всех потоков.

### Навигация при отладке

| Действие | Rider | VS | Что делает |
|----------|-------|----|-----------|
| Step Over | F10 | F10 | Выполнить строку, не входя в метод |
| Step Into | F7 | F11 | Войти внутрь метода |
| Step Out | Shift+F8 | Shift+F11 | Выйти из текущего метода |
| Run to Cursor | Alt+F9 | Ctrl+F10 | Выполнить до курсора |
| Resume | F9 | F5 | Продолжить до следующего breakpoint |

### Debug vs Release

```csharp
// Debug — для разработки
// ✅ Breakpoints работают
// ✅ Все переменные видны
// ❌ Медленнее (нет оптимизаций)

// Release — для пользователей
// ✅ Быстрее (оптимизации компилятора)
// ❌ Breakpoints могут "прыгать"
// ❌ Некоторые переменные "оптимизированы" (не видны)

// Условная компиляция
#if DEBUG
    Console.WriteLine("Мы в Debug режиме");
#endif

// Класс Debug — вывод только в Debug
System.Diagnostics.Debug.WriteLine("Виден только в Debug");
System.Diagnostics.Debug.Assert(health > 0, "Здоровье не может быть отрицательным!");
```

### Отладка Godot C#

1. **Rider**: Run → Attach to Process → выбери Godot
2. Или: настрой Run Configuration → Godot C#
3. Breakpoints в C# скриптах работают!
4. `GD.Print()` — вывод в Godot Output

```csharp
// Godot-специфичная отладка
public override void _Process(double delta)
{
    GD.Print($"Position: {Position}"); // в Godot Output

    // DebugDraw — визуальная отладка
    DebugDraw2D.DrawCircle(Position, 50, Colors.Red);
}
```

---

## Мини-упражнения
1. **⭐** Поставь conditional breakpoint: останови только когда `i > 50`.
2. **⭐** Используй Immediate Window для вызова метода во время отладки.
3. **⭐** Намеренно создай `NullReferenceException`, поймай через Exception breakpoint.

---

## Что дальше
Дальше — **логирование** (7.2).
