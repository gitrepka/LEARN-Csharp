# Тема 19.6: Polly — устойчивость и Resilience

## Что ты узнаешь
- Зачем нужна устойчивость (resilience)
- Polly: Retry, Circuit Breaker, Timeout, Fallback
- Microsoft.Extensions.Http.Resilience (.NET 8+)
- Практические сценарии

---

## Объяснение

### ЗАЧЕМ?

Внешние сервисы **падают**. Сеть **обрывается**. БД **перегружается**. Без обработки этих ситуаций — приложение крашится. **Polly** — библиотека для автоматической обработки временных сбоев.

```
Без Polly:
  Запрос к API → Timeout → Crash → Пользователь злится

С Polly:
  Запрос к API → Timeout → Retry через 1 сек → Успех!
  Или: 3 retry → всё ещё сбой → Circuit Breaker → Fallback из кэша
```

### Retry — повторная попытка

```csharp
// dotnet add package Microsoft.Extensions.Resilience
// dotnet add package Microsoft.Extensions.Http.Resilience

using Polly;
using Polly.Retry;

// Простой retry: 3 попытки с экспоненциальной задержкой
var pipeline = new ResiliencePipelineBuilder()
    .AddRetry(new RetryStrategyOptions
    {
        MaxRetryAttempts = 3,
        Delay = TimeSpan.FromSeconds(1),
        BackoffType = DelayBackoffType.Exponential,
        // 1 сек → 2 сек → 4 сек
        OnRetry = args =>
        {
            Console.WriteLine($"Попытка {args.AttemptNumber + 1}, " +
                $"задержка {args.RetryDelay}");
            return ValueTask.CompletedTask;
        }
    })
    .Build();

// Использование
await pipeline.ExecuteAsync(async ct =>
{
    var response = await httpClient.GetAsync("https://api.example.com/data", ct);
    response.EnsureSuccessStatusCode();
});
```

### Circuit Breaker — "автомат-выключатель"

```csharp
// Если сервис постоянно падает — не нужно спамить запросами.
// Circuit Breaker "размыкает цепь" после N сбоев.

// Closed  → запросы проходят нормально
// Open    → запросы сразу отклоняются (не ждём timeout)
// HalfOpen → пробуем один запрос, если OK → Closed

var pipeline = new ResiliencePipelineBuilder()
    .AddCircuitBreaker(new CircuitBreakerStrategyOptions
    {
        FailureRatio = 0.5,           // 50% ошибок → открыть
        SamplingDuration = TimeSpan.FromSeconds(30),
        MinimumThroughput = 10,       // минимум 10 запросов для оценки
        BreakDuration = TimeSpan.FromSeconds(30), // открыт 30 сек
        OnOpened = args =>
        {
            Console.WriteLine("Circuit OPEN — запросы блокируются");
            return ValueTask.CompletedTask;
        },
        OnClosed = args =>
        {
            Console.WriteLine("Circuit CLOSED — запросы восстановлены");
            return ValueTask.CompletedTask;
        }
    })
    .Build();
```

### Timeout

```csharp
var pipeline = new ResiliencePipelineBuilder()
    .AddTimeout(TimeSpan.FromSeconds(10))
    .Build();

// Если операция не завершилась за 10 сек → TimeoutRejectedException
```

### Комбинирование стратегий

```csharp
// Полноценный resilient pipeline
var pipeline = new ResiliencePipelineBuilder()
    // 1. Общий таймаут (весь pipeline)
    .AddTimeout(TimeSpan.FromSeconds(30))

    // 2. Retry
    .AddRetry(new RetryStrategyOptions
    {
        MaxRetryAttempts = 3,
        Delay = TimeSpan.FromSeconds(1),
        BackoffType = DelayBackoffType.Exponential,
    })

    // 3. Circuit Breaker
    .AddCircuitBreaker(new CircuitBreakerStrategyOptions
    {
        FailureRatio = 0.5,
        BreakDuration = TimeSpan.FromSeconds(30),
    })

    // 4. Таймаут на каждую попытку
    .AddTimeout(TimeSpan.FromSeconds(5))
    .Build();
```

### HttpClient + Resilience (.NET 8+)

```csharp
// Самый простой способ — Microsoft.Extensions.Http.Resilience
builder.Services.AddHttpClient<ApiService>(client =>
{
    client.BaseAddress = new Uri("https://api.example.com/");
})
.AddStandardResilienceHandler();
// Автоматически добавляет: Retry + Circuit Breaker + Timeout!

// Или настроить вручную:
builder.Services.AddHttpClient<ApiService>()
    .AddResilienceHandler("custom", builder =>
    {
        builder.AddRetry(new HttpRetryStrategyOptions
        {
            MaxRetryAttempts = 3,
            BackoffType = DelayBackoffType.Exponential,
        });

        builder.AddCircuitBreaker(new HttpCircuitBreakerStrategyOptions
        {
            FailureRatio = 0.5,
            BreakDuration = TimeSpan.FromSeconds(30),
        });

        builder.AddTimeout(TimeSpan.FromSeconds(10));
    });
```

### Fallback — запасной вариант

```csharp
// Если всё упало — возвращаем кэшированные данные
var pipeline = new ResiliencePipelineBuilder<List<Player>>()
    .AddFallback(new FallbackStrategyOptions<List<Player>>
    {
        FallbackAction = args =>
        {
            // Возвращаем из кэша
            var cached = _cache.Get<List<Player>>("players");
            return Outcome.FromResultAsValueTask(cached ?? new List<Player>());
        }
    })
    .AddRetry(new RetryStrategyOptions<List<Player>>
    {
        MaxRetryAttempts = 2,
    })
    .Build();

var players = await pipeline.ExecuteAsync(async ct =>
{
    return await _apiService.GetPlayersAsync(ct);
});
```

### Практические сценарии

```
Uno Platform:
- API запросы → Retry + Timeout (сеть может быть нестабильной)
- Загрузка картинок → Retry + Fallback (placeholder)
- Синхронизация → Circuit Breaker (если сервер лёг)

Godot:
- Загрузка лидерборда → Retry + Fallback (локальный кэш)
- Мультиплеер → Timeout (не ждать бесконечно)
- Проверка обновлений → Retry с малым числом попыток

ASP.NET Core:
- Вызов внешних API → Retry + Circuit Breaker
- БД запросы → Retry (transient errors)
- Микросервисы → полный набор стратегий
```

---

## Мини-упражнения

1. **⭐** Добавь Retry (3 попытки) к HttpClient запросу.
2. **⭐⭐** Настрой полный pipeline: Retry + Circuit Breaker + Timeout.
3. **⭐⭐** Добавь `AddStandardResilienceHandler()` к HttpClient в ASP.NET Core.

## Что дальше
Модуль 19 завершён! Дальше — **Модуль 20: Итоговые проекты** (практика!).
