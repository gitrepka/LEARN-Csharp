# Тема 9.2: Microsoft.Extensions.DependencyInjection

## Что ты узнаешь
- ServiceCollection и ServiceProvider
- Transient, Scoped, Singleton — время жизни сервисов
- Регистрация и разрешение зависимостей

---

## Объяснение

### ЗАЧЕМ?
Ручной DI (передавать всё в конструкторах) быстро становится неуправляемым. DI-контейнер **автоматически** создаёт объекты и внедряет зависимости.

### Основы

```csharp
using Microsoft.Extensions.DependencyInjection;

// 1. Создаём контейнер и регистрируем сервисы
ServiceCollection services = new();

services.AddTransient<IEmailSender, SmtpEmailSender>();  // каждый раз новый
services.AddScoped<IOrderService, OrderService>();       // один на запрос
services.AddSingleton<ILogger, FileLogger>();             // один на всё приложение

// 2. Строим провайдер
ServiceProvider provider = services.BuildServiceProvider();

// 3. Запрашиваем сервис
IEmailSender sender = provider.GetRequiredService<IEmailSender>();
ILogger logger = provider.GetRequiredService<ILogger>();
```

### Время жизни (Lifetime)

```csharp
// Transient — НОВЫЙ экземпляр каждый раз
services.AddTransient<IValidator, Validator>();
// GetService<IValidator>() — каждый вызов создаёт новый объект

// Scoped — ОДИН экземпляр на "область" (scope)
services.AddScoped<IDbContext, AppDbContext>();
// В ASP.NET: один на HTTP-запрос
// В Uno: один на навигационную область

// Singleton — ОДИН экземпляр на всё приложение
services.AddSingleton<ICacheService, MemoryCacheService>();
// Создаётся один раз, живёт до выхода из приложения
```

### Автоматическое внедрение

```csharp
// DI-контейнер автоматически резолвит зависимости!
class OrderService
{
    private readonly IRepository<Order> _repo;
    private readonly ILogger _logger;
    private readonly IEmailSender _sender;

    // Контейнер сам создаст все три зависимости!
    public OrderService(
        IRepository<Order> repo,
        ILogger logger,
        IEmailSender sender)
    {
        _repo = repo;
        _logger = logger;
        _sender = sender;
    }
}

// Регистрация
services.AddTransient<IRepository<Order>, OrderRepository>();
services.AddSingleton<ILogger, FileLogger>();
services.AddTransient<IEmailSender, SmtpEmailSender>();
services.AddTransient<OrderService>(); // контейнер создаст с зависимостями

// Запрос
var service = provider.GetRequiredService<OrderService>();
// OrderService получит все три зависимости автоматически!
```

### Регистрация нескольких реализаций

```csharp
// Несколько реализаций одного интерфейса
services.AddTransient<INotificationSender, EmailSender>();
services.AddTransient<INotificationSender, SmsSender>();
services.AddTransient<INotificationSender, PushSender>();

// Получить все:
IEnumerable<INotificationSender> senders = provider.GetServices<INotificationSender>();
foreach (var sender in senders)
    sender.Send("Привет!");
```

### Keyed services (.NET 8+)

```csharp
services.AddKeyedSingleton<ISaveWriter, JsonWriter>("json");
services.AddKeyedSingleton<ISaveWriter, BinaryWriter>("binary");

class GameSaver([FromKeyedServices("json")] ISaveWriter writer)
{
    // получит JsonWriter
}
```

---

## Мини-упражнения
1. **⭐** Зарегистрируй Transient, Scoped, Singleton — покажи разницу.
2. **⭐** Зарегистрируй интерфейс + реализацию, внедри через конструктор.
3. **⭐⭐** Создай цепочку зависимостей (A → B → C), контейнер разрешит автоматически.

---

## Что дальше
Дальше — **DI в Uno Platform** (9.3) и **DI в Godot** (9.4).
