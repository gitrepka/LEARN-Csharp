# Тема 1.7: Методы (функции)

## Что ты узнаешь
- Как объявлять и вызывать методы
- Параметры: ref, out, in, params, значения по умолчанию
- Перегрузка, рекурсия, локальные функции
- Extension methods — добавление методов к существующим типам

---

## Объяснение

### ЗАЧЕМ?
Без методов ты пишешь одну бесконечную простыню кода. Метод — это именованный блок кода, который можно вызывать сколько угодно раз. Как рецепт: напиши один раз, используй всегда.

### Объявление метода

```csharp
// returnType MethodName(parameters)
int Add(int a, int b)
{
    return a + b;
}

void SayHello(string name) // void = ничего не возвращает
{
    Console.WriteLine($"Привет, {name}!");
}

// Вызов:
int sum = Add(3, 5);    // 8
SayHello("Алиса");      // "Привет, Алиса!"

// Expression-bodied (одна строка)
int Multiply(int a, int b) => a * b;
```

### Параметры

```csharp
// По значению (копия) — по умолчанию
void Double(int x)
{
    x *= 2; // меняем КОПИЮ, оригинал не изменится
}

// ref — по ссылке (меняет оригинал)
void DoubleRef(ref int x)
{
    x *= 2; // меняем ОРИГИНАЛ
}

int num = 5;
Double(num);         // num = 5 (не изменился)
DoubleRef(ref num);  // num = 10 (изменился!)

// out — выходной параметр (метод ОБЯЗАН присвоить значение)
bool TryParseAge(string input, out int age)
{
    if (int.TryParse(input, out age) && age is > 0 and <= 150)
        return true;
    age = 0;
    return false;
}

if (TryParseAge("25", out int result))
    Console.WriteLine($"Возраст: {result}");

// in — входной по ссылке (только чтение, без копирования)
void PrintLength(in string text)
{
    Console.WriteLine(text.Length);
    // text = "другой"; // ❌ Нельзя! in = только чтение
}

// Значения по умолчанию
void Attack(int damage, bool isCritical = false)
{
    int total = isCritical ? damage * 2 : damage;
    Console.WriteLine($"Урон: {total}");
}
Attack(10);            // Урон: 10
Attack(10, true);      // Урон: 20

// Именованные аргументы
Attack(damage: 10, isCritical: true);

// params — произвольное количество аргументов
int Sum(params int[] numbers)
{
    int total = 0;
    foreach (int n in numbers)
        total += n;
    return total;
}
Sum(1, 2, 3);        // 6
Sum(1, 2, 3, 4, 5);  // 15

// C# 13: params для любых коллекций
int SumSpan(params ReadOnlySpan<int> numbers)
{
    int total = 0;
    foreach (int n in numbers)
        total += n;
    return total;
}
```

### Перегрузка (Method Overloading)

Несколько методов с одним именем, но разными параметрами:

```csharp
void Log(string message)
    => Console.WriteLine($"[INFO] {message}");

void Log(string message, string level)
    => Console.WriteLine($"[{level}] {message}");

void Log(string message, int errorCode)
    => Console.WriteLine($"[ERR-{errorCode}] {message}");

Log("Старт");                // [INFO] Старт
Log("Загрузка", "DEBUG");    // [DEBUG] Загрузка
Log("Ошибка", 404);          // [ERR-404] Ошибка
```

### Рекурсия

Метод вызывает сам себя. Обязателен **базовый случай** (когда остановиться).

```csharp
int Factorial(int n)
{
    if (n <= 1) return 1;        // базовый случай
    return n * Factorial(n - 1); // рекурсивный вызов
}
// Factorial(5) = 5 * 4 * 3 * 2 * 1 = 120
```

### Локальные функции

```csharp
void ProcessData(int[] data)
{
    // Локальная функция — видна только внутри ProcessData
    bool IsValid(int x) => x > 0 && x < 1000;

    foreach (int item in data)
    {
        if (IsValid(item))
            Console.WriteLine(item);
    }
}

// static локальная — не может захватывать внешние переменные
void Example()
{
    int multiplier = 2;

    // static int Double(int x) => x * multiplier; // ❌ Нельзя!
    static int Double(int x) => x * 2;             // ✅ Только свои параметры
}
```

### Extension Methods — добавляй методы к любому типу!

```csharp
// Объявление: static класс, static метод, первый параметр с this
public static class IntExtensions
{
    public static bool IsEven(this int number)
        => number % 2 == 0;

    public static bool IsBetween(this int number, int min, int max)
        => number >= min && number <= max;
}

// Использование — как будто метод встроен в int!
42.IsEven()        // true
7.IsBetween(1, 10) // true

// C# 14: Extension members (новый синтаксис)
public extension IntExtensions for int
{
    public bool IsOdd => this % 2 != 0;
}
// 7.IsOdd → true
```

---

## Частые ошибки

### 1. Забыл return
```csharp
// ❌ Не все пути возвращают значение
int GetMax(int a, int b)
{
    if (a > b) return a;
    // а если b >= a? Ошибка компиляции!
}

// ✅
int GetMax(int a, int b) => a > b ? a : b;
```

### 2. ref/out забыл при вызове
```csharp
void Swap(ref int a, ref int b) { (a, b) = (b, a); }

int x = 1, y = 2;
// Swap(x, y);       // ❌ Ошибка! Нужен ref
Swap(ref x, ref y);  // ✅
```

---

## Мини-упражнения

1. **⭐** Напиши `void Swap(ref int a, ref int b)`.
2. **⭐** Напиши `bool TryParseAge(string input, out int age)` с валидацией 1-150.
3. **⭐** Напиши рекурсивный метод для вычисления факториала.
4. **⭐** Создай extension method `bool IsEven(this int n)`. Используй: `42.IsEven()`.
5. **⭐** Напиши метод `Sum(params int[] numbers)` и вызови с разным количеством аргументов.

---

## Практика Uno Platform ⭐⭐
**"Конвертер единиц"** — методы конвертации температуры (C↔F↔K), длины (м↔фут), веса (кг↔фунт). Перегрузка для разных типов. UI: ComboBox + TextBox.

## Практика Godot ⭐⭐
**GameMath** — extension-методы: `float.Remap(fromMin, fromMax, toMin, toMax)`, `Node.GetAllChildren<T>()` (рекурсивный поиск).

---

## Контрольные вопросы
1. Чем `ref` отличается от `out`?
2. Что такое перегрузка метода? По каким параметрам различаются перегрузки?
3. Что будет если рекурсия без базового случая?
4. Как создать extension method? Какие ограничения?
5. Зачем нужны `static` локальные функции?

## Что дальше
Дальше — **Regex** (1.8): как искать и заменять текст по шаблонам.
