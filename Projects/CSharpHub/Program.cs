using System.Diagnostics;
using System.Text.RegularExpressions;
using Spectre.Console;

// ══════════════════════════════════════════════════
// CSharpHub — Консольный тренажёр для изучения C#
// ══════════════════════════════════════════════════

var contentRoot = FindContentRoot();
var sandboxDir = Path.Combine(contentRoot, "Projects", "Sandbox");
var sandboxFile = Path.Combine(sandboxDir, "Program.cs");

AnsiConsole.Clear();
ShowHeader();

// --- Главный цикл ---
while (true)
{
    var choice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("\n[green]Главное меню:[/]")
            .HighlightStyle(Style.Parse("cyan bold"))
            .AddChoices(
                "Учебные материалы",
                "Песочница",
                "Выход"));

    switch (choice)
    {
        case "Учебные материалы":
            BrowseModules();
            break;
        case "Песочница":
            OpenSandbox();
            break;
        case "Выход":
            AnsiConsole.MarkupLine("[yellow]До встречи![/]");
            return;
    }
}

// ══════════════════════════════════════════════════
// Навигация: Модули → Темы → Уроки
// ══════════════════════════════════════════════════

void BrowseModules()
{
    while (true)
    {
        var modules = GetSortedDirs(contentRoot, @"^\d{2}_");
        if (modules.Length == 0)
        {
            AnsiConsole.MarkupLine("[red]Модули не найдены.[/]");
            PressAnyKey();
            return;
        }

        var names = modules
            .Select(m => PrettyName(Path.GetFileName(m)!))
            .Append("[grey]<< Назад[/]")
            .ToArray();

        var pick = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[green]Выбери модуль:[/]")
                .PageSize(15)
                .HighlightStyle(Style.Parse("cyan bold"))
                .AddChoices(names));

        if (pick.Contains("Назад")) return;

        var idx = Array.IndexOf(names, pick);
        BrowseTopics(modules[idx], pick);
    }
}

void BrowseTopics(string modulePath, string moduleTitle)
{
    while (true)
    {
        var topics = GetSortedDirs(modulePath, @"^\d");

        // Если подпапок нет — может, .md лежат прямо в модуле
        if (topics.Length == 0)
        {
            var directFiles = GetLessonFiles(modulePath);
            if (directFiles.Length > 0)
            {
                BrowseLessons(directFiles, moduleTitle);
            }
            else
            {
                AnsiConsole.MarkupLine("[grey]Пока нет материалов в этом модуле.[/]");
                PressAnyKey();
            }
            return;
        }

        var names = topics
            .Select(t => PrettyName(Path.GetFileName(t)!))
            .Append("[grey]<< Назад[/]")
            .ToArray();

        var pick = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title($"[green]{Markup.Escape(moduleTitle)} — тема:[/]")
                .PageSize(15)
                .HighlightStyle(Style.Parse("cyan bold"))
                .AddChoices(names));

        if (pick.Contains("Назад")) return;

        var idx = Array.IndexOf(names, pick);
        var files = GetLessonFiles(topics[idx]);

        if (files.Length == 0)
        {
            AnsiConsole.MarkupLine("[grey]Пока нет уроков.[/]");
            PressAnyKey();
            continue;
        }

        BrowseLessons(files, pick);
    }
}

void BrowseLessons(string[] files, string topicTitle)
{
    while (true)
    {
        var names = files
            .Select(f => PrettyName(Path.GetFileNameWithoutExtension(f)!))
            .Append("[grey]<< Назад[/]")
            .ToArray();

        var pick = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title($"[green]{Markup.Escape(topicTitle)} — урок:[/]")
                .PageSize(15)
                .HighlightStyle(Style.Parse("cyan bold"))
                .AddChoices(names));

        if (pick.Contains("Назад")) return;

        var idx = Array.IndexOf(names, pick);
        ShowLesson(files[idx]);
    }
}

// ══════════════════════════════════════════════════
// Отображение урока
// ══════════════════════════════════════════════════

void ShowLesson(string filePath)
{
    var markdown = File.ReadAllText(filePath);

    AnsiConsole.Clear();
    MarkdownRenderer.Render(markdown);

    // Проверяем, есть ли секция Практика
    var practice = MarkdownRenderer.ExtractSection(markdown, "Практика");

    var options = new List<string>();
    if (!string.IsNullOrWhiteSpace(practice))
        options.Add("Практика");
    options.Add("[grey]<< Назад[/]");

    AnsiConsole.WriteLine();
    var action = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("[green]Что дальше?[/]")
            .HighlightStyle(Style.Parse("cyan bold"))
            .AddChoices(options));

    if (action == "Практика")
        RunPractice(practice);
}

// ══════════════════════════════════════════════════
// Практика: показать задание + запустить код
// ══════════════════════════════════════════════════

void RunPractice(string practiceMarkdown)
{
    AnsiConsole.Clear();
    AnsiConsole.Write(new Rule("[bold yellow]Практика[/]").LeftJustified());
    AnsiConsole.WriteLine();
    MarkdownRenderer.Render(practiceMarkdown);

    AnsiConsole.WriteLine();
    AnsiConsole.Write(new Rule("[grey]Песочница[/]").LeftJustified());
    AnsiConsole.MarkupLine($"[cyan]Файл:[/] {Markup.Escape(sandboxFile)}");
    AnsiConsole.MarkupLine("[grey]Открой этот файл в редакторе (Rider / VS Code / nano).[/]");
    AnsiConsole.MarkupLine("[grey]Напиши решение, сохрани, вернись сюда.[/]");

    SandboxLoop();
}

