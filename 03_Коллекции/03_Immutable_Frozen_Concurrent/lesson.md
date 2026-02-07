# Тема 3.3: Immutable, Frozen и Concurrent коллекции

## Что ты узнаешь
- ImmutableList, ImmutableDictionary, ImmutableArray — неизменяемые коллекции
- FrozenSet, FrozenDictionary — "замороженные" для максимальной скорости чтения
- ConcurrentDictionary, ConcurrentQueue и другие потокобезопасные коллекции
- Channel<T> — producer/consumer

---

## Объяснение

### ЗАЧЕМ?
Обычные коллекции (List, Dictionary) **не потокобезопасны** — если два потока одновременно меняют List, данные повреждаются. Есть три решения:
1. **Immutable** — никто не может изменить, каждое "изменение" создаёт новую коллекцию
2. **Frozen** — создаётся один раз, потом только чтение (максимальная скорость)
3. **Concurrent** — специальные коллекции, безопасные для многопоточного доступа

---

### Immutable коллекции

Каждая "модификация" возвращает **новую коллекцию**. Старая не меняется.

```csharp
using System.Collections.Immutable;

// Создание
ImmutableList<string> names = ["Алиса", "Боб"];

// "Добавление" — возвращает НОВЫЙ список
ImmutableList<string> names2 = names.Add("Вика");

Console.WriteLine(names.Count);  // 2 — оригинал не изменился!
Console.WriteLine(names2.Count); // 3

// Другие операции — тоже возвращают новый объект
ImmutableList<string> names3 = names2.Remove("Боб");
ImmutableList<string> names4 = names2.SetItem(0, "Анна"); // замена по индексу

// ImmutableDictionary
ImmutableDictionary<string, int> scores = ImmutableDictionary<string, int>.Empty
    .Add("Алиса", 100)
    .Add("Боб", 85);

// ImmutableArray — быстрее ImmutableList для чтения
ImmutableArray<int> arr = [1, 2, 3, 4, 5];
ImmutableArray<int> arr2 = arr.Add(6);

// Builder — для создания через много операций (эффективнее)
ImmutableList<int>.Builder builder = ImmutableList.CreateBuilder<int>();
for (int i = 0; i < 1000; i++)
    builder.Add(i);
ImmutableList<int> result = builder.ToImmutable();
```

### Когда Immutable?
- Многопоточность: безопасно передавать между потоками
- Функциональный стиль: каждое изменение — новое состояние
- Undo/Redo: предыдущие версии сохраняются
- Конфигурация: создал один раз, передал всем

---

### Frozen коллекции (.NET 8+)

**Оптимизированы для чтения** после однократного создания. Быстрее обычного Dictionary/HashSet на операциях поиска.

```csharp
using System.Collections.Frozen;

// Создание из обычных коллекций
Dictionary<string, int> source = new()
{
    ["fire"] = 10,
    ["ice"] = 8,
    ["lightning"] = 15
};

// "Замораживаем" — после этого нельзя изменить
FrozenDictionary<string, int> frozen = source.ToFrozenDictionary();

// Чтение — быстрее чем Dictionary!
int damage = frozen["fire"]; // 10
bool has = frozen.ContainsKey("ice"); // true

// FrozenSet
FrozenSet<string> keywords = new[] { "class", "struct", "interface", "enum" }.ToFrozenSet();
bool isKeyword = keywords.Contains("class"); // true — быстрее HashSet

// ❌ Нельзя изменить — нет Add, Remove и т.д.
```

### Когда Frozen?
- Справочные данные, которые не меняются (конфигурация, словарь)
- Горячие пути (hot paths) — код, который вызывается миллионы раз
- Game data: таблицы урона, списки предметов

---

### Concurrent коллекции

Потокобезопасные коллекции для **одновременного** доступа из нескольких потоков.

