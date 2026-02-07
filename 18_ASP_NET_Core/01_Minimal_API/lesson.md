# Тема 18.1: ASP.NET Core — Minimal API

## Что ты узнаешь
- Что такое ASP.NET Core и middleware pipeline
- Minimal API: эндпоинты, routing, model binding
- Validation, Swagger/OpenAPI
- Filters (endpoint filters)

---

## Объяснение

### ЗАЧЕМ?

ASP.NET Core — фреймворк для создания **бэкенда** (серверной части). Твоё Uno-приложение или Godot-игра отправляет HTTP-запросы → ASP.NET Core обрабатывает → возвращает данные.

### Middleware Pipeline

```
Запрос от клиента
    ↓
┌─────────────────┐
│   Logging        │  ← логирует каждый запрос
├─────────────────┤
│   Authentication │  ← проверяет JWT токен
├─────────────────┤
│   Authorization  │  ← проверяет права
├─────────────────┤
│   Routing        │  ← находит нужный endpoint
├─────────────────┤
│   Endpoint       │  ← выполняет обработчик
└─────────────────┘
    ↓
Ответ клиенту

Каждый middleware может:
- Обработать запрос и передать дальше
- Или вернуть ответ (не передавая дальше)
```

### Минимальное API

```csharp
// Program.cs — это ВЕСЬ файл!
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/time", () => DateTime.UtcNow);

app.Run(); // Запуск сервера на https://localhost:5001
```

### CRUD API

```csharp
var builder = WebApplication.CreateBuilder(args);

// Swagger для документации
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DI
builder.Services.AddSingleton<IPlayerRepository, InMemoryPlayerRepository>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(); // → https://localhost:5001/swagger

// Группа эндпоинтов
var players = app.MapGroup("/api/players")
    .WithTags("Players");

// GET /api/players
players.MapGet("/", (IPlayerRepository repo) =>
    Results.Ok(repo.GetAll()));

// GET /api/players/{id}
players.MapGet("/{id:int}", (int id, IPlayerRepository repo) =>
    repo.GetById(id) is Player player
        ? Results.Ok(player)
        : Results.NotFound(new { message = "Игрок не найден" }));

// POST /api/players
players.MapPost("/", (CreatePlayerRequest request, IPlayerRepository repo) =>
{
    var player = new Player(repo.NextId(), request.Name, request.Level);
    repo.Add(player);
    return Results.Created($"/api/players/{player.Id}", player);
});

// PUT /api/players/{id}
players.MapPut("/{id:int}", (int id, UpdatePlayerRequest request, IPlayerRepository repo) =>
{
    if (repo.GetById(id) is not Player existing)
        return Results.NotFound();

    var updated = existing with { Name = request.Name, Level = request.Level };
    repo.Update(updated);
    return Results.Ok(updated);
});

// DELETE /api/players/{id}
players.MapDelete("/{id:int}", (int id, IPlayerRepository repo) =>
{
    repo.Delete(id);
    return Results.NoContent();
});

app.Run();

// Модели
record Player(int Id, string Name, int Level);
record CreatePlayerRequest(string Name, int Level);
record UpdatePlayerRequest(string Name, int Level);
```

### Validation

```csharp
// dotnet add package FluentValidation
// dotnet add package FluentValidation.DependencyInjectionExtensions

public class CreatePlayerValidator : AbstractValidator<CreatePlayerRequest>
{
    public CreatePlayerValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Имя обязательно")
            .MaximumLength(50).WithMessage("Имя слишком длинное");

        RuleFor(x => x.Level)
            .InclusiveBetween(1, 100).WithMessage("Уровень от 1 до 100");
    }
}

// Регистрация
builder.Services.AddValidatorsFromAssemblyContaining<CreatePlayerValidator>();

// Использование в endpoint
players.MapPost("/", (
    CreatePlayerRequest request,
    IValidator<CreatePlayerRequest> validator,
    IPlayerRepository repo) =>
{
    var result = validator.Validate(request);
    if (!result.IsValid)
        return Results.ValidationProblem(result.ToDictionary());

    var player = new Player(repo.NextId(), request.Name, request.Level);
    repo.Add(player);
    return Results.Created($"/api/players/{player.Id}", player);
});
```

### Endpoint Filters

```csharp
// Фильтр — middleware для конкретного endpoint
public class ValidationFilter<T> : IEndpointFilter where T : class
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var validator = context.HttpContext.RequestServices
            .GetService<IValidator<T>>();

        if (validator is not null)
        {
            var argument = context.Arguments.OfType<T>().FirstOrDefault();
            if (argument is not null)
            {
                var result = await validator.ValidateAsync(argument);
                if (!result.IsValid)
                    return Results.ValidationProblem(result.ToDictionary());
            }
        }

        return await next(context);
    }
}

// Применение
players.MapPost("/", (CreatePlayerRequest request, IPlayerRepository repo) =>
{
    // Валидация уже прошла в фильтре!
    var player = new Player(repo.NextId(), request.Name, request.Level);
    repo.Add(player);
    return Results.Created($"/api/players/{player.Id}", player);
}).AddEndpointFilter<ValidationFilter<CreatePlayerRequest>>();
```

### Запуск

```bash
dotnet new web -n MyApi
cd MyApi
dotnet run

# Сервер: https://localhost:5001
# Swagger: https://localhost:5001/swagger
```

---

## Мини-упражнения

1. **⭐** Создай Minimal API с CRUD для одной сущности (например, Item).
2. **⭐** Добавь Swagger, проверь эндпоинты через браузер.
3. **⭐⭐** Добавь FluentValidation к POST/PUT эндпоинтам.

## Что дальше
Дальше — **SignalR** (18.2).
