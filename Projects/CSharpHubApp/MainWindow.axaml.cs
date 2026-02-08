using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace CSharpHubApp;

public partial class MainWindow : Window
{
    private static readonly IBrush ColorSuccess = new SolidColorBrush(Color.Parse("#A3BE8C"));
    private static readonly IBrush ColorError = new SolidColorBrush(Color.Parse("#BF616A"));
    private static readonly IBrush ColorInfo = new SolidColorBrush(Color.Parse("#D8DEE9"));

    private readonly string _contentRoot;
    private readonly string _sandboxDir;
    private readonly string _sandboxFile;
    private readonly List<LessonInfo> _lessons = new();

    public MainWindow()
    {
        InitializeComponent();

        _contentRoot = FindContentRoot();
        _sandboxDir = Path.Combine(_contentRoot, "Projects", "Sandbox");
        _sandboxFile = Path.Combine(_sandboxDir, "Program.cs");

        LoadLessons();
    }

    // ═══════════════════════════════════
    // Загрузка уроков
    // ═══════════════════════════════════

    private void LoadLessons()
    {
        var dir = Path.Combine(_contentRoot, "01_Основы", "1.2_Типы_данных");
        if (!Directory.Exists(dir)) return;

        var files = Directory.GetFiles(dir, "*.md")
            .Where(f => !Path.GetFileName(f)
                .Equals("README.md", StringComparison.OrdinalIgnoreCase))
            .OrderBy(f => Path.GetFileName(f))
            .ToArray();

        foreach (var file in files)
        {
            var name = Path.GetFileNameWithoutExtension(file)!.Replace('_', ' ');
            _lessons.Add(new LessonInfo(name, file));
            LessonList.Items.Add(name);
        }
    }

    // ═══════════════════════════════════
    // Выбор урока
    // ═══════════════════════════════════

    private void OnLessonSelected(object? sender, SelectionChangedEventArgs e)
    {
        var index = LessonList.SelectedIndex;
        if (index < 0 || index >= _lessons.Count) return;

        var lesson = _lessons[index];
        var markdown = File.ReadAllText(lesson.FilePath);

        LessonTitle.Text = lesson.Name;
        LessonTitle.Foreground = new SolidColorBrush(Color.Parse("#88C0D0"));

        // Рендерим контент с callback для кнопок "Попробовать"
        ContentPanel.Children.Clear();
        var controls = MarkdownToAvalonia.Render(markdown, LoadCodeToEditor);
        foreach (var control in controls)
            ContentPanel.Children.Add(control);

        // Шаблон практики в редактор
        var practice = MarkdownToAvalonia.ExtractSection(markdown, "Практика");
        CodeEditor.Text = CreateTemplate(practice, lesson.Name);
        StdinInput.Text = "";
        OutputText.Text = "...";
        OutputText.Foreground = ColorInfo;
    }

    /// <summary>
    /// Вызывается при клике "Попробовать" — загружает код из примера в редактор.
    /// </summary>
    private void LoadCodeToEditor(string code)
    {
        CodeEditor.Text = code;
        OutputText.Text = "Код загружен. Нажми Запустить.";
        OutputText.Foreground = ColorInfo;
    }

    private static string CreateTemplate(string practiceText, string lessonName)
    {
        if (string.IsNullOrWhiteSpace(practiceText))
            return $"// {lessonName}\n// Пиши код тут:\n\nConsole.WriteLine(\"Hello!\");\n";

        var lines = practiceText.Split('\n')
            .Select(l => l.TrimEnd('\r').Trim())
            .Where(l => !string.IsNullOrEmpty(l))
            .Where(l => !l.StartsWith("```"))
            .Select(l =>
            {
                l = Regex.Replace(l, @"\*\*(.+?)\*\*", "$1");
                l = Regex.Replace(l, @"`(.+?)`", "$1");
                l = l.TrimStart('#', ' ');
                return $"// {l}";
            });

        return string.Join("\n", lines) + "\n\n// Пиши свой код ниже:\n\n";
    }

    // ═══════════════════════════════════
    // Запуск кода
    // ═══════════════════════════════════

    private async void OnRunCode(object? sender, RoutedEventArgs e)
    {
        var code = CodeEditor?.Text;
        if (string.IsNullOrWhiteSpace(code))
        {
            OutputText.Text = "Напиши код сначала.";
            OutputText.Foreground = ColorError;
            return;
        }

        if (!File.Exists(Path.Combine(_sandboxDir, "Sandbox.csproj")))
        {
            OutputText.Text = "Sandbox не найден.";
            OutputText.Foreground = ColorError;
            return;
        }

        RunButton.IsEnabled = false;
        OutputText.Text = "Компиляция...";
        OutputText.Foreground = ColorInfo;

        var stdinText = StdinInput?.Text ?? "";

        try
        {
            await File.WriteAllTextAsync(_sandboxFile, code);
            var (output, error, exitCode) = await Task.Run(() => CompileAndRun(stdinText));

            if (exitCode == 0)
            {
                OutputText.Text = string.IsNullOrWhiteSpace(output)
                    ? "(нет вывода)" : output.TrimEnd();
                OutputText.Foreground = ColorSuccess;
            }
            else if (exitCode == -1)
            {
                OutputText.Text = "Время истекло (15 сек).";
                OutputText.Foreground = ColorError;
            }
            else
            {
                OutputText.Text = !string.IsNullOrWhiteSpace(error)
                    ? error.TrimEnd() : output.TrimEnd();
                OutputText.Foreground = ColorError;
            }
        }
        catch (Exception ex)
        {
            OutputText.Text = $"Ошибка: {ex.Message}";
            OutputText.Foreground = ColorError;
        }
        finally
        {
            RunButton.IsEnabled = true;
        }
    }

    private (string output, string error, int exitCode) CompileAndRun(string stdinText)
    {
        var psi = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"run --project \"{_sandboxDir}\" --verbosity quiet",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            RedirectStandardInput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using var process = Process.Start(psi)!;

        if (!string.IsNullOrEmpty(stdinText))
        {
            process.StandardInput.Write(stdinText);
            if (!stdinText.EndsWith('\n'))
                process.StandardInput.WriteLine();
        }
        process.StandardInput.Close();

        var outputTask = process.StandardOutput.ReadToEndAsync();
        var errorTask = process.StandardError.ReadToEndAsync();

        bool finished = process.WaitForExit(15000);
        if (!finished)
        {
            process.Kill(true);
            return ("", "Timeout", -1);
        }

        return (outputTask.Result, errorTask.Result, process.ExitCode);
    }

    // ═══════════════════════════════════

    private static string FindContentRoot()
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

    private record LessonInfo(string Name, string FilePath);
}
