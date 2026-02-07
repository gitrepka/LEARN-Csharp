# Тема 9.3: DI в Uno Platform

## Что ты узнаешь
- Настройка DI в Uno-приложении
- Регистрация ViewModels и Services
- Навигация с DI

---

## Объяснение

### ЗАЧЕМ?
Uno Platform использует DI для всего: ViewModels, сервисы данных, навигация, HTTP-клиенты. Правильная настройка DI — основа чистого Uno-приложения.

### Настройка в App.cs

```csharp
// App.xaml.cs (или Program.cs в Uno 6+)
public class App : Application
{
    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        var builder = this.CreateBuilder(args)
            .Configure(host => host
                .ConfigureServices((context, services) =>
                {
                    // Сервисы
                    services.AddSingleton<ISettingsService, SettingsService>();
                    services.AddTransient<IDataService, ApiDataService>();
                    services.AddSingleton<HttpClient>();

                    // ViewModels
                    services.AddTransient<MainViewModel>();
                    services.AddTransient<DetailsViewModel>();
                    services.AddTransient<SettingsViewModel>();
                })
                .UseNavigation(RegisterRoutes)
            );

        MainWindow = builder.Window;
        Host = builder.Build();
    }

    private static void RegisterRoutes(IViewRegistry views, IRouteRegistry routes)
    {
        views.Register(
            new ViewMap<MainPage, MainViewModel>(),
            new ViewMap<DetailsPage, DetailsViewModel>()
        );

        routes.Register(
            new RouteMap("Main", View: views.FindByViewModel<MainViewModel>()),
            new RouteMap("Details", View: views.FindByViewModel<DetailsViewModel>())
        );
    }
}
```

### ViewModel с внедрёнными сервисами

```csharp
partial class MainViewModel : ObservableObject
{
    private readonly IDataService _dataService;
    private readonly INavigator _navigator;

    public MainViewModel(IDataService dataService, INavigator navigator)
    {
        _dataService = dataService;
        _navigator = navigator;
    }

    [ObservableProperty]
    private ObservableCollection<ItemModel> _items = [];

    [RelayCommand]
    private async Task LoadItems()
    {
        var data = await _dataService.GetItemsAsync();
        Items = new ObservableCollection<ItemModel>(data);
    }

    [RelayCommand]
    private async Task GoToDetails(ItemModel item)
    {
        await _navigator.NavigateViewModelAsync<DetailsViewModel>(this,
            data: new { Item = item });
    }
}
```

### INavigator — навигация через DI

```csharp
// Навигация в Uno.Extensions.Navigation
await _navigator.NavigateViewModelAsync<DetailsViewModel>(this);
await _navigator.NavigateBackAsync(this);
await _navigator.NavigateRouteAsync(this, "Details", data: new { Id = 42 });
```

---

## Мини-упражнения
1. **⭐** Настрой DI в Uno-приложении, зарегистрируй ViewModel.
2. **⭐** Внедри HttpClient через конструктор ViewModel.
3. **⭐⭐** Настрой навигацию через Uno.Extensions.Navigation.

---

## Что дальше
Дальше — **DI в Godot** (9.4).
