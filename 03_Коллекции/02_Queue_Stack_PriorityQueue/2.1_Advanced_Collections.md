# Тема 3.2: LinkedList, SortedCollections, выбор коллекции

## Что ты узнаешь
- LinkedList<T> — связный список
- SortedList, SortedDictionary, SortedSet — автосортировка
- Как выбрать правильную коллекцию по задаче
- Big O нотация и почему это важно

---

## Объяснение

### ЗАЧЕМ?
List<T> и Dictionary — это 90% случаев. Но иногда нужны специальные коллекции: вставка в середину за O(1), автоматическая сортировка, или двусторонний доступ.

### LinkedList<T> — связный список

Каждый элемент хранит ссылку на **следующий** и **предыдущий**. Вставка/удаление в любом месте — O(1), но доступ по индексу — O(n).

```csharp
LinkedList<string> playlist = new();

// Добавление
playlist.AddLast("Песня 1");
playlist.AddLast("Песня 2");
playlist.AddLast("Песня 3");
playlist.AddFirst("Интро");

// Вставка в середину — быстро!
LinkedListNode<string> song2 = playlist.Find("Песня 2")!;
playlist.AddAfter(song2, "Песня 1.5");
playlist.AddBefore(song2, "Песня 1.75");

// Навигация
LinkedListNode<string>? current = playlist.First;
while (current != null)
{
    Console.Write($"{current.Value} → ");
    current = current.Next;
}
// Интро → Песня 1 → Песня 1.5 → Песня 1.75 → Песня 2 → Песня 3

// Удаление
playlist.Remove("Песня 1.5");
playlist.RemoveFirst();
playlist.RemoveLast();
```

### Когда LinkedList?
- Частые вставки/удаления в середину (плейлист, текстовый редактор)
- **Не используй** для случайного доступа по индексу — это O(n)

---

### SortedList<TKey, TValue>

Как Dictionary, но ключи **всегда отсортированы**. Основан на массиве.

```csharp
SortedList<int, string> leaderboard = new()
{
    [100] = "Алиса",
    [250] = "Боб",
    [50] = "Вика"
};

// Ключи автоматически отсортированы:
foreach (var (score, name) in leaderboard)
    Console.WriteLine($"{name}: {score}");
// Вика: 50, Алиса: 100, Боб: 250

// Доступ по индексу (в отличие от SortedDictionary!)
string first = leaderboard.Values[0]; // "Вика" (минимальный ключ)
int firstKey = leaderboard.Keys[0];   // 50
```

### SortedDictionary<TKey, TValue>

Тоже отсортированный словарь, но на основе **дерева** (Red-Black Tree).

```csharp
SortedDictionary<string, int> wordCount = new(StringComparer.OrdinalIgnoreCase);
wordCount["apple"] = 3;
wordCount["Banana"] = 1;
wordCount["cherry"] = 5;

// Ключи отсортированы:
foreach (var (word, count) in wordCount)
    Console.WriteLine($"{word}: {count}");
// apple: 3, Banana: 1, cherry: 5
```

### SortedList vs SortedDictionary

| Критерий | SortedList | SortedDictionary |
|----------|-----------|------------------|
| Структура | Массив | Дерево |
| Вставка | O(n) | O(log n) |
| Поиск по ключу | O(log n) | O(log n) |
| Доступ по индексу | O(1) ✅ | Нет ❌ |
| Память | Меньше | Больше |
| **Когда?** | Редко меняется, нужен индекс | Часто меняется |

### SortedSet<T>

HashSet + автосортировка. Элементы уникальны и отсортированы.

```csharp
SortedSet<int> scores = [50, 30, 80, 30, 10]; // 30 один раз

// Всегда отсортированы:
foreach (int s in scores)
    Console.Write($"{s} "); // 10 30 50 80

// Диапазон
SortedSet<int> range = scores.GetViewBetween(20, 60); // {30, 50}
Console.WriteLine(scores.Min); // 10
Console.WriteLine(scores.Max); // 80
```

---

## Big O — сложность алгоритмов

| O(?) | Название | Пример | 1000 элементов |
|------|----------|--------|----------------|
| O(1) | Константная | Доступ к словарю | 1 операция |
| O(log n) | Логарифмическая | Бинарный поиск | ~10 операций |
| O(n) | Линейная | Поиск в списке | 1000 операций |
| O(n log n) | Линеарифмическая | Сортировка | ~10000 операций |
| O(n²) | Квадратичная | Два вложенных цикла | 1 000 000 операций |

### Сводная таблица всех коллекций

| Коллекция | Add | Search | Delete | Sorted | Duplicates |
|-----------|-----|--------|--------|--------|------------|
| `List<T>` | O(1)* | O(n) | O(n) | Нет | Да |
| `Dictionary<K,V>` | O(1) | O(1) | O(1) | Нет | Ключи нет |
| `HashSet<T>` | O(1) | O(1) | O(1) | Нет | Нет |
| `SortedList<K,V>` | O(n) | O(log n) | O(n) | Да | Ключи нет |
| `SortedDictionary<K,V>` | O(log n) | O(log n) | O(log n) | Да | Ключи нет |
| `SortedSet<T>` | O(log n) | O(log n) | O(log n) | Да | Нет |
| `LinkedList<T>` | O(1) | O(n) | O(1)** | Нет | Да |
| `Queue<T>` | O(1) | — | O(1) | Нет | Да |
| `Stack<T>` | O(1) | — | O(1) | Нет | Да |
| `PriorityQueue<T,P>` | O(log n) | — | O(log n) | По приоритету | Да |

\* Amortized (в среднем). ** Если есть ссылка на узел.

---

## Как выбрать коллекцию? (Алгоритм)

```
Нужен ли ключ → значение?
├── Да → Нужна сортировка по ключу?
│   ├── Да → Часто меняется? → SortedDictionary
│   │                       → Редко меняется? → SortedList
│   └── Нет → Dictionary<K,V>
└── Нет → Элементы уникальны?
    ├── Да → Нужна сортировка?
    │   ├── Да → SortedSet<T>
    │   └── Нет → HashSet<T>
    └── Нет → Нужна очередь/стек?
        ├── Да → FIFO? → Queue | LIFO? → Stack | Приоритет? → PriorityQueue
        └── Нет → Частые вставки в середину?
            ├── Да → LinkedList<T>
            └── Нет → List<T>
```

---

## Мини-упражнения
1. **⭐** Создай `SortedDictionary<string, int>` для подсчёта слов в тексте. Выведи отсортированные результаты.
2. **⭐** Сравни `SortedList` и `SortedDictionary`: добавь 10000 элементов в каждый, замерь время через Stopwatch.
3. **⭐** Используй `SortedSet<int>.GetViewBetween()` для фильтрации диапазона оценок.

---

## Что дальше
Дальше — **Immutable, Frozen и Concurrent коллекции** (3.3).
