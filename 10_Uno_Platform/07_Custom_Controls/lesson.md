# Тема 10.7: Custom Controls и Dependency Properties

## Что ты узнаешь
- DependencyProperty — создание своих свойств для XAML
- Attached Properties
- UserControl vs Custom Control
- Behaviors

---

## Объяснение

### DependencyProperty — свойство для привязки

```csharp
// Кастомный контрол с DependencyProperty
public partial class RatingControl : UserControl
{
    // Dependency Property
    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register(
            nameof(Value),                    // имя свойства
            typeof(int),                      // тип
            typeof(RatingControl),            // владелец
            new PropertyMetadata(0, OnValueChanged)); // значение по умолчанию + callback

    public int Value
    {
        get => (int)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public static readonly DependencyProperty MaxValueProperty =
        DependencyProperty.Register(nameof(MaxValue), typeof(int),
            typeof(RatingControl), new PropertyMetadata(5));

    public int MaxValue
    {
        get => (int)GetValue(MaxValueProperty);
        set => SetValue(MaxValueProperty, value);
    }

    private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (RatingControl)d;
        control.UpdateStars();
    }

    private void UpdateStars() { /* обновить UI звёздочек */ }
}
```

```xml
<!-- Использование в XAML — как обычное свойство! -->
<local:RatingControl Value="{Binding Rating, Mode=TwoWay}" MaxValue="10"/>
```

### UserControl vs Custom Control

| UserControl | Custom Control |
|-------------|---------------|
| XAML + code-behind | Только C# + ControlTemplate |
| Простой, быстрый | Гибкий, темизируемый |
| Нельзя менять шаблон | Полная кастомизация |
| **Для большинства случаев** | Для библиотек компонентов |

### Attached Property

```csharp
// Свойство, которое можно добавить к ЛЮБОМУ элементу
public static class DragHelper
{
    public static readonly DependencyProperty IsDraggableProperty =
        DependencyProperty.RegisterAttached(
            "IsDraggable", typeof(bool), typeof(DragHelper),
            new PropertyMetadata(false, OnIsDraggableChanged));

    public static bool GetIsDraggable(DependencyObject obj) => (bool)obj.GetValue(IsDraggableProperty);
    public static void SetIsDraggable(DependencyObject obj, bool value) => obj.SetValue(IsDraggableProperty, value);

    private static void OnIsDraggableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is UIElement element && (bool)e.NewValue)
        {
            // подключить drag-логику
        }
    }
}
```

```xml
<Border local:DragHelper.IsDraggable="True" Background="Blue"/>
```

---

## Мини-упражнения
1. **⭐** Создай UserControl с DependencyProperty.
2. **⭐⭐** Создай RatingControl (звёздочки) с привязкой Value.
3. **⭐⭐** Создай Attached Property.

## Что дальше
Дальше — **MVVM Toolkit** (10.8).
