# Тема 11.15: Ресурсы и данные (Resources)

## Что ты узнаешь
- Resource (кастомные ресурсы): данные как ассеты
- [GlobalClass] для видимости в Inspector
- Загрузка: GD.Load, ResourceLoader
- Сохранение данных: JSON, ResourceSaver

---

## Объяснение

### Custom Resource — данные как ассет

```csharp
// Определение ресурса
[GlobalClass]
public partial class WeaponData : Resource
{
    [Export] public string Name { get; set; } = "";
    [Export] public int Damage { get; set; } = 10;
    [Export] public float AttackSpeed { get; set; } = 1.0f;
    [Export] public Texture2D? Icon { get; set; }
    [Export] public AudioStream? HitSound { get; set; }
    [Export(PropertyHint.Enum, "Melee,Ranged,Magic")]
    public int WeaponType { get; set; }
}

// Создай .tres файл в Inspector: ПКМ → New Resource → WeaponData
// Заполни поля в Inspector!

// Использование
public partial class Weapon : Node2D
{
    [Export] public WeaponData Data { get; set; } = null!;

    public void Attack()
    {
        GD.Print($"Атака {Data.Name}: {Data.Damage} урона");
    }
}
```

### Загрузка ресурсов

```csharp
// Синхронная загрузка
var weapon = GD.Load<WeaponData>("res://data/weapons/sword.tres");
var scene = GD.Load<PackedScene>("res://scenes/bullet.tscn");
var texture = GD.Load<Texture2D>("res://sprites/player.png");

// Асинхронная загрузка (для больших ресурсов)
ResourceLoader.LoadThreadedRequest("res://levels/level2.tscn");

// Проверка прогресса
Godot.Collections.Array progress = new();
var status = ResourceLoader.LoadThreadedGetStatus("res://levels/level2.tscn", progress);
if (status == ResourceLoader.ThreadLoadStatus.Loaded)
{
    var level = ResourceLoader.LoadThreadedGet("res://levels/level2.tscn") as PackedScene;
}
```

### Таблица данных через ресурсы

```csharp
// Базы данных: массив ресурсов
[GlobalClass]
public partial class EnemyDatabase : Resource
{
    [Export] public EnemyData[] Enemies { get; set; } = [];

    public EnemyData? FindByName(string name) =>
        Enemies.FirstOrDefault(e => e.Name == name);
}

// Загрузить один раз, использовать везде
var db = GD.Load<EnemyDatabase>("res://data/enemies.tres");
var zombie = db.FindByName("Zombie");
```

---

## Мини-упражнения
1. **⭐** Создай кастомный Resource (WeaponData), заполни в Inspector.
2. **⭐** [Export] ресурс в скрипт, используй данные.
3. **⭐⭐** База данных предметов через массив ресурсов.

## Что дальше
Дальше — **Viewport** (11.16).
