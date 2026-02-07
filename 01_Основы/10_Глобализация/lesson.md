# Тема 1.10: Глобализация и локализация (введение)

## Что ты узнаешь
- Что такое CultureInfo и как он влияет на программу
- Форматирование чисел и дат для разных стран
- Ресурсные файлы (.resx) для многоязычности

---

## Объяснение

### ЗАЧЕМ?
Число `1,000` — это "одна тысяча" в США и "одна целая ноль" в России. Дата `02/07/2026` — это 7 февраля в Европе и 2 июля в США. Если не учитывать культуру — будут баги.

### CultureInfo

```csharp
using System.Globalization;

double number = 1234567.89;
DateTime date = new DateTime(2026, 2, 7);

// Русская культура
var ru = new CultureInfo("ru-RU");
Console.WriteLine(number.ToString("N2", ru));  // 1 234 567,89
Console.WriteLine(date.ToString("D", ru));     // 7 февраля 2026 г.

// Американская
var us = new CultureInfo("en-US");
Console.WriteLine(number.ToString("N2", us));  // 1,234,567.89
Console.WriteLine(date.ToString("D", us));     // Saturday, February 7, 2026

// Немецкая
var de = new CultureInfo("de-DE");
Console.WriteLine(number.ToString("N2", de));  // 1.234.567,89

// InvariantCulture — нейтральная (для сохранения данных, файлов, API)
Console.WriteLine(number.ToString(CultureInfo.InvariantCulture)); // 1234567.89
```

**Правило:**
- Для ОТОБРАЖЕНИЯ пользователю → `CultureInfo.CurrentCulture`
- Для ХРАНЕНИЯ в файлах/БД/API → `CultureInfo.InvariantCulture`

### Текущая культура

```csharp
// Какая культура у текущего пользователя?
Console.WriteLine(CultureInfo.CurrentCulture.Name);     // "ru-RU" (если система на русском)
Console.WriteLine(CultureInfo.CurrentUICulture.Name);   // "ru-RU"

// Можно сменить:
CultureInfo.CurrentCulture = new CultureInfo("en-US");
```

### Ресурсные файлы (.resx)

Для многоязычного приложения строки хранятся в ресурсах:

```
Resources/
  Strings.resx          ← по умолчанию (английский)
  Strings.ru.resx       ← русский
  Strings.de.resx       ← немецкий
```

```csharp
// Strings.resx содержит: Greeting = "Hello!"
// Strings.ru.resx содержит: Greeting = "Привет!"

// Использование:
using MyApp.Resources;

string greeting = Strings.Greeting; // Автоматически выберет язык системы
```

> Подробнее о локализации — в модулях Uno Platform (10.11) и Godot (11.17).

---

## Мини-упражнения

1. **⭐** Выведи число `1234567.89` в формате трёх разных культур (ru-RU, en-US, de-DE).
2. **⭐** Покажи сегодняшнюю дату в формате разных культур.
3. **⭐** Попробуй распарсить строку `"1.234,56"` как число в немецкой культуре.

---

## Контрольные вопросы
1. Что такое `CultureInfo` и зачем он нужен?
2. Когда использовать `InvariantCulture`?
3. Почему `"1,000"` может означать разные числа в разных странах?
4. Как .resx файлы помогают с многоязычностью?

## Что дальше
Модуль 1 завершён! Ты знаешь основы C#. Дальше — **Модуль 2: ООП** — классы, наследование, полиморфизм. Это фундамент всей профессиональной разработки.
