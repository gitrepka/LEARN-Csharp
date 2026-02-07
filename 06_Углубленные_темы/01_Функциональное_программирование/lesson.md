# Тема 6.1: Функциональное программирование в C#

## Что ты узнаешь
- Функции высшего порядка
- Каррирование и частичное применение
- Замыкания: как работают и подводные камни
- Функциональные паттерны: pipe, compose

---

## Объяснение

### ЗАЧЕМ?
C# — не чисто функциональный язык, но заимствует мощные идеи: LINQ — это ФП! Лямбды — это ФП! Понимание этих концепций делает код чище, компактнее, безопаснее.

### Функции высшего порядка

Функция, которая **принимает** или **возвращает** другую функцию.

```csharp
// Принимает функцию
List<T> Filter<T>(IEnumerable<T> source, Func<T, bool> predicate)
{
    List<T> result = [];
    foreach (T item in source)
        if (predicate(item))
            result.Add(item);
    return result;
}

var evens = Filter([1, 2, 3, 4, 5], n => n % 2 == 0); // [2, 4]

// Возвращает функцию
Func<T, bool> Not<T>(Func<T, bool> predicate)
{
    return item => !predicate(item);
}

Func<int, bool> isEven = n => n % 2 == 0;
Func<int, bool> isOdd = Not(isEven);
Console.WriteLine(isOdd(3)); // true

// Compose — объединение функций
Func<T, TResult> Compose<T, TIntermediate, TResult>(
    Func<T, TIntermediate> f,
    Func<TIntermediate, TResult> g)
{
    return x => g(f(x));
}

Func<string, string> trim = s => s.Trim();
Func<string, string> upper = s => s.ToUpper();
Func<string, string> process = Compose(trim, upper);
Console.WriteLine(process("  hello  ")); // "HELLO"
```

### Каррирование (Currying)

Превращение функции с N аргументами в цепочку функций по одному аргументу.

```csharp
// Обычная функция
Func<int, int, int> add = (a, b) => a + b;

// Каррированная версия
Func<int, Func<int, int>> curriedAdd = a => b => a + b;

var add5 = curriedAdd(5);  // получили "прибавить 5"
Console.WriteLine(add5(3)); // 8
Console.WriteLine(add5(10)); // 15

// Хелпер для каррирования
Func<T1, Func<T2, TResult>> Curry<T1, T2, TResult>(Func<T1, T2, TResult> func)
{
    return a => b => func(a, b);
}

var multiply = Curry<int, int, int>((a, b) => a * b);
var double_ = multiply(2);
var triple = multiply(3);
Console.WriteLine(double_(5)); // 10
Console.WriteLine(triple(5)); // 15
```

### Частичное применение (Partial Application)

Зафиксировать часть аргументов:

```csharp
// Логгер с частичным применением
Func<string, Action<string>> createLogger = prefix =>
    message => Console.WriteLine($"[{prefix}] {message}");

Action<string> infoLog = createLogger("INFO");
Action<string> errorLog = createLogger("ERROR");

infoLog("Приложение запущено");  // [INFO] Приложение запущено
errorLog("Файл не найден");     // [ERROR] Файл не найден
```

### Pipe — цепочка преобразований

```csharp
// Extension method для pipe (как |> в F#)
static class PipeExtensions
{
    public static TResult Pipe<T, TResult>(this T value, Func<T, TResult> func)
        => func(value);
}

// Использование — читается слева направо
string result = "  Hello, World!  "
    .Pipe(s => s.Trim())
    .Pipe(s => s.ToUpper())
    .Pipe(s => s.Replace("!", "!!!"));
// "HELLO, WORLD!!!"

// Без pipe — читается изнутри наружу (хуже):
string result2 = Replace(ToUpper(Trim("  Hello, World!  ")), "!", "!!!");
```

### Иммутабельность — ключевая идея ФП

```csharp
// ❌ Мутабельный стиль
List<int> numbers = [1, 2, 3, 4, 5];
for (int i = 0; i < numbers.Count; i++)
    numbers[i] *= 2;

// ✅ Функциональный стиль (новая коллекция)
var doubled = numbers.Select(n => n * 2).ToList();
// numbers не изменился!

// record с with — иммутабельное обновление
record Config(string Host, int Port, bool Debug);
var prod = new Config("api.com", 443, false);
var dev = prod with { Host = "localhost", Port = 8080, Debug = true };
```

---

## Замыкания — подводные камни

```csharp
// Замыкание: лямбда захватывает ПЕРЕМЕННУЮ (не значение!)
int x = 10;
Func<int> getX = () => x;
x = 20;
Console.WriteLine(getX()); // 20 (не 10!)

// ⚠️ В циклах — классическая ловушка (повтор из темы 4.2)
var actions = Enumerable.Range(0, 5)
    .Select(i => (Action)(() => Console.Write(i)))
    .ToList();
// Здесь OK — LINQ Select создаёт новую переменную i для каждой итерации

// Но с обычным for:
List<Func<int>> funcs = [];
for (int i = 0; i < 5; i++)
    funcs.Add(() => i); // Все вернут 5!
```

---

## Мини-упражнения
1. **⭐** Напиши функцию `Not<T>(Func<T, bool>)` → `Func<T, bool>`.
2. **⭐** Создай каррированную функцию для умножения, получи `double` и `triple`.
3. **⭐⭐** Реализуй `Pipe` extension method, создай цепочку обработки строк.

---

## Что дальше
Дальше — **рефлексия** (6.2).
