# Тема 10.15: Uno.Extensions

## Что ты узнаешь
- Uno.Extensions.Http — HTTP-клиенты
- Uno.Extensions.Serialization — сериализация
- Uno.Extensions.Logging — логирование
- Uno.Extensions.Configuration — конфигурация

---

## Объяснение

### Uno.Extensions — набор расширений для продуктивной разработки

```csharp
// В App.xaml.cs
protected override void OnLaunched(LaunchActivatedEventArgs args)
{
    var builder = this.CreateBuilder(args)
        .Configure(host => host
            .UseConfiguration()          // appsettings.json
            .UseLogging()                // Serilog/Microsoft.Logging
            .UseHttp()                   // HttpClient factory
            .UseSerialization()          // JSON сериализация
            .UseNavigation(RegisterRoutes)
            .ConfigureServices(services =>
            {
                services.AddRefitClient<IApiClient>();
            })
        );
}
```

### Http с Refit

```csharp
// Описание API через интерфейс
interface IApiClient
{
    [Get("/products")]
    Task<List<Product>> GetProductsAsync();

    [Get("/products/{id}")]
    Task<Product> GetProductAsync(int id);

    [Post("/products")]
    Task CreateProductAsync([Body] Product product);
}

// Используется через DI — Refit создаёт реализацию автоматически!
class ProductViewModel(IApiClient api)
{
    public async Task LoadAsync()
    {
        var products = await api.GetProductsAsync();
    }
}
```

---

## Контрольные вопросы перед Модулем 11
1. Чем {Binding} отличается от {x:Bind}?
2. Что такое DependencyProperty?
3. Как работает AdaptiveTrigger?
4. Зачем x:Uid для локализации?
5. Когда C# Markup vs XAML?

## Что дальше
Модуль 10 завершён! Дальше — **Модуль 11: Godot 4.6**.
