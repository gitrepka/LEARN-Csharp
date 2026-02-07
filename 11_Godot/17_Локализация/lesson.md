# Тема 11.17: Локализация в Godot

## Что ты узнаешь
- CSV/PO файлы для переводов
- TranslationServer
- Tr() для перевода строк в коде

---

## Объяснение

### CSV-файл переводов

```csv
keys,en,ru,de
MENU_START,Start Game,Начать игру,Spiel starten
MENU_QUIT,Quit,Выход,Beenden
PLAYER_HEALTH,Health,Здоровье,Gesundheit
DIALOG_HELLO,Hello!,Привет!,Hallo!
```

Импорт: положи `.csv` в `res://`, Godot автоматически создаст `.translation` ресурсы.

### Использование в коде

```csharp
// Tr() — перевод строки
Label label = GetNode<Label>("StartLabel");
label.Text = Tr("MENU_START"); // "Начать игру" (для ru)

// Смена языка
TranslationServer.SetLocale("en"); // английский
TranslationServer.SetLocale("ru"); // русский

// В XAML-подобном подходе Godot
// Button.text = "MENU_START" (если включен автоперевод)
```

### Автоматический перевод UI

Project → Project Settings → Internationalization → Locale → Test: `ru`

В Inspector у Label/Button: `Auto Translate Mode = Always`.

---

## Мини-упражнения
1. **⭐** CSV с 3 языками, Tr() в коде.
2. **⭐** Смена языка через кнопку/ComboBox.

## Что дальше
Дальше — **экспорт** (11.18).
