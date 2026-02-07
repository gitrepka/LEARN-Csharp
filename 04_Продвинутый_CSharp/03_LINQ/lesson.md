# Тема 4.3: LINQ (Language Integrated Query)

## Что ты узнаешь
- Синтаксис запросов (from...where...select)
- Синтаксис методов (Where, Select, OrderBy...)
- Все основные операторы LINQ
- Отложенное выполнение (deferred execution)
- LINQ и производительность

---

## Объяснение

### ЗАЧЕМ?
Представь: тебе нужно из списка игроков найти всех живых, отсортировать по уровню, взять топ-5 и получить их имена. Без LINQ — 15 строк циклов. С LINQ — одна цепочка.

### Два синтаксиса

```csharp
List<int> numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

// Синтаксис запросов (SQL-подобный)
var evens1 = from n in numbers
             where n % 2 == 0
             orderby n descending
             select n * 10;

// Синтаксис методов (цепочка) — используется чаще!
var evens2 = numbers
    .Where(n => n % 2 == 0)
    .OrderByDescending(n => n)
    .Select(n => n * 10);

// Результат одинаковый: [100, 80, 60, 40, 20]
```

---

### Основные операторы LINQ

#### Фильтрация
```csharp
var adults = people.Where(p => p.Age >= 18);
var firstAdult = people.First(p => p.Age >= 18);     // исключение если нет!
var firstOrNull = people.FirstOrDefault(p => p.Age > 100); // null если нет
var single = people.Single(p => p.Id == 5);           // исключение если не ровно один!
```

#### Проекция (Select)
```csharp
// Выбор одного поля
var names = people.Select(p => p.Name);

// Трансформация
var info = people.Select(p => new { p.Name, IsAdult = p.Age >= 18 });

// SelectMany — "развернуть" вложенные коллекции
var allSkills = people.SelectMany(p => p.Skills);
// Если каждый person имеет List<string> Skills — получим один общий список
```

#### Сортировка
```csharp
var sorted = people
    .OrderBy(p => p.Name)                    // по имени (A→Z)
    .ThenByDescending(p => p.Age);           // потом по возрасту (убыв.)
```

#### Группировка
```csharp
var byCity = people.GroupBy(p => p.City);

foreach (var group in byCity)
{
    Console.WriteLine($"Город: {group.Key}, Людей: {group.Count()}");
    foreach (var person in group)
        Console.WriteLine($"  - {person.Name}");
}
```

#### Агрегация
```csharp
int count = numbers.Count();
int sum = numbers.Sum();
double avg = numbers.Average();
int max = numbers.Max();
int min = numbers.Min();

// Aggregate — "свёртка" (самая мощная)
string joined = names.Aggregate((a, b) => $"{a}, {b}");
// "Алиса, Боб, Вика"

int product = numbers.Aggregate(1, (acc, n) => acc * n);
// 1 * 2 * 3 * 4 * 5 = 120
```

#### Проверки
```csharp
bool anyAdults = people.Any(p => p.Age >= 18);   // хоть один?
bool allAdults = people.All(p => p.Age >= 18);   // все?
bool hasAlice = people.Any(p => p.Name == "Алиса");
```

#### Множества
```csharp
var unique = numbers.Distinct();                // уникальные
var union = list1.Union(list2);                 // объединение
var common = list1.Intersect(list2);            // пересечение
var diff = list1.Except(list2);                 // разность
```

#### Пагинация
```csharp
var page = items
    .Skip(20)     // пропустить первые 20
    .Take(10);    // взять 10

// Chunk (.NET 6+) — разбить на страницы
var pages = items.Chunk(10); // массив массивов по 10 элементов
```

#### Join — соединение
```csharp
record Order(int Id, int ProductId, int Quantity);
record Product(int Id, string Name, decimal Price);

List<Order> orders = [ /* ... */ ];
List<Product> products = [ /* ... */ ];

var report = orders.Join(
    products,
    order => order.ProductId,  // ключ из orders
    product => product.Id,     // ключ из products
    (order, product) => new    // результат
    {
        product.Name,
        order.Quantity,
        Total = product.Price * order.Quantity
    }
);
```

#### Zip — попарное объединение
```csharp
string[] names = ["Алиса", "Боб", "Вика"];
int[] scores = [95, 87, 92];

var pairs = names.Zip(scores, (name, score) => $"{name}: {score}");
// ["Алиса: 95", "Боб: 87", "Вика: 92"]

// C# 12: Zip без проекции → кортежи
var tuples = names.Zip(scores); // [(Алиса, 95), (Боб, 87), (Вика, 92)]
```

