# Тема 18.4: Background Services

## Что ты узнаешь
- IHostedService и BackgroundService
- PeriodicTimer — периодические задачи
- Очереди задач (Background Task Queue)
- Когда использовать

---

## Объяснение

### ЗАЧЕМ?

Некоторые задачи выполняются **в фоне**, без запроса от клиента: отправка email-ов, очистка старых данных, обновление кэша, проверка здоровья внешних сервисов.

### BackgroundService — базовый класс

```csharp
public class CleanupService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<CleanupService> _logger;

    public CleanupService(
        IServiceScopeFactory scopeFactory,
        ILogger<CleanupService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("CleanupService запущен");

        using var timer = new PeriodicTimer(TimeSpan.FromHours(1));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                // Удаляем записи старше 30 дней
                var cutoff = DateTime.UtcNow.AddDays(-30);
                var oldRecords = await db.Logs
                    .Where(l => l.CreatedAt < cutoff)
                    .ExecuteDeleteAsync(stoppingToken);

                _logger.LogInformation("Удалено {Count} старых записей", oldRecords);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, "Ошибка при очистке");
            }
        }
    }
}

// Регистрация
builder.Services.AddHostedService<CleanupService>();
```

### Очередь фоновых задач

```csharp
// Интерфейс очереди
public interface IBackgroundTaskQueue
{
    ValueTask QueueAsync(Func<CancellationToken, ValueTask> workItem);
    ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(CancellationToken ct);
}

// Реализация через Channel
public class BackgroundTaskQueue : IBackgroundTaskQueue
{
    private readonly Channel<Func<CancellationToken, ValueTask>> _queue;

    public BackgroundTaskQueue(int capacity = 100)
    {
        _queue = Channel.CreateBounded<Func<CancellationToken, ValueTask>>(capacity);
    }

    public async ValueTask QueueAsync(Func<CancellationToken, ValueTask> workItem)
    {
        await _queue.Writer.WriteAsync(workItem);
    }

    public async ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(
        CancellationToken ct)
    {
        return await _queue.Reader.ReadAsync(ct);
    }
}

// Worker — обрабатывает очередь
public class QueueWorker : BackgroundService
{
    private readonly IBackgroundTaskQueue _queue;
    private readonly ILogger<QueueWorker> _logger;

    public QueueWorker(IBackgroundTaskQueue queue, ILogger<QueueWorker> logger)
    {
        _queue = queue;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var workItem = await _queue.DequeueAsync(stoppingToken);

            try
            {
                await workItem(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка в фоновой задаче");
            }
        }
    }
}

// Регистрация
builder.Services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
builder.Services.AddHostedService<QueueWorker>();

// Использование в endpoint
app.MapPost("/api/reports", async (
    ReportRequest request,
    IBackgroundTaskQueue queue) =>
{
    // Моментальный ответ — генерация в фоне
    await queue.QueueAsync(async ct =>
    {
        // Долгая операция — генерация отчёта
        await GenerateReportAsync(request, ct);
    });

    return Results.Accepted(); // 202 — "принято, обрабатывается"
});
```

### Примеры использования

```
- Отправка email-ов в фоне (не задерживать ответ клиенту)
- Периодическое обновление кэша
- Очистка старых данных из БД
- Обновление лидерборда раз в минуту
- Проверка health checks внешних сервисов
- Обработка очереди задач (генерация отчётов)
```

---

## Мини-упражнения

1. **⭐** Создай BackgroundService, который каждые 30 секунд логирует текущее время.
2. **⭐⭐** Создай очередь задач: endpoint принимает задачу → worker обрабатывает в фоне.

## Что дальше
Дальше — **Health Checks** (18.5).
