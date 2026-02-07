# Тема 11.2: C# в Godot — специфика

## Что ты узнаешь
- [Export] — настройка из редактора
- [ExportGroup], [ExportSubgroup]
- Маршалинг: Variant, StringName, NodePath
- [GlobalClass], [Tool]
- CallDeferred, ToSignal (корутины)

---

## Объяснение

### [Export] — всё о нём

```csharp
public partial class Enemy : CharacterBody2D
{
    // Примитивы
    [Export] public float Speed { get; set; } = 100f;
    [Export] public int Health { get; set; } = 50;
    [Export] public string EnemyName { get; set; } = "Zombie";
    [Export] public bool IsBoss { get; set; }

    // Enum
    [Export] public DamageType Type { get; set; }

    // Ресурсы и сцены
    [Export] public PackedScene BulletScene { get; set; } = null!;
    [Export] public Texture2D Icon { get; set; } = null!;
    [Export] public AudioStream HitSound { get; set; } = null!;

    // Пути к узлам
    [Export] public NodePath HealthBarPath { get; set; }

    // Группировка в Inspector
    [ExportGroup("Combat")]
    [Export] public float AttackDamage { get; set; } = 10f;
    [Export] public float AttackRange { get; set; } = 50f;
    [Export] public float AttackCooldown { get; set; } = 1.0f;

    [ExportGroup("Movement")]
    [Export] public float MoveSpeed { get; set; } = 100f;
    [Export] public float Gravity { get; set; } = 980f;

    // Hints — подсказки для Inspector
    [Export(PropertyHint.Range, "0,100,1")]
    public int Level { get; set; } = 1;

    [Export(PropertyHint.File, "*.png")]
    public string TexturePath { get; set; } = "";

    [Export(PropertyHint.Enum, "Idle,Patrol,Chase,Attack")]
    public int State { get; set; }

    // Массивы и словари
    [Export] public string[] Abilities { get; set; } = [];
    [Export] public Godot.Collections.Dictionary<string, int> Stats { get; set; } = new();
}
```

### [GlobalClass] и [Tool]

```csharp
// [GlobalClass] — регистрирует как Godot-тип (видно в Create Node)
[GlobalClass]
public partial class HealthComponent : Node
{
    [Export] public int MaxHealth { get; set; } = 100;
    [Signal] public delegate void DiedEventHandler();
}

// [Tool] — скрипт работает в РЕДАКТОРЕ
[Tool]
public partial class LevelGenerator : Node2D
{
    [Export] public bool Regenerate { get; set; }
    // При изменении Regenerate в Inspector — генерируется уровень
}
```

### Маршалинг Variant / StringName

```csharp
// Godot использует Variant для универсальных данных
Variant v = 42;
Variant s = "hello";

// StringName — оптимизированные строки (для сигналов, методов)
StringName methodName = "TakeDamage";
// Сравнение StringName быстрее string

// CallDeferred — отложенный вызов (безопасно из потоков)
CallDeferred("set", "position", new Vector2(100, 200));
CallDeferred(MethodName.TakeDamage, 50);
```

### Корутины (await сигналов)

```csharp
public async void SpawnWithDelay()
{
    // Ждать 2 секунды
    await ToSignal(GetTree().CreateTimer(2.0), SceneTreeTimer.SignalName.Timeout);
    SpawnEnemy();

    // Ждать окончания анимации
    var anim = GetNode<AnimationPlayer>("AnimationPlayer");
    anim.Play("spawn");
    await ToSignal(anim, AnimationPlayer.SignalName.AnimationFinished);
    GD.Print("Анимация завершена!");
}
```

---

## Мини-упражнения
1. **⭐** Создай класс с [Export] свойствами разных типов.
2. **⭐** [ExportGroup] для группировки свойств.
3. **⭐⭐** await ToSignal для ожидания таймера.

## Что дальше
Дальше — **сигналы** (11.3).
