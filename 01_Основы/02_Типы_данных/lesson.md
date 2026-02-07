# Тема 1.2: Переменные и типы данных

## Что ты узнаешь
- Какие типы данных есть в C# и для чего каждый
- Разницу между значимыми и ссылочными типами
- Nullable-типы и null-безопасность
- var, target-typed new, const vs readonly

---

## Объяснение

### ЗАЧЕМ: Зачем нужны типы?
**Аналогия:** Типы — это контейнеры. Стакан для воды, коробка для обуви, рюкзак для книг. Ты не нальёшь воду в рюкзак. Так и в C#: число `42` хранится в `int`, текст "привет" — в `string`. Компилятор проверяет, чтобы ты не положил текст в числовую переменную.

### КАК: Значимые типы (Value Types)

Хранят значение напрямую (как записка с числом в кармане).

```csharp
// Целые числа
byte   age = 25;          // 0 до 255 (1 байт)
short  temperature = -10;  // -32768 до 32767 (2 байта)
int    score = 1000000;    // ±2 миллиарда (4 байта) — ОСНОВНОЙ
long   worldPop = 8000000000; // очень большие числа (8 байт)

// Дробные числа
float  speed = 3.14f;     // ~7 цифр точности (4 байта). Обязательно f!
double precise = 3.14159265358979; // ~15 цифр (8 байт) — ОСНОВНОЙ
decimal money = 99.99m;    // 28 цифр, для денег! (16 байт). Обязательно m!

// Другие
bool   isAlive = true;     // true или false
char   grade = 'A';        // один символ (в одинарных кавычках!)
```

**Когда что использовать:**
| Тип | Когда |
|-----|-------|
| `int` | Почти всегда для целых чисел |
| `long` | Если число > 2 миллиардов |
| `float` | Координаты в играх (Godot использует float!) |
| `double` | Точные расчёты |
| `decimal` | Деньги (никогда float для денег!) |
| `bool` | Любые да/нет условия |

### Ссылочные типы (Reference Types)

Хранят ссылку (адрес) на данные в куче (как записка с адресом склада).

```csharp
string name = "Алиса";           // текст (в двойных кавычках)
object anything = 42;             // может хранить ЧТО УГОДНО
int[] numbers = { 1, 2, 3 };     // массив
dynamic flexible = "текст";       // тип определяется в runtime
flexible = 42;                    // можно менять тип (опасно!)

// Анонимный тип — быстрый объект без класса
var hero = new { Name = "Воин", Level = 5, IsAlive = true };
Console.WriteLine(hero.Name); // "Воин"
// hero.Name = "Маг"; // ❌ Нельзя! Анонимные типы read-only
```

### Значения по умолчанию

```csharp
Console.WriteLine(default(int));     // 0
Console.WriteLine(default(bool));    // False
Console.WriteLine(default(string));  // (пусто — null)
Console.WriteLine(default(double));  // 0
Console.WriteLine(default(char));    // '\0' (пустой символ)
```

### Nullable-типы

**Проблема:** `int` не может быть null. Но иногда нужно сказать "значения нет".

```csharp
int? health = null;    // ? делает тип nullable
int? mana = 100;

// Проверка
if (health.HasValue)
    Console.WriteLine(health.Value);
else
    Console.WriteLine("Здоровье неизвестно");

// Оператор ?? — "если null, то используй значение по умолчанию"
int hp = health ?? 0;  // hp = 0, потому что health = null

// Оператор ?. — "вызови метод, только если не null"
string? name = null;
int? length = name?.Length;  // length = null (не упадёт!)

// Оператор ??= — "присвой, только если текущее значение null"
health ??= 100;  // теперь health = 100
```

### Nullable Reference Types (NRT)

С `<Nullable>enable</Nullable>` в .csproj:
```csharp
string name = "Алиса";    // НЕ может быть null
string? nickname = null;   // Может быть null (? явно разрешает)

// Компилятор предупредит:
string bad = null;         // ⚠️ Предупреждение!

// Оператор ! — "я точно знаю что это не null" (подавляет предупреждение)
string sure = nickname!;   // Опасно! Используй редко
```

### Вывод типов

```csharp
var number = 42;              // компилятор: int
var text = "привет";          // компилятор: string
var list = new List<int>();   // компилятор: List<int>

// Target-typed new (C# 9) — тип слева, new() справа
List<string> names = new();              // вместо new List<string>()
Dictionary<string, int> scores = new();  // короче!
```

**Правило:** Используй `var` когда тип ОЧЕВИДЕН из правой части. Не используй когда тип неясен.
```csharp
var player = new Player();       // ✅ Очевидно что Player
var result = GetData();          // ❌ Непонятно какой тип возвращает GetData
Player result = GetData();       // ✅ Ясно
```

### const vs readonly vs static readonly

```csharp
const int MaxHealth = 100;           // Вшивается в код при компиляции. Нельзя менять.
readonly int _startHealth;           // Устанавливается в конструкторе. После — нельзя менять.
static readonly int DefaultHealth = 100; // Как const, но определяется в runtime.

// Когда что:
// const — для вещей которые НИКОГДА не изменятся (Pi, MaxScore)
// readonly — для значений которые задаются при создании объекта
// static readonly — когда нужен "константный" объект (строки, массивы)
```

