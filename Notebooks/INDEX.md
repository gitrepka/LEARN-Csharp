# МАСТЕР-ПЛАН ОБУЧЕНИЯ C# — Polyglot Notebooks

> **Версии:** C# 14, .NET 10, Godot 4.6, Uno Platform 6.4
> **Формат:** Polyglot Notebooks (.dib) — теория + код + упражнения в одном файле

## Как работать

1. Проходи уровни по порядку: Level 1 → 2 → 3 → 4 → 5
2. Внутри уровня — иди по папкам (01, 02, 03...), внутри папки — по файлам (01, 02, 03...)
3. Каждый `.dib` файл = один мини-урок (15-30 мин): мотивация → теория → примеры → упражнение → вопросы
4. ★ NEW = тема появляется впервые на этом уровне
5. Темы без ★ — возврат к теме с прошлого уровня, но глубже (спиральное обучение)
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
> **Путь:** `Level_1_Основы/`

## 01_Синтаксис/
- ⬜ **01_Первая_программа.dib**
  - ⬜ Top-level statements (программа без class/Main)
  - ⬜ Комментарии: `//`, `/* */`, `///`
  - ⬜ Пространства имён `namespace`, file-scoped namespaces
  - ⬜ Директива `using`, implicit usings
  - ⬜ Точка с запятой `;` и блоки кода `{ }`

## 02_Типы_данных/
- ⬜ **01_Числа.dib**
  - ⬜ Целочисленные: `int`
  - ⬜ С плавающей точкой: `float`, `double`
  - ⬜ Точные деньги: `decimal`
- ⬜ **02_Текст_и_логика.dib**
  - ⬜ Строка: `string`
  - ⬜ Символ: `char`
  - ⬜ Логический: `bool`
- ⬜ **03_Default_и_категории.dib**
  - ⬜ Значения по умолчанию: `default(T)`
  - ⬜ Значимые vs ссылочные типы (обзор)

## 03_Переменные/
- ⬜ **01_Переменные.dib**
  - ⬜ Объявление и инициализация переменных
  - ⬜ Ключевое слово `var` (вывод типа)
  - ⬜ Множественное присваивание
  - ⬜ Правила именования (camelCase)
  - ⬜ Вывод в консоль: `Console.WriteLine()`
  - ⬜ Строковая интерполяция: `$"Привет, {name}!"`

## 04_Операторы/
- ⬜ **01_Арифметика.dib**
  - ⬜ Арифметические: `+`, `-`, `*`, `/`, `%`
  - ⬜ Целочисленное деление vs дробное
  - ⬜ Остаток от деления (модуло)
- ⬜ **02_Сравнение_и_логика.dib**
  - ⬜ Операторы сравнения: `==`, `!=`, `<`, `>`, `<=`, `>=`
  - ⬜ Логические: `&&` (И), `||` (ИЛИ), `!` (НЕ)
  - ⬜ Комбинирование логических операторов
- ⬜ **03_Инкремент_и_присваивание.dib**
  - ⬜ Инкремент / декремент: `++`, `--`, разница `i++` vs `++i`
  - ⬜ Составное присваивание: `+=`, `-=`, `*=`, `/=`

## 05_Условия/
- ⬜ **01_If_Else.dib**
  - ⬜ `if` / `else if` / `else`
  - ⬜ Вложенные условия
- ⬜ **02_Switch.dib**
  - ⬜ `switch` (классический, по значению)
  - ⬜ `switch` по строкам
  - ⬜ `break` в switch, `default`

## 06_Циклы/
- ⬜ **01_For.dib**
  - ⬜ `for` — цикл со счётчиком
  - ⬜ Суммирование и подсчёт
- ⬜ **02_While.dib**
  - ⬜ `while` — цикл с предусловием
  - ⬜ `do...while` — цикл с постусловием
- ⬜ **03_Foreach_и_управление.dib**
  - ⬜ `foreach` — перебор коллекций
  - ⬜ `break` — выход из цикла
  - ⬜ `continue` — пропуск итерации

## 07_Методы/
- ⬜ **01_Void_методы.dib**
  - ⬜ `void` — метод без возвращаемого значения
  - ⬜ Методы с параметрами
  - ⬜ Вызов методов
