# Тема 12.3: Dapper (Micro ORM)

## Что ты узнаешь
- Dapper vs EF Core — когда что
- Query, QueryFirst, Execute
- Параметризованные запросы

---

## Объяснение

### ЗАЧЕМ?
EF Core — мощный, но тяжёлый. Dapper — лёгкий micro-ORM: ты пишешь SQL, Dapper маппит результат на объекты. **Быстрее EF Core** для простых запросов.

```csharp
using Dapper;
using Microsoft.Data.Sqlite;

using var connection = new SqliteConnection("Data Source=game.db");

// Query — список
var players = await connection.QueryAsync<Player>(
    "SELECT * FROM Players WHERE Level > @MinLevel",
    new { MinLevel = 10 });

// QueryFirst — один объект
var player = await connection.QueryFirstOrDefaultAsync<Player>(
    "SELECT * FROM Players WHERE Id = @Id",
    new { Id = 1 });

// Execute — INSERT/UPDATE/DELETE
await connection.ExecuteAsync(
    "INSERT INTO Players (Name, Level) VALUES (@Name, @Level)",
    new { Name = "Вика", Level = 5 });

// Execute с несколькими объектами
var newPlayers = new[] {
    new { Name = "Гена", Level = 3 },
    new { Name = "Дима", Level = 7 }
};
await connection.ExecuteAsync(
    "INSERT INTO Players (Name, Level) VALUES (@Name, @Level)", newPlayers);
```

### EF Core vs Dapper

| Критерий | EF Core | Dapper |
|----------|---------|--------|
| Подход | Классы → SQL | SQL → Классы |
| Миграции | Встроенные | Нет |
| Скорость | Средняя | Быстрая |
| Сложные запросы | LINQ (иногда неоптимально) | Полный контроль SQL |
| **Когда** | CRUD, бизнес-приложения | Оптимизация, отчёты |

---

## Мини-упражнения
1. **⭐** Query и Execute через Dapper.
2. **⭐** Параметризованный запрос (защита от SQL injection).

## Что дальше
Дальше — **SQLite** (12.4).
