# Тема 8.5: MVVM (Model-View-ViewModel)

## Что ты узнаешь
- MVVM: Model, View, ViewModel — зачем разделять
- INotifyPropertyChanged — уведомление UI об изменениях
- Data Binding — привязка данных
- CommunityToolkit.Mvvm — автоматизация
- Commands — IRelayCommand

---

## Объяснение

### ЗАЧЕМ?
Без MVVM: код UI (кнопки, списки) смешан с логикой (расчёты, данные). Нельзя тестировать без UI. MVVM **разделяет** UI и логику.

### Три слоя MVVM

```
┌─────────────────────────────────────────────────┐
│  VIEW (XAML)           — что видит пользователь │
│  TextBlock, Button,    — никакой логики!        │
│  ListView              ─── Data Binding ───┐    │
├────────────────────────────────────────────┤    │
│  VIEWMODEL (C#)        — логика UI         │◄───┘
│  Properties, Commands  — что показывать,   │
│  INotifyPropertyChanged  как реагировать   │
├────────────────────────────────────────────┤
│  MODEL (C#)            — данные и бизнес-  │
│  Player, Order,          логика            │
│  Services              — ничего про UI     │
└─────────────────────────────────────────────┘
```

### INotifyPropertyChanged — ручной способ

```csharp
using System.ComponentModel;

class PlayerViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private string _name = "";
    public string Name
    {
        get => _name;
        set
        {
            if (_name != value)
            {
                _name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            }
        }
    }

    private int _health = 100;
    public int Health
    {
        get => _health;
        set
        {
            if (_health != value)
            {
                _health = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Health)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HealthText)));
            }
        }
    }

    public string HealthText => $"HP: {Health}/100";
}
```

### CommunityToolkit.Mvvm — автоматизация!

```csharp
// dotnet add package CommunityToolkit.Mvvm
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

partial class PlayerViewModel : ObservableObject
{
    [ObservableProperty]
    private string _name = "";
    // Автоматически создаёт:
    // public string Name { get; set; } + PropertyChanged

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HealthText))]
    private int _health = 100;

    public string HealthText => $"HP: {Health}/100";

    [RelayCommand]
    private void TakeDamage(int amount)
    {
        Health = Math.Max(0, Health - amount);
    }

    [RelayCommand]
    private void Heal()
    {
        Health = 100;
    }

    [RelayCommand(CanExecute = nameof(CanAttack))]
    private void Attack()
    {
        // атака
    }

    private bool CanAttack() => Health > 0;
}
```

### Привязка в XAML (Uno Platform)

```xml
<Page x:Class="MyApp.PlayerPage"
      xmlns:vm="using:MyApp.ViewModels">
    <Page.DataContext>
        <vm:PlayerViewModel />
    </Page.DataContext>

    <StackPanel Padding="20">
        <!-- OneWay: ViewModel → UI -->
        <TextBlock Text="{Binding Name}" FontSize="24"/>
        <TextBlock Text="{Binding HealthText}" />

        <!-- TwoWay: UI ↔ ViewModel -->
        <TextBox Text="{Binding Name, Mode=TwoWay}" />

        <!-- Command: кнопка → метод ViewModel -->
        <Button Content="Получить урон"
                Command="{Binding TakeDamageCommand}"
                CommandParameter="10" />

        <Button Content="Лечение"
                Command="{Binding HealCommand}" />

        <!-- ProgressBar привязан к Health -->
        <ProgressBar Value="{Binding Health}" Maximum="100" />
    </StackPanel>
</Page>
```

### Messenger — обмен между ViewModels

```csharp
using CommunityToolkit.Mvvm.Messaging;

// Сообщение
record PlayerDiedMessage(string PlayerName);

// Отправитель
WeakReferenceMessenger.Default.Send(new PlayerDiedMessage("Алиса"));

// Получатель (в другой ViewModel)
WeakReferenceMessenger.Default.Register<PlayerDiedMessage>(this, (recipient, message) =>
{
    ShowGameOver(message.PlayerName);
});
```

---

## Мини-упражнения
1. **⭐** Создай ViewModel с [ObservableProperty] и привяжи к TextBlock.
2. **⭐** Создай [RelayCommand] для кнопки.
3. **⭐⭐** Используй Messenger для общения между двумя ViewModels.

---

## Что дальше
Дальше — **Clean Architecture** (8.6).
