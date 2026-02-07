# Тема 10.1: Основы XAML

## Что ты узнаешь
- XAML — декларативный язык для UI
- Элементы, атрибуты, пространства имён
- Layout панели: StackPanel, Grid, RelativePanel, Canvas

---

## Объяснение

### ЗАЧЕМ?
XAML описывает **что показывать** на экране. C# описывает **логику**. Разделение UI и кода — основа MVVM.

```xml
<!-- Простейшая страница -->
<Page x:Class="MyApp.MainPage"
      xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
      xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">

    <!-- StackPanel — элементы друг под другом -->
    <StackPanel Padding="20" Spacing="10">
        <TextBlock Text="Привет, мир!" FontSize="24" FontWeight="Bold"/>
        <TextBox PlaceholderText="Введи имя..." x:Name="NameInput"/>
        <Button Content="Нажми меня" Click="OnButtonClick"/>
    </StackPanel>
</Page>
```

### Grid — самый мощный layout

```xml
<Grid RowDefinitions="Auto,*,Auto"
      ColumnDefinitions="*,2*">
    <!-- Row=0, Col=0 -->
    <TextBlock Text="Заголовок" Grid.Row="0" Grid.ColumnSpan="2"/>

    <!-- Row=1, Col=0 — растягивается -->
    <ListView Grid.Row="1" Grid.Column="0"/>

    <!-- Row=1, Col=1 — занимает 2/3 ширины (2*) -->
    <Frame Grid.Row="1" Grid.Column="1"/>

    <!-- Row=2 — фиксированная высота (Auto) -->
    <CommandBar Grid.Row="2" Grid.ColumnSpan="2"/>
</Grid>

<!-- RowDefinitions:
     Auto = по содержимому
     *    = оставшееся место (равные доли)
     2*   = двойная доля
     200  = фиксированные пиксели
-->
```

### Markup Extensions

```xml
<!-- Binding — привязка к данным -->
<TextBlock Text="{Binding PlayerName}"/>

<!-- x:Bind — компилируемая привязка (быстрее) -->
<TextBlock Text="{x:Bind ViewModel.PlayerName, Mode=OneWay}"/>

<!-- StaticResource — ресурс из словаря -->
<TextBlock Style="{StaticResource HeaderStyle}"/>

<!-- ThemeResource — ресурс темы (Light/Dark) -->
<TextBlock Foreground="{ThemeResource SystemControlForegroundBaseHighBrush}"/>
```

---

## Мини-упражнения
1. **⭐** Создай страницу с Grid (3 строки, 2 колонки).
2. **⭐** StackPanel с 5 элементами разных типов.
3. **⭐** Используй RowDefinitions: Auto, *, 2*.

## Что дальше
Дальше — **элементы управления** (10.2).
