# Тема 7.2: Логирование (Logging)

## Что ты узнаешь
- Microsoft.Extensions.Logging: ILogger, уровни логов
- Structured logging — почему это важно
- Serilog — популярная библиотека
- GD.Print() в Godot

---

## Объяснение

### ЗАЧЕМ?
Breakpoints — для разработки. А на сервере? У пользователя? Там breakpoints не поставишь. **Логи** — записи о том, что делает программа. Когда что-то сломалось — смотришь логи.

### Microsoft.Extensions.Logging

```csharp
using Microsoft.Extensions.Logging;

// Создание логгера
using ILoggerFactory factory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();                    // в консоль
    builder.SetMinimumLevel(LogLevel.Debug); // минимальный уровень
});

ILogger logger = factory.CreateLogger<Program>();

// Уровни логов (по важности)
logger.LogTrace("Детальная трассировка");           // самый подробный
logger.LogDebug("Отладочная информация");           // для разработки
logger.LogInformation("Приложение запущено");       // нормальная работа
logger.LogWarning("Память: {Used}/{Total} MB", 450, 512); // предупреждение
logger.LogError(exception, "Ошибка загрузки данных");      // ошибка
logger.LogCritical("Приложение не может продолжать!");      // критическая
```

### Structured Logging — ключевая идея

```csharp
// ❌ Строковая конкатенация — нельзя искать/фильтровать
logger.LogInformation($"User {userId} bought {itemName} for {price}");

// ✅ Structured logging — данные отдельно от текста
logger.LogInformation("User {UserId} bought {ItemName} for {Price}",
    userId, itemName, price);

// Разница:
// Строковый: "User 42 bought Sword for 100" — просто текст
// Structured: UserId=42, ItemName="Sword", Price=100 — можно фильтровать!
// "Покажи все логи где UserId = 42" — легко!
```

### Высокопроизводительное логирование

```csharp
// LoggerMessage.Define — compile-time оптимизация
private static readonly Action<ILogger, string, int, Exception?> LogPlayerDamage =
    LoggerMessage.Define<string, int>(
        LogLevel.Information,
        new EventId(1, "PlayerDamage"),
        "Player {PlayerName} took {Damage} damage");

// Использование:
LogPlayerDamage(logger, "Алиса", 50, null);

// Или через Source Generator (.NET 6+):
[LoggerMessage(Level = LogLevel.Information, Message = "Player {PlayerName} took {Damage} damage")]
partial void LogPlayerDamage(string playerName, int damage);
```

### Serilog — расширенное логирование

```csharp
// Установка: dotnet add package Serilog.Sinks.Console
// dotnet add package Serilog.Sinks.File

using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/app-.log",
        rollingInterval: RollingInterval.Day,  // новый файл каждый день
        retainedFileCountLimit: 7)             // хранить 7 дней
    .Enrich.WithThreadId()
    .Enrich.WithMachineName()
    .CreateLogger();

Log.Information("Приложение запущено");
Log.Warning("Медленный запрос: {Duration}ms", 1500);
Log.Error(exception, "Ошибка при сохранении {FileName}", "save.json");

// Не забудь в конце:
Log.CloseAndFlush();
```

### Логирование в Godot

```csharp
// Встроенное
GD.Print("Информация");
GD.PrintErr("Ошибка!");
GD.PushWarning("Предупреждение");
GD.PushError("Ошибка");

// Условная отладка
[Conditional("DEBUG")]
void DebugLog(string message) => GD.Print($"[DEBUG] {message}");
```

---

### Что логировать?

| Уровень | Что | Пример |
|---------|-----|--------|
| Information | Ключевые события | "Пользователь вошёл", "Заказ создан" |
| Warning | Нештатные ситуации | "Кэш почти полон", "Повторная попытка" |
| Error | Ошибки, восстановимые | "Не удалось загрузить файл" |
| Critical | Фатальные ошибки | "БД недоступна", "Нет памяти" |
| Debug | Для разработки | "Значения переменных", "Вход в метод" |

---

## Мини-упражнения
1. **⭐** Настрой `ILogger`, выведи логи разных уровней.
2. **⭐** Используй structured logging: `{UserId}`, `{Action}`.
3. **⭐⭐** Настрой Serilog с выводом в файл (rolling).

---

## Что дальше
Дальше — **стиль кода и анализаторы** (7.3).
