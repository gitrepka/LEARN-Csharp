# Тема 11.8: 3D-разработка

## Что ты узнаешь
- CharacterBody3D, RigidBody3D
- MeshInstance3D, CSG, импорт моделей (.glb, .gltf)
- Освещение: DirectionalLight3D, OmniLight3D, SpotLight3D
- Камера: Camera3D, pivot-система
- Jolt Physics (новый движок в Godot 4.6)

---

## Объяснение

### 3D-движение

```csharp
public partial class Player3D : CharacterBody3D
{
    [Export] public float Speed { get; set; } = 5f;
    [Export] public float JumpForce { get; set; } = 4.5f;
    [Export] public float Gravity { get; set; } = 9.8f;
    [Export] public float MouseSensitivity { get; set; } = 0.002f;

    private Node3D _cameraPivot = null!;

    public override void _Ready()
    {
        _cameraPivot = GetNode<Node3D>("CameraPivot");
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion)
        {
            RotateY(-mouseMotion.Relative.X * MouseSensitivity);
            _cameraPivot.RotateX(-mouseMotion.Relative.Y * MouseSensitivity);
            _cameraPivot.Rotation = new Vector3(
                Mathf.Clamp(_cameraPivot.Rotation.X, Mathf.DegToRad(-90), Mathf.DegToRad(90)),
                _cameraPivot.Rotation.Y, _cameraPivot.Rotation.Z);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector3 velocity = Velocity;

        if (!IsOnFloor())
            velocity.Y -= Gravity * (float)delta;

        if (Input.IsActionJustPressed("jump") && IsOnFloor())
            velocity.Y = JumpForce;

        Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_back");
        Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

        velocity.X = direction.X * Speed;
        velocity.Z = direction.Z * Speed;

        Velocity = velocity;
        MoveAndSlide();
    }
}
```

### Jolt Physics (Godot 4.6)

Jolt — новый 3D-физический движок, быстрее и стабильнее GodotPhysics.

Включение: Project Settings → Physics → 3D → Physics Engine → Jolt.

---

## Мини-упражнения
1. **⭐** CharacterBody3D с WASD-движением и мышью.
2. **⭐** 3D-сцена: пол, стены, освещение.
3. **⭐⭐** Включи Jolt Physics, сравни со стандартным.

## Что дальше
Дальше — **UI в играх** (11.9).
