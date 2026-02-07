# Тема 19.4: Сжатие — System.IO.Compression

## Что ты узнаешь
- GZip, Deflate, Brotli — алгоритмы сжатия
- ZipArchive — работа с ZIP-файлами
- Сжатие HTTP-ответов
- Сжатие сохранений в играх

---

## Объяснение

### ЗАЧЕМ?

Меньше данных = быстрее передача по сети + меньше место на диске. JSON-ответ API 100 KB → после GZip ~15 KB. Сохранение игры 5 MB → после сжатия ~500 KB.

### GZip — самый распространённый

```csharp
using System.IO.Compression;

public static class CompressionHelper
{
    // Сжать байты
    public static byte[] Compress(byte[] data)
    {
        using var output = new MemoryStream();
        using (var gzip = new GZipStream(output, CompressionLevel.Optimal))
        {
            gzip.Write(data);
        }
        return output.ToArray();
    }

    // Распаковать
    public static byte[] Decompress(byte[] compressed)
    {
        using var input = new MemoryStream(compressed);
        using var gzip = new GZipStream(input, CompressionMode.Decompress);
        using var output = new MemoryStream();
        gzip.CopyTo(output);
        return output.ToArray();
    }

    // Сжать строку
    public static byte[] CompressString(string text)
    {
        return Compress(Encoding.UTF8.GetBytes(text));
    }

    // Распаковать строку
    public static string DecompressString(byte[] compressed)
    {
        return Encoding.UTF8.GetString(Decompress(compressed));
    }
}

// Использование
string json = JsonSerializer.Serialize(bigObject);
byte[] compressed = CompressionHelper.CompressString(json);
// json: 100,000 bytes → compressed: ~15,000 bytes

string restored = CompressionHelper.DecompressString(compressed);
```

### Brotli — лучше сжатие (для веба)

```csharp
// Brotli сжимает лучше GZip, но медленнее
// Идеален для статических ресурсов (CSS, JS, HTML)

public static byte[] CompressBrotli(byte[] data)
{
    using var output = new MemoryStream();
    using (var brotli = new BrotliStream(output, CompressionLevel.Optimal))
    {
        brotli.Write(data);
    }
    return output.ToArray();
}

// Сравнение (типичный JSON):
// Оригинал:   100 KB
// GZip:        ~15 KB
// Brotli:      ~12 KB (лучше, но медленнее сжатие)
```

### ZipArchive — работа с ZIP

```csharp
// Создание ZIP-файла
public static void CreateZip(string zipPath, string sourceFolder)
{
    ZipFile.CreateFromDirectory(sourceFolder, zipPath, CompressionLevel.Optimal, false);
}

// Извлечение
public static void ExtractZip(string zipPath, string destFolder)
{
    ZipFile.ExtractToDirectory(zipPath, destFolder);
}

// Работа с отдельными файлами
public static void AddFileToZip(string zipPath, string fileName, byte[] content)
{
    using var zip = ZipFile.Open(zipPath, ZipArchiveMode.Update);
    var entry = zip.CreateEntry(fileName, CompressionLevel.Optimal);
    using var stream = entry.Open();
    stream.Write(content);
}

// Чтение файла из ZIP
public static byte[] ReadFromZip(string zipPath, string fileName)
{
    using var zip = ZipFile.OpenRead(zipPath);
    var entry = zip.GetEntry(fileName)
        ?? throw new FileNotFoundException($"{fileName} не найден в архиве");
    using var stream = entry.Open();
    using var ms = new MemoryStream();
    stream.CopyTo(ms);
    return ms.ToArray();
}
```

### Сжатие HTTP в ASP.NET Core

```csharp
// Сервер автоматически сжимает ответы
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});

builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest;
});

app.UseResponseCompression();

// Клиент отправляет: Accept-Encoding: gzip, br
// Сервер отвечает:   Content-Encoding: br (если поддерживает)
```

### Сжатие сохранений в Godot

```csharp
public partial class SaveManager : Node
{
    public void SaveCompressed(GameData data)
    {
        string json = JsonSerializer.Serialize(data);
        byte[] compressed = CompressionHelper.CompressString(json);

        string path = ProjectSettings.GlobalizePath("user://save.gz");
        File.WriteAllBytes(path, compressed);

        GD.Print($"Сохранено: {json.Length} → {compressed.Length} байт");
    }

    public GameData? LoadCompressed()
    {
        string path = ProjectSettings.GlobalizePath("user://save.gz");
        if (!File.Exists(path)) return null;

        byte[] compressed = File.ReadAllBytes(path);
        string json = CompressionHelper.DecompressString(compressed);
        return JsonSerializer.Deserialize<GameData>(json);
    }
}
```

---

## Мини-упражнения

1. **⭐** Сожми JSON-строку через GZip. Выведи размер до и после.
2. **⭐** Создай ZIP-архив из папки, распакуй в другую.
3. **⭐⭐** Сжатое сохранение: GameData → JSON → GZip → файл → обратно.

## Что дальше
Дальше — **C# 13-14 фичи** (19.5).
