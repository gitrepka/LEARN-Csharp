# Тема 18.5: Health Checks и Rate Limiting

## Что ты узнаешь
- Health Checks — мониторинг здоровья приложения
- Rate Limiting — ограничение частоты запросов
- Зачем это нужно в продакшене

---

## Объяснение

### Health Checks — "жив ли сервер?"

```csharp
// Мониторинг-системы (Kubernetes, Azure) периодически
// спрашивают: "Ты ещё жив? БД доступна? Redis работает?"

builder.Services.AddHealthChecks()
    .AddSqlite("Data Source=app.db", name: "database")
    .AddCheck("custom", () =>
    {
        // Своя проверка
        bool isOk = CheckExternalService();
        return isOk
            ? HealthCheckResult.Healthy("Всё работает")
            : HealthCheckResult.Unhealthy("Сервис недоступен");
    });

app.MapHealthChecks("/health"); // GET /health → 200 OK или 503

// Подробный вывод
app.MapHealthChecks("/health/details", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description
            })
        };
        await context.Response.WriteAsJsonAsync(result);
    }
});

// GET /health/details →
// {
//   "status": "Healthy",
//   "checks": [
//     { "name": "database", "status": "Healthy" },
//     { "name": "custom", "status": "Healthy", "description": "Всё работает" }
//   ]
// }
```

### Кастомный Health Check

```csharp
public class GameServerHealthCheck : IHealthCheck
{
    private readonly HttpClient _client;

    public GameServerHealthCheck(HttpClient client)
    {
        _client = client;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken ct = default)
    {
        try
        {
            var response = await _client.GetAsync("/status", ct);

            if (response.IsSuccessStatusCode)
                return HealthCheckResult.Healthy("Игровой сервер доступен");

            return HealthCheckResult.Degraded(
                $"Сервер отвечает {response.StatusCode}");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(
                "Игровой сервер недоступен", ex);
        }
    }
}

builder.Services.AddHealthChecks()
    .AddCheck<GameServerHealthCheck>("game-server");
```

### Rate Limiting — защита от спама

```csharp
// .NET 7+: встроенный Rate Limiting

builder.Services.AddRateLimiter(options =>
{
    // Фиксированное окно: 100 запросов в минуту
    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(1);
        opt.PermitLimit = 100;
        opt.QueueLimit = 0; // не ставить в очередь
    });

    // Скользящее окно: плавнее
    options.AddSlidingWindowLimiter("sliding", opt =>
    {
        opt.Window = TimeSpan.FromMinutes(1);
        opt.SegmentsPerWindow = 6; // 6 сегментов по 10 сек
        opt.PermitLimit = 100;
    });

    // По пользователю
    options.AddPolicy("per-user", context =>
    {
        var userId = context.User?.Identity?.Name ?? context.Connection.RemoteIpAddress?.ToString() ?? "anonymous";

        return RateLimitPartition.GetFixedWindowLimiter(userId, _ =>
            new FixedWindowRateLimiterOptions
            {
                Window = TimeSpan.FromMinutes(1),
                PermitLimit = 20, // 20 запросов в минуту на пользователя
            });
    });

    // Что делать при превышении
    options.OnRejected = async (context, ct) =>
    {
        context.HttpContext.Response.StatusCode = 429; // Too Many Requests
        await context.HttpContext.Response.WriteAsync(
            "Слишком много запросов. Попробуйте позже.", ct);
    };
});

app.UseRateLimiter();

// Применение к endpoint
app.MapGet("/api/scores", () => GetScores())
    .RequireRateLimiting("per-user");

app.MapPost("/api/login", (LoginRequest req) => Login(req))
    .RequireRateLimiting("fixed");
```

### Когда что использовать

```
Health Checks:
- /health/live  — "приложение запущено?" (liveness)
- /health/ready — "приложение готово к запросам?" (readiness)
- Kubernetes использует для автоматического перезапуска

Rate Limiting:
- API endpoints — защита от DDoS и спама
- Логин — защита от brute-force
- Отправка сообщений — защита от флуда
```

---

## Мини-упражнения

1. **⭐** Добавь Health Check для SQLite БД. Проверь через `/health`.
2. **⭐⭐** Добавь Rate Limiting: 10 запросов в минуту на IP для /api/login.

## Что дальше
Дальше — **Custom Middleware** (18.6).
