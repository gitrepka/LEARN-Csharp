# Тема 1.9: Дата и время

## Что ты узнаешь
- DateTime, DateTimeOffset, DateOnly, TimeOnly
- TimeSpan — промежутки времени
- Часовые пояса, форматирование
- Stopwatch — замер производительности

---

## Объяснение

### ЗАЧЕМ?
Дата и время — в каждом приложении: "когда создана заметка?", "сколько времени выжил?", "таймер обратного отсчёта". В играх: день/ночь, кулдауны, таймеры волн.

### DateTime — основной тип

```csharp
// Текущее время
DateTime now = DateTime.Now;        // локальное время
DateTime utcNow = DateTime.UtcNow;  // UTC (без часового пояса)
DateTime today = DateTime.Today;    // сегодня, время 00:00

// Создание конкретной даты
DateTime birthday = new DateTime(2000, 5, 15);          // 15 мая 2000
DateTime exact = new DateTime(2026, 2, 7, 14, 30, 0);   // 7 фев 2026 14:30:00

// Свойства
Console.WriteLine(now.Year);        // 2026
Console.WriteLine(now.Month);       // 2
Console.WriteLine(now.Day);         // 7
Console.WriteLine(now.DayOfWeek);   // Saturday
Console.WriteLine(now.Hour);        // 14
Console.WriteLine(now.Minute);      // 30

// Арифметика
DateTime tomorrow = now.AddDays(1);
DateTime nextMonth = now.AddMonths(1);
DateTime past = now.AddHours(-5);    // 5 часов назад

// Разница между датами → TimeSpan
TimeSpan diff = new DateTime(2026, 12, 31) - now;
Console.WriteLine($"До Нового года: {diff.Days} дней");
```

### Форматирование

```csharp
DateTime dt = new DateTime(2026, 2, 7, 14, 30, 45);

// Стандартные форматы
Console.WriteLine(dt.ToString("d"));    // 07.02.2026 (короткая дата)
Console.WriteLine(dt.ToString("D"));    // 7 февраля 2026 г. (длинная)
Console.WriteLine(dt.ToString("t"));    // 14:30 (короткое время)
Console.WriteLine(dt.ToString("f"));    // 7 февраля 2026 г. 14:30

// Кастомные форматы
Console.WriteLine(dt.ToString("dd.MM.yyyy"));       // 07.02.2026
Console.WriteLine(dt.ToString("yyyy-MM-dd HH:mm")); // 2026-02-07 14:30
Console.WriteLine(dt.ToString("HH:mm:ss"));          // 14:30:45

// Интерполяция
Console.WriteLine($"Дата: {dt:dd.MM.yyyy}");
```

### Parsing (из строки в дату)

```csharp
DateTime parsed = DateTime.Parse("2026-02-07");

// TryParse — безопасный (не кидает исключение)
if (DateTime.TryParse("07.02.2026", out DateTime result))
    Console.WriteLine(result);

// ParseExact — точный формат
DateTime exact = DateTime.ParseExact("07-02-2026", "dd-MM-yyyy", null);
```

### TimeSpan — промежуток времени

```csharp
TimeSpan duration = new TimeSpan(2, 30, 0);     // 2 часа 30 минут
TimeSpan fromDays = TimeSpan.FromDays(1.5);      // 1.5 дня
TimeSpan fromMinutes = TimeSpan.FromMinutes(90);  // 90 минут

Console.WriteLine(duration.TotalHours);   // 2.5
Console.WriteLine(duration.TotalMinutes); // 150

// Арифметика
TimeSpan total = duration + TimeSpan.FromHours(1); // 3:30:00
```

### DateOnly и TimeOnly (.NET 6+)

```csharp
DateOnly date = new DateOnly(2026, 2, 7);   // только дата
TimeOnly time = new TimeOnly(14, 30);        // только время

Console.WriteLine(date);   // 07.02.2026
Console.WriteLine(time);   // 14:30

// Когда использовать:
// DateOnly — дни рождения, праздники (время не важно)
// TimeOnly — расписание уроков, будильник (дата не важна)
// DateTime — когда нужны и дата, и время
```

### Часовые пояса

```csharp
// DateTimeOffset — дата + время + смещение от UTC
DateTimeOffset moscow = new DateTimeOffset(2026, 2, 7, 14, 30, 0, TimeSpan.FromHours(3));

// Конвертация
TimeZoneInfo mskZone = TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");
DateTime mskTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, mskZone);
```

### Stopwatch — замер производительности

```csharp
using System.Diagnostics;

var sw = Stopwatch.StartNew();

// Код, который замеряем
for (int i = 0; i < 1_000_000; i++) { }

sw.Stop();
Console.WriteLine($"Время: {sw.ElapsedMilliseconds} мс");
Console.WriteLine($"Точно: {sw.Elapsed.TotalMicroseconds} мкс");
```

---

## Мини-упражнения

1. **⭐** Посчитай сколько дней до твоего следующего дня рождения.
2. **⭐** Определи день недели для даты 1 января 2000 года.
3. **⭐** Замерь время выполнения цикла на 10 миллионов итераций через Stopwatch.

---

## Практика Uno Platform ⭐⭐
**"Мировые часы"** — 5 часовых поясов (Москва, Нью-Йорк, Токио, Лондон, Сидней). Обновление каждую секунду. Аналоговый циферблат через Canvas.

## Практика Godot ⭐⭐
**Игровой таймер + день/ночь.** Ускоренное время (1 мин = 1 час игрового). Освещение меняется: рассвет → день → закат → ночь.

---

## Контрольные вопросы
1. Чем `DateTime.Now` отличается от `DateTime.UtcNow`?
2. Когда использовать `DateOnly` вместо `DateTime`?
3. Что такое `DateTimeOffset` и зачем он нужен?
4. Как безопасно распарсить дату из строки?
5. Зачем нужен `Stopwatch` если есть DateTime?

## Что дальше
Дальше — **глобализация** (1.10): как приложение ведёт себя в разных странах и языках.
