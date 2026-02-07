# Тема 10.6: Анимации

## Что ты узнаешь
- Storyboard: DoubleAnimation, ColorAnimation
- Easing Functions (замедление/ускорение)
- VisualStateManager transitions
- Composition animations

---

## Объяснение

```xml
<!-- Storyboard — анимация свойств -->
<Page.Resources>
    <Storyboard x:Name="FadeInAnimation">
        <DoubleAnimation
            Storyboard.TargetName="MyElement"
            Storyboard.TargetProperty="Opacity"
            From="0" To="1" Duration="0:0:0.5">
            <DoubleAnimation.EasingFunction>
                <CubicEase EasingMode="EaseOut"/>
            </DoubleAnimation.EasingFunction>
        </DoubleAnimation>
    </Storyboard>

    <Storyboard x:Name="SlideInAnimation">
        <DoubleAnimation
            Storyboard.TargetName="MyElement"
            Storyboard.TargetProperty="(UIElement.RenderTransform).(TranslateTransform.X)"
            From="-200" To="0" Duration="0:0:0.3">
            <DoubleAnimation.EasingFunction>
                <ExponentialEase EasingMode="EaseOut"/>
            </DoubleAnimation.EasingFunction>
        </DoubleAnimation>
    </Storyboard>
</Page.Resources>

<StackPanel>
    <Border x:Name="MyElement" Opacity="0" Background="Blue" Width="100" Height="100">
        <Border.RenderTransform>
            <TranslateTransform/>
        </Border.RenderTransform>
    </Border>
    <Button Content="Появись!" Click="OnAnimate"/>
</StackPanel>
```

```csharp
private void OnAnimate(object sender, RoutedEventArgs e)
{
    FadeInAnimation.Begin();
    SlideInAnimation.Begin();
}
```

### Easing Functions (замедление)

| Easing | Эффект |
|--------|--------|
| `Linear` | Равномерное |
| `CubicEase EaseOut` | Замедление в конце (естественное) |
| `BounceEase` | Отскок |
| `ElasticEase` | Пружина |
| `ExponentialEase` | Экспоненциальное |

### Transitions — автоматические анимации

```xml
<!-- Элемент автоматически анимируется при появлении -->
<StackPanel>
    <StackPanel.ChildrenTransitions>
        <EntranceThemeTransition/>
    </StackPanel.ChildrenTransitions>
    <!-- Элементы появляются с анимацией -->
</StackPanel>

<!-- Переход при навигации -->
<Frame>
    <Frame.ContentTransitions>
        <NavigationThemeTransition/>
    </Frame.ContentTransitions>
</Frame>
```

---

## Мини-упражнения
1. **⭐** Анимация Opacity (fade in/out).
2. **⭐** Анимация с Easing (BounceEase).
3. **⭐⭐** Slide-in анимация через TranslateTransform.

## Что дальше
Дальше — **Custom Controls** (10.7).
