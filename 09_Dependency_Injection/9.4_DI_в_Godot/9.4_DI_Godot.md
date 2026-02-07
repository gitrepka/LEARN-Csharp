# Тема 9.4: DI в Godot

## Что ты узнаешь
- AutoLoad как Service Locator
- Простой DI-контейнер для Godot
- Когда DI в играх, когда нет

---

## Объяснение

### ЗАЧЕМ?
Godot не имеет встроенного DI. Но принцип тот же: `PlayerController` не должен знать, что `IAudioService` — это `FMODAudioService`. Подменяешь для тестов или другой платформы.

### AutoLoad — Godot-паттерн (Service Locator)

```csharp
// Godot AutoLoad — глобально доступные ноды
// Project → Project Settings → Globals → AutoLoad

// GameManager.cs (AutoLoad)
public partial class GameManager : Node
{
    public static GameManager Instance { get; private set; } = null!;

    public override void _Ready()
    {
        Instance = this;
    }

    // Сервисы как свойства
    public IAudioService Audio { get; private set; } = null!;
    public ISaveService Save { get; private set; } = null!;

    public override void _EnterTree()
    {
        Audio = new GodotAudioService();
        Save = new JsonSaveService();
    }
}

// Использование из любого узла:
GameManager.Instance.Audio.PlaySound("explosion");
GameManager.Instance.Save.SaveGame(data);
```

### Простой DI-контейнер

```csharp
// Минималистичный контейнер для Godot
class ServiceContainer
{
    private static ServiceContainer? _instance;
    public static ServiceContainer Instance => _instance ??= new();

    private readonly Dictionary<Type, object> _singletons = new();
    private readonly Dictionary<Type, Func<object>> _factories = new();

    public void RegisterSingleton<TInterface, TImpl>()
        where TImpl : class, TInterface, new()
    {
        _singletons[typeof(TInterface)] = new TImpl();
    }

    public void RegisterSingleton<TInterface>(TInterface instance)
        where TInterface : class
    {
        _singletons[typeof(TInterface)] = instance;
    }

    public void RegisterTransient<TInterface, TImpl>()
        where TImpl : class, TInterface, new()
    {
        _factories[typeof(TInterface)] = () => new TImpl();
    }

    public T Resolve<T>() where T : class
    {
        if (_singletons.TryGetValue(typeof(T), out var singleton))
            return (T)singleton;
        if (_factories.TryGetValue(typeof(T), out var factory))
            return (T)factory();
        throw new InvalidOperationException($"Сервис {typeof(T).Name} не зарегистрирован");
    }
}

// Регистрация (в AutoLoad _Ready)
ServiceContainer.Instance.RegisterSingleton<IAudioService, GodotAudioService>();
ServiceContainer.Instance.RegisterSingleton<ISaveService, JsonSaveService>();
ServiceContainer.Instance.RegisterTransient<IEnemyFactory, EnemyFactory>();

// Использование
var audio = ServiceContainer.Instance.Resolve<IAudioService>();
```

### Когда DI в играх?

| Сценарий | DI? |
|----------|-----|
| Сервисы (аудио, сохранение, аналитика) | Да |
| Фабрики (создание врагов, предметов) | Да |
| Простые ноды (пуля, эффект) | Нет (избыток) |
| Unit-тесты | Да (мок сервисов) |

---

## Мини-упражнения
1. **⭐** Создай AutoLoad синглтон с IAudioService.
2. **⭐⭐** Реализуй простой ServiceContainer, зарегистрируй 2 сервиса.
3. **⭐⭐** Создай моковую реализацию для тестирования без звука.

---

## Контрольные вопросы перед Модулем 10
1. Чем Constructor Injection отличается от Property Injection?
2. Разница между Transient, Scoped, Singleton?
3. Зачем DI, если можно просто `new`?

## Что дальше
Модуль 9 завершён! Дальше — **Модуль 10: Uno Platform** и **Модуль 11: Godot**.
