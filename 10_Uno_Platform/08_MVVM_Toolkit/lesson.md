# Тема 10.8: MVVM с CommunityToolkit

## Что ты узнаешь
- [ObservableProperty], [RelayCommand] — автоматизация
- ObservableValidator — валидация
- Messenger — обмен между ViewModels

---

## Объяснение

### Полный пример ViewModel

```csharp
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

public partial class ContactViewModel : ObservableValidator
{
    // Свойства с автоматическим уведомлением
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Имя обязательно")]
    [MinLength(2, ErrorMessage = "Минимум 2 символа")]
    private string _name = "";

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [EmailAddress(ErrorMessage = "Неверный email")]
    private string _email = "";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FullInfo))]
    private string _phone = "";

    [ObservableProperty]
    private bool _isLoading;

    // Вычисляемое свойство
    public string FullInfo => $"{Name} ({Email}) - {Phone}";

    // Команда (синхронная)
    [RelayCommand]
    private void Clear()
    {
        Name = "";
        Email = "";
        Phone = "";
    }

    // Async команда с loading
    [RelayCommand]
    private async Task SaveAsync()
    {
        ValidateAllProperties();
        if (HasErrors) return;

        IsLoading = true;
        try
        {
            await _contactService.SaveAsync(new Contact(Name, Email, Phone));
            WeakReferenceMessenger.Default.Send(new ContactSavedMessage(Name));
        }
        finally
        {
            IsLoading = false;
        }
    }

    // Команда с CanExecute
    [RelayCommand(CanExecute = nameof(CanDelete))]
    private void Delete() { /* ... */ }
    private bool CanDelete() => !string.IsNullOrEmpty(Name);
}
```

### Messenger

```csharp
// Определение сообщения
record ContactSavedMessage(string Name);
record ThemeChangedMessage(bool IsDark);

// Отправка
WeakReferenceMessenger.Default.Send(new ThemeChangedMessage(true));

// Приём (в другой ViewModel)
public MainViewModel()
{
    WeakReferenceMessenger.Default.Register<ThemeChangedMessage>(this, (r, m) =>
    {
        // Обновить тему
    });
}

// Обязательно отпишись при уничтожении!
WeakReferenceMessenger.Default.UnregisterAll(this);
```

---

## Мини-упражнения
1. **⭐** ViewModel с [ObservableProperty] и [RelayCommand].
2. **⭐⭐** Валидация с ObservableValidator и Data Annotations.
3. **⭐⭐** Messenger между двумя ViewModels.

## Что дальше
Дальше — **работа с данными** (10.9).
