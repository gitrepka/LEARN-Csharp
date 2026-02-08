# Тема 13.1: HttpClient

## Что ты узнаешь
- HttpClient: GET, POST, PUT, DELETE
- IHttpClientFactory — правильное создание
- Сериализация/десериализация ответов
- Retry, timeout, cancellation

---

## Объяснение

```csharp
// ❌ НЕ создавай HttpClient каждый раз!
// using var client = new HttpClient(); // socket exhaustion!

// ✅ Через DI (IHttpClientFactory)
services.AddHttpClient<ApiService>(client =>
{
    client.BaseAddress = new Uri("https://api.example.com/");
    client.Timeout = TimeSpan.FromSeconds(30);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

class ApiService
{
    private readonly HttpClient _client;
    public ApiService(HttpClient client) => _client = client;

    // GET
    public async Task<List<Product>?> GetProductsAsync(CancellationToken ct = default)
    {
        var response = await _client.GetAsync("products", ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<Product>>(ct);
    }

    // POST
    public async Task<Product?> CreateProductAsync(Product product, CancellationToken ct = default)
    {
        var response = await _client.PostAsJsonAsync("products", product, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Product>(ct);
    }

    // Загрузка файла
    public async Task DownloadFileAsync(string url, string path, CancellationToken ct = default)
    {
        using var response = await _client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, ct);
        response.EnsureSuccessStatusCode();
        await using var stream = await response.Content.ReadAsStreamAsync(ct);
        await using var file = File.Create(path);
        await stream.CopyToAsync(file, ct);
    }
}
```

---

## Мини-упражнения
1. **⭐** GET-запрос к публичному API, десериализуй JSON.
2. **⭐** POST-запрос с телом.
3. **⭐⭐** Загрузка файла с прогрессом.

## Что дальше
Дальше — **REST API** (13.2).
