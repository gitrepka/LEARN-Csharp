# Тема 8.1: SOLID-принципы

## Что ты узнаешь
- 5 принципов SOLID
- Примеры нарушения и исправления каждого
- Как SOLID применяется в реальных проектах

---

## Объяснение

### ЗАЧЕМ?
Без архитектуры код превращается в "спагетти" — всё связано со всем, изменение в одном месте ломает другое. SOLID — 5 правил, которые делают код **гибким, расширяемым и тестируемым**.

---

### S — Single Responsibility Principle (Единственная ответственность)

Класс должен иметь **одну причину для изменения**.

```csharp
// ❌ ПЛОХО — класс делает ВСЁ (God Object)
class Player
{
    public void Move() { /* физика */ }
    public void Render() { /* рисование */ }
    public void SaveToFile() { /* запись файла */ }
    public void PlaySound() { /* звук */ }
    public void SendToServer() { /* сеть */ }
}

// ✅ ХОРОШО — каждый класс отвечает за своё
class PlayerMovement { public void Move() { } }
class PlayerRenderer { public void Render() { } }
class SaveManager { public void Save(PlayerData data) { } }
class AudioManager { public void Play(string sound) { } }
```

---

### O — Open/Closed Principle (Открыт для расширения, закрыт для изменения)

Добавляй новое поведение **без изменения** существующего кода.

```csharp
// ❌ ПЛОХО — при новом типе урона нужно менять метод
float CalculateDamage(string type, float amount) => type switch
{
    "physical" => amount * 0.8f,
    "fire" => amount * 1.2f,
    // Нужен "ice"? Меняем этот метод! ❌
    _ => amount
};

// ✅ ХОРОШО — расширяем через новые классы
interface IDamageCalculator
{
    float Calculate(float amount);
}

class PhysicalDamage : IDamageCalculator
{
    public float Calculate(float amount) => amount * 0.8f;
}

class FireDamage : IDamageCalculator
{
    public float Calculate(float amount) => amount * 1.2f;
}

// Новый тип? Просто ДОБАВЬ класс, ничего не меняя!
class IceDamage : IDamageCalculator
{
    public float Calculate(float amount) => amount * 0.5f;
}
```

---

### L — Liskov Substitution Principle (Подстановка Лисков)

Подкласс должен **полностью заменять** базовый класс без ошибок.

```csharp
// ❌ ПЛОХО — квадрат нарушает контракт прямоугольника
class Rectangle
{
    public virtual int Width { get; set; }
    public virtual int Height { get; set; }
    public int Area => Width * Height;
}

class Square : Rectangle
{
    public override int Width
    {
        set { base.Width = value; base.Height = value; } // ⚠️ Неожиданное поведение!
    }
}

Rectangle r = new Square();
r.Width = 5;
r.Height = 3;
// Ожидаем Area = 15, получаем Area = 9! ❌

// ✅ ХОРОШО — отдельные типы
interface IShape { int Area { get; } }
record Rectangle(int Width, int Height) : IShape
{
    public int Area => Width * Height;
}
record Square(int Side) : IShape
{
    public int Area => Side * Side;
}
```

---

### I — Interface Segregation Principle (Разделение интерфейсов)

Много маленьких интерфейсов лучше одного большого.

```csharp
// ❌ ПЛОХО — толстый интерфейс
interface IEntity
{
    void Move();
    void Attack();
    void TakeDamage(int amount);
    void Render();
    void PlaySound();
    void Save();
}

// Дерево в игре реализует IEntity, но не может Attack!
class Tree : IEntity
{
    public void Move() => throw new NotSupportedException(); // ❌
    public void Attack() => throw new NotSupportedException(); // ❌
    // ...
}

// ✅ ХОРОШО — маленькие интерфейсы
interface IMovable { void Move(); }
interface IDamageable { void TakeDamage(int amount); }
interface IAttacker { void Attack(); }
interface IRenderable { void Render(); }

class Player : IMovable, IDamageable, IAttacker, IRenderable { }
class Tree : IDamageable, IRenderable { } // Только нужные!
```

---

### D — Dependency Inversion Principle (Инверсия зависимостей)

Зависи от **абстракций**, не от конкретных классов.

```csharp
// ❌ ПЛОХО — жёсткая зависимость от конкретного класса
class GameSaver
{
    private readonly JsonFileSaver _saver = new(); // привязан к JSON!

    public void Save(GameData data) => _saver.SaveToJson(data);
}

// ✅ ХОРОШО — зависимость от интерфейса
interface ISaveService
{
    void Save(GameData data);
    GameData? Load();
}

class JsonSaveService : ISaveService { /* JSON */ }
class BinarySaveService : ISaveService { /* Binary */ }
class CloudSaveService : ISaveService { /* Cloud */ }

class GameSaver
{
    private readonly ISaveService _saver;

    public GameSaver(ISaveService saver) // внедрение зависимости!
    {
        _saver = saver;
    }

    public void Save(GameData data) => _saver.Save(data);
}

// Легко поменять реализацию:
var saver = new GameSaver(new CloudSaveService());
```

---

## Мини-упражнения
1. **⭐** Найди нарушение SOLID в 5 примерах кода, определи какой принцип нарушен.
2. **⭐⭐** Рефакторинг God Object: разбей класс на компоненты по SRP.
3. **⭐⭐** Применяй OCP: добавь новый тип врага без изменения существующего кода.

### Практика Uno Platform ⭐⭐⭐
Рефакторь "Заметки" по SOLID: разделяй хранение, UI-логику, валидацию.

### Практика Godot ⭐⭐⭐
Компонентная архитектура: `HealthComponent`, `MovementComponent`, `AttackComponent`.

---

## Что дальше
Дальше — **паттерны проектирования** (8.2-8.4).