---

## Примеры кода

### Пример 1: Все базовые типы
```csharp
// Значимые типы
int age = 25;
float speed = 5.5f;
double gravity = 9.81;
decimal price = 19.99m;
bool isAlive = true;
char initial = 'A';

// Ссылочные типы
string name = "Игрок";
int[] scores = { 100, 200, 300 };
object box = 42;  // boxing: int → object

// Вывод
Console.WriteLine($"Имя: {name}, Возраст: {age}");
Console.WriteLine($"Скорость: {speed}, Жив: {isAlive}");
Console.WriteLine($"Цена: {price:C}");  // Выведет: Цена: 19,99 ₽
```

### Пример 2: Nullable в действии
```csharp
int? damage = null;    // Урон неизвестен
int? armor = 5;

// ?? — значение по умолчанию
int actualDamage = damage ?? 0;     // 0
int actualArmor = armor ?? 0;       // 5

// ??= — присвоить если null
damage ??= 10;  // damage теперь 10

// ?. — безопасный вызов
string? weapon = null;
int? nameLength = weapon?.Length;     // null, не NullReferenceException!
string upper = weapon?.ToUpper() ?? "НЕТ ОРУЖИЯ";
```

### Пример 3: Target-typed new
```csharp
// Старый стиль
List<string> names = new List<string>();
Dictionary<string, int> dict = new Dictionary<string, int>();

// Новый стиль (C# 9) — тип уже указан слева, зачем повторять?
List<string> names = new();
Dictionary<string, int> dict = new();
```

### Пример 4: Анонимный тип для быстрых данных
```csharp
var enemy = new
{
    Name = "Гоблин",
    Health = 50,
    Damage = 10,
    IsBoss = false
};

Console.WriteLine($"{enemy.Name}: HP={enemy.Health}, DMG={enemy.Damage}");
// enemy.Health = 30; // ❌ Ошибка! Анонимные типы immutable
```

---

## Частые ошибки

### 1. float без суффикса f
```csharp
// ❌ Ошибка: 3.14 это double, не float
float speed = 3.14;

// ✅ Правильно:
float speed = 3.14f;
```

### 2. decimal без суффикса m
```csharp
// ❌ Ошибка
decimal money = 99.99;

// ✅ Правильно:
decimal money = 99.99m;
```

### 3. float для денег
```csharp
// ❌ НИКОГДА не используй float/double для денег!
float balance = 0.1f + 0.2f;
Console.WriteLine(balance); // 0.30000001 (!!!)

// ✅ Используй decimal
decimal balance = 0.1m + 0.2m;
Console.WriteLine(balance); // 0.3 (точно!)
```

### 4. NullReferenceException
```csharp
// ❌ Падает!
string name = null;
Console.WriteLine(name.Length); // NullReferenceException!

// ✅ Проверяй:
Console.WriteLine(name?.Length ?? 0);
```

---

## Мини-упражнения

1. **⭐** Объяви переменные КАЖДОГО базового типа (`byte`, `int`, `long`, `float`, `double`, `decimal`, `bool`, `char`, `string`). Выведи значение по умолчанию через `default(T)`.

2. **⭐** Поэкспериментируй с `int?`: присвой null, проверь `HasValue`, используй `??` для значения по умолчанию.

3. **⭐** Создай анонимный тип с 3 свойствами (имя, уровень, здоровье), обратись к ним.

4. **⭐** Используй `target-typed new()` для создания `List<int>`, `Dictionary<string, int>`.

5. **⭐** Включи Nullable reference types. Поставь `string?` и `string`, посмотри предупреждения компилятора.

---

## Практика Uno Platform ⭐⭐

**"Калькулятор типов"**
- TextBox для ввода числа
- 3 кнопки: "Как int", "Как double", "Как string"
- Результат показывает: значение + размер типа в байтах
- Используй `sizeof()` для значимых типов

---

## Практика Godot ⭐⭐

**"Характеристики персонажа"**
- Имя: `string`
- Здоровье: `int`
- Скорость: `float`
- Жив: `bool`
- Класс: `enum` (Warrior, Mage, Archer)
- Label-узлы показывают всё. Кнопки меняют характеристики.

```csharp
public enum CharacterClass { Warrior, Mage, Archer }

public partial class CharacterSheet : Control
{
    private string _name = "Герой";
    private int _health = 100;
    private float _speed = 5.0f;
    private bool _isAlive = true;
    private CharacterClass _class = CharacterClass.Warrior;
}
```

---

## Контрольные вопросы

1. Чем `float` отличается от `double` и `decimal`? Когда что использовать?
2. В чём разница между `const` и `readonly`?
3. Что такое Nullable Reference Types и зачем они нужны?
4. Чем анонимные типы отличаются от обычных классов?
5. Что произойдёт при `int? x = null; int y = x;`? Почему?
6. Когда использовать `var`, а когда явный тип?

---

## Что дальше
Ты знаешь какие типы данных есть. Дальше — **операторы** (тема 1.3): как с этими данными работать (складывать, сравнивать, комбинировать).
