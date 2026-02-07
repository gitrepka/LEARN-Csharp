# Тема 11.4: Ввод (Input)

## Что ты узнаешь
- Input Map, Action-based input
- _Input vs _UnhandledInput
- Input buffering: jump buffer, coyote time

---

## Объяснение

```csharp
public partial class Player : CharacterBody2D
{
    [Export] public float Speed { get; set; } = 300f;
    [Export] public float JumpForce { get; set; } = -400f;

    public override void _PhysicsProcess(double delta)
    {
        // Направление движения через Input Map
        Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
        Velocity = new Vector2(direction.X * Speed, Velocity.Y);

        // Гравитация
        if (!IsOnFloor())
            Velocity += new Vector2(0, 980 * (float)delta);

        // Прыжок
        if (Input.IsActionJustPressed("jump") && IsOnFloor())
            Velocity = new Vector2(Velocity.X, JumpForce);

        MoveAndSlide();
    }

    // Обработка событий ввода
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("pause"))
            GetTree().Paused = !GetTree().Paused;

        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
            Shoot(GetGlobalMousePosition());
    }

    // _UnhandledInput — только если UI не обработал
    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("interact"))
            TryInteract();
    }
}
```

### Jump Buffer & Coyote Time

```csharp
// Jump buffer: нажал прыжок чуть раньше приземления → прыжок сработает
private double _jumpBufferTimer;

// Coyote time: сошёл с платформы → ещё можно прыгнуть короткое время
private double _coyoteTimer;

public override void _PhysicsProcess(double delta)
{
    if (IsOnFloor()) _coyoteTimer = 0.1; // 100ms
    else _coyoteTimer -= delta;

    if (Input.IsActionJustPressed("jump")) _jumpBufferTimer = 0.1;
    else _jumpBufferTimer -= delta;

    if (_jumpBufferTimer > 0 && _coyoteTimer > 0)
    {
        Velocity = new Vector2(Velocity.X, JumpForce);
        _jumpBufferTimer = 0;
        _coyoteTimer = 0;
    }
}
```

---

## Мини-упражнения
1. **⭐** Настрой Input Map: WASD + пробел для прыжка.
2. **⭐** Реализуй движение CharacterBody2D.
3. **⭐⭐** Jump buffer и coyote time.

## Что дальше
Дальше — **физика** (11.5).
