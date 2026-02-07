# Тема 11.1: Архитектура движка Godot

## Что ты узнаешь
- Сцены и узлы (Nodes), дерево сцен
- Жизненный цикл: _Ready, _Process, _PhysicsProcess
- Scene composition vs inheritance
- GetNode<T>, группы узлов

---

## Объяснение

### Всё — это Node (узел)

```
SceneTree (дерево сцен)
└── Root
    └── Main (Node2D)
        ├── Player (CharacterBody2D)
        │   ├── Sprite2D
        │   ├── CollisionShape2D
        │   └── Camera2D
        ├── Enemies (Node2D)
        │   ├── Zombie (CharacterBody2D)
        │   └── Skeleton (CharacterBody2D)
        └── UI (CanvasLayer)
            └── HUD (Control)
```

### Жизненный цикл

```csharp
public partial class Player : CharacterBody2D
{
    // Вызывается ОДИН раз при добавлении в дерево
    public override void _Ready()
    {
        GD.Print("Player готов!");
    }

    // Вызывается КАЖДЫЙ кадр (~60 раз в секунду)
    public override void _Process(double delta)
    {
        // delta — время между кадрами (0.016 при 60 FPS)
        // Используй для визуальных обновлений, UI
    }

    // Вызывается с ФИКСИРОВАННЫМ интервалом (по умолчанию 60 раз/сек)
    public override void _PhysicsProcess(double delta)
    {
        // Используй для физики, движения
        Velocity = new Vector2(100, 0);
        MoveAndSlide();
    }

    // При входе/выходе из дерева
    public override void _EnterTree() { }
    public override void _ExitTree() { }
}
```

### Получение узлов

```csharp
// По пути
Sprite2D sprite = GetNode<Sprite2D>("Sprite2D");
Label healthLabel = GetNode<Label>("../UI/HUD/HealthLabel");

// Безопасно (без исключения)
Sprite2D? sprite2 = GetNodeOrNull<Sprite2D>("Sprite2D");

// Через [Export] — привязка из редактора
[Export] public NodePath HealthBarPath { get; set; }
private ProgressBar _healthBar = null!;

public override void _Ready()
{
    _healthBar = GetNode<ProgressBar>(HealthBarPath);
}

// Группы
AddToGroup("enemies");
var enemies = GetTree().GetNodesInGroup("enemies");
GetTree().CallGroup("enemies", "TakeDamage", 10);
```

### Composition vs Inheritance

```csharp
// ❌ Глубокое наследование
// Entity → Character → Enemy → FlyingEnemy → BossEnemy → FinalBoss

// ✅ Composition — компоненты (дочерние узлы)
// Enemy (CharacterBody2D)
//   ├── HealthComponent (Node)
//   ├── MovementComponent (Node)
//   ├── AIComponent (Node)
//   └── DamageComponent (Node)
```

---

## Мини-упражнения
1. **⭐** Создай сцену с 3 узлами, выведи их имена в _Ready.
2. **⭐** GetNode<T> для доступа к дочерним узлам.
3. **⭐** Добавь узлы в группу, вызови метод группы.

## Что дальше
Дальше — **C# в Godot** (11.2).
