# Тема 14.3: Тестирование в Godot — GdUnit4

## Что ты узнаешь
- GdUnit4 — фреймворк для тестирования в Godot
- Тестирование игровой логики
- Тестирование нод и сцен
- Когда тестировать в играх, а когда нет

---

## Объяснение

### ЗАЧЕМ?

Игровая логика бывает сложной: урон зависит от брони, крита, баффов. Инвентарь имеет лимиты и стакается. Квесты имеют условия. **Без тестов — баги в каждом бою.**

### Установка GdUnit4

```
1. AssetLib → ищи "GdUnit4" → Install
2. Или через NuGet: dotnet add package gdUnit4.api
3. Project → Project Settings → Plugins → Enable "GdUnit4"
```

### Тестирование чистой логики (без движка)

```csharp
// Это можно тестировать обычным xUnit/NUnit — без Godot!

public class DamageCalculator
{
    public int Calculate(int attack, int defense, bool isCritical = false)
    {
        int baseDamage = Math.Max(1, attack - defense);
        return isCritical ? baseDamage * 2 : baseDamage;
    }
}

// Тест (xUnit — работает без Godot)
public class DamageCalculatorTests
{
    private readonly DamageCalculator _calc = new();

    [Fact]
    public void Calculate_AttackGreaterThanDefense_ReturnsPositive()
    {
        Assert.Equal(7, _calc.Calculate(attack: 10, defense: 3));
    }

    [Fact]
    public void Calculate_DefenseGreaterThanAttack_ReturnsMinimum1()
    {
        Assert.Equal(1, _calc.Calculate(attack: 3, defense: 10));
    }

    [Fact]
    public void Calculate_Critical_DoubleDamage()
    {
        Assert.Equal(14, _calc.Calculate(10, 3, isCritical: true));
    }

    [Theory]
    [InlineData(0, 0, false, 1)]    // минимальный урон
    [InlineData(100, 0, false, 100)] // без брони
    [InlineData(50, 50, true, 2)]   // равные, крит
    public void Calculate_VariousInputs(int atk, int def, bool crit, int expected)
    {
        Assert.Equal(expected, _calc.Calculate(atk, def, crit));
    }
}
```

### GdUnit4 — тесты с нодами

```csharp
using GdUnit4;
using static GdUnit4.Assertions;

[TestSuite]
public class HealthComponentTest
{
    [TestCase]
    public void TakeDamage_ReducesHealth()
    {
        // Создаём ноду
        var health = AutoFree(new HealthComponent());
        health.MaxHealth = 100;
        health.CurrentHealth = 100;

        health.TakeDamage(30);

        AssertInt(health.CurrentHealth).IsEqual(70);
    }

    [TestCase]
    public void TakeDamage_BelowZero_ClampedToZero()
    {
        var health = AutoFree(new HealthComponent());
        health.MaxHealth = 100;
        health.CurrentHealth = 10;

        health.TakeDamage(50);

        AssertInt(health.CurrentHealth).IsEqual(0);
    }

    [TestCase]
    public void Heal_AboveMax_ClampedToMax()
    {
        var health = AutoFree(new HealthComponent());
        health.MaxHealth = 100;
        health.CurrentHealth = 80;

        health.Heal(50);

        AssertInt(health.CurrentHealth).IsEqual(100);
    }

    [TestCase]
    public void IsDead_WhenZeroHealth_ReturnsTrue()
    {
        var health = AutoFree(new HealthComponent());
        health.CurrentHealth = 0;

        AssertBool(health.IsDead).IsTrue();
    }
}
```

### Тестирование сцен

```csharp
[TestSuite]
public class PlayerTest
{
    [TestCase]
    public async Task Player_StartsWithFullHealth()
    {
        // Загружаем целую сцену
        var scene = ResourceLoader.Load<PackedScene>("res://Scenes/Player.tscn");
        var player = AutoFree(scene.Instantiate<Player>());

        // Добавляем в дерево сцен (некоторым нодам нужен _Ready)
        AddChild(player);
        await AwaitIdleFrame(); // ждём _Ready()

        AssertInt(player.Health).IsEqual(100);
    }
}
```

### Стратегия: что тестировать в играх

```
✅ ТЕСТИРУЙ (чистая логика):
- DamageCalculator — формулы урона
- Inventory — добавление, удаление, стакинг, лимиты
- QuestSystem — условия выполнения
- LootTable — шансы дропа
- StatModifier — баффы, дебаффы, стаки
- SaveData — сериализация/десериализация

⚠️ МОЖНО ТЕСТИРОВАТЬ (с GdUnit4):
- HealthComponent — TakeDamage, Heal, Death signal
- State Machine — переходы между состояниями

❌ НЕ ТЕСТИРУЙ:
- Визуальные эффекты (particles, shaders)
- "Ощущения" (game feel, screen shake)
- Точные позиции спрайтов
- Физику движка (Godot уже тестирует)
```

### Выделяй логику из нод

```csharp
// ❌ Плохо — логика в ноде, сложно тестировать
public partial class Enemy : CharacterBody2D
{
    public void TakeDamage(int amount)
    {
        int actualDamage = Math.Max(1, amount - _defense);
        if (_hasShield) actualDamage /= 2;
        _health -= actualDamage;
        // ... ещё 20 строк
    }
}

// ✅ Хорошо — логика отдельно, нода использует
public class CombatRules // чистый C#, легко тестировать!
{
    public int CalculateDamage(int rawDamage, int defense, bool hasShield)
    {
        int damage = Math.Max(1, rawDamage - defense);
        return hasShield ? damage / 2 : damage;
    }
}

public partial class Enemy : CharacterBody2D
{
    private readonly CombatRules _combat = new();

    public void TakeDamage(int amount)
    {
        int damage = _combat.CalculateDamage(amount, _defense, _hasShield);
        _health -= damage;
    }
}
```

---

## Мини-упражнения

1. **⭐** Создай `Inventory` (List + maxSlots). Напиши 5 тестов: Add, Remove, IsFull, Contains, Count.
2. **⭐⭐** Создай `LootTable` с шансами дропа. Тест: за 10000 бросков результат ±5% от ожидаемого.
3. **⭐⭐** Выдели логику из игровой ноды, протестируй отдельно.

## Что дальше
Модуль 14 завершён! Дальше — **Модуль 15: Безопасность**.
