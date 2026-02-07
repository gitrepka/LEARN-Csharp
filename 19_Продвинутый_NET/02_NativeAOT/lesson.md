# Тема 19.2: NativeAOT

## Что ты узнаешь
- JIT vs AOT компиляция
- NativeAOT — сборка в нативный код
- Trimming — удаление неиспользуемого кода
- Ограничения и когда использовать

---

## Объяснение

### JIT vs AOT

```
JIT (Just-In-Time) — обычный .NET:
1. Компилируешь → IL код (промежуточный)
2. Запускаешь → CLR компилирует IL → машинный код на лету
3. Первый запуск медленный, потом быстрый
✅ Гибкий, рефлексия работает, кросс-платформенный
❌ Медленный старт, нужен .NET runtime

AOT (Ahead-Of-Time) — NativeAOT:
1. Компилируешь → сразу машинный код (exe/so)
2. Запускаешь → мгновенно
3. Нет CLR, нет JIT — как C/C++ программа
✅ Мгновенный старт, маленький размер, не нужен runtime
❌ Ограничения (рефлексия, dynamic, некоторые библиотеки)
```

### Включение NativeAOT

```xml
<!-- .csproj -->
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net10.0</TargetFramework>
    <PublishAot>true</PublishAot>
  </PropertyGroup>
</Project>
```

```bash
# Публикация
dotnet publish -c Release -r win-x64
dotnet publish -c Release -r linux-x64
dotnet publish -c Release -r osx-arm64

# Результат: один exe файл без зависимостей
# JIT:        150 MB (exe + runtime + библиотеки)
# NativeAOT:  ~15 MB (один файл!)
```

### Сравнение

```
                  JIT          NativeAOT
Старт:           ~500ms       ~50ms (10x быстрее)
Размер:          ~150 MB      ~15 MB
Runtime нужен:   Да           Нет
Рефлексия:       Полная       Ограниченная
Кросс-платф:     Да           Каждая платформа отдельно
Пиковая скорость: Выше (JIT оптимизации)  Чуть ниже
```

### Trimming — удаление лишнего

```xml
<PropertyGroup>
    <PublishTrimmed>true</PublishTrimmed>
    <TrimMode>link</TrimMode> <!-- агрессивный trim -->
</PropertyGroup>

<!-- Trimming анализирует код и удаляет всё, что не используется:
     - Неиспользуемые классы
     - Неиспользуемые методы
     - Неиспользуемые сборки

     Результат: значительно меньший размер -->
```

### Ограничения NativeAOT

```csharp
// ❌ НЕ работает с NativeAOT:
Type.GetType("MyClass");          // dynamic type loading
Activator.CreateInstance(type);    // без подготовки
Assembly.Load("MyAssembly");       // dynamic assembly loading
dynamic obj = GetSomething();      // dynamic keyword

// ✅ Работает:
typeof(Player).GetProperties();    // если тип известен
JsonSerializer.Serialize(obj, AppJsonContext.Default.Player); // source gen
[GeneratedRegex("...")] ...       // source gen regex

// Главное правило:
// Source Generators вместо рефлексии → AOT-совместимо
```

### Когда использовать

```
NativeAOT — идеально для:
✅ CLI утилиты (быстрый старт, один файл)
✅ Микросервисы (мгновенный старт в контейнерах)
✅ Serverless (AWS Lambda, Azure Functions — холодный старт)
✅ Мобильные (iOS требует AOT)

NativeAOT — не подходит для:
❌ Приложения с тяжёлой рефлексией
❌ Динамическая загрузка плагинов
❌ Скрипты и REPL

Для Godot:
- Godot использует свой C# runtime (Mono на мобильных)
- NativeAOT пока экспериментально для Godot

Для Uno Platform:
- iOS: AOT обязателен (ограничение Apple)
- Android: JIT + AOT (гибридный)
- WASM: AOT для производительности
```

---

## Мини-упражнения

1. **⭐** Создай консольное приложение, опубликуй как NativeAOT. Сравни размер с обычной публикацией.
2. **⭐⭐** Создай Minimal API с NativeAOT. Используй `[JsonSerializable]` для AOT-совместимости.

## Что дальше
Дальше — **System.IO.Pipelines** (19.3).
