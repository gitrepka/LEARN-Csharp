# Тема 10.12: Accessibility (Доступность)

## Что ты узнаешь
- AutomationProperties — метки для screen readers
- Keyboard navigation, Tab order
- High contrast, Screen reader support

---

## Объяснение

### ЗАЧЕМ?
10% пользователей имеют ограничения: зрение, моторика, слух. Доступное приложение — больше пользователей + требование для магазинов приложений.

```xml
<!-- Метки для screen readers -->
<Image Source="avatar.png"
       AutomationProperties.Name="Фото профиля пользователя"/>

<Button AutomationProperties.Name="Удалить элемент">
    <SymbolIcon Symbol="Delete"/>
</Button>

<!-- Связь метки с элементом -->
<TextBlock x:Name="EmailLabel" Text="Email:"/>
<TextBox AutomationProperties.LabeledBy="{Binding ElementName=EmailLabel}"/>

<!-- Tab order — порядок клавиатурной навигации -->
<TextBox TabIndex="1" PlaceholderText="Имя"/>
<TextBox TabIndex="2" PlaceholderText="Email"/>
<Button TabIndex="3" Content="Отправить"/>

<!-- Live region — объявляет изменения -->
<TextBlock AutomationProperties.LiveSetting="Polite"
           Text="{Binding StatusMessage}"/>
```

### High Contrast

```xml
<!-- ThemeResource автоматически работает с High Contrast -->
<TextBlock Foreground="{ThemeResource SystemControlForegroundBaseHighBrush}"/>
<!-- Не используй жёстко заданные цвета для текста! -->
```

---

## Мини-упражнения
1. **⭐** Добавь AutomationProperties.Name ко всем элементам без текста.
2. **⭐** Настрой TabIndex для формы.

## Что дальше
Дальше — **C# Markup** (10.13).
