# Тема 10.13: C# Markup (альтернатива XAML)

## Что ты узнаешь
- Построение UI в C# коде
- Когда C# Markup vs XAML

---

## Объяснение

```csharp
using Uno.Extensions.Markup;

// UI полностью в C#!
new StackPanel()
    .Padding(20)
    .Spacing(10)
    .Children(
        new TextBlock()
            .Text("Привет!")
            .FontSize(24)
            .FontWeight(FontWeights.Bold),

        new TextBox()
            .PlaceholderText("Введи имя...")
            .Text(x => x.Binding(() => vm.Name).TwoWay()),

        new Button()
            .Content("Сохранить")
            .Command(() => vm.SaveCommand),

        new ListView()
            .ItemsSource(() => vm.Items)
            .ItemTemplate<ItemModel>(item =>
                new StackPanel()
                    .Orientation(Orientation.Horizontal)
                    .Children(
                        new TextBlock().Text(() => item.Name),
                        new TextBlock().Text(() => item.Price)
                    ))
    );
```

### Когда что?

| XAML | C# Markup |
|------|-----------|
| Визуальный дизайн | Динамический UI |
| Hot Reload дизайна | Строгая типизация |
| Привычный UI-разработчикам | Удобнее C#-разработчикам |
| Больше примеров в документации | Рефакторинг IDE |

---

## Мини-упражнения
1. **⭐** Создай страницу полностью в C# Markup.
2. **⭐** Сравни тот же UI в XAML и C# Markup.

## Что дальше
Дальше — **платформо-специфичный код** (10.14).
