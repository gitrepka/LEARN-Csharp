# Тема 10.10: Адаптивный и отзывчивый UI

## Что ты узнаешь
- AdaptiveTrigger — разные layout для разных экранов
- Compact/Expanded views
- Телефон / Планшет / Десктоп

---

## Объяснение

```xml
<!-- Адаптивный layout: телефон vs десктоп -->
<Grid x:Name="RootGrid">
    <VisualStateManager.VisualStateGroups>
        <VisualStateGroup>
            <!-- Телефон: < 640px -->
            <VisualState x:Name="NarrowState">
                <VisualState.StateTriggers>
                    <AdaptiveTrigger MinWindowWidth="0"/>
                </VisualState.StateTriggers>
                <VisualState.Setters>
                    <Setter Target="DetailPanel.Visibility" Value="Collapsed"/>
                    <Setter Target="MasterPanel.(Grid.ColumnSpan)" Value="2"/>
                </VisualState.Setters>
            </VisualState>

            <!-- Планшет/десктоп: >= 720px -->
            <VisualState x:Name="WideState">
                <VisualState.StateTriggers>
                    <AdaptiveTrigger MinWindowWidth="720"/>
                </VisualState.StateTriggers>
                <VisualState.Setters>
                    <Setter Target="DetailPanel.Visibility" Value="Visible"/>
                    <Setter Target="MasterPanel.(Grid.ColumnSpan)" Value="1"/>
                </VisualState.Setters>
            </VisualState>
        </VisualStateGroup>
    </VisualStateManager.VisualStateGroups>

    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="*"/>
        <ColumnDefinition Width="2*"/>
    </Grid.ColumnDefinitions>

    <StackPanel x:Name="MasterPanel" Grid.Column="0">
        <ListView ItemsSource="{Binding Items}"/>
    </StackPanel>

    <StackPanel x:Name="DetailPanel" Grid.Column="1">
        <!-- Детали выбранного элемента -->
    </StackPanel>
</Grid>
```

### Responsive spacing

```xml
<!-- Отступы зависят от размера экрана -->
<StackPanel Padding="16">
    <!-- На телефоне: одна колонка -->
    <!-- На планшете: две колонки -->
    <!-- На десктопе: три колонки -->
</StackPanel>
```

---

## Мини-упражнения
1. **⭐** AdaptiveTrigger: скрытие бокового меню на маленьком экране.
2. **⭐⭐** Master-Detail layout с адаптацией.

## Что дальше
Дальше — **локализация** (10.11).
