# Тема 10.11: Локализация в Uno Platform

## Что ты узнаешь
- x:Uid для XAML — ключ локализации
- .resw ресурсные файлы
- ResourceLoader, смена языка в runtime

---

## Объяснение

### Файлы ресурсов (.resw)

```
Strings/
├── en/
│   └── Resources.resw    (Name: "Hello", Value: "Hello!")
├── ru/
│   └── Resources.resw    (Name: "Hello", Value: "Привет!")
└── de/
    └── Resources.resw    (Name: "Hello", Value: "Hallo!")
```

### x:Uid — локализация в XAML

```xml
<!-- x:Uid привязывает к ресурсу -->
<TextBlock x:Uid="WelcomeText"/>
<!-- Ищет ресурс "WelcomeText.Text" -->

<Button x:Uid="SaveButton"/>
<!-- Ищет "SaveButton.Content" -->

<!-- В .resw файле:
  Name: WelcomeText.Text    Value: Добро пожаловать!
  Name: SaveButton.Content  Value: Сохранить
-->
```

### ResourceLoader — из кода

```csharp
var loader = ResourceLoader.GetForCurrentView();
string welcome = loader.GetString("WelcomeText/Text");

// Смена языка в runtime
Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "ru";
// Перезагрузка страницы нужна для обновления UI
```

---

## Мини-упражнения
1. **⭐** Создай .resw для 2 языков, используй x:Uid.
2. **⭐⭐** Смена языка в runtime через ComboBox.

## Что дальше
Дальше — **Accessibility** (10.12).
