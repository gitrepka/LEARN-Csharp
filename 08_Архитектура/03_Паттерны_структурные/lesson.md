# Тема 8.3: Структурные паттерны (Structural Patterns)

## Что ты узнаешь
- Adapter — совместимость несовместимых интерфейсов
- Decorator — добавление поведения оборачиванием
- Facade — упрощение сложной системы
- Composite — древовидные структуры
- Proxy — заместитель

---

## Объяснение

### Adapter — переходник

Когда у тебя есть класс с одним интерфейсом, а нужен другой.

```csharp
// Старая система логирования (чужой код, нельзя менять)
class OldLogger
{
    public void WriteLog(string level, string text) =>
        Console.WriteLine($"[{level}] {text}");
}

// Наш интерфейс
interface ILogger
{
    void Info(string message);
    void Error(string message);
}

// Adapter — оборачивает старый класс в новый интерфейс
class OldLoggerAdapter : ILogger
{
    private readonly OldLogger _oldLogger = new();

    public void Info(string message) => _oldLogger.WriteLog("INFO", message);
    public void Error(string message) => _oldLogger.WriteLog("ERROR", message);
}

// Использование — как будто OldLogger всегда был ILogger
ILogger logger = new OldLoggerAdapter();
logger.Info("Работает!");
```

### Decorator — матрёшка

Оборачивает объект, добавляя поведение. Как матрёшка — каждый слой добавляет что-то.

```csharp
interface IDataStream
{
    byte[] Read();
    void Write(byte[] data);
}

class FileStream : IDataStream
{
    public byte[] Read() => File.ReadAllBytes("data.bin");
    public void Write(byte[] data) => File.WriteAllBytes("data.bin", data);
}

// Decorator 1: сжатие
class CompressedStream : IDataStream
{
    private readonly IDataStream _inner;
    public CompressedStream(IDataStream inner) => _inner = inner;

    public byte[] Read() => Decompress(_inner.Read());
    public void Write(byte[] data) => _inner.Write(Compress(data));

    private byte[] Compress(byte[] data) { /* сжатие */ return data; }
    private byte[] Decompress(byte[] data) { /* распаковка */ return data; }
}

// Decorator 2: шифрование
class EncryptedStream : IDataStream
{
    private readonly IDataStream _inner;
    public EncryptedStream(IDataStream inner) => _inner = inner;

    public byte[] Read() => Decrypt(_inner.Read());
    public void Write(byte[] data) => _inner.Write(Encrypt(data));

    private byte[] Encrypt(byte[] data) { /* шифрование */ return data; }
    private byte[] Decrypt(byte[] data) { /* расшифровка */ return data; }
}

// Комбинирование — файл + сжатие + шифрование!
IDataStream stream = new EncryptedStream(
    new CompressedStream(
        new FileStream()
    )
);
stream.Write(data); // запись: шифрование → сжатие → файл
```

### Facade — единая точка входа

```csharp
// Сложная система со множеством подсистем
class AudioSystem { public void PlaySound(string name) { } }
class ParticleSystem { public void SpawnParticles(string type) { } }
class CameraSystem { public void Shake(float intensity) { } }
class UISystem { public void ShowDamageNumber(int amount) { } }

// Facade — упрощённый интерфейс
class CombatEffects
{
    private readonly AudioSystem _audio = new();
    private readonly ParticleSystem _particles = new();
    private readonly CameraSystem _camera = new();
    private readonly UISystem _ui = new();

    public void PlayHitEffect(int damage, bool isCritical)
    {
        _audio.PlaySound(isCritical ? "crit_hit" : "hit");
        _particles.SpawnParticles(isCritical ? "crit_sparks" : "sparks");
        _camera.Shake(isCritical ? 0.5f : 0.2f);
        _ui.ShowDamageNumber(damage);
    }
}

// Вместо 4 вызовов — один:
combatEffects.PlayHitEffect(50, true);
```

### Composite — дерево объектов

```csharp
interface IUIElement
{
    void Render();
}

class Button : IUIElement
{
    public string Text { get; set; } = "";
    public void Render() => Console.WriteLine($"  [Button: {Text}]");
}

class Label : IUIElement
{
    public string Text { get; set; } = "";
    public void Render() => Console.WriteLine($"  {Text}");
}

// Composite — контейнер, который тоже IUIElement
class Panel : IUIElement
{
    private readonly List<IUIElement> _children = [];
    public string Name { get; set; } = "";

    public void Add(IUIElement element) => _children.Add(element);

    public void Render()
    {
        Console.WriteLine($"[Panel: {Name}]");
        foreach (var child in _children)
            child.Render();
    }
}

// Использование — дерево UI
Panel root = new() { Name = "Main" };
root.Add(new Label { Text = "Привет!" });
Panel toolbar = new() { Name = "Toolbar" };
toolbar.Add(new Button { Text = "Save" });
toolbar.Add(new Button { Text = "Load" });
root.Add(toolbar);
root.Render();
```

---

## Мини-упражнения
1. **⭐** Adapter: оберни чужой класс в свой интерфейс.
2. **⭐⭐** Decorator: поток данных с шифрованием и сжатием.
3. **⭐** Facade: единый API для нескольких подсистем.

---

## Что дальше
Дальше — **поведенческие паттерны** (8.4).
