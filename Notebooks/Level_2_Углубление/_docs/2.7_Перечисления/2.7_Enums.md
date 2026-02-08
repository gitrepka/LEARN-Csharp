# Тема 2.7: Перечисления (enum)

## Что ты узнаешь
- Определение enum и базовые типы
- [Flags] атрибут для комбинирования
- Методы работы с enum

---

## Объяснение

### ЗАЧЕМ?
Вместо "магических чисел" (`state = 2` — что это?) используй имена: `state = EnemyState.Chase`. Код читаемый, IDE подсказывает, компилятор проверяет.

```csharp
enum DamageType
{
    Physical,   // 0
    Fire,       // 1
    Ice,        // 2
    Lightning,  // 3
    Poison      // 4
}

DamageType dmg = DamageType.Fire;

string desc = dmg switch
{
    DamageType.Physical => "Физический",
    DamageType.Fire => "Огненный",
    _ => "Другой"
};

// [Flags] — комбинирование нескольких значений
[Flags]
enum Permissions
{
    None    = 0,
    Read    = 1,      // 0001
    Write   = 2,      // 0010
    Execute = 4,      // 0100
    All     = Read | Write | Execute  // 0111
}

Permissions userPerms = Permissions.Read | Permissions.Write;
bool canWrite = userPerms.HasFlag(Permissions.Write); // true
bool canExec = userPerms.HasFlag(Permissions.Execute); // false

// Parsing
DamageType parsed = Enum.Parse<DamageType>("Fire"); // DamageType.Fire
bool ok = Enum.TryParse<DamageType>("Ice", out var result); // true

// Все значения
foreach (DamageType dt in Enum.GetValues<DamageType>())
    Console.WriteLine(dt);
```

---

## Мини-упражнения
1. **⭐** Создай `[Flags] enum Permissions { Read=1, Write=2, Execute=4 }`, комбинируй флаги.
2. **⭐** Используй `Enum.Parse` и `Enum.TryParse`.

## Что дальше
Дальше — **records** (2.8).
