# Тема 8.2: Порождающие паттерны (Creational Patterns)

## Что ты узнаешь
- Singleton — единственный экземпляр
- Factory Method — создание через метод
- Abstract Factory — семейства объектов
- Builder — пошаговое создание сложных объектов
- Prototype — клонирование

---

## Объяснение

### Singleton — один экземпляр на всё приложение

```csharp
// Thread-safe Singleton (Lazy)
class AudioManager
{
    private static readonly Lazy<AudioManager> _instance = new(() => new AudioManager());
    public static AudioManager Instance => _instance.Value;

    private AudioManager() { } // приватный конструктор!

    public void PlaySound(string name) => Console.WriteLine($"🔊 {name}");
}

// Использование
AudioManager.Instance.PlaySound("explosion");

// В Godot — AutoLoad как Singleton
// В DI — services.AddSingleton<AudioManager>();
```

### Factory Method — делегирование создания

```csharp
// Абстрактный "создатель"
abstract class EnemySpawner
{
    public abstract Enemy Create();

    public Enemy SpawnAt(Vector2 position)
    {
        Enemy enemy = Create();
        enemy.Position = position;
        enemy.Initialize();
        return enemy;
    }
}

class ZombieSpawner : EnemySpawner
{
    public override Enemy Create() => new Zombie();
}

class SkeletonSpawner : EnemySpawner
{
    public override Enemy Create() => new Skeleton();
}

// Или простая фабрика через словарь:
class EnemyFactory
{
    private readonly Dictionary<string, Func<Enemy>> _creators = new()
    {
        ["zombie"] = () => new Zombie(),
        ["skeleton"] = () => new Skeleton(),
        ["boss"] = () => new Boss()
    };

    public Enemy Create(string type) =>
        _creators.TryGetValue(type, out var creator)
            ? creator()
            : throw new ArgumentException($"Неизвестный тип: {type}");
}
```

### Builder — пошаговое создание

```csharp
class CharacterBuilder
{
    private string _name = "Hero";
    private int _health = 100;
    private int _damage = 10;
    private List<string> _skills = [];

    public CharacterBuilder WithName(string name) { _name = name; return this; }
    public CharacterBuilder WithHealth(int hp) { _health = hp; return this; }
    public CharacterBuilder WithDamage(int dmg) { _damage = dmg; return this; }
    public CharacterBuilder WithSkill(string skill) { _skills.Add(skill); return this; }

    public Character Build() => new(_name, _health, _damage, _skills.ToList());
}

// Fluent API — читается как текст
Character warrior = new CharacterBuilder()
    .WithName("Конан")
    .WithHealth(200)
    .WithDamage(25)
    .WithSkill("Heavy Strike")
    .WithSkill("Shield Block")
    .Build();

// Практический пример: HTTP-запрос
var request = new HttpRequestBuilder()
    .WithUrl("https://api.example.com/data")
    .WithMethod("POST")
    .WithHeader("Authorization", "Bearer token")
    .WithBody(jsonContent)
    .WithTimeout(TimeSpan.FromSeconds(30))
    .Build();
```

### Prototype — клонирование

```csharp
record EnemyTemplate(string Name, int Health, int Damage, List<string> Abilities);

// record с with — встроенный Prototype!
EnemyTemplate baseZombie = new("Zombie", 50, 10, ["Bite"]);
EnemyTemplate fastZombie = baseZombie with { Name = "Fast Zombie", Damage = 15 };
EnemyTemplate tankZombie = baseZombie with { Name = "Tank Zombie", Health = 150 };

// ICloneable (старый подход)
class Enemy : ICloneable
{
    public string Name { get; set; } = "";
    public int Health { get; set; }

    public object Clone() => MemberwiseClone(); // поверхностная копия
}
```

---

## Мини-упражнения
1. **⭐** Thread-safe Singleton через Lazy<T>.
2. **⭐** Фабрика уведомлений: создаёт Email, SMS, Push.
3. **⭐⭐** Builder для HTTP-запроса: `.WithUrl().WithHeader().WithBody().Build()`.

---

## Что дальше
Дальше — **структурные паттерны** (8.3).
