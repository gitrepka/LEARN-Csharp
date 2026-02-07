# Тема 10.4: Навигация

## Что ты узнаешь
- Frame.Navigate, GoBack, передача параметров
- Uno.Extensions.Navigation (route-based)
- NavigationView, SplitView

---

## Объяснение

### Базовая навигация (Frame)

```csharp
// Переход на страницу
Frame.Navigate(typeof(DetailsPage));
Frame.Navigate(typeof(DetailsPage), parameter: selectedItem);
Frame.GoBack();

// Получение параметра на целевой странице
protected override void OnNavigatedTo(NavigationEventArgs e)
{
    if (e.Parameter is ItemModel item)
        ViewModel.LoadItem(item);
}
```

### Uno.Extensions.Navigation (рекомендуется)

```csharp
// Навигация через INavigator (DI)
await _navigator.NavigateViewModelAsync<DetailsViewModel>(this, data: new { Id = 42 });
await _navigator.NavigateBackAsync(this);
```

### NavigationView — боковое меню

```xml
<NavigationView IsSettingsVisible="True"
                SelectionChanged="OnNavSelectionChanged">
    <NavigationView.MenuItems>
        <NavigationViewItem Content="Главная" Icon="Home" Tag="Home"/>
        <NavigationViewItem Content="Профиль" Icon="People" Tag="Profile"/>
        <NavigationViewItem Content="Настройки" Icon="Setting" Tag="Settings"/>
    </NavigationView.MenuItems>

    <Frame x:Name="ContentFrame"/>
</NavigationView>
```

```csharp
private void OnNavSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
{
    if (args.SelectedItemContainer?.Tag is string tag)
    {
        Type page = tag switch
        {
            "Home" => typeof(HomePage),
            "Profile" => typeof(ProfilePage),
            "Settings" => typeof(SettingsPage),
            _ => typeof(HomePage)
        };
        ContentFrame.Navigate(page);
    }
}
```

---

## Мини-упражнения
1. **⭐** Frame.Navigate между двумя страницами с передачей параметра.
2. **⭐⭐** NavigationView с 3 пунктами меню.

## Что дальше
Дальше — **стили и темы** (10.5).