void OpenSandbox()
{
    AnsiConsole.Clear();
    AnsiConsole.Write(new Rule("[bold cyan]Свободная песочница[/]").LeftJustified());
    AnsiConsole.WriteLine();
    AnsiConsole.MarkupLine($"[cyan]Файл:[/] {Markup.Escape(sandboxFile)}");
    AnsiConsole.MarkupLine("[grey]Открой файл, пиши любой C# код, сохрани.[/]");
    AnsiConsole.MarkupLine("[grey]Возвращайся сюда и жми Enter для запуска.[/]");

    SandboxLoop();
}

void SandboxLoop()
{
    while (true)
    {
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[dim]Enter = запустить код  |  q = назад[/]");

        var input = Console.ReadLine()?.Trim().ToLower();
        if (input == "q") return;

        RunCode();
    }
}

void RunCode()
{
    if (!File.Exists(Path.Combine(sandboxDir, "Sandbox.csproj")))
    {
        AnsiConsole.MarkupLine("[red]Проект песочницы не найден.[/]");
        return;
    }

    AnsiConsole.MarkupLine("[yellow]Компиляция и запуск...[/]\n");

    var psi = new ProcessStartInfo
    {
        FileName = "dotnet",
        Arguments = $"run --project \"{sandboxDir}\" --verbosity quiet",
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
        CreateNoWindow = true
    };

    try
    {
        using var process = Process.Start(psi)!;

        var outputTask = process.StandardOutput.ReadToEndAsync();
        var errorTask = process.StandardError.ReadToEndAsync();

        bool finished = process.WaitForExit(15000);

        if (!finished)
        {
            process.Kill(true);
            AnsiConsole.MarkupLine(
                "[red]Время истекло (15 сек). Проверь код на бесконечные циклы.[/]");
            return;
        }

        var output = outputTask.Result;
        var errors = errorTask.Result;

        if (process.ExitCode == 0)
        {
            if (!string.IsNullOrWhiteSpace(output))
            {
                AnsiConsole.Write(
                    new Panel(Markup.Escape(output.TrimEnd()))
                        .Header("[green]Результат[/]")
                        .Border(BoxBorder.Rounded)
                        .BorderColor(Color.Green)
                        .Padding(1, 0));
            }
            else
            {
                AnsiConsole.MarkupLine("[grey]Программа завершилась без вывода.[/]");
            }
        }
        else
        {
            var errorText = !string.IsNullOrWhiteSpace(errors) ? errors : output;
            if (!string.IsNullOrWhiteSpace(errorText))
            {
                // Убираем шум от dotnet build
                var cleaned = string.Join("\n",
                    errorText.Split('\n')
                        .Where(l => !string.IsNullOrWhiteSpace(l)));

                AnsiConsole.Write(
                    new Panel(Markup.Escape(cleaned.TrimEnd()))
                        .Header("[red]Ошибка[/]")
                        .Border(BoxBorder.Rounded)
                        .BorderColor(Color.Red)
                        .Padding(1, 0));
            }
        }
    }
    catch (Exception ex)
    {
        AnsiConsole.MarkupLine($"[red]Не удалось запустить: {Markup.Escape(ex.Message)}[/]");
    }
}

// ══════════════════════════════════════════════════
// Утилиты
// ══════════════════════════════════════════════════

void ShowHeader()
{
    AnsiConsole.Write(
        new FigletText("CSharp Hub")
            .Color(Color.Cyan1));
    AnsiConsole.MarkupLine("[grey]Твой персональный тренажёр по C#[/]");
}

void PressAnyKey()
{
    AnsiConsole.MarkupLine("[dim]Нажми любую клавишу...[/]");
    Console.ReadKey(true);
}

/// <summary>
/// Ищет корень проекта обучения (папку с PROGRESS.md).
/// </summary>
string FindContentRoot()
{
    var dir = Directory.GetCurrentDirectory();
    while (dir != null)
    {
        if (File.Exists(Path.Combine(dir, "PROGRESS.md")))
            return dir;
        dir = Directory.GetParent(dir)?.FullName;
    }

    return "/home/aibek/RiderProjects/LEARN-Csharp";
}

/// <summary>
/// Получить отсортированные подпапки, имя которых совпадает с паттерном.
/// </summary>
string[] GetSortedDirs(string path, string pattern)
{
    if (!Directory.Exists(path)) return [];

    return Directory.GetDirectories(path)
        .Where(d => Regex.IsMatch(Path.GetFileName(d)!, pattern))
        .OrderBy(d => Path.GetFileName(d), StringComparer.OrdinalIgnoreCase)
        .ToArray();
}

/// <summary>
/// Получить .md файлы урока (без README).
/// </summary>
string[] GetLessonFiles(string path)
{
    if (!Directory.Exists(path)) return [];

    return Directory.GetFiles(path, "*.md")
        .Where(f => !Path.GetFileName(f)!
            .Equals("README.md", StringComparison.OrdinalIgnoreCase))
        .OrderBy(f => Path.GetFileName(f))
        .ToArray();
}

/// <summary>
/// Превращает имя файла/папки в читаемое название.
/// "2.1_Memory" → "2.1 Memory"
/// "01_Основы" → "01 Основы"
/// </summary>
string PrettyName(string name)
{
    return name.Replace('_', ' ');
}
