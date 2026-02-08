# МАСТЕР-ПЛАН ОБУЧЕНИЯ C# — Polyglot Notebooks

> **Версии:** C# 14, .NET 10, Godot 4.6, Uno Platform 6.4
> **Формат:** Polyglot Notebooks (.dib) — теория + код + упражнения в одном файле

## Как работать

1. Проходи уровни по порядку: Level 1 → 2 → 3 → 4 → 5
2. Внутри уровня — иди по номерам файлов (01, 02, 03...)
3. ★ NEW = тема появляется впервые на этом уровне
4. Темы без ★ — возврат к теме с прошлого уровня, но глубже (спиральное обучение)
5. `_docs/` в каждом уровне — справочные markdown-файлы для дополнительного чтения
6. Godot/ и Uno/ — отдельные платформенные треки, начинай после Level 2
7. `_Будущее/` — модули 12-20 (БД, сеть, тесты и др.), будут оформлены когда дойдёшь

### Условные обозначения
- ⬜ — не начато
- 🔄 — в процессе
- ✅ — пройдено
- 🔒 — отложено (изучать когда понадобится)

---

# LEVEL 1 — ОСНОВЫ

> **Цель:** Научиться писать простые программы на C#
> **Предварительные знания:** Нет
> **Файл:** `Level_1_Основы/*.dib`

## 01_Синтаксис.dib
- ⬜ Top-level statements (программа без class/Main)
- ⬜ Точка с запятой `;` и блоки кода `{ }`
- ⬜ Комментарии: `//`, `/* */`, `///`
- ⬜ Пространства имён `namespace`
- ⬜ File-scoped namespaces (`namespace MyApp;`)
- ⬜ Директива `using`
- ⬜ Implicit usings (.NET 6+)

## 02_Типы_данных.dib
- ⬜ Целочисленные: `int`, `long`
- ⬜ С плавающей точкой: `float`, `double`, `decimal`
- ⬜ Разница float vs double vs decimal (точность, когда что)
- ⬜ Логический: `bool`
- ⬜ Символ: `char`
- ⬜ Строка: `string`
- ⬜ Значения по умолчанию: `default(T)`
- ⬜ Значимые vs ссылочные типы (обзор)

## 03_Переменные.dib
- ⬜ Объявление и инициализация переменных
- ⬜ Ключевое слово `var` (вывод типа)
- ⬜ Множественное присваивание
- ⬜ Правила именования (camelCase)
- ⬜ Вывод в консоль: `Console.WriteLine()`
- ⬜ Строковая интерполяция: `$"Привет, {name}!"`

## 04_Операторы.dib
- ⬜ Арифметические: `+`, `-`, `*`, `/`, `%`
- ⬜ Целочисленное деление vs деление с плавающей точкой
- ⬜ Операторы сравнения: `==`, `!=`, `<`, `>`, `<=`, `>=`
- ⬜ Логические: `&&` (И), `||` (ИЛИ), `!` (НЕ)
- ⬜ Инкремент / декремент: `++`, `--`
- ⬜ Разница `i++` vs `++i`
- ⬜ Составное присваивание: `+=`, `-=`, `*=`, `/=`

## 05_Условия.dib
- ⬜ `if` / `else if` / `else`
- ⬜ Вложенные условия
- ⬜ `switch` (классический, по значению)
- ⬜ `switch` по строкам и enum
- ⬜ `break` в switch

## 06_Циклы.dib
- ⬜ `for` — цикл со счётчиком
- ⬜ `while` — цикл с условием
- ⬜ `do...while` — цикл с постусловием
- ⬜ `foreach` — перебор коллекций
- ⬜ `break` — выход из цикла
- ⬜ `continue` — пропуск итерации

## 07_Методы.dib
- ⬜ Объявление метода: возвращаемый тип, имя, параметры
- ⬜ `void` — метод без возвращаемого значения
- ⬜ `return` — возврат результата
- ⬜ Вызов методов
- ⬜ Методы с несколькими параметрами
- ⬜ Когда `void` vs когда `return`

