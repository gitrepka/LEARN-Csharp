# Тема 10.3: Data Binding

## Что ты узнаешь
- {Binding} vs {x:Bind}
- OneWay, TwoWay, OneTime
- INotifyPropertyChanged, ObservableCollection
- IValueConverter — конвертеры
- DataTemplate, DataTemplateSelector

---

## Объяснение

### ЗАЧЕМ?
Без Binding: вручную читаешь TextBox.Text, записываешь в Label.Text. С Binding: **UI автоматически** отражает данные ViewModel и наоборот.

```xml
<!-- OneWay: ViewModel → UI (обновляется при изменении) -->
<TextBlock Text="{Binding PlayerName, Mode=OneWay}"/>

<!-- TwoWay: UI ↔ ViewModel (оба направления) -->
<TextBox Text="{Binding SearchText, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}"/>

<!-- OneTime: один раз при загрузке (самый быстрый) -->
<TextBlock Text="{Binding AppVersion, Mode=OneTime}"/>

<!-- x:Bind — компилируемая привязка (быстрее, безопаснее) -->
<TextBlock Text="{x:Bind ViewModel.PlayerName, Mode=OneWay}"/>
```

### INotifyPropertyChanged

```csharp
// CommunityToolkit.Mvvm делает это автоматически:
partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private string _searchText = "";

    // Автоматически:
    // 1. Создаёт public string SearchText { get; set; }
    // 2. При изменении вызывает PropertyChanged
    // 3. UI обновляется!

    [ObservableProperty]
    private ObservableCollection<string> _items = [];
    // ObservableCollection уведомляет UI о Add/Remove
}
```

### IValueConverter — трансформация данных

```csharp
// bool → Visibility
class BoolToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return (bool)value ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return (Visibility)value == Visibility.Visible;
    }
}

// int → цвет (здоровье → красный/зелёный)
class HealthToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        int health = (int)value;
        return health > 50
            ? new SolidColorBrush(Colors.Green)
            : new SolidColorBrush(Colors.Red);
    }

    public object ConvertBack(object value, Type t, object p, string l) => throw new NotSupportedException();
}
```

```xml
<!-- Регистрация конвертера -->
<Page.Resources>
    <local:BoolToVisibilityConverter x:Key="BoolToVis"/>
    <local:HealthToColorConverter x:Key="HealthToColor"/>
</Page.Resources>

<TextBlock Visibility="{Binding IsLoading, Converter={StaticResource BoolToVis}}"/>
<ProgressBar Foreground="{Binding Health, Converter={StaticResource HealthToColor}}"/>
```

### DataTemplateSelector

```csharp
class MessageTemplateSelector : DataTemplateSelector
{
    public DataTemplate TextTemplate { get; set; }
    public DataTemplate ImageTemplate { get; set; }

    protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
    {
        return item switch
        {
            TextMessage => TextTemplate,
            ImageMessage => ImageTemplate,
            _ => TextTemplate
        };
    }
}
```

---

## Мини-упражнения
1. **⭐** TwoWay Binding: TextBox ↔ TextBlock (зеркало).
2. **⭐** Создай BoolToVisibilityConverter.
3. **⭐⭐** ObservableCollection + ListView: Add/Remove через кнопки.

## Что дальше
Дальше — **навигация** (10.4).
