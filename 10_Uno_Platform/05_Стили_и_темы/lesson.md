# Тема 10.5: Стили, ресурсы и темы

## Что ты узнаешь
- StaticResource vs ThemeResource
- ResourceDictionary — словарь ресурсов
- Именованные и неявные стили, BasedOn
- Light/Dark/Custom темы
- ControlTemplate, VisualStateManager

---

## Объяснение

```xml
<!-- Ресурсы страницы -->
<Page.Resources>
    <!-- Цвета -->
    <SolidColorBrush x:Key="AccentBrush" Color="#FF6200EE"/>

    <!-- Именованный стиль (применяется явно) -->
    <Style x:Key="HeaderStyle" TargetType="TextBlock">
        <Setter Property="FontSize" Value="28"/>
        <Setter Property="FontWeight" Value="Bold"/>
        <Setter Property="Foreground" Value="{ThemeResource SystemAccentColor}"/>
    </Style>

    <!-- Неявный стиль (применяется ко ВСЕМ Button на странице) -->
    <Style TargetType="Button">
        <Setter Property="CornerRadius" Value="8"/>
        <Setter Property="Padding" Value="16,8"/>
    </Style>

    <!-- Наследование стилей -->
    <Style x:Key="DangerButton" TargetType="Button" BasedOn="{StaticResource DefaultButtonStyle}">
        <Setter Property="Background" Value="Red"/>
        <Setter Property="Foreground" Value="White"/>
    </Style>
</Page.Resources>

<!-- Использование -->
<TextBlock Style="{StaticResource HeaderStyle}" Text="Заголовок"/>
<Button Style="{StaticResource DangerButton}" Content="Удалить"/>
```

### Темы Light/Dark

```xml
<!-- App.xaml — ресурсы для тем -->
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.ThemeDictionaries>
            <ResourceDictionary x:Key="Light">
                <SolidColorBrush x:Key="PageBackground" Color="White"/>
                <SolidColorBrush x:Key="TextPrimary" Color="Black"/>
            </ResourceDictionary>
            <ResourceDictionary x:Key="Dark">
                <SolidColorBrush x:Key="PageBackground" Color="#1E1E1E"/>
                <SolidColorBrush x:Key="TextPrimary" Color="White"/>
            </ResourceDictionary>
        </ResourceDictionary.ThemeDictionaries>
    </ResourceDictionary>
</Application.Resources>

<!-- Автоматически выбирает по теме -->
<Grid Background="{ThemeResource PageBackground}">
    <TextBlock Foreground="{ThemeResource TextPrimary}"/>
</Grid>
```

### VisualStateManager

```xml
<VisualStateManager.VisualStateGroups>
    <VisualStateGroup>
        <VisualState x:Name="Narrow">
            <VisualState.StateTriggers>
                <AdaptiveTrigger MinWindowWidth="0"/>
            </VisualState.StateTriggers>
            <VisualState.Setters>
                <Setter Target="SidePanel.Visibility" Value="Collapsed"/>
            </VisualState.Setters>
        </VisualState>
        <VisualState x:Name="Wide">
            <VisualState.StateTriggers>
                <AdaptiveTrigger MinWindowWidth="720"/>
            </VisualState.StateTriggers>
            <VisualState.Setters>
                <Setter Target="SidePanel.Visibility" Value="Visible"/>
            </VisualState.Setters>
        </VisualState>
    </VisualStateGroup>
</VisualStateManager.VisualStateGroups>
```

---

## Мини-упражнения
1. **⭐** Создай 3 именованных стиля, примени к элементам.
2. **⭐** Light/Dark тема через ThemeDictionaries.
3. **⭐⭐** AdaptiveTrigger: скрытие панели на узком экране.

## Что дальше
Дальше — **анимации** (10.6).
