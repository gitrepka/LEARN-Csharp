# Тема 19.1: Source Generators

## Что ты узнаешь
- Что такое Source Generators и зачем
- Как они работают (compile-time кодогенерация)
- Примеры: System.Text.Json, Regex, Logging
- Создание простого генератора (обзор)

---

## Объяснение

### ЗАЧЕМ?

Source Generators генерируют код **во время компиляции**. Вместо рефлексии (медленной, в runtime) — готовый код, который компилятор уже оптимизировал.

```
Рефлексия (runtime):
  JsonSerializer.Serialize(obj)
  → во время работы анализирует тип
  → находит свойства через рефлексию
  → медленно, аллоцирует память

Source Generator (compile-time):
  JsonSerializer.Serialize(obj, MyContext.Default.Player)
  → во время компиляции сгенерировал код для Player
  → прямой доступ к свойствам
  → быстро, zero allocations
```

### System.Text.Json Source Generator

```csharp
// Обычная сериализация (рефлексия)
string json = JsonSerializer.Serialize(player); // медленнее

// Source Generator — генерирует код для конкретных типов
[JsonSerializable(typeof(Player))]
[JsonSerializable(typeof(List<Player>))]
public partial class AppJsonContext : JsonSerializerContext { }

// Использование — быстрее, без рефлексии, trim-safe
string json = JsonSerializer.Serialize(player, AppJsonContext.Default.Player);
Player? p = JsonSerializer.Deserialize(json, AppJsonContext.Default.Player);

// Для Minimal API
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0,
        AppJsonContext.Default);
});
```

### Regex Source Generator

```csharp
// ❌ Обычный Regex — компилируется в runtime
var regex = new Regex(@"\d{3}-\d{2}-\d{4}");

// ✅ Source Generator — компилируется при сборке
public partial class Validators
{
    [GeneratedRegex(@"\d{3}-\d{2}-\d{4}")]
    private static partial Regex SsnRegex();

    [GeneratedRegex(@"^[\w.+-]+@[\w-]+\.[\w.]+$", RegexOptions.IgnoreCase)]
    private static partial Regex EmailRegex();

    public static bool IsValidSsn(string input)
        => SsnRegex().IsMatch(input);

    public static bool IsValidEmail(string input)
        => EmailRegex().IsMatch(input);
}
```

### LoggerMessage Source Generator

```csharp
// ❌ Обычное логирование — boxing, string interpolation
_logger.LogInformation("Player {Name} scored {Score}", name, score);

// ✅ Source Generator — zero allocation, высокая производительность
public static partial class LogMessages
{
    [LoggerMessage(Level = LogLevel.Information,
        Message = "Player {Name} scored {Score}")]
    public static partial void PlayerScored(
        ILogger logger, string name, int score);

    [LoggerMessage(Level = LogLevel.Error,
        Message = "Failed to save player {PlayerId}")]
    public static partial void SaveFailed(
        ILogger logger, int playerId, Exception ex);
}

// Использование
LogMessages.PlayerScored(_logger, "Алиса", 100);
LogMessages.SaveFailed(_logger, 42, exception);
```

### Как устроен Source Generator (обзор)

```csharp
// Source Generator — это обычный C# проект, который подключается
// как analyzer к твоему проекту

// 1. Анализирует код во время компиляции
// 2. Генерирует новые .cs файлы
// 3. Эти файлы компилируются вместе с твоим кодом

// Простой пример (структура):
[Generator]
public class HelloGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(ctx =>
        {
            ctx.AddSource("HelloGenerated.g.cs", """
                namespace Generated;

                public static class Hello
                {
                    public static string Greet(string name) => $"Hello, {name}!";
                }
                """);
        });
    }
}

// Теперь в твоём коде доступен:
// Generated.Hello.Greet("World") → "Hello, World!"
```

### Где используются Source Generators

```
В .NET:
- System.Text.Json          → сериализация без рефлексии
- Regex                     → предкомпилированные regex
- LoggerMessage             → высокопроизводительное логирование
- CommunityToolkit.Mvvm     → [ObservableProperty], [RelayCommand]
- Mapperly                  → маппинг объектов (замена AutoMapper)

В Godot:
- Godot.NET.Sdk             → генерация для [Export], [Signal], [GlobalClass]

Зачем:
✅ Быстрее (нет рефлексии в runtime)
✅ AOT-совместимость (NativeAOT, iOS)
✅ Меньше аллокаций
✅ Ошибки видны при компиляции, а не в runtime
```

---

## Мини-упражнения

1. **⭐** Используй `[JsonSerializable]` для своих типов. Сравни производительность с обычной сериализацией через BenchmarkDotNet.
2. **⭐** Используй `[GeneratedRegex]` для 3 разных паттернов.
3. **⭐⭐** Используй `[LoggerMessage]` вместо обычного _logger.Log.

## Что дальше
Дальше — **NativeAOT** (19.2).
