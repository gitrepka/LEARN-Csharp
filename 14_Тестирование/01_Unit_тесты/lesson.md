# Тема 14.1: Unit-тесты (xUnit)

## Что ты узнаешь
- Зачем нужны тесты и что такое Unit-тест
- xUnit: [Fact], [Theory], [InlineData]
- Arrange-Act-Assert паттерн
- Mocking с NSubstitute
- Что тестировать, а что нет

---

## Объяснение

### ЗАЧЕМ?

Без тестов ты **узнаёшь о баге от пользователя**. С тестами — **через 3 секунды после написания кода**. Тесты — это страховка. Рефакторишь код? Запустил тесты — всё работает. Добавил фичу? Тесты покажут, что ты ничего не сломал.

### Первый тест (xUnit)

```csharp
// 1. Создай тестовый проект:
// dotnet new xunit -n MyApp.Tests
// dotnet add MyApp.Tests reference MyApp

// 2. Напиши тест
public class CalculatorTests
{
    [Fact] // Факт — тест без параметров
    public void Add_TwoPositiveNumbers_ReturnsSum()
    {
        // Arrange — подготовка
        var calc = new Calculator();

        // Act — действие
        int result = calc.Add(2, 3);

        // Assert — проверка
        Assert.Equal(5, result);
    }

    [Fact]
    public void Divide_ByZero_ThrowsException()
    {
        var calc = new Calculator();

        Assert.Throws<DivideByZeroException>(() => calc.Divide(10, 0));
    }
}
```

### Именование тестов

```
МетодКоторыйТестируешь_Условие_ОжидаемыйРезультат

Add_TwoPositiveNumbers_ReturnsSum
Withdraw_InsufficientFunds_ThrowsException
GetUser_InvalidId_ReturnsNull
```

### [Theory] — тест с параметрами

```csharp
public class ValidatorTests
{
    [Theory]
    [InlineData("test@mail.com", true)]
    [InlineData("invalid", false)]
    [InlineData("", false)]
    [InlineData("a@b.c", true)]
    public void IsValidEmail_VariousInputs_ReturnsExpected(string email, bool expected)
    {
        var validator = new EmailValidator();

        bool result = validator.IsValid(email);

        Assert.Equal(expected, result);
    }

    // Сложные данные через MemberData
    [Theory]
    [MemberData(nameof(GetTestData))]
    public void Calculate_ReturnsCorrectResult(int a, int b, int expected)
    {
        Assert.Equal(expected, Calculator.Add(a, b));
    }

    public static IEnumerable<object[]> GetTestData()
    {
        yield return [1, 2, 3];
        yield return [0, 0, 0];
        yield return [-1, 1, 0];
        yield return [int.MaxValue, 0, int.MaxValue];
    }
}
```

### Полезные Assert-ы

```csharp
Assert.Equal(expected, actual);          // равенство
Assert.NotEqual(unexpected, actual);     // неравенство
Assert.True(condition);                  // true
Assert.False(condition);                 // false
Assert.Null(obj);                        // null
Assert.NotNull(obj);                     // не null
Assert.Contains("sub", str);            // содержит подстроку
Assert.Empty(collection);               // пустая коллекция
Assert.Single(collection);              // ровно 1 элемент
Assert.IsType<MyType>(obj);             // точный тип
Assert.InRange(val, 0, 100);            // в диапазоне
Assert.Throws<Exception>(() => ...);    // бросает исключение
await Assert.ThrowsAsync<Exception>(async () => ...); // async
```

### Mocking с NSubstitute

