# Тема 5.3: XML

## Что ты узнаешь
- LINQ to XML: XDocument, XElement
- XmlSerializer
- Когда XML vs JSON

---

## Объяснение

### ЗАЧЕМ?
XML старше JSON, но всё ещё широко используется: XAML (Uno Platform!), конфигурации .NET (.csproj!), SOAP-сервисы, RSS, SVG. Понимать XML — обязательно.

### LINQ to XML — современный подход

```csharp
using System.Xml.Linq;

// Создание XML
XDocument doc = new(
    new XElement("Players",
        new XElement("Player",
            new XAttribute("id", 1),
            new XElement("Name", "Алиса"),
            new XElement("Level", 15),
            new XElement("Class", "Маг")
        ),
        new XElement("Player",
            new XAttribute("id", 2),
            new XElement("Name", "Боб"),
            new XElement("Level", 10),
            new XElement("Class", "Воин")
        )
    )
);

// Результат:
// <Players>
//   <Player id="1">
//     <Name>Алиса</Name>
//     <Level>15</Level>
//     <Class>Маг</Class>
//   </Player>
//   ...
// </Players>

// Сохранение
doc.Save("players.xml");

// Загрузка
XDocument loaded = XDocument.Load("players.xml");

// Чтение с LINQ!
var players = loaded.Descendants("Player")
    .Select(p => new
    {
        Id = (int)p.Attribute("id")!,
        Name = (string)p.Element("Name")!,
        Level = (int)p.Element("Level")!,
        Class = (string)p.Element("Class")!
    })
    .ToList();

foreach (var player in players)
    Console.WriteLine($"{player.Name} (Lvl {player.Level})");

// Фильтрация
var mages = loaded.Descendants("Player")
    .Where(p => (string)p.Element("Class")! == "Маг");

// Добавление элемента
loaded.Root!.Add(new XElement("Player",
    new XAttribute("id", 3),
    new XElement("Name", "Вика"),
    new XElement("Level", 20),
    new XElement("Class", "Лучник")
));
loaded.Save("players.xml");

// Изменение
var alice = loaded.Descendants("Player")
    .First(p => (string)p.Element("Name")! == "Алиса");
alice.Element("Level")!.Value = "16";
```

### XmlSerializer — автоматическая сериализация

```csharp
using System.Xml.Serialization;

public class PlayerData
{
    [XmlAttribute]
    public int Id { get; set; }

    public string Name { get; set; } = "";
    public int Level { get; set; }

    [XmlIgnore]
    public string TempData { get; set; } = "";

    [XmlArray("Skills")]
    [XmlArrayItem("Skill")]
    public List<string> Skills { get; set; } = [];
}

// Сериализация
XmlSerializer serializer = new(typeof(PlayerData));
using StringWriter sw = new();
serializer.Serialize(sw, new PlayerData
{
    Id = 1, Name = "Алиса", Level = 15,
    Skills = ["Fireball", "Heal"]
});
string xml = sw.ToString();

// Десериализация
using StringReader sr = new(xml);
PlayerData? player = serializer.Deserialize(sr) as PlayerData;
```

### XML vs JSON

| Критерий | JSON | XML |
|----------|------|-----|
| Размер | Компактнее | Больше (теги) |
| Читаемость | Проще | Сложнее |
| Типизация | Нет | Схемы (XSD) |
| Где используется | API, конфиги, Web | XAML, .csproj, корпоративные системы |
| C# поддержка | System.Text.Json | System.Xml.Linq |

---

## Мини-упражнения
1. **⭐** Создай XML-документ с LINQ to XML, сохрани и прочитай.
2. **⭐** Найди элементы в XML через LINQ (Where, Select).

---

## Что дальше
Дальше — **конфигурация** (5.4).
