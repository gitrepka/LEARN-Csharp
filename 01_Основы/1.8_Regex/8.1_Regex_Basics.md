# Тема 1.8: Регулярные выражения (Regex)

## Что ты узнаешь
- Что такое Regex и зачем он нужен
- Основные шаблоны: метасимволы, квантификаторы, группы
- Как использовать Regex в C#
- GeneratedRegex для производительности

---

## Объяснение

### ЗАЧЕМ?
**Аналогия:** Regex — это "Ctrl+F на стероидах". Обычный поиск ищет точный текст. Regex ищет по ШАБЛОНУ: "найди все email-адреса", "найди телефоны в формате +7(XXX)XXX-XX-XX", "замени все даты на другой формат".

### Основы

```csharp
using System.Text.RegularExpressions;

string text = "Мой email: alice@mail.ru, а ещё bob@gmail.com";

// IsMatch — есть ли совпадение?
bool hasEmail = Regex.IsMatch(text, @"\w+@\w+\.\w+"); // true

// Match — первое совпадение
Match match = Regex.Match(text, @"\w+@\w+\.\w+");
Console.WriteLine(match.Value); // "alice@mail.ru"

// Matches — все совпадения
foreach (Match m in Regex.Matches(text, @"\w+@\w+\.\w+"))
{
    Console.WriteLine(m.Value); // alice@mail.ru, bob@gmail.com
}

// Replace — замена
string hidden = Regex.Replace(text, @"\w+@\w+\.\w+", "[СКРЫТО]");
// "Мой email: [СКРЫТО], а ещё [СКРЫТО]"
```

### Метасимволы (спецсимволы)

| Символ | Значение | Пример |
|--------|----------|--------|
| `.` | Любой символ (кроме \n) | `a.b` → "aXb", "a1b" |
| `\d` | Цифра (0-9) | `\d{3}` → "123" |
| `\D` | НЕ цифра | `\D+` → "abc" |
| `\w` | Буква, цифра или _ | `\w+` → "hello_42" |
| `\W` | НЕ буква/цифра/_ | `\W` → "!", " " |
| `\s` | Пробельный символ | `\s+` → "  " |
| `\S` | НЕ пробельный | `\S+` → "слово" |
| `\b` | Граница слова | `\bcat\b` → "cat" но не "scatter" |

### Квантификаторы (сколько раз)

| Квантификатор | Значение |
|---------------|----------|
| `*` | 0 или более |
| `+` | 1 или более |
| `?` | 0 или 1 |
| `{3}` | Ровно 3 |
| `{2,5}` | От 2 до 5 |
| `{3,}` | 3 или более |

```csharp
// Телефон: +7(999)123-45-67
string pattern = @"\+7\(\d{3}\)\d{3}-\d{2}-\d{2}";
```

### Группы и классы символов

```csharp
// Группы — захватывают часть совпадения
string date = "Дата: 2026-02-07";
Match m = Regex.Match(date, @"(\d{4})-(\d{2})-(\d{2})");
string year = m.Groups[1].Value;  // "2026"
string month = m.Groups[2].Value; // "02"
string day = m.Groups[3].Value;   // "07"

// Именованные группы
Match m2 = Regex.Match(date, @"(?<year>\d{4})-(?<month>\d{2})-(?<day>\d{2})");
string y = m2.Groups["year"].Value; // "2026"

// Классы символов
// [abc]  — a или b или c
// [a-z]  — от a до z
// [^0-9] — НЕ цифра
// [A-Za-z] — любая латинская буква

// Якоря
// ^  — начало строки
// $  — конец строки
string pattern = @"^\d{3}$"; // строка из ровно 3 цифр
```

### GeneratedRegex (C# 12+ / .NET 7+)

```csharp
public partial class Validator
{
    // Компилируется в compile-time — быстрее в runtime!
    [GeneratedRegex(@"^[\w.-]+@[\w.-]+\.\w{2,}$", RegexOptions.IgnoreCase)]
    private static partial Regex EmailRegex();

    public bool IsValidEmail(string email)
        => EmailRegex().IsMatch(email);
}
```

---

## Мини-упражнения

1. **⭐** Валидируй email через Regex.
2. **⭐** Извлеки все числа из строки `"В 2026 году мне будет 25 лет"`.
3. **⭐** Замени все телефоны `+7(999)123-45-67` на маску `+7(***)***-**-**`.
4. **⭐⭐** Парсер лога: `"[2026-01-15 14:30:05] ERROR: Connection timeout (retry 3/5)"` — извлеки дату, уровень, сообщение, номер попытки.

---

## Практика Uno Platform ⭐⭐
**"Валидатор форм"** — поля: email, телефон, пароль (мин 8, заглавная, цифра, спецсимвол), URL. Подсветка ошибок в реальном времени. `[GeneratedRegex]`.

## Практика Godot ⭐⭐
**Система чат-команд:** `/spawn zombie 5 at 100,200`, `/heal player 50` — парси и выполняй.

---

## Контрольные вопросы
1. Чем `\d` отличается от `[0-9]`?
2. Что значит `+` vs `*` в Regex?
3. Зачем нужны именованные группы `(?<name>...)`?
4. Что даёт `[GeneratedRegex]`?
5. Что значит `\b` и когда он полезен?

## Что дальше
Дальше — **дата и время** (1.9): DateTime, TimeSpan, часовые пояса.
