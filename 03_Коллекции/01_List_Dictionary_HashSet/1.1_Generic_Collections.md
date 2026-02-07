# Тема 3.1: List, Dictionary, HashSet

## Что ты узнаешь
- List<T> — динамический массив
- Dictionary<TKey, TValue> — словарь (ключ → значение)
- HashSet<T> — множество уникальных элементов
- Queue, Stack, PriorityQueue, ObservableCollection
- Когда какую коллекцию выбрать

---

## Объяснение

### ЗАЧЕМ?
Массив — это шкаф с фиксированным числом полок. А если вещей стало больше? Нужны **коллекции** — умные контейнеры, которые растут, ищут, сортируют.

### List<T> — динамический массив

Как массив, но умеет расти. Самая используемая коллекция.

```csharp
// Создание
List<string> names = ["Алиса", "Боб", "Вика"]; // collection expression (C# 12)
List<int> numbers = new();                       // пустой

// Добавление
names.Add("Гена");                // в конец
names.Insert(1, "Дима");          // на позицию 1
names.AddRange(["Ева", "Женя"]); // несколько сразу

// Доступ
string first = names[0];          // "Алиса"
string last = names[^1];          // "Женя" (Index с конца)

// Поиск
bool hasAlice = names.Contains("Алиса");        // true
int index = names.IndexOf("Боб");               // число или -1
string? found = names.Find(n => n.Length > 3);   // первый с длиной > 3
List<string> all = names.FindAll(n => n.Length > 3);

// Удаление
names.Remove("Боб");         // по значению (первое вхождение)
names.RemoveAt(0);           // по индексу
names.RemoveAll(n => n.StartsWith("В")); // все по условию

// Сортировка
names.Sort();                           // по умолчанию (алфавит)
names.Sort((a, b) => b.Length - a.Length); // по длине (убывание)

// Важно: Count vs Capacity
List<int> list = new(capacity: 100); // выделяет место заранее
Console.WriteLine(list.Count);       // 0 — элементов нет
Console.WriteLine(list.Capacity);    // 100 — место зарезервировано
```

### Как List растёт?
Когда `Count == Capacity`, List создаёт новый массив **удвоенного** размера и копирует данные. Если знаешь примерный размер — указывай `capacity` в конструкторе!

---

### Dictionary<TKey, TValue> — словарь

Хранит пары **ключ → значение**. Поиск по ключу — мгновенный (O(1)).

```csharp
// Создание
Dictionary<string, int> ages = new()
{
    ["Алиса"] = 25,
    ["Боб"] = 30,
    ["Вика"] = 22
};

// Добавление
ages["Гена"] = 28;              // добавить или перезаписать
ages.Add("Дима", 35);           // только добавить (если ключ есть → исключение!)

// ⚠️ БЕЗОПАСНЫЙ поиск — ВСЕГДА используй TryGetValue!
if (ages.TryGetValue("Алиса", out int age))
{
    Console.WriteLine($"Алисе {age} лет");
}
else
{
    Console.WriteLine("Не найдена");
}

// ❌ ПЛОХО — KeyNotFoundException если ключа нет:
// int a = ages["Несуществующий"];

// Проверка наличия
bool hasKey = ages.ContainsKey("Боб");    // true
bool hasValue = ages.ContainsValue(30);   // true (медленно! O(n))

// Перебор
foreach (KeyValuePair<string, int> pair in ages)
{
    Console.WriteLine($"{pair.Key}: {pair.Value}");
}

// Или деконструкция (красивее):
foreach (var (name, personAge) in ages)
{
    Console.WriteLine($"{name}: {personAge}");
}

// Удаление
ages.Remove("Боб");
ages.Remove("Несуществующий", out int removed); // безопасное удаление

// GetValueOrDefault (.NET 9+)
int bobAge = ages.GetValueOrDefault("Боб", 0); // 0 если не найден

// TryAdd — добавить только если ключа нет
ages.TryAdd("Алиса", 99); // false, значение НЕ перезаписано
```

### Когда словарь?
- Кэширование: URL → ответ
- Конфигурация: ключ → значение
- Подсчёт: слово → количество
- Маппинг: ID → объект

---

### HashSet<T> — множество уникальных элементов

Как математическое множество: каждый элемент уникален. Поиск, добавление, удаление — O(1).

```csharp
// Создание
HashSet<string> tags = ["C#", "Godot", "Uno"];

// Добавление (дубликаты игнорируются)
bool added = tags.Add("C#");     // false — уже есть
bool added2 = tags.Add("LINQ");  // true — добавлен

// Проверка
bool has = tags.Contains("Godot"); // true (O(1) — мгновенно!)

// Операции множеств!
HashSet<string> moreTags = ["C#", "Unity", "Blender"];

tags.UnionWith(moreTags);         // Объединение: C#, Godot, Uno, LINQ, Unity, Blender
tags.IntersectWith(moreTags);     // Пересечение: только общие
tags.ExceptWith(moreTags);        // Разность: убрать элементы из moreTags
tags.SymmetricExceptWith(moreTags); // Симметричная разность

// Уникализация массива
int[] numbers = [1, 2, 2, 3, 3, 3, 4];
HashSet<int> unique = [..numbers]; // {1, 2, 3, 4}
// Или: numbers.ToHashSet()
```

