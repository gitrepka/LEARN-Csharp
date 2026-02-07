# Тема 19.3: System.IO.Pipelines

## Что ты узнаешь
- System.IO.Pipelines — высокопроизводительный IO
- PipeReader, PipeWriter
- Когда использовать вместо Stream
- ReadOnlySequence<T>

---

## Объяснение

### ЗАЧЕМ?

Обычный Stream — копирование буферов. Pipelines — **zero-copy**, данные передаются без лишних копирований. Kestrel (веб-сервер ASP.NET Core) использует Pipelines. Это **продвинутая тема** — нужна для высоконагруженных систем.

### Проблема со Stream

```csharp
// ❌ Stream — нужно управлять буфером самому
byte[] buffer = new byte[4096];
while (true)
{
    int bytesRead = await stream.ReadAsync(buffer);
    if (bytesRead == 0) break;

    // Проблема 1: сообщение может быть разбито на 2 чтения
    // Проблема 2: нужно самому склеивать фрагменты
    // Проблема 3: если буфер мал — много чтений, если велик — расход памяти
    ProcessData(buffer.AsSpan(0, bytesRead));
}
```

### Pipelines — решение

```csharp
using System.IO.Pipelines;

// Pipe = PipeWriter (запись) + PipeReader (чтение)
// Данные передаются через общую память без копирования

var pipe = new Pipe();

// Writer — заполняет
async Task FillPipeAsync(Stream stream, PipeWriter writer)
{
    while (true)
    {
        // Получаем буфер от Pipe (без аллокации!)
        Memory<byte> memory = writer.GetMemory(4096);
        int bytesRead = await stream.ReadAsync(memory);

        if (bytesRead == 0) break;

        // Сообщаем, сколько записали
        writer.Advance(bytesRead);

        // Флаш — данные доступны читателю
        FlushResult result = await writer.FlushAsync();
        if (result.IsCompleted) break;
    }

    await writer.CompleteAsync();
}

// Reader — обрабатывает
async Task ReadPipeAsync(PipeReader reader)
{
    while (true)
    {
        ReadResult result = await reader.ReadAsync();
        ReadOnlySequence<byte> buffer = result.Buffer;

        // Ищем разделитель (например, \n для строк)
        while (TryReadLine(ref buffer, out ReadOnlySequence<byte> line))
        {
            ProcessLine(line);
        }

        // Сообщаем, сколько обработали
        reader.AdvanceTo(buffer.Start, buffer.End);

        if (result.IsCompleted) break;
    }

    await reader.CompleteAsync();
}

bool TryReadLine(ref ReadOnlySequence<byte> buffer, out ReadOnlySequence<byte> line)
{
    var position = buffer.PositionOf((byte)'\n');
    if (position == null)
    {
        line = default;
        return false;
    }

    line = buffer.Slice(0, position.Value);
    buffer = buffer.Slice(buffer.GetPosition(1, position.Value));
    return true;
}
```

### Когда использовать

```
Pipelines:
✅ Сетевые протоколы (TCP, WebSocket)
✅ Парсинг потоков данных (логи, CSV)
✅ Высоконагруженные серверы
✅ Когда важен zero-copy

Stream:
✅ Файловый IO (чтение/запись файлов)
✅ HTTP запросы/ответы
✅ Простые сценарии
✅ Когда производительность не критична

Для тебя сейчас:
→ Stream достаточно для 99% задач
→ Pipelines — знай что есть, используй когда нужна
  максимальная производительность сетевого IO
```

---

## Мини-упражнения

1. **⭐** Прочитай документацию по System.IO.Pipelines (learn.microsoft.com).
2. **⭐⭐** Напиши простой TCP-сервер с PipeReader, который разбивает входящие данные по строкам.

## Что дальше
Дальше — **Compression** (19.4).
