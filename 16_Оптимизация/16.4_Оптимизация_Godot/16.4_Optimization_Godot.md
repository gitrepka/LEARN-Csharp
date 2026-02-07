# Тема 16.3: Оптимизация Godot

## Что ты узнаешь
- Object Pooling — переиспользование объектов
- Маршалинг C# ↔ Godot — как избежать затрат
- Рендеринг: Culling, LOD, Instancing
- Физика: CollisionLayers, упрощение shapes
- Spatial partitioning

---

## Объяснение

### ЗАЧЕМ?

60 FPS = 16.6 мс на кадр. Если _Process занимает 20 мс → лаги. В играх оптимизация — не роскошь, а необходимость. Профайлер Godot → находишь узкое место → оптимизируешь.

### Object Pooling — не создавай, переиспользуй

```csharp
// ❌ Плохо — каждый выстрел = Instantiate + QueueFree
public void Shoot()
{
    var bullet = _bulletScene.Instantiate<Bullet>();
    AddChild(bullet); // дорого!
}

// В Bullet:
public void _on_lifetime_expired()
{
    QueueFree(); // дорого!
}

// ✅ Хорошо — пул объектов
public partial class BulletPool : Node
{
    [Export] private PackedScene _bulletScene = null!;
    private readonly Queue<Bullet> _pool = new();

    public override void _Ready()
    {
        // Создаём заранее
        for (int i = 0; i < 50; i++)
        {
            var bullet = _bulletScene.Instantiate<Bullet>();
            bullet.Pool = this;
            bullet.Visible = false;
            bullet.ProcessMode = ProcessModeEnum.Disabled;
            AddChild(bullet);
            _pool.Enqueue(bullet);
        }
    }

    public Bullet Get(Vector2 position, Vector2 direction)
    {
        Bullet bullet;
        if (_pool.Count > 0)
        {
            bullet = _pool.Dequeue();
        }
        else
        {
            // Пул пуст — создаём новую (расширяем пул)
            bullet = _bulletScene.Instantiate<Bullet>();
            bullet.Pool = this;
            AddChild(bullet);
        }

        bullet.GlobalPosition = position;
        bullet.Direction = direction;
        bullet.Visible = true;
        bullet.ProcessMode = ProcessModeEnum.Inherit;
        return bullet;
    }

    public void Return(Bullet bullet)
    {
        bullet.Visible = false;
        bullet.ProcessMode = ProcessModeEnum.Disabled;
        _pool.Enqueue(bullet);
    }
}
```

### Маршалинг C# ↔ Godot

```csharp
// Каждый вызов Godot API из C# = маршалинг (преобразование данных)
// Минимизируй вызовы в _Process!

// ❌ Плохо — 3 вызова маршалинга каждый кадр
public override void _Process(double delta)
{
    var pos = GlobalPosition;         // маршалинг
    pos.X += 1;
    GlobalPosition = pos;             // маршалинг
    GD.Print(GlobalPosition);         // ещё маршалинг
}

// ✅ Хорошо — минимум вызовов
private Vector2 _position;

public override void _Ready()
{
    _position = GlobalPosition;       // 1 раз
}

public override void _Process(double delta)
{
    _position.X += 100 * (float)delta;
    GlobalPosition = _position;       // 1 вызов вместо 3
}

// StringName — кэшируй!
// ❌ Каждый раз создаёт StringName из строки
Input.IsActionPressed("move_left"); // string → StringName каждый кадр

// ✅ Кэшируй StringName
private static readonly StringName MoveLeft = "move_left";
Input.IsActionPressed(MoveLeft); // без конвертации
```

### Рендеринг — что не видно, не рисуй

