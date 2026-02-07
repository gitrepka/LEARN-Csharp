# Тема 8.6: Clean Architecture

## Что ты узнаешь
- Clean Architecture — слои и зависимости
- Repository pattern
- Unit of Work
- CQRS (введение)
- Result Pattern углублённо

---

## Объяснение

### ЗАЧЕМ?
В маленьком проекте — всё в одном файле. Но проект растёт. Через полгода: UI зависит от БД, бизнес-логика в контроллерах, тесты невозможны. **Clean Architecture** — правила организации кода для средних и больших проектов.

### Слои (от центра наружу)

```
┌─────────────────────────────────────────┐
│  Presentation (UI)                      │  ← Uno Platform XAML, ViewModels
│  ┌─────────────────────────────────┐    │
│  │  Application (Use Cases)        │    │  ← Бизнес-сценарии
│  │  ┌─────────────────────────┐    │    │
│  │  │  Domain (Entities)      │    │    │  ← Чистые бизнес-сущности
│  │  │  Player, Order, Rules   │    │    │     Никаких зависимостей!
│  │  └─────────────────────────┘    │    │
│  └─────────────────────────────────┘    │
│  Infrastructure                         │  ← БД, файлы, API, Godot
└─────────────────────────────────────────┘

Правило зависимостей: стрелки ТОЛЬКО внутрь!
Presentation → Application → Domain ← Infrastructure
```

### Пример: финансовый трекер

```csharp
// ═══ DOMAIN (ядро — никаких зависимостей!) ═══
namespace Domain.Entities;

record Transaction(
    Guid Id,
    decimal Amount,
    string Category,
    DateTime Date,
    string Description);

// Domain interfaces (определены в Domain, реализованы в Infrastructure)
namespace Domain.Interfaces;

interface ITransactionRepository
{
    Task<Transaction?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<Transaction>> GetAllAsync();
    Task AddAsync(Transaction transaction);
    Task DeleteAsync(Guid id);
}

// ═══ APPLICATION (бизнес-сценарии) ═══
namespace Application.UseCases;

class AddTransactionUseCase
{
    private readonly ITransactionRepository _repository;

    public AddTransactionUseCase(ITransactionRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Transaction>> ExecuteAsync(
        decimal amount, string category, string description)
    {
        if (amount == 0)
            return Result<Transaction>.Failure("Сумма не может быть 0");

        if (string.IsNullOrWhiteSpace(category))
            return Result<Transaction>.Failure("Категория обязательна");

        var transaction = new Transaction(
            Guid.NewGuid(), amount, category, DateTime.UtcNow, description);

        await _repository.AddAsync(transaction);
        return Result<Transaction>.Success(transaction);
    }
}

// ═══ INFRASTRUCTURE (реализация) ═══
namespace Infrastructure.Repositories;

class JsonTransactionRepository : ITransactionRepository
{
    private readonly string _filePath;

    public async Task<IReadOnlyList<Transaction>> GetAllAsync()
    {
        if (!File.Exists(_filePath)) return [];
        string json = await File.ReadAllTextAsync(_filePath);
        return JsonSerializer.Deserialize<List<Transaction>>(json) ?? [];
    }

    public async Task AddAsync(Transaction transaction)
    {
        var all = (await GetAllAsync()).ToList();
        all.Add(transaction);
        await File.WriteAllTextAsync(_filePath,
            JsonSerializer.Serialize(all, new JsonSerializerOptions { WriteIndented = true }));
    }
    // ...
}

// ═══ PRESENTATION (UI) ═══
namespace Presentation.ViewModels;

partial class TransactionsViewModel : ObservableObject
{
    private readonly AddTransactionUseCase _addUseCase;

    [RelayCommand]
    private async Task AddTransaction()
    {
        var result = await _addUseCase.ExecuteAsync(Amount, Category, Description);
        if (result.IsSuccess)
            Transactions.Add(result.Value!);
        else
            ErrorMessage = result.Error!;
    }
}
```

### Repository Pattern

```csharp
// Абстрагирует доступ к данным
interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
}

// Реализация может быть: JSON, SQLite, API, InMemory
class InMemoryRepository<T> : IRepository<T> where T : class
{
    private readonly List<T> _items = [];
    // ...
}
```

### Result Pattern — углублённо

```csharp
readonly struct Result<T>
{
    public T? Value { get; }
    public string? Error { get; }
    public bool IsSuccess => Error is null;

    private Result(T value) { Value = value; Error = null; }
    private Result(string error) { Value = default; Error = error; }

    public static Result<T> Success(T value) => new(value);
    public static Result<T> Failure(string error) => new(error);

    // Map — трансформация успешного значения
    public Result<TNew> Map<TNew>(Func<T, TNew> func) =>
        IsSuccess ? Result<TNew>.Success(func(Value!)) : Result<TNew>.Failure(Error!);

    // Bind — цепочка операций (Railway-oriented programming)
    public Result<TNew> Bind<TNew>(Func<T, Result<TNew>> func) =>
        IsSuccess ? func(Value!) : Result<TNew>.Failure(Error!);
}

// Цепочка:
Result<string> result = ParseNumber("42")
    .Bind(n => Validate(n))
    .Map(n => $"Число: {n}");
```

---

## Мини-упражнения
1. **⭐⭐** Создай 3-слойную структуру: Domain → Application → Infrastructure.
2. **⭐⭐** Реализуй `IRepository<T>` с JSON-хранилищем.
3. **⭐⭐** Реализуй `Result<T>` с методами `Map` и `Bind`.

---

## Что дальше
Дальше — **DDD основы** (8.7).