```csharp
using System.Collections.Concurrent;

// ConcurrentDictionary — потокобезопасный словарь
ConcurrentDictionary<string, int> scores = new();

// Атомарные операции (безопасны из любого потока)
scores.TryAdd("Алиса", 100);
scores.AddOrUpdate("Алиса",
    addValue: 100,                          // если нет — добавить 100
    updateValueFactory: (key, old) => old + 10); // если есть — увеличить на 10

scores.GetOrAdd("Боб", 0); // получить или добавить значение по умолчанию

// ConcurrentQueue — потокобезопасная очередь
ConcurrentQueue<string> tasks = new();
tasks.Enqueue("Задача 1");
tasks.Enqueue("Задача 2");

if (tasks.TryDequeue(out string? task))
    Console.WriteLine(task);

// ConcurrentStack — потокобезопасный стек
ConcurrentStack<int> stack = new();
stack.Push(1);
stack.Push(2);
if (stack.TryPop(out int val))
    Console.WriteLine(val); // 2

// ConcurrentBag — неупорядоченная коллекция (оптимизирована для producer-consumer на одном потоке)
ConcurrentBag<string> bag = new();
bag.Add("item1");
bag.Add("item2");
```

### Когда Concurrent?
- Несколько потоков одновременно читают/пишут
- Параллельная обработка данных
- Producer/Consumer паттерн

---

### Channel<T> — современный Producer/Consumer

Лучшая альтернатива для передачи данных между потоками.

```csharp
using System.Threading.Channels;

// Ограниченный канал (максимум 100 элементов)
Channel<string> channel = Channel.CreateBounded<string>(100);

// Producer (пишет)
async Task ProduceAsync(ChannelWriter<string> writer)
{
    for (int i = 0; i < 10; i++)
    {
        await writer.WriteAsync($"Сообщение {i}");
        await Task.Delay(100);
    }
    writer.Complete(); // Больше не будет данных
}

// Consumer (читает)
async Task ConsumeAsync(ChannelReader<string> reader)
{
    await foreach (string message in reader.ReadAllAsync())
    {
        Console.WriteLine($"Получено: {message}");
    }
}

// Запуск
Task producer = ProduceAsync(channel.Writer);
Task consumer = ConsumeAsync(channel.Reader);
await Task.WhenAll(producer, consumer);
```

---

## Сравнение: обычная vs immutable vs frozen vs concurrent

| Тип | Изменяемая | Потокобезопасная | Скорость чтения | Когда |
|-----|-----------|-----------------|----------------|-------|
| `List<T>` | Да | Нет | Быстро | Обычное использование |
| `ImmutableList<T>` | Нет* | Да | Средне | Функциональный стиль |
| `FrozenSet<T>` | Нет | Да | Очень быстро | Справочные данные |
| `ConcurrentBag<T>` | Да | Да | Средне | Многопоточность |

\* Каждое "изменение" создаёт новую коллекцию.

---

## Частые ошибки

```csharp
// ❌ Забыли сохранить результат Immutable операции
ImmutableList<int> list = [1, 2, 3];
list.Add(4); // Ничего не произошло! Результат потерян!
// ✅
list = list.Add(4);

// ❌ Использование lock вместо Concurrent коллекции
// Concurrent коллекции часто быстрее ручного lock

// ❌ Frozen для часто меняющихся данных
// ToFrozenDictionary() — дорогая операция! Используй только для стабильных данных.
```

---

## Мини-упражнения
1. **⭐** Создай `ImmutableList<string>`, покажи что Add возвращает новый список, а старый не меняется.
2. **⭐** Создай `FrozenDictionary` из обычного словаря. Замерь через Stopwatch поиск в обоих (1 миллион итераций).
3. **⭐⭐** Создай `ConcurrentDictionary` для подсчёта слов из нескольких потоков параллельно.
4. **⭐⭐** Реализуй producer-consumer через `Channel<T>`: один поток генерирует числа, другой их обрабатывает.

### Практика Uno Platform ⭐⭐⭐
**"Кэш данных"** — FrozenDictionary для справочных данных (города, категории), ImmutableList для истории, ConcurrentDictionary для кэша API-ответов.

### Практика Godot ⭐⭐⭐
**"Система ресурсов"** — FrozenDictionary для таблицы предметов/врагов (загружается один раз), ImmutableArray для конфигурации уровней, Channel для передачи чанков от потока генерации.

---

## Контрольные вопросы перед Модулем 4
1. Чем `Dictionary` отличается от `SortedDictionary`?
2. Когда `HashSet` предпочтительнее `List`?
3. Что такое `FrozenDictionary` и когда его использовать?
4. Почему `ImmutableList.Add()` не меняет оригинальный список?
5. Когда нужны `Concurrent`-коллекции?

## Что дальше
Модуль 3 завершён! Дальше — **Модуль 4: Продвинутый C#** (Generics, делегаты, LINQ, async/await).
