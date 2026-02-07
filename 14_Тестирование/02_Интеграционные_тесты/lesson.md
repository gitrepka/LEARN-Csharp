# Тема 14.2: Интеграционные тесты и TDD

## Что ты узнаешь
- Интеграционные тесты vs Unit-тесты
- WebApplicationFactory для тестирования API
- TDD: Red → Green → Refactor
- Тестовые контейнеры (Testcontainers)

---

## Объяснение

### Уровни тестов

```
Unit-тесты:       Один класс/метод, моки, быстрые (мс)
Интеграционные:   Несколько компонентов вместе, реальная БД/API, медленнее (сек)
E2E (End-to-End): Всё приложение целиком, UI + бэкенд + БД, самые медленные

Пирамида тестов:
         /  E2E  \        ← мало (дорогие, хрупкие)
        / Integr. \       ← среднее количество
       /   Unit    \      ← много (дешёвые, быстрые)
```

### Интеграционный тест для API

```csharp
// dotnet add package Microsoft.AspNetCore.Mvc.Testing

public class PlayersApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public PlayersApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetPlayers_ReturnsSuccessAndList()
    {
        // Act — реальный HTTP-запрос к тестовому серверу
        var response = await _client.GetAsync("/api/players");

        // Assert
        response.EnsureSuccessStatusCode();
        var players = await response.Content.ReadFromJsonAsync<List<Player>>();
        Assert.NotNull(players);
    }

    [Fact]
    public async Task CreatePlayer_ReturnsCreated()
    {
        var newPlayer = new Player(0, "Тест", 1);

        var response = await _client.PostAsJsonAsync("/api/players", newPlayer);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task GetPlayer_InvalidId_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/api/players/99999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
```

### Кастомная фабрика (подмена БД)

```csharp
public class CustomWebFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Убираем реальную БД
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (descriptor != null) services.Remove(descriptor);

            // Добавляем In-Memory БД для тестов
            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("TestDb"));
        });
    }
}
```

### Интеграционный тест с SQLite

```csharp
public class RepositoryTests : IDisposable
{
    private readonly AppDbContext _context;

    public RepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Data Source=:memory:") // SQLite в памяти
            .Options;

        _context = new AppDbContext(options);
        _context.Database.OpenConnection();
        _context.Database.EnsureCreated();
    }

    [Fact]
    public async Task AddPlayer_CanBeRetrieved()
    {
        // Arrange
        var repo = new PlayerRepository(_context);
        var player = new Player { Name = "Алиса", Level = 5 };

        // Act
        await repo.AddAsync(player);
        var found = await repo.GetByNameAsync("Алиса");

        // Assert
        Assert.NotNull(found);
        Assert.Equal(5, found.Level);
    }

    public void Dispose() => _context.Dispose();
}
```

### TDD — Test-Driven Development

```
Цикл TDD:

1. 🔴 RED    — Напиши тест, который ПАДАЕТ (метода ещё нет!)
2. 🟢 GREEN  — Напиши МИНИМУМ кода, чтобы тест прошёл
3. 🔵 REFACTOR — Улучши код, тесты должны оставаться зелёными

Повтори.
```

**Пример TDD:**

```csharp
// Шаг 1: RED — пишем тест (класса ещё нет!)
[Fact]
public void Parse_ValidPrice_ReturnsDecimal()
{
    var parser = new PriceParser();
    Assert.Equal(19.99m, parser.Parse("$19.99"));
}

// Шаг 2: GREEN — минимальная реализация
public class PriceParser
{
    public decimal Parse(string input)
    {
        return decimal.Parse(input.TrimStart('$'));
    }
}

// Шаг 3: ещё тест (RED)
[Theory]
[InlineData("$0", 0)]
[InlineData("$1,000.50", 1000.50)]
[InlineData("  $5  ", 5)]
public void Parse_VariousFormats_ReturnsCorrect(string input, decimal expected)
{
    var parser = new PriceParser();
    Assert.Equal(expected, parser.Parse(input));
}

// Шаг 4: GREEN — дорабатываем
public decimal Parse(string input)
{
    string cleaned = input.Trim().TrimStart('$').Replace(",", "");
    return decimal.Parse(cleaned, CultureInfo.InvariantCulture);
}

// Шаг 5: тест на ошибку (RED)
[Theory]
[InlineData("")]
[InlineData("abc")]
[InlineData(null)]
public void Parse_InvalidInput_ThrowsFormatException(string? input)
{
    var parser = new PriceParser();
    Assert.Throws<FormatException>(() => parser.Parse(input!));
}
```

### Запуск тестов

```bash
# Все тесты
dotnet test

# С подробным выводом
dotnet test --verbosity normal

# Конкретный тест
dotnet test --filter "FullyQualifiedName~OrderServiceTests"

# С покрытием кода
dotnet test --collect:"XPlat Code Coverage"
```

### "Намеренно сломай" — упражнения

```csharp
// 1. Memory Leak — подпишись на событие, не отпишись
// Найди через dotMemory или VS Diagnostic Tools

// 2. Deadlock — два lock в разном порядке
// Найди через Threads окно отладчика

// 3. Race Condition — два потока пишут в List<T>
// Найди через повторный запуск теста (flaky test)
[Fact]
public void RaceCondition_Demo()
{
    var list = new List<int>();
    var tasks = Enumerable.Range(0, 1000)
        .Select(i => Task.Run(() => list.Add(i)));

    Task.WaitAll(tasks.ToArray());

    // Иногда пройдёт, иногда нет — race condition!
    Assert.Equal(1000, list.Count);
}
```

---

## Частые ошибки

| Ошибка | Правильно |
|--------|-----------|
| Тест зависит от порядка | Каждый тест — изолированный |
| Тест зависит от реального API | Мок или тестовый сервер |
| Слишком много интеграционных | Больше unit-тестов, меньше интеграционных |
| Тест проверяет реализацию | Тест проверяет **поведение** |

---

## Мини-упражнения

1. **⭐** Напиши 3 unit-теста, потом реализуй код (TDD). Метод: `Inventory.AddItem(item)` — максимум 20 слотов.
2. **⭐⭐** Интеграционный тест: SQLite in-memory, Repository, Add + GetAll.

## Что дальше
Дальше — **тестирование в Godot с GdUnit4** (14.3).