```csharp
// dotnet add package NSubstitute

// У тебя есть сервис, зависящий от репозитория:
public class OrderService
{
    private readonly IOrderRepository _repo;
    private readonly IEmailSender _email;

    public OrderService(IOrderRepository repo, IEmailSender email)
    {
        _repo = repo;
        _email = email;
    }

    public async Task<bool> PlaceOrderAsync(Order order)
    {
        if (order.Items.Count == 0) return false;

        await _repo.SaveAsync(order);
        await _email.SendAsync(order.CustomerEmail, "Заказ принят!");
        return true;
    }
}

// Тест — мы НЕ ХОТИМ реальную БД и реальные письма:
public class OrderServiceTests
{
    private readonly IOrderRepository _mockRepo;
    private readonly IEmailSender _mockEmail;
    private readonly OrderService _service;

    public OrderServiceTests()
    {
        // Создаём моки (подделки)
        _mockRepo = Substitute.For<IOrderRepository>();
        _mockEmail = Substitute.For<IEmailSender>();
        _service = new OrderService(_mockRepo, _mockEmail);
    }

    [Fact]
    public async Task PlaceOrder_ValidOrder_SavesAndSendsEmail()
    {
        // Arrange
        var order = new Order
        {
            CustomerEmail = "user@test.com",
            Items = [new OrderItem("Меч", 100)]
        };

        // Act
        bool result = await _service.PlaceOrderAsync(order);

        // Assert
        Assert.True(result);

        // Проверяем, что Save был вызван с нашим заказом
        await _mockRepo.Received(1).SaveAsync(order);

        // Проверяем, что письмо было отправлено
        await _mockEmail.Received(1).SendAsync("user@test.com", Arg.Any<string>());
    }

    [Fact]
    public async Task PlaceOrder_EmptyItems_ReturnsFalse()
    {
        var order = new Order { Items = [] };

        bool result = await _service.PlaceOrderAsync(order);

        Assert.False(result);
        await _mockRepo.DidNotReceive().SaveAsync(Arg.Any<Order>()); // НЕ сохранял
    }
}
```

### Настройка моков

```csharp
// Мок возвращает значение
_mockRepo.GetByIdAsync(42).Returns(new Order { Id = 42 });

// Мок бросает исключение
_mockRepo.SaveAsync(Arg.Any<Order>()).ThrowsAsync(new DbException());

// Мок для любого аргумента
_mockRepo.GetByIdAsync(Arg.Any<int>()).Returns(new Order());

// Проверка — метод вызван N раз
await _mockRepo.Received(2).SaveAsync(Arg.Any<Order>());

// Проверка — метод НЕ вызван
_mockEmail.DidNotReceive().SendAsync(Arg.Any<string>(), Arg.Any<string>());
```

### Что тестировать?

```
✅ Тестируй:
- Бизнес-логику (расчёты, валидация, правила)
- Граничные случаи (0, null, пустая строка, max/min)
- Ошибочные сценарии (невалидный ввод)
- Публичные методы

❌ НЕ тестируй:
- Приватные методы (тестируй через публичные)
- Фреймворк (.NET, Godot, Uno — они уже протестированы)
- Тривиальный код (геттеры/сеттеры без логики)
- UI напрямую (это интеграционные тесты)
```

### Структура тестового проекта

```
MyApp.sln
├── src/
│   └── MyApp/
│       ├── Services/OrderService.cs
│       └── Models/Order.cs
└── tests/
    └── MyApp.Tests/
        ├── Services/OrderServiceTests.cs
        └── Models/OrderTests.cs
```

---

## Частые ошибки

| Ошибка | Правильно |
|--------|-----------|
| Один огромный тест | Один тест = одна проверка |
| Тесты зависят друг от друга | Каждый тест независим |
| Тестируешь приватные методы | Тестируй через публичный API |
| Нет Assert | Без Assert тест бессмысленен |
| Магические числа в Assert | `Assert.Equal(expected, actual)` — назови expected |

---

## Мини-упражнения

1. **⭐** Создай `StringHelper.Reverse(string)` и напиши 5 тестов: обычная строка, пустая, один символ, палиндром, null.
2. **⭐** Напиши [Theory] с [InlineData] для метода `IsLeapYear(int year)`.
3. **⭐⭐** Создай `BankAccount` с `Deposit/Withdraw`. Тестируй: баланс, овердрафт, отрицательные суммы. Используй NSubstitute для мока ILogger.

## Практика для проектов

**Uno Platform:** Тесты для ViewModel: мок INavigationService, проверка что команда меняет свойство.

**Godot:** Тесты для игровой логики (без движка): `DamageCalculator.Calculate(attack, defense)` — чистая математика, легко тестировать.

## Что дальше
Дальше — **интеграционные тесты и TDD** (14.2).
