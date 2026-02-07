# Тема 16.1: Профилирование

## Что ты узнаешь
- Зачем профилировать перед оптимизацией
- BenchmarkDotNet — точные замеры производительности
- Visual Studio Profiler / dotTrace
- dotnet-counters, dotnet-trace (CLI)
- Godot Profiler

---

## Объяснение

### ЗАЧЕМ?

> "Premature optimization is the root of all evil" — Дональд Кнут

Правило: **сначала измерь, потом оптимизируй**. Без профайлера ты угадываешь, где проблема. С профайлером — видишь точно.

### BenchmarkDotNet — микробенчмарки

```csharp
// dotnet add package BenchmarkDotNet

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

[MemoryDiagnoser] // показывает аллокации памяти
public class StringBenchmarks
{
    private readonly string[] _words = Enumerable.Range(0, 1000)
        .Select(i => $"word{i}").ToArray();

    [Benchmark(Baseline = true)]
    public string ConcatWithPlus()
    {
        string result = "";
        foreach (var word in _words)
            result += word + " "; // ❌ O(n²) аллокаций!
        return result;
    }

    [Benchmark]
    public string ConcatWithStringBuilder()
    {
        var sb = new StringBuilder();
        foreach (var word in _words)
            sb.Append(word).Append(' ');
        return sb.ToString();
    }

    [Benchmark]
    public string ConcatWithJoin()
    {
        return string.Join(' ', _words);
    }
}

// Запуск
// BenchmarkRunner.Run<StringBenchmarks>();
// dotnet run -c Release

// Результат:
// |              Method |        Mean |  Allocated |
// |-------------------- |------------ |----------- |
// |     ConcatWithPlus  | 1,234.56 μs | 4,567 KB   | ← ужас
// | ConcatWithSB        |    12.34 μs |    16 KB   | ← ✅
// |     ConcatWithJoin  |     8.91 μs |    12 KB   | ← ✅ лучший
```

### Что замерять

```csharp
[MemoryDiagnoser]
public class CollectionBenchmarks
{
    private readonly List<int> _list = Enumerable.Range(0, 10_000).ToList();

    [Benchmark]
    public bool List_Contains() => _list.Contains(9999); // O(n)

    [Benchmark]
    public bool HashSet_Contains()
    {
        var set = _list.ToHashSet();
        return set.Contains(9999); // O(1)
    }

    [Benchmark]
    public int Linq_Where_Count()
        => _list.Where(x => x > 5000).Count();

    [Benchmark]
    public int Linq_Count_Predicate()
        => _list.Count(x => x > 5000); // быстрее — нет промежуточного IEnumerable
}
```

### Visual Studio / Rider Profiler

```
В Visual Studio:
Debug → Performance Profiler → выбери:
  ✅ CPU Usage     — какие методы тратят больше всего CPU
  ✅ Memory Usage  — аллокации, утечки
  ✅ .NET Counters — GC collections, thread pool
  ✅ Database       — медленные запросы

В Rider:
Run → Profile → выбери DotTrace (CPU) или DotMemory (память)

Горячие пути (Hot Paths):
- Красные строки = самые медленные
- Фокусируйся на том, что вызывается ЧАСТО
- Не оптимизируй то, что вызывается 1 раз
```

### dotnet CLI инструменты

```bash
# Установка
dotnet tool install -g dotnet-counters
dotnet tool install -g dotnet-trace

# Счётчики в реальном времени
dotnet-counters monitor --process-id 12345
# Показывает: GC heap size, Gen 0/1/2 collections, Thread Pool Queue

# Запись трейса
dotnet-trace collect --process-id 12345
# Создаёт .nettrace файл → открывай в VS или PerfView
```

### Godot Profiler

```
Debugger → Profiler (вкладка в нижней панели)

Показывает:
- FPS и время кадра
- Время каждого _Process / _PhysicsProcess
- Idle vs Physics время
- Самые "дорогие" ноды

Мониторы → включи:
- FPS
- Physics Process
- Static Memory
- Object Count

В коде:
GD.Print(Performance.GetMonitor(Performance.Monitor.TimeFps));
GD.Print(Performance.GetMonitor(Performance.Monitor.ObjectCount));
```

### Правила профилирования

```
1. Профилируй в RELEASE, не в Debug
   Debug медленнее из-за отсутствия оптимизаций

2. Профилируй реальные сценарии
   Не синтетические тесты, а реальное использование

3. Фокусируйся на Hot Paths
   80% времени тратится в 20% кода

4. Измеряй ДО и ПОСЛЕ оптимизации
   Без замера "до" — не узнаешь, помогло ли

5. Аллокации = враг
   Каждая аллокация → нагрузка на GC → фризы в играх
```

---

## Мини-упражнения

1. **⭐** BenchmarkDotNet: сравни `for` vs `foreach` vs LINQ `.Sum()` на массиве 100K int.
2. **⭐⭐** Профилируй приложение в VS: найди метод, который тратит больше всего CPU.
3. **⭐⭐** Godot: включи Profiler, найди самый "дорогой" узел в сцене.

## Что дальше
Дальше — **оптимизации C#** (16.2).
