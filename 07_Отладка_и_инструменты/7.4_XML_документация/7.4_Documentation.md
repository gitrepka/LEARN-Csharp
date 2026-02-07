# Тема 7.4: XML-документация

## Что ты узнаешь
- XML-теги: summary, param, returns, exception, example
- Как документация интегрируется с IntelliSense
- Когда документировать

---

## Объяснение

### ЗАЧЕМ?
Ты написал метод `Calculate(int a, int b, bool c)`. Через месяц — что такое `c`? XML-документация появляется в **IntelliSense** (подсказки IDE) и делает код самодокументируемым.

### Основные теги

```csharp
/// <summary>
/// Вычисляет урон после применения брони и модификаторов.
/// </summary>
/// <param name="baseDamage">Базовый урон до модификаций (> 0).</param>
/// <param name="armor">Показатель брони цели.</param>
/// <param name="isCritical">Является ли удар критическим (×2 урон).</param>
/// <returns>Итоговый урон после всех модификаций. Минимум 0.</returns>
/// <exception cref="ArgumentOutOfRangeException">
/// Если <paramref name="baseDamage"/> меньше 0.
/// </exception>
/// <example>
/// <code>
/// int damage = CalculateDamage(50, 20, true);
/// // damage = (50 × 2) - 20 = 80
/// </code>
/// </example>
/// <remarks>
/// Формула: (baseDamage × critMultiplier) - armor.
/// Критический множитель = 2.0.
/// Результат не может быть отрицательным.
/// </remarks>
/// <seealso cref="TakeDamage"/>
public int CalculateDamage(int baseDamage, int armor, bool isCritical)
{
    ArgumentOutOfRangeException.ThrowIfNegative(baseDamage);
    int damage = isCritical ? baseDamage * 2 : baseDamage;
    return Math.Max(0, damage - armor);
}
```

### Все основные теги

| Тег | Назначение |
|-----|-----------|
| `<summary>` | Краткое описание (показывается в IntelliSense) |
| `<param name="">` | Описание параметра |
| `<returns>` | Что возвращает |
| `<exception cref="">` | Какие исключения может бросить |
| `<remarks>` | Подробное описание, нюансы |
| `<example>` + `<code>` | Пример использования |
| `<see cref=""/>` | Ссылка на другой тип/метод (inline) |
| `<seealso cref=""/>` | Связанные темы |
| `<value>` | Описание свойства |
| `<typeparam name="">` | Описание generic-параметра |
| `<inheritdoc/>` | Наследование документации от базового |

### Документация классов и свойств

```csharp
/// <summary>
/// Представляет игрока в игровом мире.
/// </summary>
/// <typeparam name="TWeapon">Тип оружия игрока.</typeparam>
public class Player<TWeapon> where TWeapon : IWeapon
{
    /// <summary>
    /// Имя игрока. Не может быть null или пустым.
    /// </summary>
    public required string Name { get; init; }

    /// <value>
    /// Текущее здоровье. Диапазон: 0 — <see cref="MaxHealth"/>.
    /// </value>
    public int Health { get; private set; }

    /// <inheritdoc/>
    public override string ToString() => $"{Name} (HP: {Health})";
}
```

### Когда документировать?

| Что | Документировать? |
|-----|-----------------|
| Публичный API (библиотека) | Обязательно |
| Internal/private код | Только если неочевидно |
| Параметры с неочевидным назначением | Да |
| Самоочевидные методы (`GetName()`) | Нет |
| Сложная логика | Да (в remarks) |

### Генерация документации

```xml
<!-- .csproj — включить генерацию XML файла -->
<PropertyGroup>
    <GenerateDocumentationFile>true</GenerateDocumentationFile>
</PropertyGroup>
```

---

## Мини-упражнения
1. **⭐** Добавь XML-документацию к 3 методам (summary, param, returns).
2. **⭐** Используй `<exception cref="">` и `<example>`.
3. **⭐** Включи генерацию документации в `.csproj`.

---

## Контрольные вопросы перед Модулем 8
1. Как поставить conditional breakpoint?
2. Чем structured logging отличается от строковой конкатенации?
3. Зачем нужен `.editorconfig`?
4. Какие XML-теги документации обязательны для публичного API?

## Что дальше
Модуль 7 завершён! Дальше — **Модуль 8: Архитектурные паттерны**.
