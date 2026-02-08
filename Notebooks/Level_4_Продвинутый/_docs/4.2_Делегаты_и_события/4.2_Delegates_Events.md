# Тема 4.2: Делегаты и события

## Что ты узнаешь
- Делегаты: Action, Func, Predicate, кастомные
- Лямбда-выражения и замыкания
- События (events): publisher/subscriber
- Когда и зачем

---

## Объяснение

### ЗАЧЕМ?
Метод — это код. А что если нужно **передать метод как параметр**? Например: "вот список, отсортируй его ПО ЭТОМУ ПРАВИЛУ". Правило — это метод. Делегат — "указатель на метод".

### Делегаты

```csharp
// Объявление делегата (свой тип)
delegate int MathOperation(int a, int b);

// Методы, подходящие под сигнатуру
int Add(int a, int b) => a + b;
int Multiply(int a, int b) => a * b;

// Использование
MathOperation op = Add;
Console.WriteLine(op(3, 4)); // 7

op = Multiply;
Console.WriteLine(op(3, 4)); // 12

// Многоадресный делегат (несколько методов)
Action<string> log = Console.WriteLine;
log += msg => System.Diagnostics.Debug.WriteLine(msg);
log("Привет!"); // Вызовет ОБА метода
```

### Стандартные делегаты — Action, Func, Predicate

**Не нужно создавать свои делегаты!** .NET уже предоставляет универсальные:

```csharp
// Action — ничего не возвращает (void)
Action                     doSomething = () => Console.WriteLine("!");
Action<string>             print = s => Console.WriteLine(s);
Action<string, int>        repeat = (s, n) => { for (int i = 0; i < n; i++) Console.Write(s); };

// Func — возвращает значение (последний параметр = возврат)
Func<int>                  getNumber = () => 42;
Func<int, int>             double_ = x => x * 2;
Func<int, int, int>        add = (a, b) => a + b;
Func<string, bool>         isEmpty = s => string.IsNullOrEmpty(s);

// Predicate — всегда возвращает bool (частный случай Func<T, bool>)
Predicate<int> isEven = n => n % 2 == 0;
Predicate<string> isLong = s => s.Length > 10;

// Использование в стандартных методах .NET:
List<int> numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
List<int> evens = numbers.FindAll(isEven);  // [2, 4, 6, 8, 10]
numbers.Sort((a, b) => b - a);              // сортировка по убыванию
```

### Методы высшего порядка

Метод, который **принимает или возвращает** функцию:

```csharp
// Принимает функцию
void ExecuteWithTimer(Action action, string description)
{
    var sw = System.Diagnostics.Stopwatch.StartNew();
    action();
    sw.Stop();
    Console.WriteLine($"{description}: {sw.ElapsedMilliseconds}ms");
}

ExecuteWithTimer(
    () => Thread.Sleep(500),
    "Задержка"
);

// Возвращает функцию
Func<int, int> CreateMultiplier(int factor)
{
    return x => x * factor; // замыкание! factor "захвачен"
}

var triple = CreateMultiplier(3);
Console.WriteLine(triple(10)); // 30
```

---

### Лямбда-выражения

Краткая запись анонимных методов.

```csharp
// Expression lambda (одно выражение)
Func<int, int> square = x => x * x;

// Statement lambda (блок кода)
Func<int, int> factorial = n =>
{
    int result = 1;
    for (int i = 2; i <= n; i++)
        result *= i;
    return result;
};

// Static lambda (C# 9) — запрещает захват внешних переменных
int factor = 10;
Func<int, int> multiply = static x => x * 2; // ✅ Нет захвата
// Func<int, int> bad = static x => x * factor; // ❌ Ошибка! static запрещает

// Lambda natural type (C# 10)
var f = () => 1;               // Func<int>
var g = (int x) => x.ToString(); // Func<int, string>
```

### Замыкания (Closures) — осторожно!

Лямбда "захватывает" переменные из окружения:

