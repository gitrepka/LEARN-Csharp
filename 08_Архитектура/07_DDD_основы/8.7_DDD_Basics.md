# Тема 8.7: Domain-Driven Design (DDD) — основы

## Что ты узнаешь
- Entities vs Value Objects
- Aggregates и Aggregate Roots
- Domain Events
- Bounded Contexts
- Ubiquitous Language

---

## Объяснение

### ЗАЧЕМ?
Clean Architecture говорит "как организовать код". DDD говорит "как моделировать бизнес-логику". Это про **правильное проектирование** Domain-слоя.

### Entity vs Value Object

```csharp
// Entity — имеет уникальный ID, идентичность важна
class Player   // Два игрока с одинаковым именем — РАЗНЫЕ!
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public int Level { get; private set; }

    // Равенство — по ID!
    public override bool Equals(object? obj) =>
        obj is Player other && Id == other.Id;
    public override int GetHashCode() => Id.GetHashCode();
}

// Value Object — без ID, равенство по значению
record struct Money(decimal Amount, string Currency)
{
    // Два Money(100, "RUB") — ОДИНАКОВЫЕ!
    // Идентичности нет — только значение
    public static Money operator +(Money a, Money b)
    {
        if (a.Currency != b.Currency)
            throw new InvalidOperationException("Нельзя складывать разные валюты");
        return a with { Amount = a.Amount + b.Amount };
    }
}

record struct Address(string City, string Street, string PostalCode);
// Два одинаковых адреса — это один и тот же адрес
```

### Aggregate и Aggregate Root

Aggregate — **кластер** связанных объектов. Доступ — только через корень (Root).

```csharp
// Order — Aggregate Root
class Order
{
    public Guid Id { get; private set; }
    public string CustomerName { get; private set; }
    private readonly List<OrderItem> _items = [];
    public IReadOnlyList<OrderItem> Items => _items;

    // Бизнес-логика ВНУТРИ Aggregate:
    public void AddItem(string product, int quantity, decimal price)
    {
        if (quantity <= 0)
            throw new ArgumentException("Количество должно быть > 0");

        var existing = _items.FirstOrDefault(i => i.Product == product);
        if (existing is not null)
            existing.IncreaseQuantity(quantity);
        else
            _items.Add(new OrderItem(product, quantity, price));
    }

    public decimal TotalAmount => _items.Sum(i => i.Total);

    // ❌ НЕЛЬЗЯ: order.Items.Add(...) — обходит валидацию!
    // ✅ ТОЛЬКО: order.AddItem(...) — через Aggregate Root
}

class OrderItem
{
    public string Product { get; }
    public int Quantity { get; private set; }
    public decimal Price { get; }
    public decimal Total => Quantity * Price;

    internal OrderItem(string product, int quantity, decimal price)
    {
        Product = product;
        Quantity = quantity;
        Price = price;
    }

    internal void IncreaseQuantity(int amount) => Quantity += amount;
}
```

### Domain Events

Событие, значимое для бизнеса:

```csharp
// Событие
record OrderPlacedEvent(Guid OrderId, string CustomerName, decimal Total, DateTime PlacedAt);
record PlayerLeveledUpEvent(Guid PlayerId, int NewLevel);

// Entity генерирует события
class Player
{
    private readonly List<object> _domainEvents = [];
    public IReadOnlyList<object> DomainEvents => _domainEvents;

    public void GainExperience(int xp)
    {
        Experience += xp;
        while (Experience >= XpForNextLevel)
        {
            Level++;
            _domainEvents.Add(new PlayerLeveledUpEvent(Id, Level));
        }
    }

    public void ClearEvents() => _domainEvents.Clear();
}
```

### Bounded Context

Разные части системы могут понимать одно слово по-разному:

```
┌──────────────┐     ┌──────────────┐     ┌──────────────┐
│  Магазин     │     │  Инвентарь   │     │  Бой         │
│              │     │              │     │              │
│  Item:       │     │  Item:       │     │  Item:       │
│  - Price     │     │  - Weight    │     │  - Damage    │
│  - Stock     │     │  - SlotSize  │     │  - Cooldown  │
│  - Discount  │     │  - Stackable │     │  - ManaCost  │
└──────────────┘     └──────────────┘     └──────────────┘
```

"Item" в магазине ≠ "Item" в инвентаре ≠ "Item" в бою. Каждый контекст имеет свою модель.

### Ubiquitous Language

Используй **язык бизнеса** в коде:

```csharp
// ❌ Технический жаргон
class DataProcessor { void ProcessItems(List<DTO> data) { } }

// ✅ Язык предметной области
class OrderService { void PlaceOrder(Order order) { } }
class InventoryManager { void EquipItem(Player player, Weapon weapon) { } }
```

---

## Мини-упражнения
1. **⭐** Создай Value Object `Money(decimal, string)` с операторами.
2. **⭐⭐** Создай Aggregate `Order` с `OrderItem` — доступ только через Order.
3. **⭐⭐** Добавь Domain Events к Entity.

### Практика Uno Platform ⭐⭐⭐
**"Финансовый трекер"** с DDD: Transaction (Entity), Money (Value Object), Budget (Aggregate).

### Практика Godot ⭐⭐⭐
**Мини-RPG** с DDD: Player (Entity), DamageInfo (Value Object), Inventory (Aggregate), Domain Events для квестов.

---

## Контрольные вопросы перед Модулем 9
1. Назови 5 SOLID-принципов.
2. Чем Strategy отличается от State?
3. Что такое MVVM и зачем?
4. Как работает Clean Architecture?
5. Entity vs Value Object в DDD?

## Что дальше
Модуль 8 завершён! Дальше — **Модуль 9: Dependency Injection**.
