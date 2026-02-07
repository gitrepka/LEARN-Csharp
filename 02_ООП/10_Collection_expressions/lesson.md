# Тема 2.10: Коллекционные выражения (C# 12)

## Что ты узнаешь
- Новый синтаксис создания коллекций `[1, 2, 3]`
- Spread-оператор `..`
- Какие типы поддерживают

---

## Объяснение

### ЗАЧЕМ?
Раньше инициализация коллекций была многословной. C# 12 добавил единый, чистый синтаксис.

```csharp
// Старый стиль:
int[] oldArray = new int[] { 1, 2, 3 };
List<string> oldList = new List<string> { "a", "b", "c" };

// Новый стиль (C# 12):
int[] numbers = [1, 2, 3, 4, 5];
List<string> names = ["Алиса", "Боб", "Вика"];
Span<int> span = [10, 20, 30];

// Spread-оператор (..) — объединение коллекций
int[] first = [1, 2, 3];
int[] second = [4, 5, 6];
int[] combined = [..first, ..second];         // [1, 2, 3, 4, 5, 6]
int[] withExtra = [0, ..first, 99, ..second]; // [0, 1, 2, 3, 99, 4, 5, 6]

// Пустая коллекция
List<int> empty = [];

// Работает с:
// - int[], string[] (массивы)
// - List<T>
// - Span<T>, ReadOnlySpan<T>
// - ImmutableArray<T>
// - Любой тип с CollectionBuilder attribute
```

---

## Мини-упражнения
1. **⭐** Создай `List<string>` через collection expression.
2. **⭐** Объедини два массива через spread `..`.
3. **⭐** `List<string> names = ["Alice", "Bob", ..otherNames];`

---

## Общая практика для тем 2.6-2.10

### Uno Platform ⭐⭐
**"Цветовая палитра"** — `readonly record struct Color(byte R, byte G, byte B)` с Mix, Lighten, Darken. RGB-слайдеры, предпросмотр.

### Godot ⭐⭐
**Система урона** — `[Flags] enum StatusEffects`, `record struct DamageInfo`, `record SaveData` с автосейвами через `with`.

---

## Контрольные вопросы перед Модулем 3
1. Назови 4 принципа ООП и приведи пример каждого.
2. Чем `record` отличается от `class`? Когда использовать?
3. Что такое collection expressions и какие типы поддерживают?
4. Объясни разницу между `struct` и `class` в контексте памяти.
5. Когда `interface` vs `abstract class`?

## Что дальше
Модуль 2 завершён! Ты понимаешь ООП. Дальше — **Модуль 3: Коллекции** и **Модуль 4: Продвинутый C#** (Generics, LINQ, async/await).
