# Тема 10.14: Платформо-специфичный код

## Что ты узнаешь
- Условная компиляция: #if __ANDROID__, #if WINDOWS
- Partial classes для платформ
- Доступ к нативным API

---

## Объяснение

```csharp
// Условная компиляция
public void ShareFile(string path)
{
#if __ANDROID__
    // Android-специфичный код
    var intent = new Android.Content.Intent(Android.Content.Intent.ActionSend);
    // ...
#elif __IOS__
    // iOS-специфичный код
    var controller = new UIKit.UIActivityViewController(new[] { path }, null);
    // ...
#elif __WASM__
    // Web-специфичный код
    // JavaScript interop
#elif WINDOWS
    // Windows-специфичный код
    var file = await Windows.Storage.StorageFile.GetFileFromPathAsync(path);
    // ...
#endif
}
```

### Partial classes

```csharp
// Shared/Services/NativeService.cs
public partial class NativeService
{
    public partial string GetDeviceId();
}

// Platforms/Android/Services/NativeService.Android.cs
public partial class NativeService
{
    public partial string GetDeviceId()
    {
        return Android.Provider.Settings.Secure.GetString(
            Android.App.Application.Context.ContentResolver,
            Android.Provider.Settings.Secure.AndroidId);
    }
}

// Platforms/Windows/Services/NativeService.Windows.cs
public partial class NativeService
{
    public partial string GetDeviceId()
    {
        var token = Windows.System.Profile.HardwareIdentification.GetPackageSpecificToken(null);
        return Convert.ToBase64String(token.Id.ToArray());
    }
}
```

---

## Мини-упражнения
1. **⭐** Используй #if для разного поведения на Windows и WASM.
2. **⭐⭐** Partial class с платформо-специфичной реализацией.

## Что дальше
Дальше — **Uno.Extensions** (10.15).
