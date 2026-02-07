# Тема 6.2: Рефлексия (Reflection) и атрибуты

## Что ты узнаешь
- typeof(), GetType(), Type, Assembly
- Получение методов, свойств, атрибутов в runtime
- Создание экземпляров через Activator
- Стандартные и кастомные атрибуты
- Generic attributes (C# 11)
- Source Generators (введение)

---

## Объяснение

### ЗАЧЕМ?
Рефлексия — "зеркало" для кода. Программа может **исследовать саму себя**: какие типы, методы, свойства есть? Это основа DI-контейнеров, сериализаторов, ORM, тестовых фреймворков.

### Получение информации о типе

```csharp
// Три способа получить Type
Type t1 = typeof(string);              // по имени типа (compile-time)
Type t2 = "hello".GetType();           // по экземпляру (runtime)
Type t3 = Type.GetType("System.Int32")!; // по строке

// Информация о типе
Console.WriteLine(t1.Name);          // "String"
Console.WriteLine(t1.FullName);      // "System.String"
Console.WriteLine(t1.Namespace);     // "System"
Console.WriteLine(t1.IsClass);       // true
Console.WriteLine(t1.IsValueType);   // false
Console.WriteLine(t1.BaseType);      // System.Object
Console.WriteLine(t1.Assembly.FullName);
```

### Получение членов типа

```csharp
Type playerType = typeof(Player);

// Свойства
PropertyInfo[] props = playerType.GetProperties();
foreach (var prop in props)
    Console.WriteLine($"{prop.Name}: {prop.PropertyType.Name}");

// Методы
MethodInfo[] methods = playerType.GetMethods(BindingFlags.Public | BindingFlags.Instance);
foreach (var method in methods)
    Console.WriteLine($"{method.Name}({string.Join(", ", method.GetParameters().Select(p => p.ParameterType.Name))})");

// Конструкторы
ConstructorInfo[] ctors = playerType.GetConstructors();

// Поля (включая private!)
FieldInfo[] fields = playerType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
```

### Динамическое создание и вызов

```csharp
// Создание экземпляра
object? player = Activator.CreateInstance(typeof(Player));

// Вызов метода
MethodInfo? method = typeof(Player).GetMethod("TakeDamage");
method?.Invoke(player, [10]); // player.TakeDamage(10)

// Чтение/запись свойства
PropertyInfo? nameProp = typeof(Player).GetProperty("Name");
nameProp?.SetValue(player, "Алиса");
string? name = (string?)nameProp?.GetValue(player);

// Доступ к private полям!
FieldInfo? secretField = typeof(Player).GetField("_secret", BindingFlags.NonPublic | BindingFlags.Instance);
secretField?.SetValue(player, 42);
```

---

### Атрибуты

Метаданные, прикреплённые к коду:

```csharp
// Стандартные атрибуты
[Obsolete("Используй NewMethod() вместо этого")]
void OldMethod() { }

[Conditional("DEBUG")] // вызывается только в Debug
void DebugLog(string message) => Console.WriteLine(message);

[Serializable]
class SaveData { }

// Создание своего атрибута
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
class RangeAttribute : Attribute
{
    public int Min { get; }
    public int Max { get; }
    public RangeAttribute(int min, int max) { Min = min; Max = max; }
}

class Player
{
    [Range(1, 100)]
    public int Level { get; set; }

    [Range(0, 999)]
    public int Health { get; set; }
}

// Чтение атрибутов через рефлексию
var props = typeof(Player).GetProperties();
foreach (var prop in props)
{
    var range = prop.GetCustomAttribute<RangeAttribute>();
    if (range is not null)
        Console.WriteLine($"{prop.Name}: {range.Min}..{range.Max}");
}
// Level: 1..100
// Health: 0..999
```

### Generic attributes (C# 11)

```csharp
// Раньше: атрибут с typeof
[Validate(typeof(StringValidator))]

// Теперь: generic атрибут!
[Validate<StringValidator>]
class MyClass { }

class ValidateAttribute<T> : Attribute where T : IValidator, new()
{
    public T CreateValidator() => new();
}
```

---

### Source Generators (введение)

Compile-time альтернатива рефлексии. Код генерируется во время компиляции.

```csharp
// Пример: [GeneratedRegex] — Source Generator для Regex
[GeneratedRegex(@"\d+")]
private static partial Regex NumberPattern();

// Пример: [JsonSerializable] — Source Generator для JSON
[JsonSerializable(typeof(Player))]
partial class GameContext : JsonSerializerContext { }

// Source Generators быстрее рефлексии:
// - Нет runtime-расходов
// - Работают с NativeAOT
// - Ошибки видны при компиляции
```

---

### Когда рефлексия, когда нет?

| Сценарий | Подход |
|----------|--------|
| DI-контейнер, ORM, сериализатор | Рефлексия (или Source Gen) |
| Тестовый фреймворк | Рефлексия |
| Обычная бизнес-логика | ❌ Не используй рефлексию! |
| Горячий путь (вызывается миллионы раз) | Source Generator |
| NativeAOT | Source Generator (рефлексия ограничена) |

---

## Частые ошибки

```csharp
// ❌ Рефлексия в горячих путях — медленно!
// GetProperty/GetMethod — кэшируй!
PropertyInfo prop = typeof(Player).GetProperty("Name")!; // вызови ОДИН раз

// ❌ Рефлексия вместо интерфейса
// Если можешь определить интерфейс — лучше интерфейс

// ❌ Забыл BindingFlags
typeof(Player).GetFields(); // только public!
typeof(Player).GetFields(BindingFlags.NonPublic | BindingFlags.Instance); // private
```

---

## Мини-упражнения
1. **⭐** Получи все свойства класса через рефлексию, выведи имя и тип.
2. **⭐** Создай кастомный атрибут `[Description("текст")]`, прочитай через рефлексию.
3. **⭐⭐** Создай экземпляр класса через `Activator.CreateInstance`, вызови метод через `MethodInfo.Invoke`.

---

## Что дальше
Дальше — **многопоточность** (6.3).