- ⬜ **02_Параметры_и_return.dib**
  - ⬜ `return` — возврат результата
  - ⬜ Методы с несколькими параметрами
  - ⬜ Когда `void` vs когда `return`
  - ⬜ Методы вызывают другие методы

## 08_Массивы/
- ⬜ **01_Массивы.dib**
  - ⬜ Объявление и инициализация: `int[] arr = {1, 2, 3}`
  - ⬜ Доступ по индексу (от 0)
  - ⬜ Свойство `Length`
  - ⬜ Перебор `for` и `foreach`
  - ⬜ Изменение элементов
  - ⬜ Выход за границы массива (IndexOutOfRangeException)

## 09_Строки/
- ⬜ **01_Основы_строк.dib**
  - ⬜ Создание строк
  - ⬜ Конкатенация `+` vs интерполяция `$""`
  - ⬜ `Length` — длина строки
- ⬜ **02_Методы_строк.dib**
  - ⬜ `ToUpper()`, `ToLower()`
  - ⬜ `Trim()`, `TrimStart()`, `TrimEnd()`
  - ⬜ `Contains()`
- ⬜ **03_Replace_и_Split.dib**
  - ⬜ `Replace()`
  - ⬜ `Split()` и `string.Join()`

---

# LEVEL 2 — УГЛУБЛЕНИЕ

> **Цель:** Углубить основы + войти в ООП
> **Предварительные знания:** Level 1 пройден
> **Путь:** `Level_2_Углубление/`

## 01_Типы_данных/
- ⬜ **01_Nullable_и_null.dib**
  - ⬜ Nullable типы: `int?`, `bool?`
  - ⬜ `HasValue` и `Value`
  - ⬜ Оператор `??` (null-coalescing)
  - ⬜ Оператор `?.` (null-conditional)
  - ⬜ Оператор `??=` (null-coalescing assignment)
- ⬜ **02_Const_и_readonly.dib**
  - ⬜ `const` — константы времени компиляции
  - ⬜ `readonly` — только чтение
  - ⬜ `const` vs `readonly` — когда что
  - ⬜ `var` глубже — когда использовать, когда нет
- ⬜ **03_Приведение_типов.dib**
  - ⬜ Приведение типов: `(int)`, `(double)`
  - ⬜ `Convert.ToInt32()`, `int.Parse()`, `int.TryParse()`

## 02_Операторы/
- ⬜ **01_Тернарный_и_null.dib**
  - ⬜ Тернарный оператор: `условие ? да : нет`
  - ⬜ Null-операторы: `??`, `?.`, `??=`, `!`
- ⬜ **02_Побитовые_и_приоритет.dib**
  - ⬜ Побитовые: `&`, `|`, `^`, `~`, `<<`, `>>`
  - ⬜ Практическое применение (флаги, маски)
  - ⬜ `checked` / `unchecked` (переполнение)
  - ⬜ Приоритет операторов (таблица)

