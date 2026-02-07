# Тема 1.5: Строки (Strings) — углублённо

## Что ты узнаешь
- Все способы создания строк (включая raw strings C# 11)
- Основные методы строк
- StringBuilder и когда он нужен
- Форматирование чисел и дат
- Сравнение строк — подводные камни

---

## Объяснение

### ЗАЧЕМ?
Строки — везде: имена, диалоги, логи, JSON, UI-текст. В играх — диалоги NPC, названия предметов, HUD. В приложениях — ввод пользователя, отображение данных. Без строк никуда.

### Создание строк

```csharp
// Обычная строка
string name = "Алиса";

// Verbatim-строка (@) — для путей и многострочного текста
string path = @"C:\Users\mydev\Documents";  // без @ нужно: "C:\\Users\\mydev"

// Интерполяция ($) — вставка переменных
int hp = 100;
string status = $"Здоровье: {hp}";  // "Здоровье: 100"

// Комбинация $@
string log = $@"Путь: C:\Games\{name}\save.json";

// Raw string literals (C# 11) — для JSON, XML, многострочного текста
string json = """
    {
        "name": "Алиса",
        "level": 5,
        "health": 100
    }
    """;

// Raw string с интерполяцией ($$)
int level = 5;
string jsonWithData = $$"""
    {
        "name": "{{name}}",
        "level": {{level}}
    }
    """;

// UTF-8 строковый литерал (C# 11)
ReadOnlySpan<byte> utf8 = "hello"u8;  // для высокопроизводительного IO
```

### Основные методы строк

```csharp
string text = "  Hello, World!  ";

// Длина и регистр
text.Length              // 17
text.ToUpper()           // "  HELLO, WORLD!  "
text.ToLower()           // "  hello, world!  "

// Обрезка пробелов
text.Trim()              // "Hello, World!"
text.TrimStart()         // "Hello, World!  "
text.TrimEnd()           // "  Hello, World!"

// Поиск
text.Contains("World")   // true
text.StartsWith("  He")  // true
text.EndsWith("!  ")     // true
text.IndexOf("World")    // 9
text.LastIndexOf("l")    // 13

// Извлечение
text.Substring(8, 5)     // "World"
text[8..13]              // "World" (Range — современный способ)

// Замена и удаление
text.Replace("World", "Godot")  // "  Hello, Godot!  "
text.Remove(5, 8)               // "  Hel  "

// Разделение и объединение
string csv = "яблоко,банан,вишня";
string[] fruits = csv.Split(',');           // ["яблоко", "банан", "вишня"]
string joined = string.Join(" | ", fruits); // "яблоко | банан | вишня"

// Проверки на пустоту
string.IsNullOrEmpty("")           // true
string.IsNullOrEmpty(null)         // true
string.IsNullOrWhiteSpace("   ")   // true
```

### StringBuilder — когда много склеиваний

**Проблема:** Строка в C# **иммутабельна** (неизменяемая). Каждая "модификация" создаёт НОВЫЙ объект.

```csharp
// ❌ Медленно: 10000 новых строк в памяти!
string result = "";
for (int i = 0; i < 10000; i++)
{
    result += i + ", ";  // каждый += создаёт новую строку!
}

// ✅ Быстро: StringBuilder модифицирует один буфер
var sb = new StringBuilder();
for (int i = 0; i < 10000; i++)
{
    sb.Append(i);
    sb.Append(", ");
}
string result = sb.ToString();
```

**Правило:** Если больше ~10 конкатенаций в цикле — используй `StringBuilder`.

### Форматирование чисел

```csharp
double price = 1234.5678;

Console.WriteLine($"{price:F2}");    // 1234.57 — Fixed, 2 знака
Console.WriteLine($"{price:N2}");    // 1 234,57 — Number с разделителем тысяч
Console.WriteLine($"{price:C}");     // 1 234,57 ₽ — Currency
Console.WriteLine($"{price:E2}");    // 1.23E+003 — Scientific
Console.WriteLine($"{price:P1}");    // 123 456,8 % — Percent

int hex = 255;
Console.WriteLine($"{hex:X}");      // FF — Hexadecimal
Console.WriteLine($"{hex:D8}");     // 00000255 — Decimal с ведущими нулями

// Кастомный формат
Console.WriteLine($"{price:#,##0.00}"); // 1,234.57
```

### Сравнение строк

```csharp
string a = "hello";
string b = "Hello";

// == — регистрозависимое
Console.WriteLine(a == b);  // False

// Equals с опцией
Console.WriteLine(a.Equals(b, StringComparison.OrdinalIgnoreCase)); // True

// Для сортировки
int result = string.Compare(a, b, StringComparison.OrdinalIgnoreCase); // 0 (равны)
```

**Важно:** Всегда указывай `StringComparison`! Без него — культурозависимое сравнение, которое может дать неожиданные результаты в разных странах.

```csharp
// ✅ Для программной логики (пути, ключи, ID):
StringComparison.Ordinal
StringComparison.OrdinalIgnoreCase

// ✅ Для отображения пользователю (сортировка имён):
StringComparison.CurrentCulture
```

### Span и строки (без аллокаций)

```csharp
string text = "Hello, World!";
ReadOnlySpan<char> span = text.AsSpan();
ReadOnlySpan<char> hello = span[..5];   // "Hello" — без копирования!
ReadOnlySpan<char> world = span[7..12]; // "World" — без копирования!
```

---

## Частые ошибки

### 1. Строка иммутабельна
```csharp
string name = "алиса";
name.ToUpper(); // ❌ Не меняет name! Возвращает НОВУЮ строку
Console.WriteLine(name); // "алиса" — не изменилось!

name = name.ToUpper(); // ✅ Присвой обратно
Console.WriteLine(name); // "АЛИСА"
```

### 2. Сравнение без учёта регистра
```csharp
// ❌ Не сработает если пользователь ввёл "ДА"
if (input == "да") { }

// ✅ Игнорируй регистр:
if (input.Equals("да", StringComparison.OrdinalIgnoreCase)) { }
```

---

## Мини-упражнения

1. **⭐** Переверни строку тремя способами:
   - Через цикл
   - Через `Array.Reverse()`
   - Через LINQ `.Reverse()`

2. **⭐** Посчитай количество каждой гласной (а, е, и, о, у) в тексте.

3. **⭐** Используй raw string literal для JSON-шаблона с интерполяцией.

4. **⭐** Замерь разницу: 10000 конкатенаций `string +=` vs `StringBuilder` через `Stopwatch`.

5. **⭐⭐** Напиши методы `CamelCaseToSnakeCase("helloWorld")` → `"hello_world"` и обратно.

---

## Практика Uno Platform ⭐⭐
**"Текстовый анализатор"** — TextBox ввода → показывай: символы, слова, предложения, самое длинное слово, частоту букв. Поиск и замена.

## Практика Godot ⭐⭐
**Система диалогов NPC** — строки с тегами: парсинг, побуквенное появление (typewriter-эффект).

---

## Контрольные вопросы
1. Почему string иммутабельна? Что это значит на практике?
2. Когда использовать `StringBuilder`?
3. Чем `@""` отличается от `$""`? Можно ли совмещать?
4. Что такое raw string literal (`"""`)? Когда он полезен?
5. Почему для программной логики лучше `StringComparison.Ordinal`?

## Что дальше
Дальше — **массивы и диапазоны** (1.6): как хранить и обрабатывать коллекции данных.