```csharp
// VisibilityNotifier2D — отключай невидимые объекты
public partial class Enemy : CharacterBody2D
{
    private VisibleOnScreenNotifier2D _notifier = null!;
    private bool _isOnScreen;

    public override void _Ready()
    {
        _notifier = GetNode<VisibleOnScreenNotifier2D>("Notifier");
        _notifier.ScreenEntered += () => _isOnScreen = true;
        _notifier.ScreenExited += () => _isOnScreen = false;
    }

    public override void _Process(double delta)
    {
        if (!_isOnScreen) return; // не обрабатывай невидимых!

        // логика AI, анимации и т.д.
    }
}

// Для 3D:
// - LOD (Level of Detail): далёкие объекты = меньше полигонов
// - Occlusion Culling: объекты за стенами не рендерятся
// - MultiMeshInstance3D: 10000 деревьев одним draw call
```

### Физика — не проверяй лишнее

```
Collision Layers и Masks:

Layer 1: Player
Layer 2: Enemies
Layer 3: Bullets
Layer 4: Walls
Layer 5: Pickups

Player   Mask: 2, 4, 5    (сталкивается с врагами, стенами, пикапами)
Enemies  Mask: 1, 3, 4    (с игроком, пулями, стенами)
Bullets  Mask: 2, 4       (с врагами и стенами, НЕ с игроком)
Pickups  Mask: 1           (только с игроком)

→ Пули не проверяют столкновения друг с другом!
→ Пикапы не сталкиваются с врагами!
→ Меньше проверок = быстрее!
```

```csharp
// Упрощай collision shapes
// ❌ Polygon с 100 точками
// ✅ CircleShape2D или RectangleShape2D — намного быстрее

// Отключай физику для далёких врагов
if (GlobalPosition.DistanceTo(playerPos) > 1000)
{
    // Упрощённая логика, без физики
    return;
}
```

### Spatial Partitioning — не проверяй ВСЁ со ВСЕМ

```csharp
// ❌ Проверять каждого врага с каждым — O(n²)
foreach (var enemy in enemies)
    foreach (var bullet in bullets)
        if (enemy.Overlaps(bullet)) // 1000 × 500 = 500,000 проверок!

// ✅ Пространственное разделение — Grid
public class SpatialGrid<T> where T : Node2D
{
    private readonly Dictionary<(int, int), List<T>> _cells = new();
    private readonly int _cellSize;

    public SpatialGrid(int cellSize = 64)
    {
        _cellSize = cellSize;
    }

    private (int, int) GetCell(Vector2 pos)
        => ((int)(pos.X / _cellSize), (int)(pos.Y / _cellSize));

    public void Insert(T node)
    {
        var cell = GetCell(node.GlobalPosition);
        if (!_cells.ContainsKey(cell))
            _cells[cell] = new List<T>();
        _cells[cell].Add(node);
    }

    public IEnumerable<T> GetNearby(Vector2 pos)
    {
        var (cx, cy) = GetCell(pos);
        // Проверяем только 9 соседних клеток
        for (int dx = -1; dx <= 1; dx++)
            for (int dy = -1; dy <= 1; dy++)
                if (_cells.TryGetValue((cx + dx, cy + dy), out var list))
                    foreach (var item in list)
                        yield return item;
    }

    public void Clear() => _cells.Clear();
}
```

### Общие советы

```
1. _Process vs _PhysicsProcess
   - Логику AI → в таймер (каждые 0.5 сек), не каждый кадр
   - Физику → _PhysicsProcess (фиксированный шаг)
   - Визуал → _Process

2. SetDeferred — если меняешь дерево сцен
   - AddChild/RemoveChild в _Process могут быть опасны
   - Используй CallDeferred("add_child", node)

3. Группы вместо GetNode
   - GetTree().GetNodesInGroup("enemies") быстрее,
     чем поиск по дереву

4. Signals вместо polling
   ❌ if (health <= 0) Die();  // каждый кадр
   ✅ HealthChanged += OnHealthChanged; // только когда изменилось
```

---

## Мини-упражнения

1. **⭐** Создай BulletPool на 100 пуль. Сравни FPS с Instantiate/QueueFree vs Pool.
2. **⭐⭐** Кэшируй StringName для всех Input actions.
3. **⭐⭐** Настрой CollisionLayers — враги не сталкиваются между собой.

## Что дальше
Модуль 16 завершён! Дальше — **Модуль 17: CI/CD**.
