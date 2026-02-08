# Тема 2.1: Классы и объекты

## Что ты узнаешь
- Что такое класс и объект
- Поля, свойства, конструкторы
- required, init, primary constructors
- Модификаторы доступа

---

## Объяснение

### ЗАЧЕМ?
**Аналогия:** Класс — это чертёж дома. Объект — это конкретный дом, построенный по чертежу. Один чертёж → много домов. Один класс `Enemy` → много врагов в игре.

### Основы

```csharp
// Класс — чертёж
class Player
{
    // Поле — данные внутри объекта
    private int _health = 100;

    // Свойство — контролируемый доступ к данным
    public string Name { get; set; } = "Герой";

    // Метод — что объект умеет делать
    public void TakeDamage(int amount)
    {
        _health -= amount;
        if (_health < 0) _health = 0;
    }
}

// Объект — конкретный экземпляр
Player player1 = new Player();
player1.Name = "Алиса";

Player player2 = new Player();
player2.Name = "Боб";
```

### Свойства (Properties)

```csharp
class Character
{
    // Auto-property — компилятор сам создаёт backing field
    public string Name { get; set; }

    // С валидацией (ручное backing field)
    private int _health;
    public int Health
    {
        get => _health;
        set => _health = value < 0 ? 0 : value; // не меньше 0
    }

    // init-only (C# 9) — можно задать только при создании
    public int MaxHealth { get; init; }

    // required (C# 11) — ОБЯЗАТЕЛЬНО задать при создании
    public required string Class { get; set; }

    // Expression-bodied (только чтение)
    public bool IsAlive => Health > 0;

    // field keyword (C# 14) — доступ к auto-backing field
    public int Level
    {
        get => field;
        set => field = value > 0 ? value : 1; // минимум 1
    }
}

// Использование:
var hero = new Character
{
    Name = "Алиса",
    Class = "Маг",        // required — обязательно!
    MaxHealth = 100,      // init — задаётся тут
    Health = 100,
    Level = 5
};
// hero.MaxHealth = 200; // ❌ Ошибка! init-only нельзя менять после создания
```

### Конструкторы

```csharp
class Weapon
{
    public string Name { get; }
    public int Damage { get; }
    public int Ammo { get; private set; }

    // Конструктор по умолчанию
    public Weapon()
    {
        Name = "Кулаки";
        Damage = 1;
        Ammo = int.MaxValue;
    }

    // Параметризованный конструктор
    public Weapon(string name, int damage, int ammo)
    {
        Name = name;
        Damage = damage;
        Ammo = ammo;
    }

    // Цепочка конструкторов (this)
    public Weapon(string name, int damage) : this(name, damage, 30) { }
}

// Использование:
var fists = new Weapon();                   // "Кулаки", 1 урон
var rifle = new Weapon("Винтовка", 25, 10); // "Винтовка", 25 урон
var pistol = new Weapon("Пистолет", 15);    // ammo = 30 (по умолчанию)
```

### Primary Constructors (C# 12)

```csharp
// Параметры конструктора прямо в объявлении класса
class Enemy(string name, int health, float speed)
{
    public string Name => name;       // параметр захвачен
    public int Health { get; } = health;
    public float Speed { get; } = speed;

    // ⚠️ ВАЖНО: параметры НЕ становятся полями автоматически!
    // Они "захватываются" как в замыкании
    public void PrintInfo() => Console.WriteLine($"{name}: HP={health}");
}

var goblin = new Enemy("Гоблин", 50, 3.5f);
```

### Модификаторы доступа

| Модификатор | Доступ |
|-------------|--------|
| `public` | Отовсюду |
| `private` | Только внутри класса |
| `protected` | Внутри класса + наследники |
| `internal` | Внутри сборки (проекта) |
| `protected internal` | Наследники ИЛИ та же сборка |
| `private protected` | Наследники И та же сборка |
| `file` (C# 11) | Только в этом файле |

```csharp
file class InternalHelper  // видим ТОЛЬКО в этом .cs файле
{
    public static void Help() { }
}
```

### Статические члены

```csharp
class GameManager
{
    public static int Score { get; set; } = 0;  // один на всех
    public static void AddScore(int points) => Score += points;
}

// Вызов без создания объекта:
GameManager.AddScore(100);
Console.WriteLine(GameManager.Score); // 100
```

---

## Частые ошибки

### 1. Забыл required при создании
```csharp
class Item { public required string Name { get; set; } }
// var item = new Item(); // ❌ Ошибка: Name обязателен
var item = new Item { Name = "Меч" }; // ✅
```

### 2. Primary constructor — параметры мутабельны!
```csharp
class Bad(int health)
{
    public void Damage(int amount) { health -= amount; } // Меняет захваченный параметр
    public int Health => health; // Отражает изменения!
}
// Это может быть неожиданно. Лучше сохраняй в readonly свойство.
```

---

## Мини-упражнения
1. **⭐** Создай класс `Person` с `required` свойствами Name и Age.
2. **⭐** Создай класс с primary constructor.
3. **⭐** Создай класс с `init`-only свойством.
4. **⭐** Создай `file class Helper` — покажи что он не виден из другого файла.
5. **⭐⭐** Класс `BankAccount`: Deposit(), Withdraw() с проверками.

---

## Практика Uno Platform ⭐⭐
**"Адресная книга"** — класс Contact с required свойствами. ListView, форма добавления.

## Практика Godot ⭐⭐
**Класс Weapon** — Name, Damage, FireRate, Ammo. Shoot(), Reload(). Переключение оружий.

---

## Контрольные вопросы
1. Чем `required` отличается от обязательного параметра конструктора?
2. Чем primary constructor класса отличается от primary constructor record?
3. Когда использовать `init` vs `set` vs `private set`?
4. Объясни разницу между `internal` и `protected internal`.

## Что дальше
Дальше — **наследование** (2.2): как создавать иерархии классов.