```csharp
int counter = 0;
Action increment = () => counter++;
increment();
increment();
Console.WriteLine(counter); // 2 — лямбда изменила внешнюю переменную

// ⚠️ ЛОВУШКА в цикле!
List<Action> actions = [];
for (int i = 0; i < 5; i++)
{
    actions.Add(() => Console.Write(i + " "));
}
foreach (var action in actions)
    action(); // 5 5 5 5 5  ← НЕ 0 1 2 3 4!

// Почему? Все лямбды захватили ОДНУ переменную i, которая в конце = 5

// ✅ Исправление — локальная копия:
for (int i = 0; i < 5; i++)
{
    int local = i; // каждая итерация — своя переменная
    actions.Add(() => Console.Write(local + " "));
}
// Теперь: 0 1 2 3 4
```

---

### События (Events)

Паттерн **Publisher/Subscriber**: один объект сообщает, что что-то произошло, другие реагируют.

```csharp
class Player
{
    // Объявление события
    public event EventHandler<int>? HealthChanged;
    public event Action? Died;

    private int _health = 100;
    public int Health
    {
        get => _health;
        private set
        {
            _health = Math.Max(0, value);
            HealthChanged?.Invoke(this, _health); // уведомляем подписчиков
            if (_health == 0)
                Died?.Invoke();
        }
    }

    public void TakeDamage(int amount) => Health -= amount;
    public void Heal(int amount) => Health += amount;
}

// Подписка
Player player = new();

// Подписчик 1: UI
player.HealthChanged += (sender, hp) =>
    Console.WriteLine($"HP: {hp}/100");

// Подписчик 2: звуковой эффект
player.HealthChanged += (sender, hp) =>
{
    if (hp < 20) Console.WriteLine("⚠️ Критическое здоровье!");
};

// Подписчик 3: game over
player.Died += () => Console.WriteLine("💀 Game Over!");

// Действие
player.TakeDamage(30); // HP: 70/100
player.TakeDamage(60); // HP: 10/100, ⚠️ Критическое здоровье!
player.TakeDamage(20); // HP: 0/100, ⚠️ ..., 💀 Game Over!
```

### Кастомные EventArgs

```csharp
class DamageEventArgs : EventArgs
{
    public int Amount { get; init; }
    public string Source { get; init; } = "";
    public bool WasCritical { get; init; }
}

class Enemy
{
    public event EventHandler<DamageEventArgs>? DamageReceived;

    public void TakeDamage(int amount, string source, bool crit)
    {
        DamageReceived?.Invoke(this, new DamageEventArgs
        {
            Amount = amount,
            Source = source,
            WasCritical = crit
        });
    }
}
```

### Отписка — предотвращение утечек памяти!

```csharp
// ⚠️ Если подписчик "умер", но подписка осталась — утечка памяти!
void Subscribe(Player player)
{
    EventHandler<int> handler = (s, hp) => Console.WriteLine(hp);
    player.HealthChanged += handler;

    // Когда больше не нужно — ОТПИШИСЬ:
    player.HealthChanged -= handler;
}
```

---

## Частые ошибки

```csharp
// ❌ event без проверки на null
HealthChanged.Invoke(this, hp); // NullReferenceException если нет подписчиков!
// ✅
HealthChanged?.Invoke(this, hp);

// ❌ async void в обработчике (теряется исключение)
player.Died += async () => await SaveGameAsync(); // ⚠️ async void!
// Это допустимо для event handlers, но будь осторожен с исключениями

// ❌ Забыл отписаться
// Подписка на событие создаёт ссылку publisher → subscriber
// Если publisher живёт дольше subscriber — утечка памяти!
```

---

## Мини-упражнения
1. **⭐** Создай `Func<int,int,int>` для сложения, вычитания, умножения. Вызови все три.
2. **⭐** Покажи проблему замыкания в цикле и исправь.
3. **⭐** Используй static lambda, попробуй захватить переменную.
4. **⭐** Создай event, подпишись из двух мест, отпишись от одного.

### Практика Uno Platform ⭐⭐⭐
**"Таймер с уведомлениями"** — `CountdownTimer` с событиями OnTick, OnCompleted, OnWarning. Несколько таймеров одновременно, прогресс-бары.

### Практика Godot ⭐⭐⭐
**Event Bus (Singleton)** — `GameEvents`: OnPlayerDamaged, OnEnemyKilled, OnItemCollected. UI, звук, частицы подписываются — слабая связность.

---

## Что дальше
Дальше — **LINQ** (4.3) — мощный инструмент запросов к данным.