#### Материализация
```csharp
List<int> list = query.ToList();
int[] array = query.ToArray();
Dictionary<int, string> dict = people.ToDictionary(p => p.Id, p => p.Name);
HashSet<string> set = names.ToHashSet();
ILookup<string, Person> lookup = people.ToLookup(p => p.City);
// Lookup — как Dictionary, но один ключ → несколько значений
```

---

### Отложенное выполнение (Deferred Execution)

**Критически важно понять!** LINQ запрос НЕ выполняется при создании. Он выполняется когда ты начинаешь перечислять результат.

```csharp
List<int> numbers = [1, 2, 3, 4, 5];

// Запрос СОЗДАН, но НЕ выполнен!
var query = numbers.Where(n => n > 2);

numbers.Add(6); // Добавляем после создания запроса

// Запрос выполняется СЕЙЧАС (при перечислении):
foreach (int n in query)
    Console.Write($"{n} "); // 3 4 5 6 ← 6 включён!

// Немедленное выполнение (материализация):
var result = numbers.Where(n => n > 2).ToList(); // выполняется СРАЗУ
numbers.Add(7);
// result НЕ содержит 7 — это снимок
```

### Что отложенное, что немедленное?

| Отложенное | Немедленное |
|------------|-------------|
| Where, Select, OrderBy | ToList, ToArray, ToDictionary |
| Skip, Take, Zip | Count, Sum, Average, Min, Max |
| GroupBy, Join | First, Single, Last |
| SelectMany, Distinct | Any, All, Contains |
| Chunk, Union, Except | Aggregate |

---

### LINQ и производительность

```csharp
// ⚠️ LINQ красив, но иногда медленнее ручного цикла

// Медленно: создаёт промежуточные итераторы
var result = numbers
    .Where(n => n > 0)
    .Select(n => n * 2)
    .Where(n => n < 100)
    .ToList();

// Быстрее для горячих путей:
List<int> result2 = new(numbers.Count);
foreach (int n in numbers)
{
    if (n > 0)
    {
        int doubled = n * 2;
        if (doubled < 100)
            result2.Add(doubled);
    }
}

// ⚠️ Многократное перечисление
IEnumerable<int> query = GetNumbers().Where(n => n > 0);
Console.WriteLine(query.Count()); // перечислил 1-й раз
Console.WriteLine(query.Sum());   // перечислил 2-й раз!
// ✅ Материализуй один раз:
var list = query.ToList();
Console.WriteLine(list.Count);
Console.WriteLine(list.Sum());
```

---

## Практический пример

```csharp
record Player(string Name, int Level, int Score, string Class, bool IsAlive);

List<Player> players =
[
    new("Алиса", 15, 1200, "Маг", true),
    new("Боб", 10, 800, "Воин", true),
    new("Вика", 20, 2000, "Лучник", true),
    new("Гена", 5, 300, "Воин", false),
    new("Дима", 18, 1800, "Маг", true),
];

// Живые маги, отсортированные по уровню
var topMages = players
    .Where(p => p.IsAlive && p.Class == "Маг")
    .OrderByDescending(p => p.Level)
    .Select(p => $"{p.Name} (Lvl {p.Level})")
    .ToList();
// ["Дима (Lvl 18)", "Алиса (Lvl 15)"]

// Статистика по классам
var stats = players
    .Where(p => p.IsAlive)
    .GroupBy(p => p.Class)
    .Select(g => new
    {
        Class = g.Key,
        Count = g.Count(),
        AvgScore = g.Average(p => p.Score),
        MaxLevel = g.Max(p => p.Level)
    });
```

---

## Мини-упражнения
1. **⭐** Отфильтруй список чисел: оставь чётные, отсортируй по убыванию.
2. **⭐** Сгруппируй людей по городу, посчитай количество в каждой группе.
3. **⭐⭐** Join двух списков: Orders и Products по ProductId.
4. **⭐⭐** Используй Aggregate для конкатенации строк.
5. **⭐⭐** Chunk(10) для разбиения на страницы.

### Практика Uno Platform ⭐⭐⭐
**"Аналитика продаж"** — генерируй данные, фильтруй по дате/региону, группируй по продавцу, топ-10 товаров, средний чек.

### Практика Godot ⭐⭐⭐
**Система целей AI** — `enemies.Where(e => e.IsAlive).OrderBy(e => pos.DistanceTo(e.Pos)).FirstOrDefault()`. Группировка врагов по типу.

---

## Что дальше
Дальше — **обработка исключений** (4.4).
