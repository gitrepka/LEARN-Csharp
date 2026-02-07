# Тема 10.9: Работа с данными

## Что ты узнаешь
- HttpClient + REST API
- SQLite для локального хранения
- LocalSettings / LocalFolder
- Clipboard, Drag & Drop, FilePicker

---

## Объяснение

### HttpClient — работа с API

```csharp
class ApiService
{
    private readonly HttpClient _client;

    public ApiService(HttpClient client)
    {
        _client = client;
        _client.BaseAddress = new Uri("https://api.example.com/");
    }

    public async Task<List<Product>?> GetProductsAsync()
    {
        var response = await _client.GetAsync("products");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<Product>>();
    }

    public async Task CreateProductAsync(Product product)
    {
        var response = await _client.PostAsJsonAsync("products", product);
        response.EnsureSuccessStatusCode();
    }
}
```

### LocalSettings — быстрые настройки

```csharp
// Сохранение настроек (key-value, маленькие данные)
var settings = Windows.Storage.ApplicationData.Current.LocalSettings;
settings.Values["Theme"] = "Dark";
settings.Values["Volume"] = 80;

// Чтение
string theme = settings.Values["Theme"] as string ?? "Light";
int volume = (int)(settings.Values["Volume"] ?? 100);
```

### LocalFolder — файлы приложения

```csharp
var folder = Windows.Storage.ApplicationData.Current.LocalFolder;
var file = await folder.CreateFileAsync("data.json", CreationCollisionOption.ReplaceExisting);
await FileIO.WriteTextAsync(file, jsonString);

// Чтение
var readFile = await folder.GetFileAsync("data.json");
string content = await FileIO.ReadTextAsync(readFile);
```

### FilePicker

```csharp
var picker = new FileOpenPicker();
picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
picker.FileTypeFilter.Add(".json");
picker.FileTypeFilter.Add(".txt");

var file = await picker.PickSingleFileAsync();
if (file != null)
{
    string content = await FileIO.ReadTextAsync(file);
}
```

---

## Мини-упражнения
1. **⭐** HttpClient: GET запрос к публичному API.
2. **⭐** Сохранение/чтение настроек через LocalSettings.
3. **⭐⭐** FileOpenPicker для загрузки JSON-файла.

## Что дальше
Дальше — **адаптивный UI** (10.10).