### Когда HashSet?
- Проверка уникальности (уже видели этот элемент?)
- Быстрая проверка наличия
- Операции множеств (пересечение, объединение)

---

### Queue<T>, Stack<T>, PriorityQueue

```csharp
// Queue — очередь (FIFO: первый пришёл — первый ушёл)
Queue<string> tasks = new();
tasks.Enqueue("Загрузить данные");
tasks.Enqueue("Обработать");
tasks.Enqueue("Сохранить");

string next = tasks.Dequeue();  // "Загрузить данные" (забирает)
string peek = tasks.Peek();     // "Обработать" (смотрит, не забирая)

// Stack — стек (LIFO: последний пришёл — первый ушёл)
Stack<string> undoHistory = new();
undoHistory.Push("Написал текст");
undoHistory.Push("Удалил слово");
undoHistory.Push("Вставил картинку");

string undo = undoHistory.Pop();  // "Вставил картинку" (последнее действие)

// PriorityQueue — очередь с приоритетом (.NET 6+)
PriorityQueue<string, int> taskQueue = new();
taskQueue.Enqueue("Критический баг", 1);      // приоритет 1 (высший)
taskQueue.Enqueue("Новая фича", 3);           // приоритет 3
taskQueue.Enqueue("Рефакторинг", 5);          // приоритет 5
taskQueue.Enqueue("Срочный фикс", 1);         // приоритет 1

string urgent = taskQueue.Dequeue(); // "Критический баг" (наименьший приоритет)
```

---

### ObservableCollection<T> — для UI!

Уведомляет UI об изменениях (добавление, удаление). **Ключевая коллекция для Uno Platform!**

```csharp
using System.Collections.ObjectModel;

ObservableCollection<string> items = ["Яблоко", "Банан"];
items.CollectionChanged += (sender, e) =>
{
    Console.WriteLine($"Действие: {e.Action}, новые: {e.NewItems?.Count}");
};

items.Add("Вишня"); // UI автоматически обновится!
items.Remove("Банан"); // UI автоматически уберёт элемент
```

---

### IReadOnlyList<T>, IReadOnlyDictionary — безопасный возврат

```csharp
class Inventory
{
    private readonly List<string> _items = ["Меч", "Щит"];

    // Возвращаем "только для чтения" — вызывающий не может изменить
    public IReadOnlyList<string> Items => _items;
}
```

---

## Таблица: когда что использовать

| Коллекция | Для чего | Добавить | Найти | Удалить |
|-----------|----------|----------|-------|---------|
| `List<T>` | Упорядоченный список | O(1)* | O(n) | O(n) |
| `Dictionary<K,V>` | Ключ → значение | O(1) | O(1) | O(1) |
| `HashSet<T>` | Уникальные элементы | O(1) | O(1) | O(1) |
| `Queue<T>` | FIFO очередь | O(1) | — | O(1) |
| `Stack<T>` | LIFO стек | O(1) | — | O(1) |
| `PriorityQueue` | По приоритету | O(log n) | — | O(log n) |
| `SortedList<K,V>` | Отсортированный | O(n) | O(log n) | O(n) |
| `LinkedList<T>` | Вставка в середину | O(1)** | O(n) | O(1)** |

\* В конец. ** Если есть ссылка на узел.

---

## Частые ошибки

```csharp
// ❌ Обращение к словарю без проверки
int val = dict["key"]; // KeyNotFoundException!
// ✅
if (dict.TryGetValue("key", out int val)) { ... }

// ❌ Модификация коллекции во время foreach
foreach (var item in list)
    if (item == "bad") list.Remove(item); // InvalidOperationException!
// ✅ Используй RemoveAll или итерацию в обратном порядке
list.RemoveAll(item => item == "bad");

// ❌ Не указываешь capacity для больших списков
List<int> huge = new(); // будет много перевыделений
// ✅
List<int> huge = new(10000);

// ❌ Contains на List вместо HashSet для частых проверок
List<int> slowSearch = Enumerable.Range(0, 100000).ToList();
bool found = slowSearch.Contains(99999); // O(n)!
// ✅
HashSet<int> fastSearch = [..Enumerable.Range(0, 100000)];
bool found2 = fastSearch.Contains(99999); // O(1)!
```

---

## Мини-упражнения
1. **⭐** Создай `Dictionary<string, int>`, обработай ситуацию отсутствующего ключа через `TryGetValue`.
2. **⭐** Используй `HashSet<int>` для нахождения пересечения двух массивов.
3. **⭐** Реализуй стек для undo: `Push` действия, `Pop` для отмены.
4. **⭐** Используй `PriorityQueue` для задач с приоритетом.
5. **⭐** Создай `ObservableCollection<string>`, подпишись на `CollectionChanged`.

### Практика Uno Platform ⭐⭐⭐
**"Словарь иностранных слов"** — Dictionary для слов и переводов, ObservableCollection для привязки к ListView, HashSet тегов, поиск и фильтрация.

### Практика Godot ⭐⭐⭐
**"Система инвентаря"** — Dictionary для предметов (ID → Item), List для слотов, HashSet для уникальных тегов ("rare", "weapon"), PriorityQueue для приоритета использования.

---

## Что дальше
Дальше — **Immutable, Frozen и Concurrent коллекции** (3.2-3.3).
