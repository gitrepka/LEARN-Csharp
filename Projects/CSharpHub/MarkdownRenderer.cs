using System.Text.RegularExpressions;
using Spectre.Console;

/// <summary>
/// Рендерит Markdown-файлы в красивый вывод через Spectre.Console.
/// </summary>
public static class MarkdownRenderer
{
    /// <summary>
    /// Отрисовать весь markdown-документ в консоли.
    /// </summary>
    public static void Render(string markdown)
    {
        var lines = markdown.Split('\n');
        bool inCodeBlock = false;
        var codeBuffer = new List<string>();
        string codeLang = "";
        var tableBuffer = new List<string>();

        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i].TrimEnd('\r');

            // Если собирали таблицу, а строка уже не табличная — рисуем таблицу
            if (tableBuffer.Count > 0 && !line.TrimStart().StartsWith('|'))
            {
                RenderTable(tableBuffer);
                tableBuffer.Clear();
            }

            // --- Блоки кода (```) ---
            if (line.TrimStart().StartsWith("```"))
            {
                if (inCodeBlock)
                {
                    // Конец блока — рисуем панель с кодом
                    var code = string.Join("\n", codeBuffer);
                    var panel = new Panel(Markup.Escape(code))
                        .Header($"[green]{Markup.Escape(codeLang)}[/]")
                        .Border(BoxBorder.Rounded)
                        .BorderColor(Color.Green)
                        .Padding(1, 0);
                    AnsiConsole.Write(panel);
                    codeBuffer.Clear();
                    inCodeBlock = false;
                }
                else
                {
                    var trimmed = line.TrimStart();
                    codeLang = trimmed.Length > 3 ? trimmed[3..].Trim() : "code";
                    inCodeBlock = true;
                }
                continue;
            }

            if (inCodeBlock)
            {
                codeBuffer.Add(line);
                continue;
            }

            // --- Таблицы (| col | col |) ---
            if (line.TrimStart().StartsWith('|'))
            {
                tableBuffer.Add(line);
                continue;
            }

            // --- Заголовки ---
            if (line.StartsWith("# "))
            {
                AnsiConsole.WriteLine();
                AnsiConsole.Write(
                    new Rule($"[bold cyan]{Markup.Escape(line[2..].Trim())}[/]")
                        .LeftJustified());
                AnsiConsole.WriteLine();
                continue;
            }

            if (line.StartsWith("## "))
            {
                AnsiConsole.WriteLine();
                AnsiConsole.Write(
                    new Rule($"[bold yellow]{Markup.Escape(line[3..].Trim())}[/]")
                        .LeftJustified());
                continue;
            }

            if (line.StartsWith("### "))
            {
                AnsiConsole.MarkupLine($"\n[bold]{Markup.Escape(line[4..].Trim())}[/]");
                continue;
            }

            // --- Горизонтальная линия ---
            if (line.StartsWith("---"))
            {
                AnsiConsole.Write(new Rule().RuleStyle(Style.Parse("grey dim")));
                continue;
            }

            // --- Пустая строка ---
            if (string.IsNullOrWhiteSpace(line))
            {
                AnsiConsole.WriteLine();
                continue;
            }

            // --- Обычный текст ---
            AnsiConsole.MarkupLine(FormatInline(line));
        }

        // Дорисовать оставшуюся таблицу
        if (tableBuffer.Count > 0)
            RenderTable(tableBuffer);
    }

    /// <summary>
    /// Извлечь секцию из markdown по ключевому слову в заголовке ##.
    /// Например: ExtractSection(md, "Практика") вернёт всё от ## Практика до следующего ##.
    /// </summary>
    public static string ExtractSection(string markdown, string keyword)
    {
        var lines = markdown.Split('\n');
        var result = new List<string>();
        bool capturing = false;

        foreach (var rawLine in lines)
        {
            var line = rawLine.TrimEnd('\r');

            if (line.StartsWith("## ") &&
                line.Contains(keyword, StringComparison.OrdinalIgnoreCase))
            {
                capturing = true;
                result.Add(line);
                continue;
            }

            if (capturing)
            {
                if (line.StartsWith("## "))
                    break;
                result.Add(line);
            }
        }

        return string.Join("\n", result);
    }

    // --- Inline-форматирование ---
    private static string FormatInline(string text)
    {
        // Сначала экранируем спецсимволы Spectre ([ и ])
        text = Markup.Escape(text);

        // **жирный** → [bold]жирный[/]
        text = Regex.Replace(text, @"\*\*(.+?)\*\*", "[bold]$1[/]");

        // `код` → [green]код[/]
        text = Regex.Replace(text, @"`(.+?)`", "[green]$1[/]");

        // Маркеры списка: * элемент  или  - элемент → • элемент
        if (Regex.IsMatch(text, @"^\s*[\*\-]\s+"))
            text = Regex.Replace(text, @"^(\s*)[\*\-]\s+", "$1  • ");

        // *курсив* → [italic]курсив[/] (после жирного, чтобы ** не мешал)
        text = Regex.Replace(text, @"(?<!\*)(?<![•])\*([^\*\n]+?)\*(?!\*)", "[italic]$1[/]");

        return text;
    }

    // --- Таблицы ---
    private static void RenderTable(List<string> lines)
    {
        if (lines.Count < 2) return;

        var headers = ParseTableRow(lines[0]);
        if (headers.Length == 0) return;

        var table = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Grey);

        foreach (var header in headers)
            table.AddColumn(new TableColumn($"[bold]{FormatInline(header)}[/]"));

        for (int i = 1; i < lines.Count; i++)
        {
            // Пропускаем разделитель (|---|---|)
            if (lines[i].Contains("---") || lines[i].Contains(":---"))
                continue;

            var cells = ParseTableRow(lines[i]);
            if (cells.Length == 0) continue;

            var row = new string[headers.Length];
            for (int j = 0; j < headers.Length; j++)
                row[j] = j < cells.Length ? FormatInline(cells[j]) : "";

            table.AddRow(row);
        }

        AnsiConsole.Write(table);
    }

    private static string[] ParseTableRow(string line)
    {
        return line.Split('|', StringSplitOptions.TrimEntries)
            .Where(c => !string.IsNullOrEmpty(c))
            .ToArray();
    }
}