## 08_Массивы.dib
- ⬜ Объявление и инициализация: `int[] arr = {1, 2, 3}`
- ⬜ Доступ по индексу (от 0)
- ⬜ Свойство `Length`
- ⬜ Перебор `for` и `foreach`
- ⬜ Изменение элементов
- ⬜ Выход за границы массива (IndexOutOfRangeException)

## 09_Строки.dib
- ⬜ Создание строк
- ⬜ Конкатенация `+` vs интерполяция `$""`
- ⬜ `Length` — длина строки
- ⬜ `ToUpper()`, `ToLower()`
- ⬜ `Trim()`, `TrimStart()`, `TrimEnd()`
- ⬜ `Contains()`, `StartsWith()`, `EndsWith()`
- ⬜ `Replace()`, `Remove()`
- ⬜ `Split()` и `string.Join()`
- ⬜ `IndexOf()`, `Substring()`

---

# LEVEL 2 — УГЛУБЛЕНИЕ

> **Цель:** Углубить основы + войти в ООП
> **Предварительные знания:** Level 1 пройден
> **Файл:** `Level_2_Углубление/*.dib`

## 01_Типы_данных.dib
- ⬜ Nullable типы: `int?`, `bool?`, `string?`
- ⬜ `HasValue` и `Value`
- ⬜ Оператор `??` (null-coalescing)
- ⬜ Оператор `?.` (null-conditional)
- ⬜ Оператор `??=` (null-coalescing assignment)
- ⬜ `const` — константы времени компиляции
- ⬜ `readonly` — только чтение
- ⬜ `const` vs `readonly` — когда что
- ⬜ `var` глубже — когда использовать, когда нет
- ⬜ Приведение типов: `(int)`, `(double)`, `Convert.ToInt32()`

## 02_Операторы.dib
- ⬜ Тернарный оператор: `условие ? да : нет`
- ⬜ Null-операторы: `??`, `?.`, `??=`, `!`
- ⬜ Побитовые: `&`, `|`, `^`, `~`, `<<`, `>>`
- ⬜ Практическое применение побитовых (флаги, маски)
- ⬜ `checked` / `unchecked` (переполнение)
- ⬜ Приоритет операторов (таблица)

