# Тема 5.1: Файловая система

## Что ты узнаешь
- File, FileInfo, Directory, DirectoryInfo, Path
- Чтение/запись: ReadAllText, WriteAllText, StreamReader, StreamWriter
- Потоки: Stream, MemoryStream, FileStream
- using / await using и IDisposable

---

## Объяснение

### ЗАЧЕМ?
Игра сохраняется в файл. Приложение читает конфигурацию. Логи пишутся на диск. Работа с файлами — базовый навык.

### Статические методы File

```csharp
// Чтение
string content = File.ReadAllText("data.txt");         // всё в одну строку
string[] lines = File.ReadAllLines("data.txt");        // массив строк
byte[] bytes = File.ReadAllBytes("image.png");         // байты

// Запись
File.WriteAllText("output.txt", "Hello, World!");      // перезаписывает
File.WriteAllLines("output.txt", ["Line 1", "Line 2"]);
File.WriteAllBytes("copy.png", bytes);

// Дописывание
File.AppendAllText("log.txt", "Новая запись\n");
File.AppendAllLines("log.txt", ["Строка 1", "Строка 2"]);

// Проверки
bool exists = File.Exists("data.txt");
File.Copy("source.txt", "dest.txt", overwrite: true);
File.Move("old.txt", "new.txt");
File.Delete("temp.txt");

// Асинхронные версии (рекомендуется!)
string text = await File.ReadAllTextAsync("data.txt");
await File.WriteAllTextAsync("output.txt", "Hello!");
```

### Path — работа с путями

```csharp
// НИКОГДА не склеивай пути строками! Используй Path.
string fullPath = Path.Combine("C:", "Users", "mydev", "file.txt");
// "C:\Users\mydev\file.txt"

string fileName = Path.GetFileName("C:/folder/file.txt");       // "file.txt"
string nameOnly = Path.GetFileNameWithoutExtension("file.txt"); // "file"
string ext = Path.GetExtension("file.txt");                     // ".txt"
string dir = Path.GetDirectoryName("C:/folder/file.txt")!;     // "C:\folder"

// Временные файлы
string tempFile = Path.GetTempFileName(); // создаёт файл и возвращает путь
string tempDir = Path.GetTempPath();       // папка для временных файлов
```

### Directory — папки

```csharp
// Создание
Directory.CreateDirectory("data/saves"); // создаёт всю цепочку

// Проверка
bool dirExists = Directory.Exists("data");

// Содержимое
string[] files = Directory.GetFiles("data", "*.json");            // файлы
string[] dirs = Directory.GetDirectories("data");                  // подпапки
string[] all = Directory.GetFiles("data", "*.*", SearchOption.AllDirectories); // рекурсивно

// Удаление
Directory.Delete("temp", recursive: true); // ⚠️ удаляет всё внутри!
```

---

### Потоки (Stream) — для больших файлов

File.ReadAllText загружает ВЕСЬ файл в память. Для больших файлов — используй потоки.

```csharp
// StreamReader — чтение текста построчно
using StreamReader reader = new("bigfile.txt");
string? line;
while ((line = reader.ReadLine()) is not null)
{
    Console.WriteLine(line);
}

// StreamWriter — запись
using StreamWriter writer = new("output.txt", append: false);
writer.WriteLine("Строка 1");
writer.WriteLine("Строка 2");
// writer автоматически закроется (using)

// Асинхронное чтение
using StreamReader asyncReader = new("bigfile.txt");
while ((line = await asyncReader.ReadLineAsync()) is not null)
{
    // обработка
}
```

### FileStream — низкоуровневый доступ

```csharp
// Чтение байтов
using FileStream fs = new("data.bin", FileMode.Open, FileAccess.Read);
byte[] buffer = new byte[1024];
int bytesRead = await fs.ReadAsync(buffer);

// Запись байтов
using FileStream output = new("out.bin", FileMode.Create, FileAccess.Write);
await output.WriteAsync(buffer.AsMemory(0, bytesRead));

// MemoryStream — поток в памяти (для тестов, промежуточной обработки)
using MemoryStream ms = new();
ms.Write([1, 2, 3, 4, 5]);
byte[] result = ms.ToArray();
```

### BinaryReader / BinaryWriter

```csharp
// Запись структурированных данных
using BinaryWriter bw = new(File.Open("save.dat", FileMode.Create));
bw.Write(42);           // int
bw.Write(3.14f);        // float
bw.Write("Player1");    // string
bw.Write(true);         // bool

// Чтение в том же порядке!
using BinaryReader br = new(File.Open("save.dat", FileMode.Open));
int score = br.ReadInt32();
float health = br.ReadSingle();
string name = br.ReadString();
bool alive = br.ReadBoolean();
```

---

### using и IDisposable

Потоки и файлы используют системные ресурсы. **Обязательно закрывай их!**

```csharp
// using-блок (классический)
using (StreamReader reader = new("file.txt"))
{
    string text = reader.ReadToEnd();
} // reader.Dispose() вызывается автоматически

// using-объявление (C# 8) — без скобок, Dispose в конце метода
using StreamReader reader2 = new("file.txt");
string text2 = reader2.ReadToEnd();
// Dispose при выходе из метода

// await using для асинхронных потоков
await using FileStream fs = new("data.bin", FileMode.Open);
// ...
```

---

## Частые ошибки

```csharp
// ❌ Склеивание путей строками
string path = "C:\\Users" + "\\" + "file.txt"; // ❌
// ✅
string path2 = Path.Combine("C:\\Users", "file.txt");

// ❌ Забыл using — утечка ресурсов
StreamReader r = new("file.txt");
string text = r.ReadToEnd();
// Файл остаётся открытым! Другие не могут его использовать
// ✅ using StreamReader r = new("file.txt");

// ❌ ReadAllText для огромного файла (1 GB)
string huge = File.ReadAllText("huge.txt"); // OutOfMemoryException!
// ✅ StreamReader построчно

// ❌ Не проверяешь существование файла
string data = File.ReadAllText("data.txt"); // FileNotFoundException!
// ✅
if (File.Exists("data.txt")) { ... }
```

---

## Мини-упражнения
1. **⭐** Прочитай файл через `File.ReadAllTextAsync`, запиши в другой.
2. **⭐** Используй `StreamReader` для построчного чтения большого файла.
3. **⭐** Создай класс сохранения игры с `BinaryWriter` / `BinaryReader`.

### Практика Uno Platform ⭐⭐⭐
**"Заметки"** — создание, редактирование, сохранение в файлы. FileOpenPicker / FileSavePicker для экспорта.

### Практика Godot ⭐⭐⭐
**Система сохранения** — бинарные save-файлы через BinaryWriter. Несколько слотов сохранения. Обработка повреждённых файлов.

---

## Что дальше
Дальше — **JSON-сериализация** (5.2).
