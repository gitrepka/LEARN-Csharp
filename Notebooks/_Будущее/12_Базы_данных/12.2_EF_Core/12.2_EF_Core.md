# Тема 12.2: Entity Framework Core

## Что ты узнаешь
- DbContext, DbSet<T>
- Code-First: миграции
- CRUD-операции через LINQ
- Связи: One-to-Many, Many-to-Many

---

## Объяснение

### ЗАЧЕМ?
Писать SQL вручную — утомительно и ошибкоопасно. EF Core — **ORM** (Object-Relational Mapper): работай с классами C#, EF Core сам генерирует SQL.

```csharp
// 1. Модели (POCO)
class Player
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Level { get; set; } = 1;
    public List<Item> Items { get; set; } = [];
}

class Item
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int PlayerId { get; set; }  // FK
    public Player Player { get; set; } = null!;
}

// 2. DbContext
class GameDbContext : DbContext
{
    public DbSet<Player> Players => Set<Player>();
    public DbSet<Item> Items => Set<Item>();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=game.db");
    }
}

// 3. Использование
using var db = new GameDbContext();

// Create
db.Players.Add(new Player { Name = "Алиса", Level = 15 });
await db.SaveChangesAsync();

// Read
var topPlayers = await db.Players
    .Where(p => p.Level > 10)
    .OrderByDescending(p => p.Level)
    .Include(p => p.Items) // загрузить связанные Items
    .ToListAsync();

// Update
var player = await db.Players.FindAsync(1);
if (player != null)
{
    player.Level = 20;
    await db.SaveChangesAsync();
}

// Delete
var toDelete = await db.Players.FindAsync(5);
if (toDelete != null)
{
    db.Players.Remove(toDelete);
    await db.SaveChangesAsync();
}
```

### Миграции

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet ef migrations add AddPlayerLevel
dotnet ef database update
```

---

## Мини-упражнения
1. **⭐** Создай DbContext + модели, выполни CRUD.
2. **⭐** Создай миграцию и примени.
3. **⭐⭐** Связь One-to-Many с Include.

## Что дальше
Дальше — **Dapper** (12.3) и **SQLite** (12.4).