## 03_Условия.dib
- ⬜ Switch expressions (C# 8+): `var result = x switch { ... }`
- ⬜ Паттерны в switch: relational (`> 0`), logical (`and`, `or`)
- ⬜ Когда `if` vs `switch` vs switch expression
- ⬜ Guard clauses — ранний выход вместо вложенности

## 04_Методы.dib
- ⬜ Перегрузка методов (одинаковое имя, разные параметры)
- ⬜ Параметры по ссылке: `ref`
- ⬜ Выходные параметры: `out`
- ⬜ Входные параметры: `in` (readonly ref)
- ⬜ `params` — массив параметров переменной длины
- ⬜ Значения по умолчанию: `void Foo(int x = 10)`
- ⬜ Именованные аргументы: `Foo(name: "test", age: 25)`

## 05_Массивы.dib
- ⬜ Многомерные массивы: `int[,]`
- ⬜ Зубчатые массивы (jagged): `int[][]`
- ⬜ Методы Array: `Sort()`, `Reverse()`, `IndexOf()`
- ⬜ `Array.Find()`, `Array.FindAll()`, `Array.Exists()`
- ⬜ `Array.Copy()`, `Array.Resize()`
- ⬜ Index с конца: `arr[^1]`, `arr[^2]`
- ⬜ Range (срезы): `arr[1..3]`, `arr[..3]`, `arr[2..]`

## 06_Строки.dib
- ⬜ `StringBuilder` — зачем и когда (правило ~10 конкатенаций)
- ⬜ `Append()`, `Insert()`, `Remove()`, `Replace()`, `ToString()`
- ⬜ Форматирование чисел: N, F, C, P, D, X
- ⬜ Кастомные форматы: `#,##0.00`
- ⬜ Verbatim-строки: `@"C:\path\file"`
- ⬜ Raw string literals: `"""..."""` (C# 11)
- ⬜ Сравнение строк: `==`, `Equals()`, `string.Compare()`
- ⬜ `StringComparison` (Ordinal, OrdinalIgnoreCase, CurrentCulture)

## 07_Классы.dib ★ NEW
- ⬜ Что такое класс и объект (аналогия: чертёж → дом)
- ⬜ Поля (fields)
- ⬜ Свойства (properties): `get`, `set`
- ⬜ Auto-properties: `public string Name { get; set; }`
- ⬜ Конструктор по умолчанию
- ⬜ Параметризованный конструктор
- ⬜ Ключевое слово `this`
- ⬜ Модификаторы доступа: `public`, `private`, `internal`
- ⬜ `static` члены и `static` классы
- ⬜ Expression-bodied members: `=>`

## 08_Структуры.dib ★ NEW
- ⬜ `struct` — что это и зачем
- ⬜ Значимый тип vs ссылочный тип (стек vs куча)
- ⬜ Копирование struct vs копирование class
- ⬜ `readonly struct`
- ⬜ Когда struct, когда class (правила выбора)

## 09_Перечисления.dib ★ NEW
- ⬜ Объявление `enum`
- ⬜ Базовый тип enum (int по умолчанию)
- ⬜ Явное задание значений
- ⬜ `[Flags]` — побитовые флаги
- ⬜ `Enum.Parse()`, `Enum.TryParse()`
- ⬜ `Enum.GetValues()`, `Enum.GetNames()`
- ⬜ `HasFlag()` — проверка флагов

## 10_Дата_и_время.dib ★ NEW
- ⬜ `DateTime.Now`, `DateTime.UtcNow`, `DateTime.Today`
- ⬜ Свойства: Year, Month, Day, Hour, Minute, Second
- ⬜ Создание конкретной даты: `new DateTime(2026, 2, 8)`
- ⬜ Арифметика: `AddDays()`, `AddHours()`, разница дат
- ⬜ `TimeSpan` — промежуток времени
- ⬜ `DateOnly` и `TimeOnly` (.NET 6+)
- ⬜ Форматирование дат
- ⬜ `Stopwatch` — измерение производительности

## 11_Regex.dib ★ NEW
- ⬜ `Regex.IsMatch()` — проверка соответствия
- ⬜ `Regex.Match()` — поиск первого совпадения
- ⬜ `Regex.Matches()` — все совпадения
- ⬜ `Regex.Replace()` — замена
- ⬜ Метасимволы: `.`, `\d`, `\w`, `\s`, `\b`
- ⬜ Квантификаторы: `*`, `+`, `?`, `{n}`, `{n,m}`
- ⬜ Классы символов: `[a-z]`, `[^0-9]`
- ⬜ Якоря: `^`, `$`
- ⬜ Группы: `()`, именованные `(?<name>...)`

---

# LEVEL 3 — ООП И КОЛЛЕКЦИИ

> **Цель:** Освоить объектно-ориентированное программирование и основные структуры данных
> **Предварительные знания:** Level 1-2 пройдены (знаешь классы, struct, enum)
> **Файл:** `Level_3_ООП/*.dib`

## 01_Типы_данных.dib
- ⬜ `record class` — что это и зачем
  - ⬜ Позиционный синтаксис: `record Person(string Name, int Age)`
  - ⬜ Value equality (сравнение по значению)
  - ⬜ `with` — создание копии с изменениями
  - ⬜ Деконструкция: `var (name, age) = person`
- ⬜ `record struct` (C# 10)
- ⬜ Кортежи (ValueTuple)
  - ⬜ `(int x, string name)` — именованные элементы
  - ⬜ Возврат нескольких значений из метода
  - ⬜ Деконструкция кортежей
- ⬜ Collection expressions (C# 12)
  - ⬜ `int[] arr = [1, 2, 3]`
  - ⬜ `List<int> list = [1, 2, 3]`
  - ⬜ Spread operator: `[..first, ..second]`
- ⬜ Анонимные типы: `var obj = new { Name = "Test" }`

## 02_Методы.dib
- ⬜ Extension methods
  - ⬜ Синтаксис `this` в первом параметре
  - ⬜ Создание своих расширений для string, int, коллекций
- ⬜ Локальные функции (вложенные методы)
- ⬜ `static` локальные функции
- ⬜ Рекурсия углублённо
  - ⬜ Fibonacci (наивный vs мемоизированный)
  - ⬜ Бинарный поиск

## 03_Классы.dib
- ⬜ Primary constructors (C# 12): `class Person(string name, int age)`
  - ⬜ Захват параметров (НЕ становятся полями автоматически!)
- ⬜ `required` свойства (C# 11) — обязательная инициализация
- ⬜ `init`-only сеттеры (C# 9)
- ⬜ Цепочка конструкторов: `this()`
- ⬜ Статические конструкторы
- ⬜ `file class` (C# 11) — видимость только в файле

## 04_Наследование.dib ★ NEW
- ⬜ Базовый и производный класс: `: BaseClass`
- ⬜ Ключевое слово `base` (вызов конструктора/метода родителя)
- ⬜ Виртуальные методы: `virtual` и `override`
- ⬜ Сокрытие метода: `new` (и почему лучше не использовать)
- ⬜ `sealed` — запрет наследования/переопределения
- ⬜ Абстрактные классы: `abstract class`
- ⬜ Абстрактные методы: `abstract void Method()`
- ⬜ Переопределение `ToString()`, `Equals()`, `GetHashCode()`

## 05_Полиморфизм.dib ★ NEW
- ⬜ Полиморфизм через наследование (массив базового типа)
- ⬜ Полиморфизм через интерфейсы
- ⬜ Проверка типа: `is` (с pattern matching)
- ⬜ Приведение: `as` (безопасное, возвращает null)
- ⬜ Приведение: `(Type)obj` (жёсткое, бросает exception)
- ⬜ `typeof()` и `GetType()`

## 06_Инкапсуляция.dib ★ NEW
- ⬜ Приватные поля + публичные свойства с валидацией
- ⬜ Сравнение: `init` vs `set` vs `private set`
- ⬜ `readonly` поля
- ⬜ Построение иммутабельных классов
- ⬜ record как простейший иммутабельный тип

## 07_Интерфейсы.dib ★ NEW
- ⬜ Определение интерфейса: методы, свойства
- ⬜ Реализация интерфейса классом
- ⬜ Множественная реализация (класс : IFoo, IBar)
- ⬜ Явная реализация (explicit): `void IFoo.Method()`
- ⬜ Default interface methods (C# 8+)
- ⬜ Стандартные интерфейсы .NET:
  - ⬜ `IComparable<T>` — сравнение/сортировка
  - ⬜ `IEquatable<T>` — равенство
  - ⬜ `IEnumerable<T>` — перебор foreach
  - ⬜ `IDisposable` — освобождение ресурсов + `using`

## 08_Коллекции.dib ★ NEW
- ⬜ `List<T>`
  - ⬜ Add, Remove, RemoveAt, Insert, Contains, IndexOf
  - ⬜ Count vs Capacity
  - ⬜ Sort, Reverse, FindAll
- ⬜ `Dictionary<TKey, TValue>`
  - ⬜ Add, Remove, ContainsKey
  - ⬜ `TryGetValue()` — безопасный доступ (!)
  - ⬜ Перебор KeyValuePair
- ⬜ `HashSet<T>`
  - ⬜ Уникальные элементы
  - ⬜ Операции множеств: UnionWith, IntersectWith, ExceptWith
- ⬜ `Queue<T>` — FIFO (Enqueue, Dequeue, Peek)
- ⬜ `Stack<T>` — LIFO (Push, Pop, Peek)
- ⬜ `PriorityQueue<TElement, TPriority>` (.NET 6+)
- ⬜ Выбор коллекции: когда что использовать (таблица Big O)

## 09_Исключения.dib ★ NEW
- ⬜ `try` / `catch` / `finally`
- ⬜ Множественные `catch` (от конкретного к общему)
- ⬜ Фильтры `when`: `catch (Exception ex) when (ex.Message.Contains(...))`
- ⬜ Иерархия исключений: NullReferenceException, ArgumentException...
- ⬜ Кастомные исключения: `class MyException : Exception`
- ⬜ `throw;` vs `throw ex;` (сохранение stack trace)
- ⬜ Guard clauses: `ArgumentNullException.ThrowIfNull()` (.NET 6+)
- ⬜ Try-паттерн: `TryParse`, `TryGetValue`

## 10_Отладка.dib ★ NEW
- ⬜ Типы breakpoints
  - ⬜ Обычный breakpoint
  - ⬜ Conditional breakpoint (условие)
  - ⬜ Tracepoint / Logpoint (без остановки)
- ⬜ Окна отладки: Watch, Locals, Immediate Window
- ⬜ Навигация: Step Over (F10), Step Into (F11), Step Out (Shift+F11)
- ⬜ Call Stack — стек вызовов
- ⬜ Debug vs Release конфигурация
- ⬜ `#if DEBUG`, `Debug.Assert()`
- ⬜ `Stopwatch` для профилирования

---

# LEVEL 4 — ПРОДВИНУТЫЙ C#

> **Цель:** Освоить мощные возможности языка
> **Предварительные знания:** Level 1-3 пройдены (ООП, коллекции, исключения)
> **Файл:** `Level_4_Продвинутый/*.dib`

## 01_Типы_данных.dib
- ⬜ Generics — обобщения
  - ⬜ Обобщённые классы: `class Box<T>`
  - ⬜ Обобщённые методы: `T Max<T>(T a, T b)`
  - ⬜ Ограничения (constraints):
    - ⬜ `where T : class` / `struct` / `new()`
    - ⬜ `where T : IInterface` / `BaseClass`
    - ⬜ `where T : notnull`
  - ⬜ Ковариантность: `out T` (IEnumerable<Dog> → IEnumerable<Animal>)
  - ⬜ Контравариантность: `in T`
- ⬜ `Span<T>` — срез без копирования
- ⬜ `ReadOnlySpan<T>`
- ⬜ `Memory<T>` — когда Span не подходит (async)

## 02_Операторы.dib
- ⬜ Перегрузка операторов: `+`, `-`, `*`, `==`, `!=`
- ⬜ Индексаторы: `this[int index]`
- ⬜ Индексаторы с разными ключами: `this[string key]`
- ⬜ Неявное приведение: `implicit operator`
- ⬜ Явное приведение: `explicit operator`

## 03_Методы.dib
- ⬜ Делегаты
  - ⬜ Объявление: `delegate int MathOp(int a, int b)`
  - ⬜ Многоадресные делегаты (цепочка вызовов)
- ⬜ Встроенные делегаты:
  - ⬜ `Action` / `Action<T>` — без возвращаемого значения
  - ⬜ `Func<T, TResult>` — с возвращаемым значением
  - ⬜ `Predicate<T>` — возвращает bool
- ⬜ Лямбда-выражения
  - ⬜ `x => x * 2` (expression lambda)
  - ⬜ `x => { ... }` (statement lambda)
  - ⬜ Замыкания (closures) — захват переменных
  - ⬜ Ловушка замыканий в цикле
- ⬜ События (Events)
  - ⬜ `event EventHandler<T>`
  - ⬜ Publisher / Subscriber
  - ⬜ Кастомные EventArgs
  - ⬜ Отписка (предотвращение утечек памяти)

## 04_Коллекции.dib
- ⬜ `ImmutableList<T>`, `ImmutableDictionary`, `ImmutableArray`
- ⬜ `FrozenSet<T>`, `FrozenDictionary` (.NET 8+)
- ⬜ `ConcurrentDictionary`, `ConcurrentQueue`, `ConcurrentBag`
- ⬜ `ObservableCollection<T>` — привязка к UI
- ⬜ Таблица сравнения всех коллекций

## 05_LINQ.dib ★ NEW
- ⬜ Query syntax: `from x in list where ... select ...`
- ⬜ Method syntax: `.Where().Select().OrderBy()`
- ⬜ Фильтрация: `Where`
- ⬜ Проекция: `Select`, `SelectMany`
- ⬜ Сортировка: `OrderBy`, `ThenBy`, `OrderByDescending`
- ⬜ Группировка: `GroupBy`
- ⬜ Объединение: `Join`
- ⬜ Агрегация: `Count`, `Sum`, `Average`, `Min`, `Max`, `Aggregate`
- ⬜ Элементы: `First`, `FirstOrDefault`, `Single`, `Any`, `All`
- ⬜ Разбиение: `Take`, `Skip`, `Distinct`, `Chunk`
- ⬜ Материализация: `ToList()`, `ToArray()`, `ToDictionary()`
- ⬜ Отложенное выполнение (deferred execution)

## 06_Async.dib ★ NEW
- ⬜ `async` / `await` — основы
- ⬜ `Task` и `Task<T>`
- ⬜ `ValueTask<T>` — когда использовать
- ⬜ Параллельное выполнение: `Task.WhenAll()`
- ⬜ Гонка задач: `Task.WhenAny()`
- ⬜ Фоновая работа: `Task.Run()`
- ⬜ Отмена: `CancellationToken` / `CancellationTokenSource`
- ⬜ `IAsyncEnumerable<T>` и `await foreach`
- ⬜ Проблемы:
  - ⬜ `async void` — почему плохо
  - ⬜ Deadlock через `.Result`
  - ⬜ Забытый `await`

## 07_Pattern_Matching.dib ★ NEW
- ⬜ Типовой: `is Type variable`
- ⬜ Константный: `is null`, `is 42`
- ⬜ Реляционный: `is > 0`, `is >= -30 and < 0`
- ⬜ Логический: `and`, `or`, `not`
- ⬜ Свойств: `is { Age: >= 18, Name.Length: > 0 }`
- ⬜ Позиционный (деконструкция)
- ⬜ Списков: `is [1, 2, .., var last]` (C# 11)
- ⬜ Switch expressions с паттернами + `when`

## 08_Файлы.dib ★ NEW
- ⬜ Простое чтение/запись:
  - ⬜ `File.ReadAllText()`, `File.WriteAllText()`
  - ⬜ `File.ReadAllLines()`, `File.AppendAllText()`
- ⬜ Потоковое: `StreamReader`, `StreamWriter`
- ⬜ Бинарное: `FileStream`
- ⬜ `using` / `await using` и `IDisposable`
- ⬜ Класс `Path`: `Combine`, `GetFileName`, `GetExtension`
- ⬜ JSON сериализация (`System.Text.Json`):
  - ⬜ `JsonSerializer.Serialize()` / `Deserialize()`
  - ⬜ `[JsonPropertyName]`, `[JsonIgnore]`
  - ⬜ `JsonSerializerOptions`

## 09_Функциональное.dib ★ NEW
- ⬜ Функции высшего порядка (функция как параметр / результат)
- ⬜ Замыкания глубже: состояние, мемоизация
- ⬜ Каррирование: `f(a, b, c)` → `f(a)(b)(c)`
- ⬜ Частичное применение
- ⬜ Композиция функций
- ⬜ Пайплайн (цепочка преобразований)

---

# LEVEL 5 — АРХИТЕКТУРА

> **Цель:** Проектирование, паттерны, профессиональные инструменты
> **Предварительные знания:** Level 1-4 пройдены
> **Файл:** `Level_5_Архитектура/*.dib`

## 01_Рефлексия.dib ★ NEW
- ⬜ `typeof()` и `GetType()` — получение типа
- ⬜ Класс `Type`: Name, Namespace, BaseType, IsAbstract...
- ⬜ `GetMethods()`, `GetProperties()`, `GetFields()`
- ⬜ `GetCustomAttributes()` — чтение атрибутов
- ⬜ `Activator.CreateInstance()` — динамическое создание объекта
- ⬜ `MethodInfo.Invoke()` — динамический вызов метода
- ⬜ Стандартные атрибуты: `[Obsolete]`, `[Conditional]`
- ⬜ Создание своих атрибутов
- ⬜ Generic attributes (C# 11): `[MyAttribute<string>]`

## 02_Многопоточность.dib ★ NEW
- ⬜ `Thread` — создание потоков
- ⬜ `Thread.Sleep()`, `Thread.Join()`
- ⬜ Race condition — гонка потоков (проблема)
- ⬜ `lock` — синхронизация (решение)
- ⬜ `Monitor`, `Mutex`
- ⬜ `SemaphoreSlim` — ограничение параллелизма
- ⬜ `Interlocked` — атомарные операции
- ⬜ `Task.Run()` vs `Thread` — когда что
- ⬜ `Channel<T>` — producer/consumer

## 03_Память.dib ★ NEW
- ⬜ Garbage Collector
  - ⬜ Поколения: 0, 1, 2
  - ⬜ Large Object Heap (LOH)
  - ⬜ Как GC решает что собирать
- ⬜ `IDisposable` и Dispose pattern
  - ⬜ Полный паттерн с финализатором
  - ⬜ `using` / `await using`
- ⬜ `ArrayPool<T>` — переиспользование массивов
- ⬜ `Span<T>` углублённо
- ⬜ `stackalloc` — аллокация на стеке
- ⬜ `WeakReference<T>` — слабые ссылки

## 04_SOLID.dib ★ NEW
- ⬜ **S** — Single Responsibility (одна причина для изменения)
- ⬜ **O** — Open/Closed (открыт для расширения, закрыт для изменения)
- ⬜ **L** — Liskov Substitution (подтип заменяет базовый тип)
- ⬜ **I** — Interface Segregation (маленькие интерфейсы лучше)
- ⬜ **D** — Dependency Inversion (зависимость от абстракций)
- ⬜ Каждый принцип: пример нарушения → рефакторинг → правильный код

## 05_Паттерны.dib ★ NEW
- ⬜ **Singleton** — единственный экземпляр (thread-safe с `Lazy<T>`)
- ⬜ **Factory Method** — создание через метод, не `new`
- ⬜ **Strategy** — семейство алгоритмов через интерфейс
- ⬜ **Observer** — подписка на события
- ⬜ **State** — конечный автомат (FSM) для состояний
- ⬜ **Command** — действие как объект (undo/redo)
- ⬜ **Builder** — пошаговое построение сложного объекта
- ⬜ **Decorator** — обёртка для добавления поведения

## 06_Архитектура.dib ★ NEW
- ⬜ MVVM (Model-View-ViewModel)
  - ⬜ `INotifyPropertyChanged`
  - ⬜ `ICommand` / `RelayCommand`
  - ⬜ Data Binding
- ⬜ Clean Architecture
  - ⬜ Domain → Application → Infrastructure → Presentation
  - ⬜ Зависимости направлены внутрь
- ⬜ Repository pattern
- ⬜ CQRS (Command Query Responsibility Segregation) — обзор

## 07_DI.dib ★ NEW
- ⬜ Что такое DI и зачем (проблема жёстких зависимостей)
- ⬜ Constructor injection
- ⬜ `Microsoft.Extensions.DependencyInjection`:
  - ⬜ `AddTransient<T>` — новый каждый раз
  - ⬜ `AddScoped<T>` — один на scope
  - ⬜ `AddSingleton<T>` — один на всё приложение
  - ⬜ `IServiceProvider`
- ⬜ Регистрация интерфейс → реализация
- ⬜ DI в реальном приложении

## 08_Логирование.dib ★ NEW
- ⬜ `Debug.WriteLine()`, `Trace.WriteLine()`
- ⬜ `Microsoft.Extensions.Logging`
  - ⬜ `ILogger`, `ILoggerFactory`
  - ⬜ Уровни: Trace, Debug, Information, Warning, Error, Critical
- ⬜ Structured logging: `"User {UserId} logged in"` (плейсхолдеры, не интерполяция!)
- ⬜ `LoggerMessage.Define()` — высокопроизводительное
- ⬜ Serilog — обзор: Sinks, Enrichers

## 09_Стиль_кода.dib ★ NEW
- ⬜ Именование .NET:
  - ⬜ PascalCase — публичные (ClassName, MethodName, PropertyName)
  - ⬜ camelCase — параметры, локальные переменные
  - ⬜ _camelCase — приватные поля
  - ⬜ I — префикс интерфейсов (IDisposable)
  - ⬜ T — префикс generic параметров
- ⬜ `.editorconfig` — правила стиля для проекта
- ⬜ Roslyn analyzers, StyleCop
- ⬜ XML-документация: `<summary>`, `<param>`, `<returns>`

---

# ОТДЕЛЬНЫЕ ТРЕКИ

## Godot (после Level 2+)
> Файлы: `Notebooks/Godot/`
> Справочник: `Notebooks/Godot/_docs/`

- ⬜ Архитектура Godot: сцены, узлы, дерево сцен
- ⬜ Жизненный цикл: `_Ready()`, `_Process()`, `_PhysicsProcess()`
- ⬜ C# в Godot: `[Export]`, маршалинг, `[GlobalClass]`, `[Tool]`
- ⬜ Сигналы: встроенные, кастомные, `await ToSignal()`
- ⬜ Ввод: Input Map, `_Input()`, `_UnhandledInput()`
- ⬜ Физика (Jolt): CharacterBody, RigidBody, Area, RayCast
- ⬜ Анимации: AnimationPlayer, AnimationTree, Tween
- ⬜ 2D: TileMapLayer, Camera2D, Parallax, частицы
- ⬜ 3D: MeshInstance3D, освещение, материалы
- ⬜ UI в играх: Control, Theme, HUD
- ⬜ Аудио: AudioStreamPlayer, AudioBus
- ⬜ AI: NavigationAgent, FSM
- ⬜ Математика: Vector2/3, Transform, тригонометрия
- ⬜ Шейдеры и VFX
- ⬜ Game Feel: screenshake, hitstop, particles
- ⬜ Ресурсы: Resource, PackedScene
- ⬜ Локализация и экспорт

## Uno Platform (после Level 2+)
> Файлы: `Notebooks/Uno/`
> Справочник: `Notebooks/Uno/_docs/`

- ⬜ Основы XAML: элементы, атрибуты, Layout (Grid, StackPanel)
- ⬜ Элементы управления: Button, TextBox, ListView, ComboBox...
- ⬜ Data Binding: `{Binding}`, `{x:Bind}`, OneWay, TwoWay
- ⬜ Навигация: Frame.Navigate(), NavigationView
- ⬜ Стили и ресурсы: StaticResource, ThemeResource, темы
- ⬜ Анимации: Storyboard, VisualStateManager
- ⬜ Custom Controls: DependencyProperty, UserControl
- ⬜ MVVM Toolkit: `[ObservableProperty]`, `[RelayCommand]`, Messenger
- ⬜ Работа с данными: HttpClient, SQLite, FileOpenPicker
- ⬜ Адаптивный UI: адаптивные триггеры, разные размеры экрана
- ⬜ Локализация: `x:Uid`, `.resw`
- ⬜ Accessibility: AutomationProperties, клавиатурная навигация
- ⬜ C# Markup (альтернатива XAML)
- ⬜ Платформо-специфичный код: `#if __ANDROID__`...
- ⬜ Uno.Extensions

---

# БУДУЩЕЕ (модули 12-20)
> Файлы: `Notebooks/_Будущее/`
> Будут оформлены в Level 6+ когда дойдёшь

- ⬜ **Базы данных:** SQL основы, EF Core, Dapper, SQLite
- ⬜ **Сеть:** HttpClient, REST API, WebSockets, SignalR, мультиплеер Godot
- ⬜ **Тестирование:** Unit-тесты (xUnit), интеграционные тесты, GdUnit4
- ⬜ **Безопасность:** криптография, хеширование, аутентификация
- ⬜ **Оптимизация:** BenchmarkDotNet, профилирование, ArrayPool, Span
- ⬜ **CI/CD:** Git advanced, GitHub Actions, Docker
- ⬜ **ASP.NET Core:** Minimal API, SignalR, Identity, Middleware
- ⬜ **Продвинутый .NET:** Source Generators, NativeAOT, IO Pipelines, Polly
- ⬜ **Итоговые проекты:** финальный Uno + финальный Godot
