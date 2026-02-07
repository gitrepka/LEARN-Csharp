# Тема 11.18: Экспорт проекта

## Что ты узнаешь
- Export Presets: Windows, Linux, Android, Web
- Настройки экспорта: иконки, имя, версия
- Оптимизация размера билда
- Подводные камни экспорта C# проектов

---

## Объяснение

### Настройка экспорта

1. **Project → Export → Add Preset** → выбери платформу
2. Установи Export Templates (при первом использовании)
3. Настрой параметры:
   - Name, Version
   - Icon
   - Screen orientation (для мобильных)
   - Features / permissions

### C# специфика

```csharp
// ⚠️ C# проекты требуют .NET runtime!
// Windows: .NET установлен или self-contained
// Android: Mono runtime включён автоматически
// Web: экспериментальная поддержка

// Для экспорта:
// 1. Убедись что проект компилируется (dotnet build)
// 2. Export → Export Project
```

### Оптимизация размера

```xml
<!-- .csproj -->
<PropertyGroup Condition="'$(Configuration)' == 'Release'">
    <PublishTrimmed>true</PublishTrimmed>  <!-- удалить неиспользуемый код -->
</PropertyGroup>
```

### Подводные камни

- **Reflection + Trimming**: если используешь рефлексию, trimmer может удалить нужный код. Добавь `[DynamicDependency]` или отключи trimming для этих типов.
- **Resource paths**: `GD.Load("res://...")` — все ресурсы должны быть в проекте.
- **Platform-specific**: некоторые API недоступны на Web/Mobile.

---

## Мини-упражнения
1. **⭐** Экспортируй проект для Windows.
2. **⭐** Настрой иконку и имя приложения.
3. **⭐⭐** Экспортируй для Android (если есть SDK).

---

## Контрольные вопросы перед Модулем 12
1. Чем _Process отличается от _PhysicsProcess?
2. Что такое "Call down, Signal up"?
3. Зачем [Export] и [GlobalClass]?
4. Как работает NavigationAgent2D?
5. Что такое Game Feel и зачем?

## Что дальше
Модуль 11 завершён! Дальше — **Модуль 12: Базы данных**.
