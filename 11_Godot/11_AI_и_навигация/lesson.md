# Тема 11.11: AI и навигация

## Что ты узнаешь
- NavigationAgent2D/3D — pathfinding
- State Machine для AI
- Steering behaviors: seek, flee, wander

---

## Объяснение

### NavigationAgent2D

```csharp
public partial class Enemy : CharacterBody2D
{
    [Export] public float Speed { get; set; } = 150f;
    private NavigationAgent2D _agent = null!;
    private Node2D _target = null!;

    public override void _Ready()
    {
        _agent = GetNode<NavigationAgent2D>("NavigationAgent2D");
        _target = GetTree().GetFirstNodeInGroup("player") as Node2D;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_target == null || _agent.IsNavigationFinished()) return;

        _agent.TargetPosition = _target.GlobalPosition;
        Vector2 nextPos = _agent.GetNextPathPosition();
        Vector2 direction = (nextPos - GlobalPosition).Normalized();
        Velocity = direction * Speed;
        MoveAndSlide();
    }
}
```

### AI State Machine

```csharp
enum AIState { Idle, Patrol, Chase, Attack, Flee }

public partial class EnemyAI : Node
{
    private AIState _state = AIState.Patrol;
    [Export] public float DetectRange { get; set; } = 200f;
    [Export] public float AttackRange { get; set; } = 50f;
    [Export] public float FleeHealthThreshold { get; set; } = 20f;

    public override void _PhysicsProcess(double delta)
    {
        float distToPlayer = GlobalPosition.DistanceTo(_player.GlobalPosition);

        _state = _state switch
        {
            AIState.Patrol when distToPlayer < DetectRange => AIState.Chase,
            AIState.Chase when distToPlayer < AttackRange => AIState.Attack,
            AIState.Chase when distToPlayer > DetectRange * 1.5f => AIState.Patrol,
            AIState.Attack when distToPlayer > AttackRange => AIState.Chase,
            _ when _health < FleeHealthThreshold => AIState.Flee,
            _ => _state
        };

        switch (_state)
        {
            case AIState.Patrol: Patrol(delta); break;
            case AIState.Chase: ChasePlayer(delta); break;
            case AIState.Attack: Attack(delta); break;
            case AIState.Flee: Flee(delta); break;
        }
    }
}
```

---

## Мини-упражнения
1. **⭐** NavigationAgent2D: враг следует за игроком.
2. **⭐⭐** AI State Machine: Patrol → Chase → Attack.

## Что дальше
Дальше — **математика для игр** (11.12).
