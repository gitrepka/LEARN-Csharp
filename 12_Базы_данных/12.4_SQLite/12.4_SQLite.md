# Тема 12.4: SQLite — встраиваемая БД

## Что ты узнаешь
- SQLite: один файл = вся БД
- Microsoft.Data.Sqlite
- Когда SQLite, когда PostgreSQL/SQL Server
- SQLite в Uno Platform и Godot

---

## Объяснение

### ЗАЧЕМ?
SQLite — самая распространённая БД в мире. Один файл `.db` — вся база. Не нужен сервер. Идеально для: мобильных приложений, игр, настольных приложений, прототипов.

```csharp
using Microsoft.Data.Sqlite;

// Создание/подключение (файл создаётся автоматически)
using var connection = new SqliteConnection("Data Source=myapp.db");
connection.Open();

// Создание таблицы
var createCmd = connection.CreateCommand();
createCmd.CommandText = """
    CREATE TABLE IF NOT EXISTS Settings (
        Key TEXT PRIMARY KEY,
        Value TEXT NOT NULL
    )
    """;
createCmd.ExecuteNonQuery();

// Вставка
var insertCmd = connection.CreateCommand();
insertCmd.CommandText = "INSERT OR REPLACE INTO Settings (Key, Value) VALUES (@key, @value)";
insertCmd.Parameters.AddWithValue("@key", "theme");
insertCmd.Parameters.AddWithValue("@value", "dark");
insertCmd.ExecuteNonQuery();

// Чтение
var selectCmd = connection.CreateCommand();
selectCmd.CommandText = "SELECT Value FROM Settings WHERE Key = @key";
selectCmd.Parameters.AddWithValue("@key", "theme");
string? value = selectCmd.ExecuteScalar() as string;
```

### Когда SQLite vs Server DB

| SQLite | PostgreSQL / SQL Server |
|--------|------------------------|
| Один пользователь | Много пользователей |
| Мобильные / Desktop | Веб-серверы |
| < 1GB данных | Большие данные |
| Простые запросы | Сложная аналитика |
| Не нужен сервер | Нужна надёжность |

### SQLite в Uno Platform

```csharp
// Путь к файлу в данных приложения
string dbPath = Path.Combine(
    Windows.Storage.ApplicationData.Current.LocalFolder.Path,
    "app.db");

var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseSqlite($"Data Source={dbPath}")
    .Options;
```

### SQLite в Godot

```csharp
// Путь к файлу save
string dbPath = $"user://saves/game.db";
string fullPath = ProjectSettings.GlobalizePath(dbPath);
```

---

## Мини-упражнения
1. **⭐** Создай SQLite БД, таблицу, вставь и прочитай данные.
2. **⭐⭐** Оберни в Repository<T> для чистого кода.

## Что дальше
Модуль 12 завершён! Дальше — **Модуль 13: Сеть**.
