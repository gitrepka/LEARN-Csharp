# Тема 4.4: Обработка исключений

## Что ты узнаешь
- try-catch-finally, множественные catch, фильтры when
- Иерархия исключений .NET
- Пользовательские исключения
- throw vs throw ex
- Guard clauses и Result pattern

---

## Объяснение

### ЗАЧЕМ?
Программа может упасть: файл не найден, сеть недоступна, пользователь ввёл текст вместо числа. Исключения — механизм **обработки ошибок** без завала всей программы.

### try-catch-finally

```csharp
try
{
    int result = 10 / int.Parse("0"); // DivideByZeroException!
}
catch (DivideByZeroException ex)
{
    Console.WriteLine($"Деление на ноль: {ex.Message}");
}
catch (FormatException ex)
{
    Console.WriteLine($"Неверный формат: {ex.Message}");
}
catch (Exception ex) // ловит ВСЁ остальное (всегда последний!)
{
    Console.WriteLine($"Ошибка: {ex.Message}");
}
finally
{
    // Выполняется ВСЕГДА — и при ошибке, и без
    Console.WriteLine("Очистка ресурсов");
}
```

### Фильтры when

```csharp
try
{
    await httpClient.GetAsync("https://api.example.com");
}
catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
{
    Console.WriteLine("Ресурс не найден (404)");
}
catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
{
    Console.WriteLine("Нет доступа (401)");
}
catch (HttpRequestException ex)
{
    Console.WriteLine($"HTTP ошибка: {ex.StatusCode}");
}
```

---

### Иерархия исключений

```
Exception
├── SystemException
│   ├── NullReferenceException      — обращение к null
│   ├── ArgumentException           — плохой аргумент
│   │   ├── ArgumentNullException   — аргумент = null
│   │   └── ArgumentOutOfRangeException — аргумент вне диапазона
│   ├── InvalidOperationException   — операция невалидна в текущем состоянии
│   ├── InvalidCastException        — невозможное приведение типа
│   ├── IndexOutOfRangeException    — индекс за пределами массива
│   ├── DivideByZeroException       — деление на ноль
│   ├── OverflowException           — переполнение (checked)
│   ├── NotSupportedException       — операция не поддерживается
│   ├── NotImplementedException     — не реализовано (заглушка)
│   ├── ObjectDisposedException     — объект уже disposed
│   └── StackOverflowException      — переполнение стека (не ловится!)
├── IOException
│   ├── FileNotFoundException
│   ├── DirectoryNotFoundException
│   └── EndOfStreamException
├── TimeoutException
└── OperationCanceledException      — отмена через CancellationToken
    └── TaskCanceledException
```

---

### throw vs throw ex

```csharp
try
{
    DoSomething();
}
catch (Exception ex)
{
    // ✅ throw; — СОХРАНЯЕТ оригинальный stack trace
    throw;

    // ❌ throw ex; — ТЕРЯЕТ оригинальный stack trace!
    // throw ex; // стек начнётся с этой строки, а не с реального места ошибки
}

// Throw expression (можно в выражениях)
string name = input ?? throw new ArgumentNullException(nameof(input));
int value = x > 0 ? x : throw new ArgumentOutOfRangeException(nameof(x));
```

---

### Пользовательские исключения

```csharp
class InsufficientFundsException : Exception
{
    public decimal Required { get; }
    public decimal Available { get; }

    public InsufficientFundsException(decimal required, decimal available)
        : base($"Недостаточно средств. Требуется: {required}, Доступно: {available}")
    {
        Required = required;
        Available = available;
    }
}

class BankAccount
{
    public decimal Balance { get; private set; }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Сумма должна быть положительной");

        if (amount > Balance)
            throw new InsufficientFundsException(amount, Balance);

        Balance -= amount;
    }
}
```

---

### Guard clauses — быстрая валидация (.NET 6+)

```csharp
void ProcessUser(string name, int age, string? email)
{
    // Старый стиль:
    if (name == null) throw new ArgumentNullException(nameof(name));

    // Новый стиль (.NET 6+):
    ArgumentNullException.ThrowIfNull(name);
    ArgumentOutOfRangeException.ThrowIfNegativeOrZero(age);

    // .NET 8+:
    ArgumentException.ThrowIfNullOrWhiteSpace(name);

    // Теперь работаем с валидными данными
}
```

---

### Result pattern — альтернатива исключениям

Исключения — дорогие (медленные). Для **ожидаемых** ошибок лучше Result:

```csharp
// Свой Result<T>
readonly struct Result<T>
{
    public T? Value { get; }
    public string? Error { get; }
    public bool IsSuccess => Error is null;

    private Result(T value) { Value = value; Error = null; }
    private Result(string error) { Value = default; Error = error; }

    public static Result<T> Success(T value) => new(value);
    public static Result<T> Failure(string error) => new(error);
}

// Использование
Result<int> ParseNumber(string input)
{
    if (int.TryParse(input, out int value))
        return Result<int>.Success(value);
    return Result<int>.Failure($"'{input}' не является числом");
}

var result = ParseNumber("abc");
if (result.IsSuccess)
    Console.WriteLine($"Число: {result.Value}");
else
    Console.WriteLine($"Ошибка: {result.Error}");
```

### Когда что?
| Ситуация | Подход |
|----------|--------|
| Баг в программе (null ref, index out of range) | Exception |
| Внешняя ошибка (файл не найден, сеть) | Exception + catch |
| Ожидаемая невалидность (парсинг, валидация) | Result pattern или TryParse |
| Нарушение контракта метода | ArgumentException + guard |

---

## Частые ошибки

```csharp
// ❌ Ловить Exception и ничего не делать (проглатывание)
try { DoSomething(); }
catch { } // ← ошибка потерялась!

// ❌ throw ex вместо throw
catch (Exception ex) { throw ex; } // теряет stack trace!
// ✅
catch (Exception ex) { throw; }

// ❌ Использование исключений для управления потоком
try { int x = dict[key]; } // ❌ медленно!
catch (KeyNotFoundException) { /* ... */ }
// ✅
if (dict.TryGetValue(key, out int x)) { /* ... */ }

// ❌ Ловить StackOverflowException (невозможно!)
// ❌ Ловить OutOfMemoryException (нет смысла — нет памяти)
```

---

## Мини-упражнения
1. **⭐** Напиши try-catch с фильтром `when`.
2. **⭐** Создай свой `InsufficientFundsException(decimal required, decimal available)`.
3. **⭐** Покажи разницу `throw;` vs `throw ex;` через стек вызовов.
4. **⭐⭐** Реализуй `Result<T>` с Success/Failure.

---

## Что дальше
Дальше — **async/await** (4.5) — асинхронное программирование.
