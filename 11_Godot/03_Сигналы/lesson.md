# Тема 11.3: Сигналы (Signals)

## Что ты узнаешь
- Встроенные сигналы Godot
- Кастомные сигналы: [Signal]
- Подключение из кода и из редактора
- Паттерн "Call down, Signal up"

---

## Объяснение

### ЗАЧЕМ?
Сигналы — Observer паттерн в Godot. Узел **не знает**, кто слушает. Это развязывает код.

### Встроенные сигналы

```csharp
public override void _Ready()
{
    // Подключение через +=
    var button = GetNode<Button>("Button");
    button.Pressed += OnButtonPressed;

    var area = GetNode<Area2D>("Area2D");
    area.BodyEntered += OnBodyEntered;

    var timer = GetNode<Timer>("Timer");
    timer.Timeout += OnTimerTimeout;

    var anim = GetNode<AnimationPlayer>("AnimationPlayer");
    anim.AnimationFinished += OnAnimationFinished;
}

private void OnButtonPressed() => GD.Print("Нажато!");
private void OnBodyEntered(Node2D body) => GD.Print($"Вошёл: {body.Name}");
private void OnTimerTimeout() => GD.Print("Таймер!");
private void OnAnimationFinished(StringName animName) => GD.Print($"Анимация {animName} завершена");
```

### Кастомные сигналы

```csharp
public partial class Player : CharacterBody2D
{
    // Объявление кастомного сигнала
    [Signal]
    public delegate void HealthChangedEventHandler(int currentHealth, int maxHealth);

    [Signal]
    public delegate void DiedEventHandler();

    [Signal]
    public delegate void DamageReceivedEventHandler(int amount, string source);

    private int _health = 100;

    public void TakeDamage(int amount, string source = "Unknown")
    {
        _health -= amount;

        // Отправка сигнала
        EmitSignal(SignalName.HealthChanged, _health, 100);
        EmitSignal(SignalName.DamageReceived, amount, source);

        if (_health <= 0)
            EmitSignal(SignalName.Died);
    }
}

// Подписка из другого узла
public partial class HUD : Control
{
    public override void _Ready()
    {
        var player = GetNode<Player>("../Player");
        player.HealthChanged += OnHealthChanged;
        player.Died += OnPlayerDied;
    }

    private void OnHealthChanged(int current, int max)
    {
        var bar = GetNode<ProgressBar>("HealthBar");
        bar.Value = current;
        bar.MaxValue = max;
    }

    private void OnPlayerDied()
    {
        GetNode<Label>("GameOverLabel").Visible = true;
    }
}
```

### "Call down, Signal up"

```
    Parent (Main)
    ├── Player          ← Emit signal UP (Died, HealthChanged)
    │   └── Weapon      ← Call method DOWN (player.weapon.Fire())
    └── UI/HUD          ← Listens to player signals
```

- **Вниз** (к детям): вызывай методы напрямую
- **Вверх** (к родителям/соседям): используй сигналы

---

## Мини-упражнения
1. **⭐** Подключи встроенные сигналы: Button.Pressed, Area2D.BodyEntered.
2. **⭐** Создай кастомный сигнал [Signal], emit и подпишись.
3. **⭐⭐** Реализуй "Call down, Signal up" между 3 узлами.

## Что дальше
Дальше — **ввод** (11.4).
