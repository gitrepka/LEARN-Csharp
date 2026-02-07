# МАСТЕР-ПЛАН ИЗУЧЕНИЯ C# — ОТ НУЛЯ ДО ПРОФЕССИОНАЛА (v2.0)

> **Платформы для практики:**
> - **Uno Platform 6.4+** — кроссплатформенные приложения (Windows, Android, iOS, Web, Linux, macOS)
> - **Godot 4.6** — игровая разработка на C#
>
> **Актуальные версии:** C# 14, .NET 10 (LTS), Godot 4.6, Uno Platform 6.4
>
> **Принцип:** Каждая тема закрепляется практическими заданиями на ОБЕИХ платформах.
> Консольные задачи даются ТОЛЬКО там, где это необходимо (помечены ⚠️ ТЕРМИНАЛ).
>
> **Система упражнений на каждую тему:**
> - 🟢 **Мини-упражнения** (5-10 мин) — быстрое закрепление синтаксиса
> - 🟡 **Средние задачи** (30-60 мин) — применение на практике
> - 🔴 **Проект** (несколько часов) — полноценное применение в Uno/Godot
> - ❓ **Контрольные вопросы** — проверка понимания перед переходом дальше
>
> **Уровни сложности задач:** ⭐ Базовый | ⭐⭐ Средний | ⭐⭐⭐ Продвинутый

---

## МОДУЛЬ 0: ПОДГОТОВКА СРЕДЫ РАЗРАБОТКИ

### 0.1 Установка инструментов
- Установить .NET 10 SDK
- Установить Visual Studio 2022/2026 или JetBrains Rider
- Установить Godot 4.6 (.NET / Mono версия)
- Установить Uno Platform расширение и шаблоны
- Установить Git

> ⚠️ ТЕРМИНАЛ необходим для: `dotnet --version`, `dotnet new`, `git init`, создания проектов.

### 0.2 Структура проекта .NET
- Файл `.csproj`: `<TargetFramework>`, `<Nullable>`, `<ImplicitUsings>`, `<LangVersion>`
- Файл `.sln` / `.slnx` (VS 2026)
- SDK-style проекты
- Команды CLI: `dotnet new`, `dotnet build`, `dotnet run`, `dotnet publish`, `dotnet add package`, `dotnet watch`

