# Тема 17.2: GitHub Actions — CI/CD

## Что ты узнаешь
- Что такое CI/CD и зачем
- GitHub Actions: workflow, jobs, steps
- Автосборка .NET проекта
- Автозапуск тестов при Pull Request
- Экспорт Godot-проекта

---

## Объяснение

### ЗАЧЕМ?

CI/CD — **Continuous Integration / Continuous Delivery**.

```
Без CI/CD:
1. Написал код
2. Забыл запустить тесты
3. Push в main
4. Всё сломалось
5. Ищешь баг час

С CI/CD:
1. Написал код
2. Push → GitHub Actions автоматически:
   - Собирает проект
   - Запускает тесты
   - Если что-то сломано → ❌ тебе письмо
3. Баг найден через 2 минуты
```

### Первый workflow

```yaml
# .github/workflows/build.yml
name: Build and Test

on:
  push:
    branches: [main]
  pull_request:
    branches: [main]

jobs:
  build:
    runs-on: ubuntu-latest  # бесплатная виртуалка

    steps:
    - uses: actions/checkout@v4  # скачать код

    - name: Setup .NET
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: '10.0.x'

    - name: Restore dependencies
      run: dotnet restore

    - name: Build
      run: dotnet build --no-restore --configuration Release

    - name: Test
      run: dotnet test --no-build --configuration Release --verbosity normal
```

### Как это работает

```
1. Ты push'ишь код → GitHub видит .github/workflows/*.yml
2. GitHub запускает виртуальную машину (ubuntu/windows/macos)
3. Выполняет шаги (steps) по порядку
4. Если любой шаг падает → ❌ (красная галка на PR)
5. Если всё OK → ✅ (зелёная галка)

В PR видно:
  ✅ Build and Test — All checks passed
  или
  ❌ Build and Test — Test failed: 2 tests failed
```

### Матрица — тестирование на разных ОС

```yaml
jobs:
  build:
    strategy:
      matrix:
        os: [ubuntu-latest, windows-latest, macos-latest]
        dotnet: ['9.0.x', '10.0.x']

    runs-on: ${{ matrix.os }}

    steps:
    - uses: actions/checkout@v4
    - uses: actions/setup-dotnet@v4
      with:
        dotnet-version: ${{ matrix.dotnet }}
    - run: dotnet build
    - run: dotnet test
```

### Автотесты при PR

```yaml
# .github/workflows/pr-check.yml
name: PR Check

on:
  pull_request:
    branches: [main]

jobs:
  test:
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v4
    - uses: actions/setup-dotnet@v4
      with:
        dotnet-version: '10.0.x'

    - name: Run Tests with Coverage
      run: dotnet test --collect:"XPlat Code Coverage"

    - name: Check code style
      run: dotnet format --verify-no-changes
```

### Экспорт Godot-проекта

```yaml
# .github/workflows/godot-export.yml
name: Godot Export

on:
  push:
    tags: ['v*']  # Запускается при создании тега v1.0.0

jobs:
  export:
    runs-on: ubuntu-latest

    steps:
    - uses: actions/checkout@v4
      with:
        lfs: true  # скачать LFS файлы

    - name: Setup .NET
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: '10.0.x'

    - name: Godot Export (Windows)
      uses: firebelley/godot-export@v6
      with:
        godot_executable_download_url: "https://github.com/godotengine/godot/releases/download/4.6-stable/Godot_v4.6-stable_mono_linux_x86_64.zip"
        relative_export_path: "exports"
        export_preset: "Windows Desktop"

    - name: Upload Artifact
      uses: actions/upload-artifact@v4
      with:
        name: windows-build
        path: exports/

    # Для создания GitHub Release:
    - name: Create Release
      uses: softprops/action-gh-release@v2
      with:
        files: exports/*
      env:
        GITHUB_TOKEN: ${{ secrets.GITHUB_TOKEN }}
```

### Секреты (Secrets)

```
Никогда не хардкодь ключи в workflow!

Settings → Secrets and Variables → Actions → New repository secret

NUGET_API_KEY    = "abc123..."
DEPLOY_TOKEN     = "xyz789..."

Использование в workflow:
  env:
    API_KEY: ${{ secrets.NUGET_API_KEY }}
```

### Workflow для Uno Platform

```yaml
# .github/workflows/uno-build.yml
name: Uno Platform Build

on: [push, pull_request]

jobs:
  build-wasm:
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v4
    - uses: actions/setup-dotnet@v4
      with:
        dotnet-version: '10.0.x'

    - name: Install Uno workload
      run: dotnet workload install wasm-tools

    - name: Build WASM
      run: dotnet build MyApp.Wasm -c Release

    - name: Deploy to GitHub Pages
      uses: peaceiris/actions-gh-pages@v4
      with:
        github_token: ${{ secrets.GITHUB_TOKEN }}
        publish_dir: ./MyApp.Wasm/bin/Release/net10.0/publish/wwwroot
```

---

## Мини-упражнения

1. **⭐** Создай workflow, который собирает .NET-проект при каждом push.
2. **⭐** Добавь шаг с запуском тестов.
3. **⭐⭐** Настрой экспорт Godot-проекта при создании тега.

## Что дальше
Дальше — **NuGet** (17.3).
