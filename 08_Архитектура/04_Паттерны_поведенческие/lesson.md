# Тема 8.4: Поведенческие паттерны (Behavioral Patterns)

## Что ты узнаешь
- Observer — уведомления
- Strategy — сменяемые алгоритмы
- Command — команды с undo
- State — конечный автомат
- Mediator, Template Method

---

## Объяснение

### Observer — "подпишись и узнай"

Один объект оповещает множество подписчиков о событиях (C# events — это Observer!).

```csharp
// Event Bus — глобальный Observer
class GameEvents
{
    public static GameEvents Instance { get; } = new();

    public event Action<string, int>? OnEnemyKilled;
    public event Action<int>? OnPlayerDamaged;
    public event Action<string>? OnItemCollected;

    public void EnemyKilled(string name, int xp) => OnEnemyKilled?.Invoke(name, xp);
    public void PlayerDamaged(int amount) => OnPlayerDamaged?.Invoke(amount);
    public void ItemCollected(string item) => OnItemCollected?.Invoke(item);
}

// Подписчики — ничего не знают друг о друге
// UI:
GameEvents.Instance.OnEnemyKilled += (name, xp) => UpdateXPBar(xp);
// Звук:
GameEvents.Instance.OnEnemyKilled += (name, xp) => PlaySound("kill");
// Статистика:
GameEvents.Instance.OnEnemyKilled += (name, xp) => killCount++;
```

### Strategy — сменяемый алгоритм

```csharp
interface IMovementStrategy
{
    void Move(Character character, float delta);
}

class WalkStrategy : IMovementStrategy
{
    public void Move(Character c, float delta) => c.Position += c.Direction * 5f * delta;
}

class RunStrategy : IMovementStrategy
{
    public void Move(Character c, float delta) => c.Position += c.Direction * 12f * delta;
}

class FlyStrategy : IMovementStrategy
{
    public void Move(Character c, float delta) => c.Position += c.Direction * 20f * delta;
}

class Character
{
    public Vector2 Position { get; set; }
    public Vector2 Direction { get; set; }
    public IMovementStrategy MovementStrategy { get; set; } = new WalkStrategy();

    public void Update(float delta) => MovementStrategy.Move(this, delta);
}

// Переключение в runtime:
character.MovementStrategy = new FlyStrategy(); // полетел!
```

### Command — операция как объект (с undo!)

```csharp
interface ICommand
{
    void Execute();
    void Undo();
}

class MoveCommand : ICommand
{
    private readonly Player _player;
    private readonly Vector2 _direction;
    private Vector2 _previousPosition;

    public MoveCommand(Player player, Vector2 direction)
    {
        _player = player;
        _direction = direction;
    }

    public void Execute()
    {
        _previousPosition = _player.Position;
        _player.Position += _direction;
    }

    public void Undo() => _player.Position = _previousPosition;
}

// Command Manager с undo/redo
class CommandManager
{
    private readonly Stack<ICommand> _undoStack = new();
    private readonly Stack<ICommand> _redoStack = new();

    public void Execute(ICommand command)
    {
        command.Execute();
        _undoStack.Push(command);
        _redoStack.Clear();
    }

    public void Undo()
    {
        if (_undoStack.TryPop(out var command))
        {
            command.Undo();
            _redoStack.Push(command);
        }
    }

    public void Redo()
    {
        if (_redoStack.TryPop(out var command))
        {
            command.Execute();
            _undoStack.Push(command);
        }
    }
}
```

### State — конечный автомат

```csharp
interface IPlayerState
{
    void Enter(Player player);
    void Update(Player player, float delta);
    void Exit(Player player);
}

class IdleState : IPlayerState
{
    public void Enter(Player p) => p.PlayAnimation("idle");
    public void Update(Player p, float delta)
    {
        if (p.InputDirection != Vector2.Zero)
            p.ChangeState(new RunState());
        if (p.JumpPressed)
            p.ChangeState(new JumpState());
    }
    public void Exit(Player p) { }
}

class RunState : IPlayerState
{
    public void Enter(Player p) => p.PlayAnimation("run");
    public void Update(Player p, float delta)
    {
        p.Move(p.InputDirection * p.Speed * delta);
        if (p.InputDirection == Vector2.Zero)
            p.ChangeState(new IdleState());
        if (p.JumpPressed)
            p.ChangeState(new JumpState());
    }
    public void Exit(Player p) { }
}

class Player
{
    private IPlayerState _currentState = new IdleState();

    public void ChangeState(IPlayerState newState)
    {
        _currentState.Exit(this);
        _currentState = newState;
        _currentState.Enter(this);
    }

    public void Update(float delta) => _currentState.Update(this, delta);
}
```

---

## Мини-упражнения
1. **⭐** Observer — EventAggregator с подпиской/отпиской.
2. **⭐** Strategy — 3 алгоритма сортировки через ISortStrategy.
3. **⭐⭐** State — светофор (Red → Green → Yellow → Red).
4. **⭐⭐** Command — калькулятор с undo.

### Практика Godot ⭐⭐⭐
**State Machine** — состояния персонажа (Idle, Run, Jump, Attack, Fall).

### Практика Uno Platform ⭐⭐⭐
**Command + Memento** — текстовый редактор с undo/redo.

---

## Что дальше
Дальше — **MVVM** (8.5) и **Clean Architecture** (8.6).
