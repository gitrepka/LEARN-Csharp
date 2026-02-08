# Тема 5.4: Конфигурация приложений

## Что ты узнаешь
- appsettings.json и IConfiguration
- Options pattern (IOptions<T>)
- Переменные окружения и аргументы командной строки
- Когда что использовать

---

## Объяснение

### ЗАЧЕМ?
Строки подключения к БД, API-ключи, параметры приложения — всё это не должно быть захардкожено в коде. **Конфигурация** — внешние настройки, которые можно менять без перекомпиляции.

### appsettings.json

```json
{
  "AppSettings": {
    "Title": "Моё приложение",
    "MaxRetries": 3,
    "Debug": true
  },
  "Database": {
    "ConnectionString": "Server=localhost;Database=mydb",
    "Timeout": 30
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

### IConfiguration — чтение конфигурации

```csharp
using Microsoft.Extensions.Configuration;

// Построение конфигурации
IConfiguration config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")}.json", optional: true)
    .AddEnvironmentVariables()      // переменные окружения
    .AddCommandLine(args)           // аргументы командной строки
    .Build();

// Чтение значений
string title = config["AppSettings:Title"]!;       // "Моё приложение"
int retries = config.GetValue<int>("AppSettings:MaxRetries"); // 3
string connStr = config.GetConnectionString("Database")!;      // shortcut для ConnectionStrings:Database
```

### Options pattern — типизированная конфигурация

```csharp
// Класс-модель для секции конфигурации
class AppSettings
{
    public string Title { get; set; } = "";
    public int MaxRetries { get; set; } = 3;
    public bool Debug { get; set; }
}

class DatabaseSettings
{
    public string ConnectionString { get; set; } = "";
    public int Timeout { get; set; } = 30;
}

// Регистрация в DI
services.Configure<AppSettings>(config.GetSection("AppSettings"));
services.Configure<DatabaseSettings>(config.GetSection("Database"));

// Использование через IOptions<T>
class MyService
{
    private readonly AppSettings _settings;

    public MyService(IOptions<AppSettings> options)
    {
        _settings = options.Value;
        Console.WriteLine(_settings.Title); // "Моё приложение"
    }
}

// IOptionsMonitor<T> — реагирует на изменения файла в runtime
// IOptionsSnapshot<T> — новое значение при каждом запросе (scoped)
```

### Переменные окружения

```csharp
// Чтение
string? home = Environment.GetEnvironmentVariable("HOME");
string? path = Environment.GetEnvironmentVariable("PATH");

// Установка (только для текущего процесса)
Environment.SetEnvironmentVariable("MY_VAR", "value");

// Системные свойства
string machine = Environment.MachineName;
string user = Environment.UserName;
string os = Environment.OSVersion.ToString();
int cpuCount = Environment.ProcessorCount;
```

### Аргументы командной строки

```csharp
// dotnet run -- --name Алиса --verbose
string[] args = Environment.GetCommandLineArgs();

// Или через конфигурацию:
// config.AddCommandLine(args);
// string name = config["name"]; // "Алиса"
```

---

### Приоритет источников конфигурации

Последний добавленный источник **перезаписывает** предыдущие:

```
1. appsettings.json           (базовые настройки)
2. appsettings.Development.json (специфичные для среды)
3. Переменные окружения        (серверные настройки)
4. Командная строка            (высший приоритет)
```

### Секреты (для разработки)

```bash
# Безопасное хранение секретов (не попадают в git!)
dotnet user-secrets init
dotnet user-secrets set "Database:Password" "my-secret-pass"
```

```csharp
// Подключение
.AddUserSecrets<Program>() // только в Development
```

---

## Мини-упражнения
1. **⭐** Создай `appsettings.json`, прочитай значения через `IConfiguration`.
2. **⭐** Создай класс настроек, привяжи через `config.GetSection().Get<T>()`.
3. **⭐** Прочитай переменную окружения.

---

## Контрольные вопросы перед Модулем 6
1. Чем `File.ReadAllText` отличается от `StreamReader`?
2. Когда JSON, когда XML?
3. Что такое `[JsonDerivedType]`?
4. Зачем нужен Options pattern?

## Что дальше
Модуль 5 завершён! Дальше — **Модуль 6: Углублённые темы** (функциональное программирование, рефлексия, многопоточность, память).
