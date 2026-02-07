# Тема 5.2: JSON-сериализация (System.Text.Json)

## Что ты узнаешь
- JsonSerializer.Serialize / Deserialize
- Атрибуты: [JsonPropertyName], [JsonIgnore], [JsonInclude]
- Полиморфная сериализация: [JsonDerivedType]
- Source generators: [JsonSerializable]
- Utf8JsonReader / Utf8JsonWriter (высокопроизводительные)

---

## Объяснение

### ЗАЧЕМ?
JSON — главный формат обмена данными. API возвращает JSON. Конфигурация — JSON. Сохранение игры — JSON. Сериализация — превращение объекта C# в текст (и обратно).

### Базовая сериализация/десериализация

```csharp
using System.Text.Json;

record Player(string Name, int Level, float Health);

// Сериализация (объект → JSON)
Player player = new("Алиса", 15, 95.5f);
string json = JsonSerializer.Serialize(player);
// {"Name":"Алиса","Level":15,"Health":95.5}

// Красивый JSON (с отступами)
string pretty = JsonSerializer.Serialize(player, new JsonSerializerOptions
{
    WriteIndented = true
});
// {
//   "Name": "Алиса",
//   "Level": 15,
//   "Health": 95.5
// }

// Десериализация (JSON → объект)
Player? loaded = JsonSerializer.Deserialize<Player>(json);
Console.WriteLine(loaded?.Name); // "Алиса"

// Списки
List<Player> team = [new("Алиса", 15, 95.5f), new("Боб", 10, 80f)];
string teamJson = JsonSerializer.Serialize(team);
List<Player>? loadedTeam = JsonSerializer.Deserialize<List<Player>>(teamJson);
```

### JsonSerializerOptions — настройки

```csharp
JsonSerializerOptions options = new()
{
    WriteIndented = true,                              // красивый формат
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // Name → name
    PropertyNameCaseInsensitive = true,                // при чтении: Name == name
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, // пропускать null
    NumberHandling = JsonNumberHandling.AllowReadingFromString,   // "42" → 42
    Converters = { new JsonStringEnumConverter() }     // enum как строки
};

string json = JsonSerializer.Serialize(player, options);

// .NET 10: предустановленные настройки для Web
JsonSerializerOptions webOptions = JsonSerializerOptions.Web;
// camelCase + case insensitive + reading numbers from strings
```

### Атрибуты для управления сериализацией

```csharp
class GameSave
{
    [JsonPropertyName("player_name")] // JSON-имя поля
    public string Name { get; set; } = "";

    public int Level { get; set; }

    [JsonIgnore] // НЕ сериализовать
    public string TempData { get; set; } = "";

    [JsonInclude] // Сериализовать приватное поле (с public сеттер — не нужен)
    internal int InternalScore { get; set; }

    [JsonRequired] // Обязательное поле (.NET 7+)
    public int Score { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public DamageType Type { get; set; }
}
```

### Полиморфная сериализация (.NET 7+)

```csharp
[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(Circle), "circle")]
[JsonDerivedType(typeof(Rectangle), "rectangle")]
abstract class Shape
{
    public string Color { get; set; } = "Red";
}

class Circle : Shape
{
    public double Radius { get; set; }
}

class Rectangle : Shape
{
    public double Width { get; set; }
    public double Height { get; set; }
}

// Сериализация
Shape shape = new Circle { Color = "Blue", Radius = 5 };
string json = JsonSerializer.Serialize(shape);
// {"$type":"circle","Color":"Blue","Radius":5}

// Десериализация — правильно определит тип!
Shape? loaded = JsonSerializer.Deserialize<Shape>(json);
if (loaded is Circle circle)
    Console.WriteLine(circle.Radius); // 5
```

### Source Generators — compile-time сериализация

Быстрее, не использует рефлексию. Работает с NativeAOT.

```csharp
[JsonSerializable(typeof(Player))]
[JsonSerializable(typeof(List<Player>))]
partial class GameJsonContext : JsonSerializerContext
{
}

// Использование
string json = JsonSerializer.Serialize(player, GameJsonContext.Default.Player);
Player? loaded = JsonSerializer.Deserialize(json, GameJsonContext.Default.Player);
```

### Высокопроизводительный API

```csharp
// Utf8JsonWriter — запись JSON без промежуточных объектов
using MemoryStream ms = new();
using Utf8JsonWriter writer = new(ms, new JsonWriterOptions { Indented = true });

writer.WriteStartObject();
writer.WriteString("name", "Алиса");
writer.WriteNumber("level", 15);
writer.WriteStartArray("skills");
writer.WriteStringValue("Fireball");
writer.WriteStringValue("Heal");
writer.WriteEndArray();
writer.WriteEndObject();
writer.Flush();

string result = System.Text.Encoding.UTF8.GetString(ms.ToArray());
```

---

## Практический пример: система сохранения

```csharp
record SaveData(
    string PlayerName,
    int Level,
    float Health,
    List<string> Inventory,
    Dictionary<string, int> Stats,
    DateTime SaveTime
);

class SaveManager
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public static async Task SaveAsync(SaveData data, string path)
    {
        string json = JsonSerializer.Serialize(data, Options);
        await File.WriteAllTextAsync(path, json);
    }

    public static async Task<SaveData?> LoadAsync(string path)
    {
        if (!File.Exists(path)) return null;
        string json = await File.ReadAllTextAsync(path);
        return JsonSerializer.Deserialize<SaveData>(json, Options);
    }
}
```

---

## Мини-упражнения
1. **⭐** Сериализуй `List<Person>` в JSON, десериализуй обратно.
2. **⭐** Используй `[JsonDerivedType]` для полиморфной сериализации Shape → Circle / Rectangle.
3. **⭐⭐** Создай систему сохранения с JsonSerializer и async файловыми операциями.

### Практика Uno Platform ⭐⭐⭐
**"Заметки" с JSON-сохранением** — список заметок → JSON файл. Экспорт/импорт. Настройки приложения в JSON.

### Практика Godot ⭐⭐⭐
**Система save/load** — JSON с полиморфными данными. Несколько слотов. Автосохранение. Обработка повреждённых файлов.

---

## Что дальше
Дальше — **XML** (5.3) и **конфигурация** (5.4).
