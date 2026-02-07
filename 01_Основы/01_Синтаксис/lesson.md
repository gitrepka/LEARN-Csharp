# Тема 1.1: Синтаксис и структура программы

## Что ты узнаешь
- Как устроена C# программа — что запускается первым
- Что такое `namespace` и зачем он нужен
- Как подключать библиотеки через `using`
- Как писать комментарии
- Условная компиляция (#if DEBUG)

---

## Объяснение

### ЗАЧЕМ: Почему это важно?
Прежде чем строить дом, нужно понять как устроен фундамент. Синтаксис — это алфавит и грамматика языка. Без него ты не напишешь ни строчки.

### КАК: Структура программы

**Самая простая программа на C#:**
```csharp
// Это файл Program.cs
Console.WriteLine("Привет, мир!");
```
Да, одна строка. Это называется **top-level statements** (C# 9+). Компилятор сам оборачивает это в класс и метод Main.

**Классическая структура (до C# 9):**
```csharp
namespace MyApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Привет, мир!");
        }
    }
}
```

### Пространства имён (namespace)

**Аналогия:** namespace — это фамилия. Иванов Сергей и Петров Сергей — разные люди, хотя имя одинаковое. Так и `MyGame.Player` и `MyApp.Player` — разные классы.

**Классический стиль:**
```csharp
namespace MyGame
{
    class Player
    {
        // весь код внутри фигурных скобок
    }
}
```

**File-scoped namespace (C# 10) — современный стиль:**
```csharp
namespace MyGame;  // точка с запятой вместо скобок

class Player
{
    // код без лишнего отступа!
}
```
Разница: file-scoped экономит один уровень отступа. Один файл = один namespace. Используй его всегда.

### Директива using

**Аналогия:** `using` — это как "добавить в контакты". Вместо полного номера +7-999-123-45-67 ты просто звонишь "Маме".

```csharp
// Без using — нужно писать полный путь:
System.Console.WriteLine("Привет");
System.Collections.Generic.List<int> numbers = new();

// С using — коротко:
using System;
using System.Collections.Generic;

Console.WriteLine("Привет");
List<int> numbers = new();
```

**Global using (C# 10) — подключает для ВСЕХ файлов проекта:**
```csharp
// Файл GlobalUsings.cs (создай отдельный файл)
global using System;
global using System.Linq;
global using System.Collections.Generic;
```
Теперь System.Linq доступен везде без повторных using.

**Implicit usings (.NET 6+):**
Если в .csproj стоит `<ImplicitUsings>enable</ImplicitUsings>`, то .NET автоматически подключает:
- System
- System.Collections.Generic
- System.Linq
- System.Threading.Tasks
- и другие базовые

**Алиасы using (C# 12 — можно для ЛЮБЫХ типов):**
```csharp
using Point = (int X, int Y);           // алиас для кортежа
using StringList = System.Collections.Generic.List<string>;

Point p = (10, 20);
StringList names = ["Алиса", "Боб"];
```

### Комментарии

```csharp
// Однострочный комментарий

/*
   Многострочный
   комментарий
*/

/// <summary>
/// XML-документация — появляется в подсказках IDE!
/// </summary>
/// <param name="name">Имя пользователя</param>
/// <returns>Приветствие</returns>
public string Greet(string name)
{
    return $"Привет, {name}!";
}
```

### Регионы

```csharp
#region Методы атаки
public void Slash() { }
public void Stab() { }
#endregion
```
Позволяют сворачивать блоки кода в IDE. Не злоупотребляй — если класс настолько большой, что нужны регионы, возможно его стоит разбить.

### Условная компиляция

```csharp
#if DEBUG
    Console.WriteLine("Это видно только в Debug-сборке");
#else
    Console.WriteLine("Это видно только в Release-сборке");
#endif

#define MY_FEATURE
#if MY_FEATURE
    // Этот код компилируется только если MY_FEATURE определён
#endif
```

Полезно для: отладочного вывода, платформо-специфичного кода (Uno), фичей в разработке.

---

## Примеры кода

### Пример 1: Минимальная программа (top-level)
```csharp
// Program.cs — файл с точкой входа
Console.WriteLine("Я учу C#!");
Console.WriteLine($"Сегодня: {DateTime.Now:d}");
```

### Пример 2: File-scoped namespace + using
```csharp
// Файл: Models/Player.cs
namespace MyGame.Models;

public class Player
{
    public string Name { get; set; } = "Герой";
    public int Health { get; set; } = 100;
}
```

### Пример 3: Global using в отдельном файле
```csharp
// Файл: GlobalUsings.cs
global using System.Text;
global using MyGame.Models;
```

```csharp
// Файл: Program.cs — Player доступен без using!
var player = new Player();
Console.WriteLine(player.Name); // "Герой"
```

### Пример 4: Using-алиас для сложного типа
```csharp
using PlayerStats = System.Collections.Generic.Dictionary<string, int>;

PlayerStats stats = new()
{
    ["Сила"] = 10,
    ["Ловкость"] = 8,
    ["Интеллект"] = 12
};
```

### Пример 5: Условная компиляция для платформ (Uno)
```csharp
#if __ANDROID__
    Console.WriteLine("Запущено на Android");
#elif __IOS__
    Console.WriteLine("Запущено на iOS");
#elif WINDOWS
    Console.WriteLine("Запущено на Windows");
#else
    Console.WriteLine("Другая платформа");
#endif
```

---

## Частые ошибки

### 1. Забыл using
```csharp
// ❌ Ошибка: "List<> not found"
List<int> numbers = new();

// ✅ Решение: добавь using (или включи ImplicitUsings)
using System.Collections.Generic;
List<int> numbers = new();
```

### 2. Два namespace в file-scoped
```csharp
// ❌ Нельзя — file-scoped допускает только один
namespace A;
namespace B;  // Ошибка!

// ✅ Либо один file-scoped, либо классические вложенные
namespace A;
// Всё в файле принадлежит A
```

### 3. Top-level statements в нескольких файлах
```csharp
// ❌ Top-level statements можно только в ОДНОМ файле (Program.cs)
// Если создашь второй файл с "голым" кодом — ошибка компиляции
```

---

## Мини-упражнения

1. **⭐ Создай программу с file-scoped namespace.** Создай файл `Greeting.cs` с классом `Greeter` и методом `SayHello()`. Вызови из Program.cs.

2. **⭐ Создай global using.** Создай файл `GlobalUsings.cs`, добавь `global using System.Text;`. Убедись, что `StringBuilder` доступен в других файлах без using.

3. **⭐ Создай using-алиас.** Напиши `using StringList = System.Collections.Generic.List<string>;` и создай список имён.

4. **⭐ Поэкспериментируй с #if DEBUG.** Выведи "Debug mode" в Debug-сборке и "Release mode" в Release. Переключи конфигурацию в Rider и проверь.

5. **⭐ Что выведет?** Прочитай код и предскажи вывод ДО запуска:
```csharp
namespace MyApp;
// Подсказка: implicit usings включены

var sb = new StringBuilder();
sb.Append("Hello");
sb.Append(' ');
sb.Append("World");
Console.WriteLine(sb);
```

---

## Практика Uno Platform ⭐

**"Визитка"**
Создай приложение с одной страницей. На ней TextBlock-и отображают:
- Твоё имя
- Возраст
- 3 хобби

В code-behind (файл .xaml.cs) задай данные в конструкторе и передай в UI через `x:Name`.

```xml
<!-- MainPage.xaml -->
<StackPanel Padding="20" Spacing="10">
    <TextBlock x:Name="NameText" FontSize="24"/>
    <TextBlock x:Name="AgeText" FontSize="18"/>
    <TextBlock x:Name="HobbiesText" FontSize="18"/>
</StackPanel>
```

```csharp
// MainPage.xaml.cs
public MainPage()
{
    InitializeComponent();
    NameText.Text = "Твоё имя";
    AgeText.Text = "Возраст: 20";
    HobbiesText.Text = "Хобби: игры, музыка, код";
}
```

---

## Практика Godot ⭐

**Информация о персонаже**
Создай сцену с несколькими Label-узлами. В скрипте `_Ready()` задай текст для каждого. Отобрази информацию о себе (имя, возраст, хобби).

```csharp
// CharacterInfo.cs
using Godot;

public partial class CharacterInfo : Node2D
{
    public override void _Ready()
    {
        GetNode<Label>("NameLabel").Text = "Имя: Твоё имя";
        GetNode<Label>("AgeLabel").Text = "Возраст: 20";
        GetNode<Label>("HobbyLabel").Text = "Хобби: геймдев";
    }
}
```

---

## Контрольные вопросы

1. Чем `global using` отличается от обычного `using`?
2. Зачем нужен `implicit usings`? Какие пространства подключаются автоматически?
3. Что такое file-scoped namespace и чем он лучше классического?
4. Можно ли иметь top-level statements в двух файлах? Почему?
5. Когда полезна условная компиляция `#if`?
6. Для чего нужны XML-комментарии `///`?

---

## Что дальше

Ты знаешь как устроена программа. Следующий шаг — **переменные и типы данных** (тема 1.2). Ты узнаешь какие "коробки" есть в C# для хранения чисел, текста и других данных.
