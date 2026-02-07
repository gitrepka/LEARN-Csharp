# Тема 9.1: Концепция Dependency Injection

## Что ты узнаешь
- Что такое DI и зачем
- Constructor / Property / Method Injection
- IoC (Inversion of Control)

---

## Объяснение

### ЗАЧЕМ?
Класс `PlayerSaver` сам создаёт `JsonFileWriter`. Хочешь вместо JSON сохранять в облако? Переписывай `PlayerSaver`. С DI — просто подменяешь реализацию.

### Без DI vs с DI

```csharp
// ❌ Без DI — жёсткая зависимость
class PlayerSaver
{
    private readonly JsonFileWriter _writer = new(); // привязан к JSON!

    public void Save(Player player) => _writer.Write(player);
}
// Хочешь тесты без файлов? Невозможно!
// Хочешь облачное сохранение? Переписывай!

// ✅ С DI — зависимость от абстракции
interface ISaveWriter
{
    void Write<T>(T data);
}

class PlayerSaver
{
    private readonly ISaveWriter _writer;

    public PlayerSaver(ISaveWriter writer) // Constructor Injection
    {
        _writer = writer;
    }

    public void Save(Player player) => _writer.Write(player);
}

// Подменяем реализацию:
var jsonSaver = new PlayerSaver(new JsonFileWriter());
var cloudSaver = new PlayerSaver(new CloudWriter());
var testSaver = new PlayerSaver(new MockWriter()); // для тестов!
```

### Три вида Injection

```csharp
// 1. Constructor Injection (основной — рекомендуется!)
class Service
{
    private readonly IRepository _repo;
    public Service(IRepository repo) => _repo = repo;
}

// 2. Property Injection (опциональные зависимости)
class Service
{
    public ILogger? Logger { get; set; } // необязательная
}

// 3. Method Injection (для одноразовых)
class Service
{
    public void Process(IValidator validator) { ... }
}
```

### IoC — Inversion of Control

**Обычно**: класс сам создаёт зависимости (`new JsonWriter()`).
**С IoC**: "контейнер" создаёт и внедряет зависимости.

```csharp
// Контейнер знает: "если просят ISaveWriter, дай JsonFileWriter"
// Ты только говоришь ЧТО тебе нужно (ISaveWriter)
// Контейнер решает КАК это создать
```

---

## Мини-упражнения
1. **⭐** Рефакторь класс с `new` на Constructor Injection.
2. **⭐** Создай 2 реализации одного интерфейса, подмени.

---

## Что дальше
Дальше — **Microsoft.Extensions.DependencyInjection** (9.2).