## 03_Условия/
- ⬜ **01_Switch_expressions.dib**
  - ⬜ Switch expressions (C# 8+): `var result = x switch { ... }`
  - ⬜ Паттерны: relational (`> 0`), logical (`and`, `or`)
  - ⬜ Когда `if` vs `switch` vs switch expression
  - ⬜ Guard clauses — ранний выход вместо вложенности

## 04_Методы/
- ⬜ **01_Перегрузка.dib**
  - ⬜ Перегрузка методов (одинаковое имя, разные параметры)
- ⬜ **02_Ref_out_params.dib**
  - ⬜ Параметры по ссылке: `ref`
  - ⬜ Выходные параметры: `out`
  - ⬜ Входные параметры: `in` (readonly ref)
  - ⬜ `params` — массив параметров переменной длины
  - ⬜ Значения по умолчанию, именованные аргументы

## 05_Массивы/
- ⬜ **01_Многомерные.dib**
  - ⬜ Многомерные массивы: `int[,]`
  - ⬜ Зубчатые массивы (jagged): `int[][]`
- ⬜ **02_Методы_и_диапазоны.dib**
  - ⬜ Методы Array: `Sort()`, `Reverse()`, `IndexOf()`, `Find()`
  - ⬜ Index с конца: `arr[^1]`
  - ⬜ Range (срезы): `arr[1..3]`, `arr[..3]`, `arr[2..]`

## 06_Строки/
- ⬜ **01_StringBuilder.dib**
  - ⬜ `StringBuilder` — зачем и когда
  - ⬜ `Append()`, `Insert()`, `Remove()`, `Replace()`, `ToString()`
- ⬜ **02_Форматирование.dib**
  - ⬜ Форматирование чисел: N, F, C, P, D, X
  - ⬜ Verbatim-строки: `@"C:\path\file"`
  - ⬜ Raw string literals: `"""..."""` (C# 11)
  - ⬜ Сравнение строк: `==`, `Equals()`, `StringComparison`

## 07_Классы/ ★ NEW
- ⬜ **01_Класс_и_объект.dib**
  - ⬜ Что такое класс и объект
  - ⬜ Поля (fields)
  - ⬜ Свойства (properties): `get`, `set`, auto-properties
- ⬜ **02_Конструкторы.dib**
  - ⬜ Конструктор по умолчанию
  - ⬜ Параметризованный конструктор
  - ⬜ Ключевое слово `this`
- ⬜ **03_Доступ_и_static.dib**
  - ⬜ Модификаторы доступа: `public`, `private`, `internal`
  - ⬜ `static` члены и `static` классы
  - ⬜ Expression-bodied members: `=>`

## 08_Структуры/ ★ NEW
- ⬜ **01_Структуры.dib**
  - ⬜ `struct` — что это и зачем
  - ⬜ Значимый тип vs ссылочный тип
  - ⬜ `readonly struct`
  - ⬜ Когда struct, когда class

## 09_Перечисления/ ★ NEW
- ⬜ **01_Перечисления.dib**
  - ⬜ Объявление `enum`
  - ⬜ `[Flags]` — побитовые флаги
  - ⬜ `Enum.Parse()`, `Enum.TryParse()`
  - ⬜ `HasFlag()` — проверка флагов

## 10_Дата_и_время/ ★ NEW
- ⬜ **01_DateTime.dib**
  - ⬜ `DateTime.Now`, `DateTime.UtcNow`, `DateTime.Today`
  - ⬜ Свойства: Year, Month, Day, Hour, Minute, Second
  - ⬜ Арифметика: `AddDays()`, `AddHours()`, разница дат
  - ⬜ `TimeSpan` — промежуток времени
- ⬜ **02_DateOnly_и_форматы.dib**
  - ⬜ `DateOnly` и `TimeOnly` (.NET 6+)
  - ⬜ Форматирование дат
  - ⬜ `Stopwatch` — измерение производительности

## 11_Regex/ ★ NEW
- ⬜ **01_Основы_Regex.dib**
  - ⬜ `Regex.IsMatch()`, `Regex.Match()`, `Regex.Matches()`
  - ⬜ Метасимволы: `.`, `\d`, `\w`, `\s`, `\b`
  - ⬜ Якоря: `^`, `$`
- ⬜ **02_Продвинутый_Regex.dib**
  - ⬜ Квантификаторы: `*`, `+`, `?`, `{n}`, `{n,m}`
  - ⬜ Классы символов: `[a-z]`, `[^0-9]`
  - ⬜ Группы: `()`, именованные `(?<name>...)`
  - ⬜ `Regex.Replace()`

---

# LEVEL 3 — ООП И КОЛЛЕКЦИИ

> **Цель:** Освоить объектно-ориентированное программирование и основные структуры данных
> **Предварительные знания:** Level 1-2 пройдены
> **Путь:** `Level_3_ООП/`

## 01_Типы_данных/
- ⬜ **01_Records.dib**
  - ⬜ `record class` — позиционный синтаксис, value equality
  - ⬜ `with` — создание копии с изменениями
  - ⬜ Деконструкция
  - ⬜ `record struct` (C# 10)
- ⬜ **02_Кортежи_и_новинки.dib**
  - ⬜ Кортежи (ValueTuple): именованные элементы, деконструкция
  - ⬜ Collection expressions (C# 12): `[1, 2, 3]`, spread `[..first, ..second]`
  - ⬜ Анонимные типы

## 02_Методы/
- ⬜ **01_Extension_methods.dib**
  - ⬜ Синтаксис `this` в первом параметре
  - ⬜ Создание расширений для string, int, коллекций
- ⬜ **02_Локальные_и_рекурсия.dib**
  - ⬜ Локальные функции, `static` локальные функции
  - ⬜ Рекурсия: Fibonacci, бинарный поиск

## 03_Классы/
- ⬜ **01_Primary_constructors.dib**
  - ⬜ Primary constructors (C# 12)
  - ⬜ `required` свойства (C# 11)
  - ⬜ `init`-only сеттеры (C# 9)
- ⬜ **02_Цепочки_и_static.dib**
  - ⬜ Цепочка конструкторов: `this()`
  - ⬜ Статические конструкторы
  - ⬜ `file class` (C# 11)

## 04_Наследование/ ★ NEW
- ⬜ **01_Наследование.dib**
  - ⬜ Базовый и производный класс, `base`
  - ⬜ `virtual` и `override`
  - ⬜ `sealed`, `abstract class`, `abstract` методы
  - ⬜ Переопределение `ToString()`, `Equals()`, `GetHashCode()`

## 05_Полиморфизм/ ★ NEW
- ⬜ **01_Полиморфизм.dib**
  - ⬜ Полиморфизм через наследование и интерфейсы
  - ⬜ `is`, `as`, `(Type)obj`
  - ⬜ `typeof()` и `GetType()`

## 06_Инкапсуляция/ ★ NEW
- ⬜ **01_Инкапсуляция.dib**
  - ⬜ Приватные поля + публичные свойства с валидацией
  - ⬜ `init` vs `set` vs `private set`
  - ⬜ Иммутабельные классы, record

## 07_Интерфейсы/ ★ NEW
- ⬜ **01_Основы_интерфейсов.dib**
  - ⬜ Определение и реализация интерфейса
  - ⬜ Множественная реализация
  - ⬜ Явная реализация (explicit)
  - ⬜ Default interface methods (C# 8+)
- ⬜ **02_Стандартные_интерфейсы.dib**
  - ⬜ `IComparable<T>` — сравнение/сортировка
  - ⬜ `IEquatable<T>` — равенство
  - ⬜ `IEnumerable<T>` — перебор foreach
  - ⬜ `IDisposable` — освобождение ресурсов + `using`

## 08_Коллекции/ ★ NEW
- ⬜ **01_List.dib**
  - ⬜ `List<T>`: Add, Remove, Insert, Contains, IndexOf
  - ⬜ Count vs Capacity, Sort, Reverse, FindAll
- ⬜ **02_Dictionary.dib**
  - ⬜ `Dictionary<TKey, TValue>`: Add, Remove, ContainsKey
  - ⬜ `TryGetValue()`, перебор KeyValuePair
- ⬜ **03_HashSet.dib**
  - ⬜ `HashSet<T>`: уникальные элементы
  - ⬜ Операции множеств: UnionWith, IntersectWith, ExceptWith
- ⬜ **04_Queue_Stack.dib**
  - ⬜ `Queue<T>` — FIFO, `Stack<T>` — LIFO
  - ⬜ `PriorityQueue<TElement, TPriority>`
  - ⬜ Выбор коллекции (таблица Big O)

## 09_Исключения/ ★ NEW
- ⬜ **01_Try_Catch.dib**
  - ⬜ `try` / `catch` / `finally`
  - ⬜ Множественные `catch`
  - ⬜ Фильтры `when`
  - ⬜ Иерархия исключений
- ⬜ **02_Кастомные_и_паттерны.dib**
  - ⬜ Кастомные исключения
  - ⬜ `throw;` vs `throw ex;`
  - ⬜ Guard clauses, Try-паттерн

## 10_Отладка/ ★ NEW
- ⬜ **01_Отладка.dib**
  - ⬜ Breakpoints: обычный, conditional, tracepoint
  - ⬜ Окна отладки: Watch, Locals, Immediate Window
  - ⬜ Step Over, Step Into, Step Out, Call Stack
  - ⬜ Debug vs Release, `#if DEBUG`, `Debug.Assert()`

---

# LEVEL 4 — ПРОДВИНУТЫЙ C#

> **Цель:** Освоить мощные возможности языка
> **Предварительные знания:** Level 1-3 пройдены
> **Путь:** `Level_4_Продвинутый/`

## 01_Типы_данных/
- ⬜ **01_Generics.dib**
  - ⬜ Обобщённые классы: `class Box<T>`
  - ⬜ Обобщённые методы: `T Max<T>(T a, T b)`
  - ⬜ Ограничения (constraints): `where T : class/struct/new()/IInterface`
  - ⬜ Ковариантность `out T`, контравариантность `in T`
- ⬜ **02_Span_и_Memory.dib**
  - ⬜ `Span<T>` — срез без копирования
  - ⬜ `ReadOnlySpan<T>`
  - ⬜ `Memory<T>` — когда Span не подходит (async)

## 02_Операторы/
- ⬜ **01_Перегрузка.dib**
  - ⬜ Перегрузка операторов: `+`, `-`, `==`, `!=`
  - ⬜ Индексаторы: `this[int index]`
  - ⬜ `implicit` / `explicit` operator

## 03_Методы/
- ⬜ **01_Делегаты.dib**
  - ⬜ Делегаты: объявление, многоадресные
  - ⬜ `Action`, `Func<T>`, `Predicate<T>`
- ⬜ **02_Лямбды.dib**
  - ⬜ Лямбда-выражения: expression и statement
  - ⬜ Замыкания (closures)
  - ⬜ Ловушка замыканий в цикле
- ⬜ **03_События.dib**
  - ⬜ `event EventHandler<T>`
  - ⬜ Publisher / Subscriber
  - ⬜ Кастомные EventArgs, отписка

## 04_Коллекции/
- ⬜ **01_Продвинутые_коллекции.dib**
  - ⬜ `ImmutableList<T>`, `ImmutableDictionary`
  - ⬜ `FrozenSet<T>`, `FrozenDictionary`
  - ⬜ `ConcurrentDictionary`, `ConcurrentQueue`
  - ⬜ `ObservableCollection<T>`

## 05_LINQ/ ★ NEW
- ⬜ **01_Основы_LINQ.dib**
  - ⬜ Query syntax vs Method syntax
  - ⬜ `Where`, `Select`, `OrderBy`, `ThenBy`
  - ⬜ `First`, `FirstOrDefault`, `Any`, `All`
- ⬜ **02_Продвинутый_LINQ.dib**
  - ⬜ `GroupBy`, `Join`
  - ⬜ Агрегация: `Count`, `Sum`, `Average`, `Min`, `Max`
  - ⬜ `Take`, `Skip`, `Distinct`, `Chunk`
  - ⬜ Отложенное выполнение (deferred execution)

## 06_Async/ ★ NEW
- ⬜ **01_Async_Await.dib**
  - ⬜ `async` / `await` — основы
  - ⬜ `Task` и `Task<T>`
  - ⬜ `ValueTask<T>`
- ⬜ **02_Параллельность.dib**
  - ⬜ `Task.WhenAll()`, `Task.WhenAny()`
  - ⬜ `CancellationToken`
  - ⬜ Проблемы: `async void`, deadlock, забытый `await`

## 07_Pattern_Matching/ ★ NEW
- ⬜ **01_Pattern_Matching.dib**
  - ⬜ Типовой, константный, реляционный, логический паттерны
  - ⬜ Паттерн свойств, позиционный, списков
  - ⬜ Switch expressions с паттернами + `when`

## 08_Файлы/ ★ NEW
- ⬜ **01_Файловый_ввод_вывод.dib**
  - ⬜ `File.ReadAllText()`, `File.WriteAllText()`
  - ⬜ `StreamReader`, `StreamWriter`
  - ⬜ `Path`: Combine, GetFileName, GetExtension
  - ⬜ `using` / `await using`
- ⬜ **02_JSON.dib**
  - ⬜ `System.Text.Json`: Serialize / Deserialize
  - ⬜ `[JsonPropertyName]`, `[JsonIgnore]`
  - ⬜ `JsonSerializerOptions`

## 09_Функциональное/ ★ NEW
- ⬜ **01_Функциональное.dib**
  - ⬜ Функции высшего порядка
  - ⬜ Замыкания, мемоизация
  - ⬜ Каррирование, частичное применение
  - ⬜ Композиция функций, пайплайн

---

# LEVEL 5 — АРХИТЕКТУРА

> **Цель:** Проектирование, паттерны, профессиональные инструменты
> **Предварительные знания:** Level 1-4 пройдены
> **Путь:** `Level_5_Архитектура/`

## 01_Рефлексия/ ★ NEW
- ⬜ **01_Рефлексия.dib**
  - ⬜ `typeof()` и `GetType()`, класс `Type`
  - ⬜ `GetMethods()`, `GetProperties()`, `GetFields()`
  - ⬜ `Activator.CreateInstance()`, `MethodInfo.Invoke()`
- ⬜ **02_Атрибуты.dib**
  - ⬜ Стандартные атрибуты: `[Obsolete]`, `[Conditional]`
  - ⬜ Создание своих атрибутов
  - ⬜ Generic attributes (C# 11)

## 02_Многопоточность/ ★ NEW
- ⬜ **01_Потоки.dib**
  - ⬜ `Thread` — создание потоков
  - ⬜ `Thread.Sleep()`, `Thread.Join()`
  - ⬜ Race condition — гонка потоков
- ⬜ **02_Синхронизация.dib**
  - ⬜ `lock`, `Monitor`, `Mutex`
  - ⬜ `SemaphoreSlim`, `Interlocked`
  - ⬜ `Channel<T>` — producer/consumer

## 03_Память/ ★ NEW
- ⬜ **01_Garbage_Collector.dib**
  - ⬜ Поколения: 0, 1, 2
  - ⬜ Large Object Heap (LOH)
  - ⬜ Как GC решает что собирать
- ⬜ **02_Оптимизация_памяти.dib**
  - ⬜ `IDisposable` и Dispose pattern
  - ⬜ `ArrayPool<T>`, `Span<T>` углублённо
  - ⬜ `stackalloc`, `WeakReference<T>`

## 04_SOLID/ ★ NEW
- ⬜ **01_SOLID.dib**
  - ⬜ **S** — Single Responsibility
  - ⬜ **O** — Open/Closed
  - ⬜ **L** — Liskov Substitution
  - ⬜ **I** — Interface Segregation
  - ⬜ **D** — Dependency Inversion

## 05_Паттерны/ ★ NEW
- ⬜ **01_Singleton.dib** — единственный экземпляр (thread-safe с `Lazy<T>`)
- ⬜ **02_Factory_Method.dib** — создание через метод, не `new`
- ⬜ **03_Strategy.dib** — семейство алгоритмов через интерфейс
- ⬜ **04_Observer.dib** — подписка на события
- ⬜ **05_State.dib** — конечный автомат (FSM)
- ⬜ **06_Command.dib** — действие как объект (undo/redo)
- ⬜ **07_Builder.dib** — пошаговое построение сложного объекта
- ⬜ **08_Decorator.dib** — обёртка для добавления поведения

## 06_Архитектура/ ★ NEW
- ⬜ **01_MVVM.dib**
  - ⬜ `INotifyPropertyChanged`, `ICommand`, Data Binding
- ⬜ **02_Clean_Architecture.dib**
  - ⬜ Domain → Application → Infrastructure → Presentation
  - ⬜ Repository pattern, CQRS (обзор)

## 07_DI/ ★ NEW
- ⬜ **01_DI.dib**
  - ⬜ Constructor injection
  - ⬜ `AddTransient`, `AddScoped`, `AddSingleton`
  - ⬜ `IServiceProvider`

## 08_Логирование/ ★ NEW
- ⬜ **01_Логирование.dib**
  - ⬜ `Microsoft.Extensions.Logging`, `ILogger`
  - ⬜ Уровни: Trace → Critical
  - ⬜ Structured logging, `LoggerMessage.Define()`
  - ⬜ Serilog (обзор)

## 09_Стиль_кода/ ★ NEW
- ⬜ **01_Стиль_кода.dib**
  - ⬜ Именование .NET: PascalCase, camelCase, _camelCase
  - ⬜ `.editorconfig`, Roslyn analyzers
  - ⬜ XML-документация

---

# ОТДЕЛЬНЫЕ ТРЕКИ

## Godot (после Level 2+)
> Файлы: `Notebooks/Godot/`

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
