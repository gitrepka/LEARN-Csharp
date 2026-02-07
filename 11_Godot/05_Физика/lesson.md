# Тема 11.5: Физика (Jolt Physics в 4.6)

## Что ты узнаешь
- CharacterBody2D/3D: MoveAndSlide, Velocity
- RigidBody2D/3D: силы, импульсы
- Area2D/3D: триггеры
- CollisionLayers/Masks, RayCast

---

## Объяснение

### CharacterBody2D — управляемый персонаж

```csharp
public partial class Player : CharacterBody2D
{
    [Export] public float Speed { get; set; } = 300f;
    [Export] public float JumpForce { get; set; } = -500f;
    [Export] public float Gravity { get; set; } = 980f;

    public override void _PhysicsProcess(double delta)
    {
        float direction = Input.GetAxis("move_left", "move_right");
        Velocity = new Vector2(direction * Speed, Velocity.Y);

        if (!IsOnFloor())
            Velocity += new Vector2(0, Gravity * (float)delta);

        if (Input.IsActionJustPressed("jump") && IsOnFloor())
            Velocity = new Vector2(Velocity.X, JumpForce);

        MoveAndSlide();

        // Обработка столкновений
        for (int i = 0; i < GetSlideCollisionCount(); i++)
        {
            var collision = GetSlideCollision(i);
            if (collision.GetCollider() is Enemy enemy)
                enemy.TakeDamage(10);
        }
    }
}
```

### RigidBody2D — физический объект

```csharp
public partial class Bullet : RigidBody2D
{
    [Export] public float BulletSpeed { get; set; } = 800f;

    public override void _Ready()
    {
        // Импульс в направлении
        ApplyImpulse(Vector2.Right.Rotated(Rotation) * BulletSpeed);

        // Самоуничтожение через 3 секунды
        GetTree().CreateTimer(3.0).Timeout += QueueFree;
    }
}
```

### Area2D — триггеры

```csharp
public partial class Collectible : Area2D
{
    [Signal] public delegate void CollectedEventHandler(string itemName);
    [Export] public string ItemName { get; set; } = "Coin";

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is Player)
        {
            EmitSignal(SignalName.Collected, ItemName);
            QueueFree(); // удалить из сцены
        }
    }
}
```

### Collision Layers

```
Layer 1: Player
Layer 2: Enemies
Layer 3: Collectibles
Layer 4: Environment

Player:  Layer=1, Mask=2,3,4 (столкнуть с врагами, предметами, окружением)
Enemy:   Layer=2, Mask=1,4   (столкнуть с игроком, окружением)
Coin:    Layer=3, Mask=1     (столкнуть только с игроком)
```

### RayCast2D

```csharp
var ray = GetNode<RayCast2D>("RayCast2D");
if (ray.IsColliding())
{
    Node collider = (Node)ray.GetCollider();
    Vector2 point = ray.GetCollisionPoint();
    Vector2 normal = ray.GetCollisionNormal();
}
```

---

## Мини-упражнения
1. **⭐** CharacterBody2D с MoveAndSlide.
2. **⭐** Area2D как триггер для сбора монет.
3. **⭐⭐** RayCast для обнаружения земли/стен.

## Что дальше
Дальше — **анимации** (11.6).