### 0.3 Первые проекты
- Создать пустой проект Uno Platform (XAML + MVVM)
- Создать пустой проект Godot 4.6 C#
- Убедиться, что оба проекта компилируются и запускаются
- Познакомиться с Hot Reload (XAML Hot Reload в Uno, C# Hot Reload)

### 0.4 Знакомство с IDE
- Навигация по коду: Go to Definition, Find All References, Find Usages
- Горячие клавиши (основные)
- Solution Explorer / File System
- Окно ошибок (Error List)
- Терминал в IDE

**Практика:**
- ⭐ Создай оба проекта, запусти, убедись что работают
- ⭐ Измени текст на экране в Uno и в Godot, используй Hot Reload
- ⭐ Открой .csproj, найди `TargetFramework`, `Nullable`, `LangVersion`

---

## МОДУЛЬ 1: ОСНОВЫ C# — ФУНДАМЕНТ

### 1.1 Синтаксис и структура программы
- Пространства имён (`namespace`) и file-scoped namespaces (`namespace MyApp;` — C# 10)
- Точка входа: `Main` метод и top-level statements (C# 9+)
- Комментарии: `//`, `/* */`, XML-документация `///`
- Директивы `using`, `global using` (C# 10), `implicit usings` (.NET 6+)
- Алиасы using: `using Point = (int X, int Y);` (C# 12: для любых типов)
- Регионы `#region` / `#endregion`
- Условная компиляция: `#if`, `#else`, `#endif`, `#define`

🟢 **Мини-упражнения:**
1. ⭐ Напиши программу с file-scoped namespace
2. ⭐ Создай `global using` для System.Linq и используй его в другом файле
3. ⭐ Создай alias: `using StringList = System.Collections.Generic.List<string>;`

**Практика Uno Platform:** ⭐
Создай приложение "Визитка". TextBlock-и отображают: имя, возраст, хобби. В code-behind задай данные и передай в UI.

**Практика Godot:** ⭐
Создай сцену с Label-узлами. В `_Ready()` задай текст для каждого. Отобрази информацию о себе.

❓ **Контрольные вопросы:**
- Чем `global using` отличается от обычного `using`?
- Зачем нужен `implicit usings`? Какие пространства подключаются автоматически?
- Что такое file-scoped namespace и чем он отличается от обычного?

---

### 1.2 Переменные и типы данных

#### 1.2.1 Значимые типы (Value Types)
- Целочисленные: `byte`, `sbyte`, `short`, `ushort`, `int`, `uint`, `long`, `ulong`, `nint`, `nuint`
- С плавающей точкой: `float`, `double`, `decimal`
- `bool` — логический тип
- `char` — символ
- `struct` — пользовательские структуры (введение)
- `enum` — перечисления (введение)

#### 1.2.2 Ссылочные типы (Reference Types)
- `string` — строки
- `object` — базовый тип всего
- `dynamic` — динамический тип
- Массивы (введение)
- Классы (введение)
- Анонимные типы: `var obj = new { Name = "Test", Age = 5 };`

#### 1.2.3 Nullable-типы
- `int?`, `bool?` и т.д.
- Оператор `??` (null-coalescing)
- Оператор `?.` (null-conditional)
- Оператор `??=` (null-coalescing assignment)
- Nullable reference types (NRT): включение `<Nullable>enable</Nullable>`
- `!` (null-forgiving operator)

#### 1.2.4 Вывод типов и инициализация
- `var` — вывод типа компилятором
- Target-typed `new()`: `List<string> list = new();` (C# 9)
- Когда использовать `var`, когда явный тип

#### 1.2.5 Константы и readonly
- `const` vs `readonly` vs `static readonly`
- Когда что использовать

🟢 **Мини-упражнения:**
1. ⭐ Объяви переменные каждого базового типа, выведи их значения по умолчанию через `default(T)`
2. ⭐ Поэкспериментируй с `int?`: присвой null, проверь `HasValue`, используй `??`
3. ⭐ Создай анонимный тип с 3 свойствами, обрати к ним
4. ⭐ Используй `target-typed new()` для List, Dictionary и своего класса
5. ⭐ Включи Nullable reference types, поставь `string?` и `string`, посмотри предупреждения

🟡 **Средняя задача:** ⭐⭐
Напиши класс `TypeExplorer` с методами: `GetTypeName(object obj)`, `GetSizeInBytes(Type t)`, `IsNullable(Type t)`. Протестируй со всеми типами.

**Практика Uno Platform:** ⭐⭐
Приложение "Калькулятор типов". TextBox для ввода числа, кнопки "Как int", "Как double", "Как string". Показывай результат и размер типа в байтах. Используй `typeof()` и `Marshal.SizeOf()` где возможно.

**Практика Godot:** ⭐⭐
Сцена "Характеристики персонажа". Имя (string), здоровье (int), скорость (float), жив (bool), класс (enum: Warrior, Mage, Archer). Label-ы показывают всё. Кнопки меняют характеристики.

❓ **Контрольные вопросы:**
- Чем `float` отличается от `double` и `decimal`? Когда что использовать?
- В чём разница между `const` и `readonly`?
- Что такое Nullable Reference Types и зачем они?
- Чем анонимные типы отличаются от обычных классов?

---

### 1.3 Операторы

#### 1.3.1 Арифметические: `+`, `-`, `*`, `/`, `%`, `++`, `--`
- Целочисленное деление vs деление с плавающей точкой
- Префиксный vs постфиксный инкремент/декремент
- `checked` / `unchecked` контексты (проверка переполнения)

#### 1.3.2 Сравнения: `==`, `!=`, `<`, `>`, `<=`, `>=`
#### 1.3.3 Логические: `&&`, `||`, `!` (короткое замыкание)
#### 1.3.4 Побитовые: `&`, `|`, `^`, `~`, `<<`, `>>`, `>>>` (unsigned right shift, C# 11)
#### 1.3.5 Присваивания: `=`, `+=`, `-=`, `*=`, `/=`, `%=`, `&=`, `|=`, `^=`, `<<=`, `>>=`
#### 1.3.6 Тернарный: `условие ? значение1 : значение2`
#### 1.3.7 Null-операторы: `??`, `?.`, `??=`, `!`
#### 1.3.8 Приоритет операторов

🟢 **Мини-упражнения:**
1. ⭐ Что выведет: `int a = 5; Console.WriteLine(a++ + ++a);`? Объясни почему.
2. ⭐ Используй `checked` блок, вызови переполнение int.MaxValue + 1
3. ⭐ Напиши выражение с `??` для получения значения по умолчанию из nullable-переменной
4. ⭐ "Что выведет этот код?" — 5 примеров с приоритетом операторов

**Практика Uno Platform:** ⭐⭐
Приложение "Умный калькулятор" с полноценным UI: кнопки для цифр и операций, дисплей (как калькулятор Windows).

**Практика Godot:** ⭐⭐
Система урона: `(базовыйУрон * множитель) - бронь`. Критический удар через тернарный оператор. Отображай расчёт на экране.

---

### 1.4 Управляющие конструкции

#### 1.4.1 Условные операторы
- `if`, `else if`, `else`
- Вложенные условия
- `switch` (классический)
- `switch` выражения (expression-based, C# 8+)

#### 1.4.2 Циклы
- `for`, `foreach`, `while`, `do...while`
- Вложенные циклы

#### 1.4.3 Управление потоком
- `break`, `continue`, `return`
- `goto` (и почему его лучше не использовать)

🟢 **Мини-упражнения:**
1. ⭐ Перепиши цепочку if/else if как switch expression
2. ⭐ Напиши цикл, который выводит только чётные числа от 0 до 100 (используй `continue`)
3. ⭐ Реши "FizzBuzz" через switch expression
4. ⭐ "Найди ошибку" — 3 примера с бесконечными циклами и неправильными условиями

**Практика Uno Platform:** ⭐⭐
"Генератор таблицы умножения". Ввод числа → Grid с таблицей, цветная подсветка чётных/нечётных.

**Практика Godot:** ⭐⭐
AI врага: патруль между точками (for по массиву). Обнаружение игрока (if — дистанция). Поведение через switch по состояниям: Patrol, Chase, Attack, Flee.

---

### 1.5 Строки (Strings) — углублённо

#### 1.5.1 Создание строк
- Строковые литералы
- Verbatim-строки: `@"C:\path\file"`
- Интерполяция: `$"Hello, {name}!"`
- Raw string literals: `"""..."""` (C# 11) — многострочные, отступы, `$$"""{{value}}"""`
- UTF-8 строковые литералы: `"hello"u8` → `ReadOnlySpan<byte>` (C# 11)
- Конкатенация: `+`, `string.Concat()`

#### 1.5.2 Основные методы строк
- `Length`, `ToUpper()`, `ToLower()`, `Trim()`, `TrimStart()`, `TrimEnd()`
- `Substring()`, `IndexOf()`, `LastIndexOf()`, `Contains()`
- `Replace()`, `Remove()`, `Insert()`
- `Split()`, `string.Join()`
- `StartsWith()`, `EndsWith()`
- `PadLeft()`, `PadRight()`
- `string.IsNullOrEmpty()`, `string.IsNullOrWhiteSpace()`

#### 1.5.3 StringBuilder
- Зачем: string иммутабельна → конкатенация создаёт новые объекты
- `Append()`, `Insert()`, `Remove()`, `Replace()`, `ToString()`
- Когда string vs StringBuilder (правило ~10 конкатенаций)

#### 1.5.4 Форматирование
- `string.Format()`, интерполяция `$""`
- Числовые форматы: N, F, C, P, D, X, E
- Форматирование дат (подробнее в 1.10)
- Кастомные форматы: `#,##0.00`
- `IFormattable`, `ISpanFormattable`

#### 1.5.5 Сравнение строк
- `==` vs `Equals()` vs `string.Compare()`
- `StringComparison` (Ordinal, OrdinalIgnoreCase, CurrentCulture, InvariantCulture)
- Культурозависимое сравнение — подводные камни

#### 1.5.6 Span и строки
- `ReadOnlySpan<char>` — работа без аллокаций
- `string.AsSpan()`, Slice

🟢 **Мини-упражнения:**
1. ⭐ Переверни строку тремя способами (цикл, Array.Reverse, LINQ)
2. ⭐ Посчитай количество каждой гласной в тексте
3. ⭐ Используй raw string literal для JSON-шаблона с интерполяцией
4. ⭐ Замерь разницу: 10000 конкатенаций string vs StringBuilder (Stopwatch)

🟡 **Средняя задача:** ⭐⭐
Напиши метод `string CamelCaseToSnakeCase(string input)` и `string SnakeCaseToCamelCase(string input)`.

**Практика Uno Platform:** ⭐⭐
"Текстовый анализатор". TextBox ввода → символы, слова, предложения, самое длинное слово, частота букв (ProgressBar). Поиск и замена.

**Практика Godot:** ⭐⭐
Система диалогов NPC. Строки с тегами: `"[name]Торговец[/name]: [color=yellow]Привет![/color]"`. Парсинг, отображение с typewriter-эффектом (побуквенное появление).

---

### 1.6 Массивы и диапазоны

#### 1.6.1 Одномерные массивы
- Объявление, инициализация, доступ по индексу
- `Length`, перебор (for, foreach)

#### 1.6.2 Многомерные массивы
- Двумерные `[,]`, зубчатые (jagged) `[][]`

#### 1.6.3 Методы Array
- `Sort()`, `Reverse()`, `IndexOf()`, `Find()`, `FindAll()`, `Copy()`, `Resize()`, `Exists()`, `TrueForAll()`

#### 1.6.4 Индексы и диапазоны (C# 8+)
- Index: `^1` (с конца), `^2`, `^0` (IndexOutOfRange)
- Range: `array[1..3]`, `array[..3]`, `array[2..]`, `array[1..^1]`
- `Index` и `Range` типы

#### 1.6.5 Span<T> и Memory<T>
- `Span<T>` как срез массива без копирования
- `ReadOnlySpan<T>`
- `stackalloc` (введение)

🟢 **Мини-упражнения:**
1. ⭐ Получи последние 3 элемента массива через `^3..`
2. ⭐ Используй Range для получения подстроки без Substring
3. ⭐ Создай Span из массива, измени элемент через Span — убедись что массив изменился
4. ⭐ Найди максимальный элемент массива без LINQ (циклом)
5. ⭐ "Что выведет?" — 3 примера с `^` и `..`

**Практика Uno Platform:** ⭐⭐
"Визуальный сортировщик". Ввод чисел → столбиковая диаграмма → анимация сортировки (пузырьком или выбором, выбирает пользователь).

**Практика Godot:** ⭐⭐
Игра "Memory Cards" (найди пару). 2D-массив для расположения карт. Клик → переворот. Совпадение → остаются. Массивы для логики поля.

---

### 1.7 Методы (функции)

#### 1.7.1 Объявление и вызов
- Возвращаемый тип, имя, параметры, `void`, `return`

#### 1.7.2 Параметры
- По значению, по ссылке (`ref`), выходные (`out`), входные (`in`)
- Значения по умолчанию, именованные аргументы
- `params` — массив параметров
- `params` для любых коллекций: `params Span<T>`, `params IEnumerable<T>` (C# 13)

#### 1.7.3 Перегрузка методов (Method Overloading)
#### 1.7.4 Рекурсия (базовый случай, стек вызовов, хвостовая рекурсия)
#### 1.7.5 Локальные функции (вложенные методы, `static` локальные функции)
#### 1.7.6 Методы расширения (Extension Methods)
- Синтаксис `this`
- Extension members — новый синтаксис в C# 14

🟢 **Мини-упражнения:**
1. ⭐ Напиши метод `Swap(ref int a, ref int b)`
2. ⭐ Напиши метод `bool TryParseAge(string input, out int age)` с валидацией (1-150)
3. ⭐ Напиши метод с `params Span<int>` (C# 13) для суммирования
4. ⭐ Напиши рекурсивный метод для вычисления факториала
5. ⭐ Создай extension method `bool IsEven(this int n)` и используй: `42.IsEven()`

**Практика Uno Platform:** ⭐⭐
"Конвертер единиц". Методы конвертации: температура (C↔F↔K), длина (м↔фут), вес (кг↔фунт). Перегрузка для разных типов ввода. UI: ComboBox + TextBox.

**Практика Godot:** ⭐⭐
Утилитный класс `GameMath` с extension-методами: `Vector2.DirectionTo()`, `float.Remap(fromMin, fromMax, toMin, toMax)`, `Node.GetAllChildren<T>()` (рекурсивный поиск). Используй в сцене с движущимися объектами.

---

### 1.8 Регулярные выражения (Regex)

#### 1.8.1 Основы Regex
- `Regex.IsMatch()`, `Regex.Match()`, `Regex.Matches()`, `Regex.Replace()`
- Метасимволы: `.`, `\d`, `\w`, `\s`, `\b`
- Квантификаторы: `*`, `+`, `?`, `{n}`, `{n,m}`
- Группы: `()`, именованные группы `(?<name>...)`, обратные ссылки
- Классы символов: `[a-z]`, `[^0-9]`
- Якоря: `^`, `$`

#### 1.8.2 Regex Source Generator (C# 12+ / .NET 7+)
- `[GeneratedRegex(@"pattern")]` — компиляция в compile-time
- Преимущества по производительности

🟢 **Мини-упражнения:**
1. ⭐ Валидируй email через Regex
2. ⭐ Извлеки все числа из строки
3. ⭐ Замени все телефоны формата +7(999)123-45-67 на маску +7(***)*****

🟡 **Средняя задача:** ⭐⭐
Напиши парсер лога: `"[2026-01-15 14:30:05] ERROR: Connection timeout (retry 3/5)"`. Извлеки дату, уровень, сообщение, номер попытки.

**Практика Uno Platform:** ⭐⭐
"Валидатор форм". Поля: email, телефон, пароль (мин 8 символов, заглавная, цифра, спецсимвол), URL. Regex-валидация с подсветкой ошибок в реальном времени. Используй `[GeneratedRegex]`.

**Практика Godot:** ⭐⭐
Система чат-команд: `/spawn zombie 5 at 100,200`, `/heal player 50`, `/tp 300 400`. Парси команды через Regex, извлекай аргументы, выполняй.

---

### 1.9 Дата и время

#### 1.9.1 DateTime
- Создание, свойства (Year, Month, Day, Hour, etc.)
- `DateTime.Now`, `DateTime.UtcNow`, `DateTime.Today`
- Арифметика: `AddDays()`, `AddHours()`, разница (TimeSpan)
- Parsing: `DateTime.Parse()`, `DateTime.TryParse()`, `DateTime.ParseExact()`
- Форматирование: стандартные и кастомные форматы

#### 1.9.2 DateTimeOffset — дата с часовым поясом
- Когда DateTime, когда DateTimeOffset
- `DateTimeKind` (Local, Utc, Unspecified)

#### 1.9.3 TimeSpan — промежуток времени
- Создание, арифметика, форматирование

#### 1.9.4 DateOnly и TimeOnly (.NET 6+)
- Когда использовать вместо DateTime

#### 1.9.5 Часовые пояса
- `TimeZoneInfo`
- Конвертация между зонами

#### 1.9.6 Stopwatch — измерение производительности

🟢 **Мини-упражнения:**
1. ⭐ Посчитай сколько дней до твоего следующего дня рождения
2. ⭐ Определи день недели любой даты
3. ⭐ Замерь время выполнения цикла через Stopwatch

**Практика Uno Platform:** ⭐⭐
"Мировые часы". Показывай время в 5 часовых поясах (Москва, Нью-Йорк, Токио, Лондон, Сидней). Обновление каждую секунду (DispatcherTimer). Аналоговый циферблат через Canvas-рисование.

**Практика Godot:** ⭐⭐
Игровой таймер и система день/ночь. Внутриигровое время идёт ускоренно (1 мин реального = 1 час игрового). Освещение меняется: рассвет → день → закат → ночь. HUD показывает игровое время.

---

### 1.10 Глобализация и локализация (введение)

#### 1.10.1 CultureInfo
- `CultureInfo.CurrentCulture`, `CultureInfo.InvariantCulture`
- Влияние на форматирование чисел, дат, сравнение строк
- `IFormatProvider`

#### 1.10.2 Ресурсные файлы (.resx)
- Строковые ресурсы для многоязычности
- `ResourceManager`

> Подробнее — в платформо-специфичных модулях (9, 10).

🟢 **Мини-упражнения:**
1. ⭐ Выведи число 1234567.89 в формате разных культур (ru-RU, en-US, de-DE)
2. ⭐ Покажи дату в формате разных культур

---

## МОДУЛЬ 2: ОБЪЕКТНО-ОРИЕНТИРОВАННОЕ ПРОГРАММИРОВАНИЕ (ООП)

### 2.1 Классы и объекты

#### 2.1.1 Определение класса
- Поля (fields)
- Свойства (properties): get, set, init
- Auto-properties
- `required` свойства (C# 11): обязательная инициализация
- Field-backed properties: `field` keyword (C# 14)
- Expression-bodied members: `=>`
- `init`-only сеттеры (C# 9) — установка только при инициализации

#### 2.1.2 Конструкторы
- Конструктор по умолчанию
- Параметризованные конструкторы
- Цепочка конструкторов (`this()`)
- Статические конструкторы
- Primary constructors для классов (C# 12): `class Person(string name, int age)`
  - Захват параметров (они НЕ становятся полями автоматически!)
  - Отличия от primary constructors в records
- Partial constructors (C# 14)

#### 2.1.3 Финализаторы / Деструкторы
- `~ClassName()` — когда использовать (почти никогда)

#### 2.1.4 `this`, модификаторы доступа
- `public`, `private`, `protected`, `internal`, `protected internal`, `private protected`
- `file` (C# 11) — тип виден только в файле

#### 2.1.5 Статические члены и классы

🟢 **Мини-упражнения:**
1. ⭐ Создай класс `Person` с `required` свойствами Name и Age
2. ⭐ Создай класс с primary constructor, покажи как параметр захватывается
3. ⭐ Создай класс с `init`-only свойством, покажи что после создания его нельзя изменить
4. ⭐ Создай `file class Helper` — покажи что он не виден из другого файла
5. ⭐ "Найди ошибку" — 3 примера с неправильным использованием модификаторов доступа

🟡 **Средняя задача:** ⭐⭐
Класс `BankAccount`: свойства (номер, владелец, баланс). Конструктор с валидацией. Методы `Deposit()`, `Withdraw()` (с проверкой). `required` для владельца. `init` для номера счёта.

**Практика Uno Platform:** ⭐⭐
"Адресная книга". Класс `Contact` с required свойствами. ListView, форма добавления с валидацией, поиск.

**Практика Godot:** ⭐⭐
Класс `Weapon`: Name, Damage, FireRate, AmmoCapacity, CurrentAmmo. Конструктор, методы: `Shoot()`, `Reload()`. Переключение оружий в игре. UI информация.

❓ **Контрольные вопросы:**
- Чем `required` отличается от обязательного параметра конструктора?
- Чем primary constructor класса отличается от primary constructor record?
- Когда использовать `init` vs `set` vs `private set`?
- Объясни разницу между `internal` и `protected internal`.

---

### 2.2 Наследование

#### 2.2.1 Базовый и производный класс: `: BaseClass`, `base`, вызов конструктора базового
#### 2.2.2 Виртуальные методы: `virtual`, `override`, `new` (hiding), `sealed`
#### 2.2.3 Ковариантные возвращаемые типы (C# 9): override может возвращать более конкретный тип
#### 2.2.4 Абстрактные классы: `abstract class`, `abstract` методы
#### 2.2.5 Класс `object`: `ToString()`, `Equals()`, `GetHashCode()`, `GetType()` — переопределение

🟢 **Мини-упражнения:**
1. ⭐ Создай иерархию Animal → Dog, Cat с переопределением `Speak()`
2. ⭐ Покажи разницу между `override` и `new` на примере
3. ⭐ Используй covariant return: `Clone()` в базовом возвращает `Animal`, в Dog — `Dog`
4. ⭐ Переопредели `ToString()` для красивого вывода

**Практика Uno Platform:** ⭐⭐⭐
"Графический редактор". `abstract Shape` → `Circle`, `Rectangle`, `Triangle`. `Draw()`, `CalculateArea()`, `CalculatePerimeter()`. Canvas для рисования, панель инструментов.

**Практика Godot:** ⭐⭐⭐
Иерархия врагов. `abstract BaseEnemy : Node2D` → `Zombie`, `Skeleton`, `Boss`. Виртуальные `Move()`, `Attack()`, `TakeDamage()`, `Die()`. Волны врагов.

---

### 2.3 Полиморфизм

#### 2.3.1 Через наследование — базовый тип для работы с производными
#### 2.3.2 Через интерфейсы — множественная реализация
#### 2.3.3 Приведение и проверка типов: `is` (с pattern matching), `as`, `typeof()`, `GetType()`

🟢 **Мини-упражнения:**
1. ⭐ Массив `Shape[]` с разными фигурами, цикл вызывает `CalculateArea()` — полиморфизм
2. ⭐ Используй `is Type variable` для безопасного приведения
3. ⭐ Покажи разницу: `(Dog)animal` vs `animal as Dog` при невозможном приведении

**Практика Uno Platform:** ⭐⭐⭐
Расширь "Графический редактор": `IResizable`, `IRotatable`, `IDraggable`. Не все фигуры реализуют всё. UI показывает доступные действия через `is`.

**Практика Godot:** ⭐⭐⭐
`IInteractable.Interact()`: `Chest` (открывается), `Door` (открывается/закрывается), `NPC` (диалог), `Switch` (переключает). Игрок взаимодействует через полиморфизм.

---

### 2.4 Инкапсуляция
- Приватные поля + публичные свойства с валидацией
- `init`-only, `required`
- Иммутабельность: `readonly` поля, иммутабельные классы

### 2.5 Интерфейсы

#### 2.5.1 Определение: методы, свойства, события, индексаторы
- Default interface methods (C# 8+)
- Статические абстрактные/виртуальные члены (C# 11): `static abstract void Method();`
- Generic Math: `INumber<T>`, `IAdditionOperators<TSelf,TOther,TResult>` (C# 11 / .NET 7+)

#### 2.5.2 Множественная реализация, явная реализация
#### 2.5.3 Стандартные интерфейсы .NET
- `IComparable<T>`, `IEquatable<T>`, `IComparer<T>`
- `IEnumerable<T>`, `IEnumerator<T>`
- `IDisposable`, `IAsyncDisposable`
- `ICloneable`
- `INotifyPropertyChanged` (ключевой для UI!)
- `IReadOnlyList<T>`, `IReadOnlyCollection<T>`, `IReadOnlyDictionary<TKey,TValue>`

🟢 **Мини-упражнения:**
1. ⭐ Создай generic метод `T Add<T>(T a, T b) where T : INumber<T>` — работает с int, float, decimal
2. ⭐ Реализуй `IComparable<T>` для класса Student (по оценке)
3. ⭐ Реализуй интерфейс явно и неявно, покажи разницу при вызове

**Практика Uno Platform:** ⭐⭐⭐
"Менеджер задач" (To-Do). `INotifyPropertyChanged` для привязки. `ISortable`, `IFilterable`. ListView с сортировкой/фильтрацией.

**Практика Godot:** ⭐⭐⭐
Система способностей. `IAbility` (Activate, Deactivate, Cooldown). `IDamageable` (TakeDamage, Health). Fireball, Shield — способности. Панель с кулдаунами.

---

### 2.6 Структуры (struct)
- struct vs class: значимый тип, стек, копирование
- `readonly struct`, `ref struct`, `record struct` (C# 10)
- Когда struct vs class

### 2.7 Перечисления (enum)
- Определение, базовый тип, `[Flags]`
- `Enum.Parse()`, `TryParse()`, `GetValues()`, `HasFlag()`

### 2.8 Records (записи)
- `record class`: позиционные, `with`, value equality, деконструкция
- `record struct`
- Наследование records
- record vs class vs struct — когда что

### 2.9 Кортежи (Tuples)
- `ValueTuple`: `(int x, string name)`, именованные элементы
- Деконструкция, возврат нескольких значений
- Метод `Deconstruct()` в своих классах

### 2.10 Коллекционные выражения (C# 12)
- `List<int> numbers = [1, 2, 3, 4, 5];`
- `int[] arr = [1, 2, 3];`
- Spread operator: `int[] combined = [..first, ..second];`
- Работает с: массивами, List, Span, ImmutableArray и др.

🟢 **Мини-упражнения:**
1. ⭐ Создай `record Point(int X, int Y)`, используй `with` для создания нового с изменённым X
2. ⭐ Создай `[Flags] enum Permissions { Read=1, Write=2, Execute=4 }`, комбинируй флаги
3. ⭐ Используй collection expression: `List<string> names = ["Alice", "Bob", ..otherNames];`
4. ⭐ Верни кортеж `(int min, int max)` из метода, деконструируй при вызове
5. ⭐ Создай `readonly record struct Color(byte R, byte G, byte B)`

**Практика Uno Platform (общая для 2.6-2.10):** ⭐⭐
"Цветовая палитра". `readonly record struct Color(byte R, byte G, byte B, byte A)` с операторами Mix, Lighten, Darken. Слайдеры RGB, предпросмотр, сохранение палитры через collection expressions.

**Практика Godot (общая для 2.6-2.10):** ⭐⭐
`[Flags] enum StatusEffects`, `record struct DamageInfo(float Amount, DamageType Type, bool IsCritical)`. `record SaveData(...)` с `with` для автосейвов. Система баффов/урона.

❓ **Контрольные вопросы перед модулем 3:**
- Назови 4 принципа ООП и приведи пример каждого
- Чем `record` отличается от `class`? Когда использовать?
- Что такое `collection expressions` и какие типы поддерживают?
- Объясни разницу между `struct` и `class` в контексте памяти
- Когда использовать `interface` vs `abstract class`?

---

## МОДУЛЬ 3: КОЛЛЕКЦИИ И СТРУКТУРЫ ДАННЫХ

### 3.1 Обобщённые коллекции (Generic Collections)

#### 3.1.1 List<T> — создание, добавление, удаление, поиск, сортировка, Capacity vs Count
#### 3.1.2 Dictionary<TKey, TValue> — TryGetValue, ContainsKey, перебор KeyValuePair
#### 3.1.3 HashSet<T> — уникальные элементы, операции множеств (Union, Intersect, Except)
#### 3.1.4 Queue<T> и Stack<T> — FIFO, LIFO, Enqueue/Dequeue, Push/Pop, Peek
#### 3.1.5 LinkedList<T>, SortedList, SortedDictionary, SortedSet
#### 3.1.6 PriorityQueue<TElement, TPriority> (.NET 6+)
#### 3.1.7 ObservableCollection<T> — для привязки UI!
#### 3.1.8 IReadOnlyList<T>, IReadOnlyDictionary — для безопасного возврата из методов

### 3.2 Immutable & Frozen Collections
- `ImmutableList<T>`, `ImmutableDictionary`, `ImmutableArray`
- `FrozenSet<T>`, `FrozenDictionary` (.NET 8+) — высокая скорость чтения

### 3.3 Concurrent Collections
- `ConcurrentDictionary`, `ConcurrentQueue`, `ConcurrentStack`, `ConcurrentBag`
- `BlockingCollection<T>`, `Channel<T>`

### 3.4 Выбор коллекции — таблица сравнения Big O

🟢 **Мини-упражнения:**
1. ⭐ Создай словарь и обработай ситуацию отсутствующего ключа (TryGetValue)
2. ⭐ Используй HashSet для нахождения пересечения двух массивов
3. ⭐ Реализуй стек вызовов (Stack) и очередь заданий (Queue)
4. ⭐ Используй PriorityQueue для задач с приоритетом
5. ⭐ Создай FrozenDictionary из словаря, замерь скорость поиска

**Практика Uno Platform:** ⭐⭐⭐
"Словарь иностранных слов". Dictionary для слов, ObservableCollection для привязки к ListView, HashSet тегов, Queue истории, PriorityQueue для слов по сложности.

**Практика Godot:** ⭐⭐⭐
Система квестов с коллекциями: Dictionary квестов, Queue диалогов, Stack для undo, HashSet достижений, PriorityQueue для приоритета целей AI.

---

## МОДУЛЬ 4: ПРОДВИНУТЫЙ C#

### 4.1 Обобщения (Generics)

#### 4.1.1 Обобщённые классы и методы
#### 4.1.2 Ограничения (Constraints): `where T : class/struct/new()/BaseClass/IInterface/notnull/unmanaged`
- `allows ref struct` (C# 13)
#### 4.1.3 Ковариантность (`out T`) и контравариантность (`in T`)
#### 4.1.4 Обобщённые делегаты: `Func<T>`, `Action<T>`, `Predicate<T>`

🟢 **Мини-упражнения:**
1. ⭐ Создай `class Repository<T> where T : class, new()` с Add, Remove, GetAll
2. ⭐ Покажи ковариантность: `IEnumerable<Dog>` присваивается `IEnumerable<Animal>`
3. ⭐ Создай generic метод `T Max<T>(T a, T b) where T : IComparable<T>`

**Практика Uno Platform:** ⭐⭐⭐
Универсальный `DataGrid<T>` — принимает List<T>, отображает свойства в таблице через рефлексию, поддержка сортировки.

**Практика Godot:** ⭐⭐⭐
`ObjectPool<T> where T : Node` — пул объектов для пуль, врагов, эффектов. Методы `Get()`, `Return()`. Используй в стрельбе.

---

### 4.2 Делегаты и события

#### 4.2.1 Делегаты — объявление, многоадресные, анонимные методы
- `Action`, `Action<T>`, `Func<T,TResult>`, `Predicate<T>`

#### 4.2.2 Лямбда-выражения
- `=>`, statement lambdas, замыкания (closures)
- Static lambdas: `static () => ...` (C# 9) — запрет захвата переменных
- Lambda natural type: `var f = () => 1;` (C# 10)

#### 4.2.3 События (Events)
- `event`, publisher/subscriber, `EventHandler<T>`
- Кастомные EventArgs
- Отписка (предотвращение утечек памяти)

🟢 **Мини-упражнения:**
1. ⭐ Создай `Func<int,int,int>` для сложения, вычитания, умножения, вызови
2. ⭐ Покажи проблему замыкания в цикле: `for (int i=0; i<5; i++) actions.Add(() => Console.Write(i));`
3. ⭐ Используй static lambda, попробуй захватить переменную — получи ошибку
4. ⭐ Создай event, подпишись из двух мест, отпишись от одного

**Практика Uno Platform:** ⭐⭐⭐
"Таймер с уведомлениями". `CountdownTimer` с событиями OnTick, OnCompleted, OnWarning. Несколько таймеров, прогресс-бары, визуальные уведомления.

**Практика Godot:** ⭐⭐⭐
Event Bus (Singleton) `GameEvents`: OnPlayerDamaged, OnEnemyKilled, OnItemCollected. UI, звук, частицы подписываются — слабая связность.

---

### 4.3 LINQ

#### 4.3.1 Синтаксис запросов: `from`, `where`, `select`, `orderby`, `group by`, `join`, `let`
#### 4.3.2 Синтаксис методов: Where, Select, SelectMany, OrderBy, GroupBy, Join, First, Any, All, Count, Sum, Average, Min, Max, Take, Skip, Distinct, Union, Intersect, Except, Zip, Aggregate, ToList, ToArray, ToDictionary, ToHashSet, Chunk (.NET 6+)
#### 4.3.3 Отложенное выполнение (Deferred Execution) vs немедленное
#### 4.3.4 LINQ и производительность — когда LINQ медленнее ручного цикла

🟢 **Мини-упражнения (по нарастающей):**
1. ⭐ Отфильтруй список чисел: оставь только чётные, отсортируй по убыванию
2. ⭐ Сгруппируй список людей по возрасту, посчитай количество в каждой группе
3. ⭐⭐ Join двух списков: Orders и Products по ProductId
4. ⭐⭐ Используй Aggregate для "схлопывания" списка строк в одну
5. ⭐⭐ Используй Chunk(10) для разбиения большого списка на страницы

**Практика Uno Platform:** ⭐⭐⭐
"Аналитика продаж". Генерируй данные, фильтруй по дате/региону, группируй по продавцу, топ-10 товаров, средний чек. Таблицы и диаграммы.

**Практика Godot:** ⭐⭐⭐
Система целей AI: `enemies.Where(e => e.IsAlive).OrderBy(e => pos.DistanceTo(e.Pos)).FirstOrDefault()`. Группировка врагов по типу. Визуализация.

---

### 4.4 Обработка исключений
- try-catch-finally, множественные catch, фильтры `when`
- Иерархия: NullReferenceException, ArgumentException, InvalidOperationException, etc.
- Пользовательские исключения (наследование от Exception)
- `throw;` vs `throw ex;`, throw expressions
- Guard clauses: `ArgumentNullException.ThrowIfNull()` (.NET 6+)
- **Result pattern** (альтернатива исключениям): `Result<T>`, `OneOf<T0,T1>`

🟢 **Мини-упражнения:**
1. ⭐ Напиши try-catch с фильтром `when`: лови только IOException с определённым HResult
2. ⭐ Создай свой Exception: `InsufficientFundsException(decimal required, decimal available)`
3. ⭐ Покажи разницу между `throw;` и `throw ex;` через стек вызовов
4. ⭐ "Найди утечку" — пример где Exception теряет original stack trace

**Практика Uno / Godot:** интегрируется в другие проекты.

---

### 4.5 Асинхронное программирование

#### 4.5.1 async/await — `Task`, `Task<T>`, `ValueTask`, `ValueTask<T>`
#### 4.5.2 Под капотом: state machine, SynchronizationContext, ConfigureAwait(false)
#### 4.5.3 Параллельное: `Task.WhenAll()`, `Task.WhenAny()`, `Task.Run()`, `Parallel.ForEachAsync()`
#### 4.5.4 Отмена: `CancellationToken`, `CancellationTokenSource`
#### 4.5.5 Асинхронные потоки: `IAsyncEnumerable<T>`, `await foreach`
#### 4.5.6 Проблемы: deadlocks, async void, fire-and-forget, thread safety

> ⚠️ ТЕРМИНАЛ полезен для экспериментов с SynchronizationContext в консольных приложениях.

🟢 **Мини-упражнения:**
1. ⭐ Напиши async метод, который "загружает данные" (Task.Delay)
2. ⭐ Запусти 3 задачи параллельно через Task.WhenAll, покажи результаты
3. ⭐⭐ Реализуй CancellationToken: начни загрузку, отмени через 2 секунды
4. ⭐⭐ Создай IAsyncEnumerable, который yield-ит числа с задержкой, потреби через await foreach
5. ⭐⭐ "Найди deadlock" — код с `.Result` в UI-потоке

**Практика Uno Platform:** ⭐⭐⭐
"Загрузчик изображений". URL-ы → параллельная загрузка (HttpClient + Task.WhenAll), прогресс каждой, CancellationToken, кнопка "Отменить".

**Практика Godot:** ⭐⭐⭐
Процедурный генератор мира. Чанки генерируются в фоне (Task.Run). Индикатор загрузки. CancellationToken при выходе из зоны. `ToSignal()` для await сигналов Godot.

---

### 4.6 Паттерн-матчинг (Pattern Matching)
- Типовой: `is Type var`, константный: `is null`, `is 42`
- Реляционный: `is > 0 and <= 100`
- Логический: `and`, `or`, `not`
- Свойств: `is { Property: value }`
- Позиционный (деконструкция), Var-паттерн
- Списков: `is [1, 2, .., var last]` (C# 11)
- Switch expressions с паттернами

🟢 **Мини-упражнения:**
1. ⭐ Классифицируй число через switch expression: отрицательное, ноль, положительное, > 100
2. ⭐ Используй list pattern: `if (array is [var first, .., var last])`
3. ⭐⭐ Паттерн свойств: `person is { Age: >= 18, Name.Length: > 0 }`

**Практика Godot:** ⭐⭐⭐
Система столкновений через pattern matching — switch по типу body с property patterns.

---

### 4.7 Индексаторы и операторы
- Индексаторы: `this[int index]`, с разными ключами
- Перегрузка операторов: `+`, `-`, `*`, `==`, `!=`, `<`, `>`
- Неявное/явное приведение: `implicit`, `explicit`
- Checked операторы (C# 11): `checked operator +`
- User-defined compound assignment (C# 14): `+=`, `-=`

🟢 **Мини-упражнения:**
1. ⭐ Создай класс Vector2D с перегруженными `+`, `-`, `*scalar`
2. ⭐ Добавь implicit conversion `int → Vector2D` и explicit `Vector2D → (int,int)`

**Практика Uno Platform:** ⭐⭐
Класс `Matrix` с `+`, `-`, `*`, индексатором `this[int row, int col]`. UI для ввода и отображения.

**Практика Godot:** ⭐⭐
`GameCurrency` с перегруженными операторами. Магазин: цены, покупка, проверка.

---

## МОДУЛЬ 5: РАБОТА С ФАЙЛАМИ И ДАННЫМИ

### 5.1 Файловая система
- `File`, `FileInfo`, `Directory`, `DirectoryInfo`, `Path`
- Чтение/запись: `ReadAllText`, `WriteAllText`, `ReadAllLines`, `StreamReader`, `StreamWriter`
- Потоки: `Stream`, `MemoryStream`, `FileStream`, `BinaryReader`, `BinaryWriter`
- `using` / `await using` и `IDisposable`
- `FileOpenPicker`, `FileSavePicker` (Uno Platform)

### 5.2 Сериализация
#### 5.2.1 JSON (`System.Text.Json`)
- `JsonSerializer.Serialize/Deserialize`, `JsonSerializerOptions`
- Атрибуты: `[JsonPropertyName]`, `[JsonIgnore]`, `[JsonInclude]`
- Полиморфная сериализация: `[JsonDerivedType]`, `[JsonPolymorphic]` (.NET 7+)
- Source generators: `[JsonSerializable]`
- Высокопроизводительный: `Utf8JsonReader`, `Utf8JsonWriter`
- Новое в .NET 10: `JsonSerializerOptions.Web`

#### 5.2.2 XML: `XDocument`, `XElement` (LINQ to XML), `XmlSerializer`

### 5.3 Конфигурация
- `appsettings.json`, `IConfiguration`, Options pattern (`IOptions<T>`)
- `Environment` класс: переменные окружения, аргументы командной строки

🟢 **Мини-упражнения:**
1. ⭐ Сериализуй List<Person> в JSON, десериализуй обратно
2. ⭐ Используй `[JsonDerivedType]` для полиморфной сериализации Shape → Circle/Rectangle
3. ⭐⭐ Прочитай файл построчно через StreamReader, посчитай строки

**Практика Uno Platform:** ⭐⭐⭐
"Заметки" с JSON-сохранением. FileOpenPicker/FileSavePicker для экспорта. Настройки в отдельном файле.

**Практика Godot:** ⭐⭐⭐
Система сохранения/загрузки. JSON с полиморфными данными. Несколько слотов. Автосохранение. Обработка повреждённых файлов.

---

## МОДУЛЬ 6: УГЛУБЛЁННЫЕ ТЕМЫ C#

### 6.1 Функциональное программирование
- Функции высшего порядка, каррирование, частичное применение
- Замыкания: как работают, проблемы в циклах
- 🔒 Expression Trees: `Expression<Func<T, bool>>`, компиляция — **ОТЛОЖЕНО, изучать при необходимости**

### 6.2 Рефлексия (Reflection)
- `typeof()`, `GetType()`, `Type`, `Assembly`
- `GetMethods()`, `GetProperties()`, `GetCustomAttributes()`
- `Activator.CreateInstance()`, `MethodInfo.Invoke()`
- Атрибуты: стандартные (`[Obsolete]`, `[Conditional]`), создание своих
- Generic attributes (C# 11): `[MyAttribute<string>]`
- Source Generators (compile-time рефлексия) — введение

### 6.3 Потоки и многопоточность
- `Thread` класс, `Thread.Sleep()`, `Thread.Join()`
- Синхронизация: `lock` / `System.Threading.Lock` (C# 13), `Monitor`, `Mutex`, `Semaphore`, `SemaphoreSlim`, `ReaderWriterLockSlim`, `Interlocked`, `volatile`
- `scoped` модификатор (C# 11) для ref-параметров
- TPL: `Task.Run()`, `Parallel.For/ForEach/ForEachAsync`, `TaskCompletionSource`
- Channels: `Channel<T>` — producer/consumer
- ThreadPool, `PeriodicTimer` (.NET 6+)

> ⚠️ ТЕРМИНАЛ полезен для экспериментов с deadlocks и race conditions.

🟢 **Мини-упражнения:**
1. ⭐⭐ Создай race condition с двумя потоками, инкрементирующими общий счётчик. Исправь через lock.
2. ⭐⭐ Используй SemaphoreSlim(3) для ограничения параллельных операций
3. ⭐⭐ Создай producer-consumer через Channel<T>

**Практика Uno Platform:** ⭐⭐⭐
"Параллельный обработчик изображений". Обработка нескольких картинок параллельно, SemaphoreSlim для лимита, прогресс каждой.

**Практика Godot:** ⭐⭐⭐
Процедурная генерация ландшафта в фоновых потоках. Channel<ChunkData> для передачи готовых чанков в основной поток.

### 6.4 Управление памятью
- GC: поколения, LOH, финализаторы
- IDisposable: Dispose pattern, using, await using
- `Span<T>`, `Memory<T>`, `ArrayPool<T>`, `stackalloc`
- `WeakReference<T>`
- Профилирование: dotMemory, VS Diagnostic Tools

> ⚠️ ТЕРМИНАЛ: `dotnet-counters`, `dotnet-trace` для профилирования.

### 🔒 6.5 Unsafe-код и интероп — ОТЛОЖЕНО
> Изучать только при реальной необходимости (работа с нативными библиотеками, экстремальная оптимизация)
- `unsafe`, указатели, `fixed`, `stackalloc`
- P/Invoke, `[LibraryImport]` (.NET 7+)
- NativeAOT компиляция

---

## МОДУЛЬ 7: ОТЛАДКА И ИНСТРУМЕНТЫ РАЗРАБОТЧИКА

> Этот модуль критически важен — без отладки невозможно быть эффективным разработчиком.

### 7.1 Отладка (Debugging)

#### 7.1.1 Breakpoints
- Обычные breakpoints
- Conditional breakpoints (условие для остановки)
- Hit count breakpoints (остановка после N попаданий)
- Tracepoints (logpoint — выводит сообщение без остановки)
- Exception breakpoints (остановка при определённом исключении)

#### 7.1.2 Окна отладки
- Watch — наблюдение за переменными
- Locals / Autos — локальные переменные
- Immediate Window — выполнение выражений во время отладки
- Call Stack — стек вызовов
- Threads — потоки
- Tasks — задачи (для async кода)
- Output — вывод отладки

#### 7.1.3 Навигация
- Step Over (F10), Step Into (F11), Step Out (Shift+F11)
- Run to Cursor
- Set Next Statement (перемещение точки выполнения)

#### 7.1.4 Debug vs Release конфигурация
- Оптимизации компилятора
- Символы отладки (.pdb)
- `#if DEBUG`

#### 7.1.5 Отладка Godot C# проектов
- Подключение отладчика из VS/Rider к Godot
- Breakpoints в C# скриптах Godot
- Godot Debugger (встроенный)

### 7.2 Логирование (Logging)

#### 7.2.1 `Microsoft.Extensions.Logging`
- `ILogger`, `ILoggerFactory`
- Log levels: Trace, Debug, Information, Warning, Error, Critical
- Structured logging: `_logger.LogInformation("User {UserId} logged in", userId);`
- `LoggerMessage.Define()` — высокопроизводительное логирование

#### 7.2.2 Serilog (популярная библиотека)
- Sinks: Console, File, Seq
- Enrichers

#### 7.2.3 `Debug.WriteLine()`, `Trace.WriteLine()`
- `GD.Print()`, `GD.PrintErr()` в Godot

### 7.3 Стиль кода и анализаторы

#### 7.3.1 Именование (.NET conventions)
- PascalCase для публичных, camelCase для приватных, _camelCase для полей
- Префиксы: I для интерфейсов, T для generic параметров

#### 7.3.2 .editorconfig — правила стиля для проекта
#### 7.3.3 Анализаторы кода
- Roslyn analyzers, StyleCop.Analyzers
- Severity levels: Error, Warning, Suggestion, Hidden

### 7.4 XML-документация
- `<summary>`, `<param>`, `<returns>`, `<exception>`, `<remarks>`, `<example>`
- `<see cref=""/>`, `<seealso/>`
- Генерация документации
- IntelliSense интеграция

🟢 **Мини-упражнения:**
1. ⭐ Поставь conditional breakpoint: остановись только когда i > 50
2. ⭐ Используй Immediate Window для вызова метода во время отладки
3. ⭐ Намеренно создай NullReferenceException, поймай через Exception breakpoint
4. ⭐ Настрой ILogger, выведи логи разных уровней
5. ⭐ Создай .editorconfig с правилами именования

**Практика Uno Platform:** ⭐⭐
Добавь логирование (ILogger) в "Финансовый трекер". Логируй: добавление транзакций, ошибки, время операций. Structured logging.

**Практика Godot:** ⭐⭐
Создай дебаг-оверлей для игры: FPS, количество объектов, память (GC), логи последних событий. Включение/выключение по F3. Подключи отладчик из IDE.

---

## МОДУЛЬ 8: АРХИТЕКТУРНЫЕ ПАТТЕРНЫ

### 8.1 SOLID-принципы
- S: Single Responsibility, O: Open/Closed, L: Liskov Substitution, I: Interface Segregation, D: Dependency Inversion

🟢 **Мини-упражнения:**
1. ⭐ "Нарушение SOLID" — 5 примеров кода, определи какой принцип нарушен и исправь
2. ⭐⭐ Рефакторинг: класс God Object → разбей на компоненты по SRP

**Практика Uno Platform:** ⭐⭐⭐
Рефакторь "Заметки" по SOLID: разделяй хранение, UI-логику, валидацию. Маленькие интерфейсы. Зависимости через абстракции.

**Практика Godot:** ⭐⭐⭐
Компонентная архитектура: `HealthComponent`, `MovementComponent`, `AttackComponent`. Враги и игрок используют одни компоненты.

---

### 8.2 Паттерны проектирования (Design Patterns)

#### Порождающие: Singleton, Factory Method, Abstract Factory, Builder, Prototype
#### Структурные: Adapter, Decorator, Facade, Composite, Proxy, Bridge, Flyweight
#### Поведенческие: Observer, Strategy, Command, State, Template Method, Iterator, Mediator, Chain of Responsibility, Memento, Visitor

🟢 **Мини-упражнения (по 1 на каждый ключевой паттерн):**
1. ⭐ Singleton — создай thread-safe singleton
2. ⭐ Strategy — 3 алгоритма сортировки через интерфейс ISortStrategy
3. ⭐ Observer — простой EventAggregator
4. ⭐ Factory — фабрика создаёт разные типы уведомлений
5. ⭐⭐ State — светофор (Red → Green → Yellow → Red)
6. ⭐⭐ Command — калькулятор с undo (через стек команд)
7. ⭐⭐ Decorator — поток данных с шифрованием и сжатием (декораторы на Stream)
8. ⭐⭐ Builder — конструктор HTTP-запроса: `.WithUrl().WithHeader().WithBody().Build()`

**Практика Uno Platform (3 больших задачи):**
1. ⭐⭐ **Command + Memento:** Текстовый редактор с undo/redo
2. ⭐⭐⭐ **Builder + Decorator + Facade:** Конструктор форм + уведомления + единый API хранилища
3. ⭐⭐ **Observer + Singleton:** Сервис настроек, UI обновляется при смене темы

**Практика Godot (3 больших задачи):**
1. ⭐⭐⭐ **State Machine:** Состояния персонажа (Idle, Run, Jump, Attack) с переходами
2. ⭐⭐⭐ **Strategy + Observer:** AI с переключаемыми стратегиями + EventBus
3. ⭐⭐ **Command + Object Pool:** Система ввода (переназначение клавиш) + пул объектов

---

### 8.3 Архитектурные паттерны

#### 8.3.1 MVVM — Data Binding, INotifyPropertyChanged, Commands, CommunityToolkit.MVVM
#### 8.3.2 MVUX — Uno Platform подход: Feeds, States
#### 8.3.3 Clean Architecture — Domain, Application, Infrastructure, Presentation
#### 8.3.4 Repository, Unit of Work, CQRS (+ MediatR)
#### 🔒 8.3.5 Vertical Slice Architecture — ОТЛОЖЕНО (альтернатива Clean Architecture — изучить после освоения Clean Architecture)
#### 8.3.6 Result Pattern — углублённо: `Result<T, TError>`, Railway-oriented programming

### 8.4 Domain-Driven Design (DDD) — основы
- Entities vs Value Objects
- Aggregates, Aggregate Roots
- Domain Events
- Bounded Contexts
- Ubiquitous Language

**Практика Uno Platform:** ⭐⭐⭐
"Финансовый трекер" с Clean Architecture + DDD: Domain (Transaction, Category, Budget entities), Application (use cases), Infrastructure (JSON/SQLite), Presentation (MVVM).

**Практика Godot:** ⭐⭐⭐
Мини-RPG с Component System + State Machine + Event Bus: Core (компоненты), Systems (CombatSystem, QuestSystem), Data (Resources для предметов, квестов).

---

## МОДУЛЬ 9: DEPENDENCY INJECTION

### 9.1 Концепция: Constructor / Property / Method Injection
### 9.2 Microsoft.Extensions.DependencyInjection: AddTransient/Scoped/Singleton, IServiceProvider
### 9.3 DI в Uno Platform: регистрация, ViewModels, навигация
### 9.4 DI в Godot: свой контейнер, AutoLoad как Service Locator

**Практика Uno Platform:** ⭐⭐⭐
Рефакторинг "Финансового трекера" с полным DI.

**Практика Godot:** ⭐⭐
Простой DI-контейнер: `IAudioService`, `ISaveService` — интерфейсы. Подмена реализаций для тестирования.

---

## МОДУЛЬ 10: UNO PLATFORM — ГЛУБОКОЕ ИЗУЧЕНИЕ

### 10.1 Основы XAML
- Элементы, атрибуты, пространства имён
- Markup Extensions: `{Binding}`, `{x:Bind}`, `{StaticResource}`, `{ThemeResource}`
- Layout: StackPanel, Grid, RelativePanel, Canvas, Border, ScrollViewer, WrapPanel

### 10.2 Элементы управления
- TextBlock, TextBox, PasswordBox, RichTextBlock
- Button, ToggleButton, RepeatButton, HyperlinkButton
- CheckBox, RadioButton, ToggleSwitch
- Slider, ProgressBar, ProgressRing
- ComboBox, ListView, GridView, ItemsRepeater (виртуализация!)
- Image, DatePicker, TimePicker, CalendarView
- MenuBar, CommandBar, NavigationView
- **ContentDialog** — модальные диалоги
- **Flyout, MenuFlyout, TeachingTip** — всплывающие элементы

### 10.3 Data Binding
- `{Binding}` vs `{x:Bind}` (компилируемая)
- OneWay, TwoWay, OneTime
- DataContext, наследование DataContext
- INotifyPropertyChanged, CommunityToolkit: `[ObservableProperty]`
- **IValueConverter** — кастомные конвертеры
- DataTemplate, DataTemplateSelector

### 10.4 Навигация
- Frame.Navigate(), GoBack(), передача параметров
- Uno.Extensions.Navigation (route-based)
- NavigationView, SplitView

### 10.5 Стили и ресурсы
- StaticResource vs ThemeResource
- ResourceDictionary, именованные/неявные стили, `BasedOn`
- Light/Dark/Custom темы
- ControlTemplate, VisualStateManager

### 10.6 Анимации
- Storyboard: DoubleAnimation, ColorAnimation, Easing
- VisualStateManager: состояния, AdaptiveTrigger
- Transitions

### 10.7 Custom Controls и Dependency Properties
- **DependencyProperty.Register()** — создание своих свойств
- **Attached properties** — создание своих
- **Custom controls** — наследование от Control, ControlTemplate
- **UserControl** vs Custom Control
- **Behaviors** (Microsoft.Xaml.Behaviors): EventTriggerBehavior, InvokeCommandAction

### 10.8 MVVM с CommunityToolkit
- `[ObservableProperty]`, `[RelayCommand]`
- **Messenger** — обмен между ViewModels
- **ObservableValidator** — валидация

### 10.9 Работа с данными
- HttpClient, REST API, десериализация
- SQLite, LocalFolder, LocalSettings
- **Clipboard** (DataPackage)
- **Drag and Drop** (DragStarting, Drop, AllowDrop)
- **FileOpenPicker, FileSavePicker, FolderPicker**

### 10.10 Адаптивный и отзывчивый UI
- Адаптивные триггеры (ширина экрана)
- Compact/Expanded views
- Телефон / Планшет / Десктоп — разные layouts
- Tailored views

### 10.11 Локализация в Uno Platform
- `x:Uid` для XAML, `.resw` ресурсные файлы
- `ResourceLoader`, смена языка в runtime
- RTL поддержка

### 10.12 Accessibility (Доступность)
- `AutomationProperties.Name`, `AutomationProperties.LabeledBy`
- Keyboard navigation, Tab order
- High contrast, Screen reader support

### 10.13 C# Markup (альтернатива XAML)
- Построение UI в C# коде
- Когда C# Markup vs XAML

### 10.14 Платформо-специфичный код
- `#if __ANDROID__`, `#if __IOS__`, `#if __WASM__`, `#if WINDOWS`
- Partial classes, нативные API

### 10.15 Uno.Extensions
- Uno.Extensions.Http, Serialization, Logging, Configuration

**Практики (по нарастающей):**
1. ⭐⭐ "Dashboard" — Grid layout, карточки KPI, NavigationView
2. ⭐⭐ "Каталог фильмов" — ListView + DataTemplate + TwoWay Binding + Converter
3. ⭐⭐⭐ "CRM" — валидация, Messenger, Detail page, адаптивный UI
4. ⭐⭐⭐ "Погода" — HttpClient + API + SQLite кэш + анимации
5. ⭐⭐ Создай кастомный контрол `RatingControl` (звёздочки) с DependencyProperty `Value`
6. ⭐⭐ Локализуй "Каталог фильмов" на 3 языка с x:Uid

---

## МОДУЛЬ 11: GODOT 4.6 — ГЛУБОКОЕ ИЗУЧЕНИЕ

### 11.1 Архитектура Godot
- Сцены и узлы, дерево сцен
- Жизненный цикл: `_Ready()`, `_Process()`, `_PhysicsProcess()`, `_EnterTree()`, `_ExitTree()`, `_Notification()`
- Scene composition vs inheritance — когда что
- `GetNode<T>()`, `GetNodeOrNull<T>()`, `$` path syntax
- Группы узлов: `AddToGroup()`, `GetNodesInGroup()`, `CallGroup()`

### 11.2 C# в Godot — специфика
- **[Export]** атрибут — ВСЁ о нём:
  - Примитивы, строки, enum, Resource, NodePath, PackedScene
  - `[ExportGroup("Name")]`, `[ExportSubgroup("Name")]`, `[ExportCategory("Name")]`
  - Hints: `[Export(PropertyHint.Range, "0,100,1")]`, `[Export(PropertyHint.File, "*.png")]`
  - Экспорт массивов, словарей
  - Экспорт кастомных ресурсов
- **Маршалинг**: Variant, StringName, NodePath — как работает обмен между C# и Godot
- **[GlobalClass]** — регистрация C# класса как Godot-тип
- **[Tool]** — скрипты, выполняющиеся в редакторе
- `CallDeferred()`, `SetDeferred()` — отложенные вызовы
- **Coroutines**: `await ToSignal(timer, Timer.SignalName.Timeout)`

### 11.3 Сигналы (Signals)
- Встроенные: подключение в редакторе и коде (`signal += handler`)
- Кастомные: `[Signal] delegate void NameEventHandler(params)`
- `EmitSignal(SignalName.Name, args)`
- Паттерн: Call down, Signal up
- Await сигналов: `await ToSignal(node, "signal_name")`

### 11.4 Ввод (Input)
- Input Map, `Input.IsActionPressed/JustPressed/JustReleased`
- `_Input()`, `_UnhandledInput()`, типы InputEvent
- Тач: InputEventScreenTouch, InputEventScreenDrag
- **Input buffering**: jump buffer, coyote time — для responsive controls

### 11.5 Физика (Jolt Physics в 4.6)
- CharacterBody2D/3D: `MoveAndSlide()`, Velocity
- RigidBody2D/3D: силы, импульсы
- StaticBody2D/3D, Area2D/3D (триггеры)
- CollisionShape, CollisionLayers/Masks
- RayCast2D/3D, ShapeCast
- **Jolt Physics** для 3D (новый движок в 4.6)

### 11.6 Анимации
- AnimationPlayer: ключевые кадры, треки, Play/Stop/Queue
- AnimationTree: State Machine, Blend Trees
- **Tween**: `CreateTween()`, `TweenProperty()`, `TweenCallback()`, `TweenInterval()`, Easing
- AnimatedSprite2D, SpriteFrames

### 11.7 2D-разработка
- TileMap, TileSet, авто-тайлинг (terrain), слои
- Camera2D: следование, ограничения, screen shake (trauma-based, Perlin noise)
- Parallax: ParallaxBackground, ParallaxLayer
- 2D Light: PointLight2D, DirectionalLight2D, LightOccluder2D, тени

### 11.8 3D-разработка
- MeshInstance3D, CSGShape3D, Materials (StandardMaterial3D, ShaderMaterial)
- 3D физика, CharacterBody3D (FPS/TPS контроллер)
- Освещение: DirectionalLight3D, OmniLight3D, SpotLight3D, GI
- Camera3D: FPS, Third Person, Orbit

### 11.9 UI в играх
- Control-узлы, Anchors, Container-ы, Theme
- HUD: TextureProgressBar, миникарта, floating damage numbers (Tween)
- Меню: главное, пауза, настройки, Game Over
- **Scene transitions**: fade in/out, SceneTree.ChangeSceneToFile(), additive loading

### 11.10 Аудио
- AudioStreamPlayer/2D/3D, AudioBus, пространственный звук
- AudioManager Singleton, fade music transitions

### 11.11 Навигация и AI
- NavigationServer2D/3D, NavigationAgent
- FSM для AI: Patrol → Detect → Chase → Attack → Return
- Steering Behaviours: Seek, Flee, Arrive, Wander, Pursue
- Line of Sight (RayCast)

### 11.12 Математика для игр
- **Векторы**: dot product, cross product, нормализация, отражение
- **Интерполяция**: Lerp, Slerp, MoveToward, SmoothStep
- **Кривые**: Bezier, Curve2D, Path2D
- **Тригонометрия**: Atan2, углы, повороты
- **Transform2D/3D**: манипуляция трансформациями
- **Quaternions** (3D): вращения без gimbal lock

### 11.13 Шейдеры и VFX
- Vertex, Fragment, Light шейдеры, Uniforms
- Visual Shaders (нодовый редактор)
- Частицы: GPUParticles2D/3D, CPUParticles
- Постобработка: WorldEnvironment, Glow, SSAO, SSR, AGX Tone Mapping (4.6)

### 11.14 Game Feel / Juice / Polish
- Screen shake (trauma + Perlin noise)
- Hit stop / freeze frames
- Squash & stretch (scale animation)
- Particle bursts on impact
- Camera effects: zoom in/out, slow motion
- Visual и audio feedback на каждое действие

### 11.15 Кастомные ресурсы и данные
- Создание Resource классов с `[GlobalClass]`
- Наследование ресурсов
- Editor Tools (`[Tool]`): кастомные инструменты редактора

### 11.16 Viewport и SubViewport
- Render textures, picture-in-picture
- Миникарта через SubViewport
- Split-screen

### 11.17 Локализация в Godot
- `TranslationServer`, CSV/PO файлы, `Tr()`, font fallbacks

### 11.18 Экспорт
- Windows, Linux, macOS, Android, iOS, Web (C# поддержка в 4.6)
- Export presets, оптимизация для мобильных

> ⚠️ ТЕРМИНАЛ: Android SDK, подписи, CI/CD.

**Практики (по нарастающей):**
1. ⭐⭐ Контроллер персонажа: WASD + мышь + геймпад + тач (виртуальный джойстик)
2. ⭐⭐ Физическая головоломка: платформы, RigidBody, Area2D зоны, RayCast лазеры
3. ⭐⭐⭐ 2D-платформер: TileMap, Parallax, Camera, свет, враги, монеты, boss
4. ⭐⭐⭐ Tower Defense: NavigationAgent враги, башни, волны, LINQ для выбора целей
5. ⭐⭐⭐ 3D подземелье: FPS контроллер, освещение, интерактивные объекты, AI врагов
6. ⭐⭐ Полная система UI: меню, HUD, инвентарь, настройки, scene transitions
7. ⭐⭐ Аудио система: AudioManager, AudioBus, fade transitions, позиционный звук
8. ⭐⭐⭐ Game Feel: добавь juice ко всем действиям в платформере (shake, particles, hitstop)

---

## МОДУЛЬ 12: БАЗЫ ДАННЫХ И ORM

### 12.1 SQL основы: SELECT, INSERT, UPDATE, DELETE, JOIN, нормализация

> ⚠️ ТЕРМИНАЛ / DB Browser for SQLite для изучения SQL.

### 12.2 Entity Framework Core
- DbContext, DbSet, Code First, миграции
- Fluent API vs Data Annotations (`[Required]`, `[MaxLength]`, `[Range]`)
- LINQ to Entities, Tracking/No-Tracking
- Loading: Eager, Lazy, Explicit
- Raw SQL

### 12.3 Dapper (micro-ORM) — когда EF Core, когда Dapper
### 12.4 SQLite в мобильных и играх

**Практика Uno Platform:** ⭐⭐⭐
"Библиотека книг" с EF Core + SQLite: Books, Authors, Categories. CRUD, поиск, фильтры, пагинация.

**Практика Godot:** ⭐⭐
SQLite для данных RPG: предметы, враги, диалоги. Загрузка при старте, сохранение прогресса.

---

## МОДУЛЬ 13: СЕТЕВОЕ ПРОГРАММИРОВАНИЕ

### 13.1 HttpClient, HttpClientFactory, Polly (retry policies)
### 13.2 REST API, статус-коды, версионирование
### 13.3 WebSockets, SignalR
### 🔒 13.4 gRPC — ОТЛОЖЕНО (Protobuf, streaming — изучать при работе с микросервисами)
### 13.5 Мультиплеер в Godot: High-level API, MultiplayerSpawner/Synchronizer, RPC, Authority

**Практика Uno Platform:** ⭐⭐⭐
"Чат" через SignalR: сообщения, пользователи онлайн, история, "печатает...".

> ⚠️ ТЕРМИНАЛ: ASP.NET Core сервер.

**Практика Godot:** ⭐⭐⭐
Мультиплеер 2-4 игрока по LAN: lobby, синхронизация, RPC для действий, PvP-арена.

---

## МОДУЛЬ 14: ТЕСТИРОВАНИЕ

### 14.1 Unit-тесты: xUnit, NUnit, MSTest
- Arrange-Act-Assert, `[Fact]`, `[Theory]`, `[InlineData]`
- **Mocking**: Moq, NSubstitute

### 14.2 Интеграционные тесты, TDD (Red-Green-Refactor)
### 14.3 Тестирование в Godot: GdUnit4
### 14.4 "Намеренно сломай" упражнения
- Создай memory leak → найди через профайлер
- Создай deadlock → найди через отладчик
- Создай race condition → найди через тест

> ⚠️ ТЕРМИНАЛ: `dotnet test`.

**Практика Uno Platform:** ⭐⭐⭐
Тесты для "Финансового трекера": бизнес-логика, мок репозитория, тесты ViewModel. 20+ тестов, покрытие > 80%.

**Практика Godot:** ⭐⭐⭐
Тесты: HealthComponent, Inventory, DamageCalculator, QuestSystem через GdUnit4.

---

## МОДУЛЬ 15: БЕЗОПАСНОСТЬ

### 15.1 OWASP Top 10, Input validation
### 15.2 Криптография: SHA256, HMAC, AES, RSA, `System.Security.Cryptography`
### 15.3 Хранение: DPAPI, Secure Storage
### 15.4 Аутентификация: JWT, OAuth 2.0, ASP.NET Core Identity

**Практика Uno Platform:** ⭐⭐⭐
"Менеджер паролей": мастер-пароль (хеш), AES шифрование, генератор паролей, авто-очистка буфера.

**Практика Godot:** ⭐⭐
Защищённые сохранения: AES шифрование, checksum, защита от редактирования.

---

## МОДУЛЬ 16: ПРОИЗВОДИТЕЛЬНОСТЬ И ОПТИМИЗАЦИЯ

### 16.1 Профилирование: VS Profiler, dotTrace, BenchmarkDotNet, Godot Profiler

> ⚠️ ТЕРМИНАЛ: `dotnet-counters`, `dotnet-trace`.

### 16.2 Оптимизации C#
- Span<T>, Memory<T>, ArrayPool<T>, stackalloc, ValueTask
- Кэширование: MemoryCache, Lazy<T>
- Коллекции: FrozenDictionary, начальная capacity
- SIMD: Vector128, Vector256

### 16.3 Оптимизация Godot
- Рендеринг: Culling, LOD, Occlusion, Instancing
- Физика: CollisionLayers, упрощение shapes
- Маршалинг C#↔Godot
- Object pooling

**Практика Uno Platform:** ⭐⭐⭐
Оптимизируй "Каталог фильмов": виртуализация (ItemsRepeater), ленивая загрузка, MemoryCache, BenchmarkDotNet замеры.

**Практика Godot:** ⭐⭐⭐
1000+ врагов на 60 FPS: object pooling, spatial partitioning, Span для данных, профилирование.

---

## МОДУЛЬ 17: CI/CD И DEVOPS

### 17.1 Git углублённо: branching (Git Flow, GitHub Flow), merge vs rebase, cherry-pick, stash, hooks, .gitignore, LFS (для ассетов)
### 17.2 CI/CD: GitHub Actions, Azure DevOps
### 17.3 NuGet: создание пакетов, SemVer
### 17.4 Docker: Dockerfile, Docker Compose

> ⚠️ ТЕРМИНАЛ обязателен для всего модуля.

**Практика:**
1. ⭐⭐ GitHub Actions: автосборка Uno + Godot export
2. ⭐⭐ Автотесты при PR
3. ⭐⭐ Свой NuGet-пакет с утилитами
4. ⭐⭐ Git LFS для ассетов Godot-проекта

---

## МОДУЛЬ 18: ASP.NET CORE (БЭКЕНД)

### 18.1 Middleware, Routing, Controllers vs Minimal API
### 18.2 Web API: REST, Model binding, Validation, Swagger
### 18.3 SignalR: Hubs, Clients, real-time
### 18.4 Identity: JWT, OAuth, Roles, Claims, Policies
### 18.5 Minimal API (.NET 7+): endpoints, filters
### 18.6 Background Services: IHostedService, BackgroundService, PeriodicTimer
### 18.7 Health Checks, Rate Limiting (.NET 7+)
### 18.8 Middleware: написание custom middleware

> ⚠️ ТЕРМИНАЛ обязателен.

**Практика Uno Platform:** ⭐⭐⭐
API для "Финансового трекера": Minimal API, JWT, CRUD, синхронизация данных.

**Практика Godot:** ⭐⭐⭐
Бэкенд для мультиплеера: SignalR, лидерборд, профили, достижения.

---

## МОДУЛЬ 19: ПРОДВИНУТЫЕ ВОЗМОЖНОСТИ .NET

### 19.1 Source Generators — compile-time code generation
### 19.2 NativeAOT — AOT компиляция, Trimming
### 19.3 System.IO.Pipelines — высокопроизводительный IO
### 19.4 System.IO.Compression — ZIP/GZIP
### 19.5 Фичи C# 13-14
- `params` для любых коллекций, `Lock` тип, `\e` escape, `ref struct` интерфейсы, partial properties (C# 13)
- `field` keyword, extension members, partial constructors, user-defined compound assignment (C# 14)
### 19.6 Resilience: Polly, Circuit Breaker, Retry policies
### 🔒 19.7 Microservices basics — ОТЛОЖЕНО (API Gateway, service discovery — изучать после опыта с монолитом)

---

## МОДУЛЬ 20: ИТОГОВЫЕ ПРОЕКТЫ

### 20.1 Итоговый проект Uno Platform: "Super Productivity App" ⭐⭐⭐

**MVP (минимум):**
- Задачи с приоритетами и дедлайнами
- Сохранение в SQLite
- MVVM + DI
- Light/Dark тема

**Полная версия:**
- Подзадачи, привычки (streak), заметки, календарь
- Статистика с графиками
- REST API бэкенд + JWT
- Синхронизация + оффлайн режим
- Уведомления, локализация (3 языка)
- Accessibility
- Unit-тесты (20+)
- Clean Architecture + DDD

```
├── Domain/           (Entities, Interfaces, Value Objects)
├── Application/      (Use Cases, DTOs, Validators)
├── Infrastructure/   (EF Core, HTTP, SQLite)
├── Presentation/     (ViewModels, Views, Converters)
├── Server/           (ASP.NET Core Minimal API)
└── Tests/            (xUnit + Moq)
```

---

### 20.2 Итоговый проект Godot: "Complete Action RPG" ⭐⭐⭐

**MVP (минимум):**
- Персонаж: движение, атака, анимации
- 3 типа врагов с AI (FSM)
- 1 уровень (TileMap)
- Здоровье, смерть, респавн

**Полная версия:**
- Ближний и дальний бой, dash, комбо
- Система урона: типы, резисты, крит, баффы/дебаффы
- Инвентарь: экипировка, расходники, drag-and-drop
- Квесты: NPC, ветвления, награды
- Диалоги с вариантами ответа
- Прокачка: уровни, дерево навыков
- Процедурные подземелья
- Полный UI: HUD, инвентарь, карта, меню, scene transitions
- Аудио: музыка + SFX + пространственный звук
- Game Feel: shake, particles, hitstop, juice
- Сохранение/загрузка (JSON + шифрование)
- Локализация (2 языка)
- Оптимизация: 60 FPS, object pooling
- Экспорт: Windows + Android
- Unit-тесты (GdUnit4)

```
├── Autoload/         (GameManager, AudioManager, SaveManager, EventBus)
├── Components/       (HealthComponent, HitboxComponent, MovementComponent)
├── Entities/Player/  (PlayerController, StateMachine, States/)
├── Entities/Enemies/ (BaseEnemy, Zombie, Skeleton, Boss)
├── Entities/NPCs/    (DialogueTrigger, QuestGiver)
├── Systems/          (QuestSystem, InventorySystem, DialogueSystem, LootSystem)
├── Resources/        (Items/, Quests/, Dialogues/ — Custom Resources)
├── UI/               (HUD/, Inventory/, Menus/, Dialogue/)
├── Levels/           (World, Dungeons)
├── Shaders/          (Water, Dissolve, Outline)
├── Audio/            (Music/, SFX/)
└── Tests/            (GdUnit4 tests)
```

---

## ДОРОЖНАЯ КАРТА (ПОРЯДОК ПРОХОЖДЕНИЯ)

```
ЭТАП 1: ОСНОВЫ (2-4 недели)
│ Модуль 0: Среда → Модуль 1: Синтаксис, типы, операторы, условия, циклы, строки,
│ массивы, методы, regex, дата/время
│
ЭТАП 2: ООП (2-3 недели)
│ Модуль 2: Классы → Наследование → Полиморфизм → Инкапсуляция →
│ Интерфейсы → struct → enum → record → кортежи → collection expressions
│
ЭТАП 3: КОЛЛЕКЦИИ + ПРОДВИНУТЫЙ C# (3-4 недели)
│ Модуль 3: Коллекции → Модуль 4: Generics → Делегаты/События → LINQ →
│ Исключения → async/await → Pattern Matching → Операторы
│
ЭТАП 4: ФАЙЛЫ + УГЛУБЛЁННЫЕ ТЕМЫ + ОТЛАДКА (2-3 недели)
│ Модуль 5: Файлы/JSON → Модуль 6: Функц. программирование, рефлексия,
│ потоки, память → Модуль 7: ОТЛАДКА (практикуй параллельно!)
│
ЭТАП 5: АРХИТЕКТУРА + DI (2-3 недели)
│ Модуль 8: SOLID → Паттерны → Clean Architecture → DDD →
│ Модуль 9: DI
│
ЭТАП 6: ПЛАТФОРМЫ — ПАРАЛЛЕЛЬНО (6-8 недель)
│ Модуль 10: Uno Platform (чередуй через день) +
│ Модуль 11: Godot 4.6
│ Неделя 1: Uno XAML + Godot основы
│ Неделя 2: Uno Binding + Godot сигналы/ввод
│ Неделя 3: Uno навигация/стили + Godot физика/анимации
│ Неделя 4: Uno MVVM + Godot 2D-разработка
│ Неделя 5: Uno данные/сеть + Godot AI/навигация
│ Неделя 6: Uno адаптивность/локализация + Godot шейдеры/VFX/polish
│
ЭТАП 7: ПОЛНЫЙ СТЕК (4-5 недель)
│ Модуль 12: БД → Модуль 13: Сеть → Модуль 14: Тесты → Модуль 15: Безопасность
│
ЭТАП 8: МАСТЕРСТВО (3-4 недели)
│ Модуль 16: Оптимизация → Модуль 17: CI/CD → Модуль 18: ASP.NET Core →
│ Модуль 19: Продвинутый .NET
│
ЭТАП 9: ИТОГОВЫЕ ПРОЕКТЫ (4-8 недель)
│ Модуль 20: Super Productivity App (Uno) + Complete Action RPG (Godot)
│ Начни с MVP, наращивай функционал постепенно
│
═══════════════════════════════════════════════
ОБЩАЯ ДЛИТЕЛЬНОСТЬ: ~6-9 месяцев при 3-4 часах в день
```

---

## ДОПОЛНИТЕЛЬНЫЕ РЕСУРСЫ

- [Microsoft C# Documentation](https://learn.microsoft.com/en-us/dotnet/csharp/)
- [C# 14 What's New](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-14)
- [.NET 10 What's New](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-10/overview)
- [Godot Engine Documentation](https://docs.godotengine.org/)
- [Godot 4.6 Features](https://godotengine.org/article/dev-snapshot-godot-4-6-beta-2/)
- [Uno Platform Documentation](https://platform.uno/docs/)
- [Uno Platform 6.4](https://platform.uno/blog/uno-platform-6-4/)
- [CommunityToolkit.MVVM](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/)

---

> **v2.0 — Полностью переработанный план**
> - 20 модулей, 120+ тем, 200+ упражнений
> - 3 уровня заданий: мини-упражнения → средние задачи → проекты
> - Контрольные вопросы между модулями
> - Уровни сложности (⭐/⭐⭐/⭐⭐⭐)
> - MVP + полная версия для итоговых проектов
> - Добавлены: отладка, regex, даты, локализация, accessibility, game math, game feel, [Export], и 100+ других пропущенных тем
