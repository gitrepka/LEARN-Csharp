# Тема 18.6: Custom Middleware

## Что ты узнаешь
- Что такое Middleware и как он работает
- Написание своего Middleware
- Порядок Middleware в pipeline
- Практические примеры

---

## Объяснение

### ЗАЧЕМ?

Middleware — это "слои" обработки запроса. Каждый запрос проходит через цепочку middleware: логирование → аутентификация → CORS → routing → endpoint. Ты можешь добавить **свой слой**.

### Как работает Middleware

```
Запрос →  [Middleware 1] → [Middleware 2] → [Middleware 3] → Endpoint
Ответ  ←  [Middleware 1] ← [Middleware 2] ← [Middleware 3] ←

Каждый middleware может:
1. Обработать запрос
2. Вызвать next() — передать следующему
3. Обработать ответ (после next)
4. НЕ вызвать next() — прервать цепочку (short-circuit)
```

### Простой Middleware (лямбда)

```csharp
// Логирование каждого запроса
app.Use(async (context, next) =>
{
    var start = DateTime.UtcNow;
    var path = context.Request.Path;
    var method = context.Request.Method;

    await next(); // передаём следующему middleware

    var elapsed = DateTime.UtcNow - start;
    var status = context.Response.StatusCode;

    Console.WriteLine($"{method} {path} → {status} ({elapsed.TotalMilliseconds:F1}ms)");
});

// GET /api/players → 200 (3.2ms)
// POST /api/players → 201 (12.5ms)
```

### Middleware-класс (правильный способ)

```csharp
public class RequestTimingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestTimingMiddleware> _logger;

    public RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var sw = Stopwatch.StartNew();

        // Добавляем заголовок с ID запроса
        var requestId = Guid.NewGuid().ToString("N")[..8];
        context.Response.Headers["X-Request-Id"] = requestId;

        try
        {
            await _next(context); // передаём дальше
        }
        finally
        {
            sw.Stop();
            _logger.LogInformation(
                "[{RequestId}] {Method} {Path} → {Status} ({Elapsed}ms)",
                requestId,
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                sw.ElapsedMilliseconds);
        }
    }
}

// Регистрация
app.UseMiddleware<RequestTimingMiddleware>();

// Или через extension method (красивее)
public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseRequestTiming(this IApplicationBuilder app)
        => app.UseMiddleware<RequestTimingMiddleware>();
}

app.UseRequestTiming();
```

### Примеры полезных Middleware

```csharp
// 1. API Key проверка
public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private const string ApiKeyHeader = "X-Api-Key";

    public ApiKeyMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(ApiKeyHeader, out var key)
            || key != "my-secret-api-key")
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Invalid API Key");
            return; // short-circuit — не передаём дальше!
        }

        await _next(context);
    }
}

// 2. Exception Handler — ловит все необработанные исключения
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Необработанная ошибка: {Message}", ex.Message);

            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new
            {
                error = "Внутренняя ошибка сервера",
                requestId = context.TraceIdentifier
            });
        }
    }
}
```

### Порядок Middleware ВАЖЕН!

```csharp
var app = builder.Build();

// Порядок имеет значение!
app.UseMiddleware<GlobalExceptionMiddleware>(); // 1. Ловим ошибки (оборачивает всё)
app.UseHttpsRedirection();                      // 2. HTTP → HTTPS
app.UseCors("AllowMyApp");                      // 3. CORS (до auth!)
app.UseAuthentication();                        // 4. Кто ты?
app.UseAuthorization();                         // 5. Что тебе можно?
app.UseRateLimiter();                           // 6. Не слишком часто?
app.UseRequestTiming();                         // 7. Замер времени

// Endpoints (после всех middleware)
app.MapGet("/api/data", () => "Hello!");

app.Run();
```

---

## Мини-упражнения

1. **⭐** Напиши middleware, который логирует метод + путь каждого запроса.
2. **⭐⭐** Напиши GlobalExceptionMiddleware, который ловит исключения и возвращает JSON.
3. **⭐⭐** Напиши middleware для проверки API-ключа в заголовке.

## Что дальше
Модуль 18 завершён! Дальше — **Модуль 19: Продвинутый .NET**.
