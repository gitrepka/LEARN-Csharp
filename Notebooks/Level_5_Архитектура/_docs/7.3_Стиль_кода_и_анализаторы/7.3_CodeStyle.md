# Тема 7.3: Стиль кода и анализаторы

## Что ты узнаешь
- .NET Naming Conventions (PascalCase, camelCase, _camelCase)
- .editorconfig — правила стиля для проекта
- Roslyn Analyzers, StyleCop — автоматическая проверка кода

---

## Объяснение

### ЗАЧЕМ?
В команде 5 разработчиков. Каждый пишет по-своему: кто-то `myVar`, кто-то `MyVar`, кто-то `m_var`. Код нечитаем. **Единый стиль** — читаемость, профессионализм, меньше споров на код-ревью.

### .NET Naming Conventions

```csharp
// PascalCase — публичные члены
public class PlayerCharacter              // класс
{
    public string Name { get; set; }      // свойство
    public int MaxHealth { get; set; }    // свойство
    public void TakeDamage(int amount) {} // метод
    public event Action? Died;            // событие
    public const int MaxLevel = 100;      // константа
}

// camelCase — параметры и локальные переменные
public void ProcessDamage(int damageAmount, bool isCritical)
{
    int reducedDamage = damageAmount - armor;
    bool shouldDie = reducedDamage >= health;
}

// _camelCase — приватные поля
private int _health;
private readonly List<string> _inventory;
private static int _instanceCount;

// Интерфейсы — префикс I
public interface IMovable { }
public interface IDamageable { }

// Generics — префикс T
public class Repository<TEntity> where TEntity : class { }
public T Max<T>(T a, T b) where T : IComparable<T> { }

// Async — суффикс Async
public async Task<Player> LoadPlayerAsync() { }
public async Task SaveGameAsync() { }
```

### .editorconfig

Файл правил стиля для всего проекта. IDE автоматически применяет.

```ini
# .editorconfig — положи в корень проекта

root = true

[*.cs]
# Отступы
indent_style = space
indent_size = 4

# Именование
dotnet_naming_rule.private_fields_should_be_camel_case.severity = warning
dotnet_naming_rule.private_fields_should_be_camel_case.symbols = private_fields
dotnet_naming_rule.private_fields_should_be_camel_case.style = camel_case_underscore

dotnet_naming_symbols.private_fields.applicable_kinds = field
dotnet_naming_symbols.private_fields.applicable_accessibilities = private
dotnet_naming_symbols.private_fields.required_modifiers =

dotnet_naming_style.camel_case_underscore.required_prefix = _
dotnet_naming_style.camel_case_underscore.capitalization = camel_case

# Предпочтения var
csharp_style_var_for_built_in_types = false:suggestion
csharp_style_var_when_type_is_apparent = true:suggestion

# Скобки
csharp_new_line_before_open_brace = all
csharp_prefer_braces = true:warning

# Using-и
dotnet_sort_system_directives_first = true
csharp_using_directive_placement = outside_namespace:warning

# Nullable
dotnet_diagnostic.CS8600.severity = warning
dotnet_diagnostic.CS8602.severity = warning
```

### Roslyn Analyzers

Встроенные анализаторы проверяют код при компиляции:

```xml
<!-- .csproj -->
<PropertyGroup>
    <AnalysisLevel>latest</AnalysisLevel>
    <EnforceCodeStyleInBuild>true</EnforceCodeStyleInBuild>
</PropertyGroup>
```

### StyleCop.Analyzers

```bash
dotnet add package StyleCop.Analyzers
```

Проверяет: порядок using, документацию, именование, форматирование.

### Severity Levels

```ini
# В .editorconfig
dotnet_diagnostic.IDE0044.severity = warning  # readonly field
dotnet_diagnostic.CA1822.severity = suggestion # static method
dotnet_diagnostic.SA1633.severity = none       # отключить правило
```

| Severity | Значение |
|----------|----------|
| error | Не скомпилируется |
| warning | Предупреждение (жёлтый) |
| suggestion | Подсказка (серый) |
| none | Отключено |

---

## Мини-упражнения
1. **⭐** Создай `.editorconfig` с правилами именования.
2. **⭐** Установи StyleCop.Analyzers, исправь все warnings.
3. **⭐** Настрой severity для 3 разных правил.

---

## Что дальше
Дальше — **XML-документация** (7.4).
