# Тема 10.2: Элементы управления (Controls)

## Что ты узнаешь
- Текст: TextBlock, TextBox, PasswordBox, RichTextBlock
- Кнопки: Button, ToggleButton, HyperlinkButton
- Выбор: CheckBox, RadioButton, ToggleSwitch, ComboBox, Slider
- Списки: ListView, GridView, ItemsRepeater
- Диалоги: ContentDialog, Flyout, MenuFlyout

---

## Основные элементы

```xml
<!-- Текст -->
<TextBlock Text="Только для чтения" TextWrapping="Wrap"/>
<TextBox Text="{Binding Name, Mode=TwoWay}" PlaceholderText="Имя..."/>
<PasswordBox PlaceholderText="Пароль"/>

<!-- Кнопки -->
<Button Content="Сохранить" Command="{Binding SaveCommand}"/>
<ToggleButton Content="Вкл/Выкл" IsChecked="{Binding IsEnabled, Mode=TwoWay}"/>
<HyperlinkButton Content="Подробнее" NavigateUri="https://example.com"/>

<!-- Выбор -->
<CheckBox Content="Запомнить" IsChecked="{Binding RememberMe, Mode=TwoWay}"/>
<RadioButton Content="Опция А" GroupName="Options"/>
<ToggleSwitch Header="Тёмная тема" IsOn="{Binding IsDark, Mode=TwoWay}"/>
<Slider Minimum="0" Maximum="100" Value="{Binding Volume, Mode=TwoWay}"/>

<ComboBox SelectedItem="{Binding SelectedCity, Mode=TwoWay}"
          ItemsSource="{Binding Cities}"/>

<!-- Прогресс -->
<ProgressBar Value="{Binding Progress}" Maximum="100"/>
<ProgressRing IsActive="{Binding IsLoading}"/>

<!-- Список -->
<ListView ItemsSource="{Binding Items}"
          SelectedItem="{Binding SelectedItem, Mode=TwoWay}">
    <ListView.ItemTemplate>
        <DataTemplate>
            <StackPanel Orientation="Horizontal" Spacing="10">
                <TextBlock Text="{Binding Name}" FontWeight="Bold"/>
                <TextBlock Text="{Binding Description}" Opacity="0.6"/>
            </StackPanel>
        </DataTemplate>
    </ListView.ItemTemplate>
</ListView>

<!-- ContentDialog — модальный диалог -->
<ContentDialog x:Name="ConfirmDialog"
               Title="Подтверждение"
               Content="Удалить элемент?"
               PrimaryButtonText="Удалить"
               SecondaryButtonText="Отмена"/>
```

```csharp
// Показать диалог из code-behind:
var result = await ConfirmDialog.ShowAsync();
if (result == ContentDialogResult.Primary)
    DeleteItem();
```

### ItemsRepeater — виртуализация!

```xml
<!-- Для БОЛЬШИХ списков (тысячи элементов) — быстрее ListView -->
<ScrollViewer>
    <ItemsRepeater ItemsSource="{Binding LargeList}">
        <ItemsRepeater.Layout>
            <StackLayout Spacing="4"/>
        </ItemsRepeater.Layout>
        <ItemsRepeater.ItemTemplate>
            <DataTemplate>
                <TextBlock Text="{Binding}"/>
            </DataTemplate>
        </ItemsRepeater.ItemTemplate>
    </ItemsRepeater>
</ScrollViewer>
```

---

## Мини-упражнения
1. **⭐** Создай форму: TextBox, ComboBox, CheckBox, Button.
2. **⭐** ListView с DataTemplate (имя + описание).
3. **⭐⭐** ContentDialog для подтверждения действия.

## Что дальше
Дальше — **Data Binding** (10.3).
