# Тема 17.3: NuGet — пакеты

## Что ты узнаешь
- NuGet: установка и использование пакетов
- Создание своего NuGet-пакета
- SemVer (семантическое версионирование)
- Публикация на nuget.org

---

## Объяснение

### ЗАЧЕМ?

NuGet — менеджер пакетов для .NET. Вместо копирования кода — `dotnet add package` и готово. 400,000+ пакетов: от JSON-сериализации до game engine.

### Использование пакетов

```bash
# Установка
dotnet add package Newtonsoft.Json
dotnet add package NSubstitute --version 5.1.0

# Удаление
dotnet remove package Newtonsoft.Json

# Список установленных
dotnet list package

# Обновление
dotnet add package NSubstitute  # без версии = последняя
```

### Популярные пакеты

```
Общие:
- Newtonsoft.Json          — JSON (если System.Text.Json не хватает)
- Serilog                  — логирование
- Polly                    — retry/circuit breaker
- FluentValidation         — валидация
- AutoMapper               — маппинг объектов
- MediatR                  — CQRS/Mediator паттерн
- BenchmarkDotNet          — бенчмарки

Тестирование:
- xUnit / NUnit            — тестовые фреймворки
- NSubstitute / Moq        — моки
- FluentAssertions         — читаемые Assert-ы
- Bogus                    — генерация тестовых данных

Uno Platform:
- CommunityToolkit.Mvvm    — MVVM
- Uno.Extensions.*         — расширения

Godot:
- gdUnit4.api              — тесты в Godot
```

### SemVer — семантическое версионирование

```
MAJOR.MINOR.PATCH

1.0.0 → 1.0.1   PATCH: баг-фикс, обратно совместимый
1.0.1 → 1.1.0   MINOR: новая фича, обратно совместимый
1.1.0 → 2.0.0   MAJOR: breaking change, НЕ совместимый

Pre-release:
1.0.0-alpha.1    — альфа
1.0.0-beta.2     — бета
1.0.0-rc.1       — release candidate

В .csproj:
<PackageReference Include="MyLib" Version="1.2.3" />
<PackageReference Include="MyLib" Version="1.*" />   <!-- любой 1.x.x -->
```

### Создание своего пакета

```xml
<!-- MyLibrary.csproj -->
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>

    <!-- NuGet metadata -->
    <PackageId>MyAwesomeLibrary</PackageId>
    <Version>1.0.0</Version>
    <Authors>YourName</Authors>
    <Description>Полезные утилиты для C# проектов</Description>
    <PackageTags>utilities;helpers;csharp</PackageTags>
    <PackageLicenseExpression>MIT</PackageLicenseExpression>
    <PackageReadmeFile>README.md</PackageReadmeFile>
    <RepositoryUrl>https://github.com/you/MyAwesomeLibrary</RepositoryUrl>
  </PropertyGroup>

  <ItemGroup>
    <None Include="README.md" Pack="true" PackagePath="\" />
  </ItemGroup>
</Project>
```

```csharp
// Код библиотеки
namespace MyAwesomeLibrary;

public static class StringExtensions
{
    public static string Truncate(this string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return value.Length <= maxLength ? value : value[..maxLength] + "...";
    }

    public static string ToSlug(this string value)
    {
        return value.ToLowerInvariant()
            .Replace(' ', '-')
            .Replace("--", "-");
    }
}
```

```bash
# Сборка пакета
dotnet pack -c Release
# Создаёт bin/Release/MyAwesomeLibrary.1.0.0.nupkg

# Публикация на nuget.org
dotnet nuget push bin/Release/MyAwesomeLibrary.1.0.0.nupkg \
    --api-key YOUR_API_KEY \
    --source https://api.nuget.org/v3/index.json

# Локальный feed (для тестирования)
dotnet nuget add source ./local-packages --name local
dotnet pack -c Release -o ./local-packages
```

### Публикация через GitHub Actions

```yaml
# .github/workflows/publish.yml
name: Publish NuGet

on:
  push:
    tags: ['v*']

jobs:
  publish:
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v4
    - uses: actions/setup-dotnet@v4
      with:
        dotnet-version: '10.0.x'

    - name: Pack
      run: dotnet pack -c Release

    - name: Publish to NuGet
      run: dotnet nuget push **/*.nupkg
           --api-key ${{ secrets.NUGET_API_KEY }}
           --source https://api.nuget.org/v3/index.json
```

---

## Мини-упражнения

1. **⭐** Установи 3 NuGet-пакета, используй их в коде.
2. **⭐⭐** Создай библиотеку утилит (3-5 extension methods), упакуй в .nupkg.
3. **⭐⭐** Подключи свой пакет через локальный feed.

## Что дальше
Модуль 17 завершён! Дальше — **Модуль 18: ASP.NET Core**.
