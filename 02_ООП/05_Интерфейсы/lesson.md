# Тема 2.5: Интерфейсы

## Что ты узнаешь
- Что такое интерфейс и зачем он нужен
- Default interface methods, static abstract members
- Generic Math (INumber<T>)
- Стандартные интерфейсы .NET

---

## Объяснение

### ЗАЧЕМ?
**Аналогия:** Интерфейс — это контракт. "Если ты IDamageable — ты ОБЯЗАН иметь метод TakeDamage()". Неважно кто ты — Player, Enemy, Barrel — если реализуешь интерфейс, с тобой можно работать одинаково.

Класс может наследовать только от ОДНОГО класса, но реализовать МНОГО интерфейсов.

### Основы

```csharp
interface IDamageable
{
    int Health { get; }
    void TakeDamage(int amount);
    bool IsAlive => Health > 0; // Default implementation (C# 8+)
}

interface IHealable
{
    void Heal(int amount);
}

// Класс реализует НЕСКОЛЬКО интерфейсов
class Player : IDamageable, IHealable
{
    public int Health { get; private set; } = 100;

    public void TakeDamage(int amount) => Health -= amount;
    public void Heal(int amount) => Health += amount;
}

class Barrel : IDamageable // Бочка — тоже может получить урон!
{
    public int Health { get; private set; } = 30;
    public void TakeDamage(int amount) => Health -= amount;
}

// Полиморфизм через интерфейс:
void DealDamage(IDamageable target, int damage)
{
    target.TakeDamage(damage);
    Console.WriteLine($"Осталось HP: {target.Health}. Жив: {target.IsAlive}");
}

DealDamage(new Player(), 30);  // работает!
DealDamage(new Barrel(), 30);  // тоже работает!
```

### Static abstract members (C# 11) + Generic Math

```csharp
// INumber<T> позволяет писать generic математику
T Add<T>(T a, T b) where T : INumber<T>
{
    return a + b;
}

Console.WriteLine(Add(5, 3));       // 8 (int)
Console.WriteLine(Add(2.5, 1.5));   // 4.0 (double)
Console.WriteLine(Add(10m, 20m));   // 30 (decimal)
```

### Явная реализация интерфейса

```csharp
interface ILogger { void Log(string msg); }
interface IFileWriter { void Log(string msg); } // тот же метод!

class Service : ILogger, IFileWriter
{
    void ILogger.Log(string msg) => Console.WriteLine($"[LOG] {msg}");
    void IFileWriter.Log(string msg) => File.AppendAllText("log.txt", msg);
}

var service = new Service();
// service.Log("test"); // ❌ Не работает! Нужен каст:
((ILogger)service).Log("test");      // [LOG] test
((IFileWriter)service).Log("test");  // пишет в файл
```

### Ключевые интерфейсы .NET

| Интерфейс | Зачем |
|-----------|-------|
| `INotifyPropertyChanged` | Привязка данных в UI (Uno!) |
| `IComparable<T>` | Сортировка объектов |
| `IEquatable<T>` | Сравнение объектов |
| `IEnumerable<T>` | foreach-перебор |
| `IDisposable` | Освобождение ресурсов (файлы, БД) |
| `IReadOnlyList<T>` | Безопасный возврат коллекции |

---

## Мини-упражнения
1. **⭐** Создай generic метод `T Add<T>(T a, T b) where T : INumber<T>`.
2. **⭐** Реализуй `IComparable<T>` для класса Student (по оценке).
3. **⭐** Реализуй интерфейс явно и неявно, покажи разницу.

## Практика Uno ⭐⭐⭐
**"Менеджер задач"** — INotifyPropertyChanged для привязки. ISortable, IFilterable.

## Практика Godot ⭐⭐⭐
**Система способностей** — IAbility (Activate, Deactivate, Cooldown), IDamageable.

## Контрольные вопросы
1. Чем интерфейс отличается от абстрактного класса?
2. Зачем нужны default interface methods?
3. Когда использовать interface vs abstract class?
4. Что такое Generic Math?

## Что дальше
Дальше — **struct, enum, record, кортежи, collection expressions** (2.6-2.10): другие типы данных.
