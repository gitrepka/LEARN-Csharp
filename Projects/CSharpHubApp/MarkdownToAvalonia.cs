using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Documents;
using Avalonia.Layout;
using Avalonia.Media;

namespace CSharpHubApp;

public static class MarkdownToAvalonia
{
    // Nord Palette
    private static readonly FontFamily MonoFont =
        new("JetBrains Mono, Cascadia Code, Consolas, monospace");

    private static readonly IBrush TextPrimary = SolidBrush("#D8DEE9");
    private static readonly IBrush TextMuted = SolidBrush("#7B88A1");
    private static readonly IBrush HeadingColor = SolidBrush("#88C0D0");
    private static readonly IBrush SubheadColor = SolidBrush("#81A1C1");
    private static readonly IBrush InlineCode = SolidBrush("#A3BE8C");
    private static readonly IBrush CodeBlockBg = SolidBrush("#272C36");
    private static readonly IBrush CodeBlockFg = SolidBrush("#D8DEE9");
    private static readonly IBrush BorderDim = SolidBrush("#3B4252");
    private static readonly IBrush HrColor = SolidBrush("#434C5E");
    private static readonly IBrush BtnBg = SolidBrush("#4C566A");
    private static readonly IBrush BtnFg = SolidBrush("#D8DEE9");

    private static SolidColorBrush SolidBrush(string hex) => new(Color.Parse(hex));

    /// <summary>
    /// Рендерит markdown. onTryCode вызывается при клике "Попробовать" на блоке кода.
    /// </summary>
    public static List<Control> Render(string markdown, Action<string>? onTryCode = null)
    {
        var controls = new List<Control>();
        var lines = markdown.Split('\n');
        bool inCodeBlock = false;
        var codeBuffer = new List<string>();
        string codeLang = "";
        var tableBuffer = new List<string>();

        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i].TrimEnd('\r');

            if (tableBuffer.Count > 0 && !line.TrimStart().StartsWith('|'))
            {
                controls.Add(CreateTable(tableBuffer));
                tableBuffer.Clear();
            }

            if (line.TrimStart().StartsWith("```"))
            {
                if (inCodeBlock)
                {
                    var code = string.Join("\n", codeBuffer);
                    bool isRunnable = codeLang is "" or "csharp" or "cs";
                    controls.Add(CreateCodeBlock(code, isRunnable ? onTryCode : null));
                    codeBuffer.Clear();
                    inCodeBlock = false;
                }
                else
                {
                    var trimmed = line.TrimStart();
                    codeLang = trimmed.Length > 3 ? trimmed[3..].Trim() : "";
                    inCodeBlock = true;
                }
                continue;
            }
            if (inCodeBlock) { codeBuffer.Add(line); continue; }

            if (line.TrimStart().StartsWith('|'))
            { tableBuffer.Add(line); continue; }

            if (line.StartsWith("# "))
            { controls.Add(CreateHeading(line[2..].Trim(), 21, HeadingColor)); continue; }
            if (line.StartsWith("## "))
            { controls.Add(CreateHeading(line[3..].Trim(), 16, SubheadColor)); continue; }
            if (line.StartsWith("### "))
            { controls.Add(CreateHeading(line[4..].Trim(), 14, TextPrimary)); continue; }

            if (line.StartsWith("---"))
            {
                controls.Add(new Border { Height = 1, Background = HrColor, Margin = new Thickness(0, 8) });
                continue;
            }

            if (string.IsNullOrWhiteSpace(line))
            {
                controls.Add(new Border { Height = 4 });
                continue;
            }

            controls.Add(CreateFormattedText(line));
        }

        if (tableBuffer.Count > 0)
            controls.Add(CreateTable(tableBuffer));

        return controls;
    }

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
            { capturing = true; continue; }

            if (capturing)
            {
                if (line.StartsWith("## ")) break;
                result.Add(line);
            }
        }

        return string.Join("\n", result).Trim();
    }

    // ════════════ Контролы ════════════

    private static TextBlock CreateHeading(string text, double size, IBrush color)
    {
        return new TextBlock
        {
            Text = text,
            FontSize = size,
            FontWeight = FontWeight.SemiBold,
            Foreground = color,
            TextWrapping = TextWrapping.Wrap,
            Margin = new Thickness(0, 8, 0, 2)
        };
    }

    private static Border CreateCodeBlock(string code, Action<string>? onTryCode)
    {
        var codeText = new SelectableTextBlock
        {
            Text = code,
            FontFamily = MonoFont,
            FontSize = 12,
            Foreground = CodeBlockFg,
            TextWrapping = TextWrapping.NoWrap
        };

        Control child;

        if (onTryCode != null)
        {
            var btn = new Button
            {
                Content = "Попробовать",
                FontSize = 10,
                Padding = new Thickness(8, 2),
                Background = BtnBg,
                Foreground = BtnFg,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                Opacity = 0.9
            };
            btn.Click += (_, _) => onTryCode(code);

            var grid = new Grid();
            grid.Children.Add(codeText);
            grid.Children.Add(btn);
            child = grid;
        }
        else
        {
            child = codeText;
        }

        return new Border
        {
            Background = CodeBlockBg,
            CornerRadius = new CornerRadius(6),
            Padding = new Thickness(12, 8),
            Margin = new Thickness(0, 4),
            Child = child
        };
    }

    private static TextBlock CreateFormattedText(string line)
    {
        if (Regex.IsMatch(line, @"^\s*[\*\-]\s+"))
            line = Regex.Replace(line, @"^\s*[\*\-]\s+", "  \u2022 ");
        else if (Regex.IsMatch(line, @"^\s*\d+\.\s+"))
            line = Regex.Replace(line, @"^(\s*\d+)\.\s+", "$1. ");

        var tb = new TextBlock
        {
            TextWrapping = TextWrapping.Wrap,
            Foreground = TextPrimary,
            Margin = new Thickness(0, 1),
            LineHeight = 21,
            FontSize = 13
        };

        AddFormattedInlines(tb, line);
        return tb;
    }

    private static void AddFormattedInlines(TextBlock tb, string text)
    {
        var pattern = @"(\*\*(.+?)\*\*)|(`(.+?)`)|(?<!\*)\*([^\*\n]+?)\*(?!\*)";
        int lastIndex = 0;
        bool hasInlines = false;

        foreach (Match match in Regex.Matches(text, pattern))
        {
            if (match.Index > lastIndex)
            {
                tb.Inlines!.Add(new Run(text[lastIndex..match.Index]));
                hasInlines = true;
            }

            if (match.Groups[2].Success)
            {
                tb.Inlines!.Add(new Run(match.Groups[2].Value) { FontWeight = FontWeight.Bold });
                hasInlines = true;
            }
            else if (match.Groups[4].Success)
            {
                tb.Inlines!.Add(new Run(match.Groups[4].Value)
                    { FontFamily = MonoFont, Foreground = InlineCode });
                hasInlines = true;
            }
            else if (match.Groups[5].Success)
            {
                tb.Inlines!.Add(new Run(match.Groups[5].Value)
                    { FontStyle = FontStyle.Italic, Foreground = TextMuted });
                hasInlines = true;
            }

            lastIndex = match.Index + match.Length;
        }

        if (lastIndex < text.Length && hasInlines)
            tb.Inlines!.Add(new Run(text[lastIndex..]));

        if (!hasInlines)
            tb.Text = text;
    }

    private static Control CreateTable(List<string> lines)
    {
        var rows = new List<string[]>();
        foreach (var line in lines)
        {
            if (line.Contains("---") || line.Contains(":---")) continue;
            var cells = line.Split('|', StringSplitOptions.TrimEntries)
                .Where(c => !string.IsNullOrEmpty(c)).ToArray();
            if (cells.Length > 0) rows.Add(cells);
        }

        if (rows.Count == 0) return new TextBlock();

        int cols = rows.Max(r => r.Length);
        var grid = new Grid { Margin = new Thickness(0, 4) };

        for (int c = 0; c < cols; c++)
            grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
        for (int r = 0; r < rows.Count; r++)
            grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));

        for (int r = 0; r < rows.Count; r++)
        {
            for (int c = 0; c < rows[r].Length && c < cols; c++)
            {
                var tb = new TextBlock
                {
                    TextWrapping = TextWrapping.Wrap,
                    Foreground = r == 0 ? SubheadColor : TextPrimary,
                    FontWeight = r == 0 ? FontWeight.SemiBold : FontWeight.Normal,
                    Margin = new Thickness(8, 4),
                    FontSize = 12
                };
                AddFormattedInlines(tb, rows[r][c]);
                Grid.SetRow(tb, r);
                Grid.SetColumn(tb, c);
                grid.Children.Add(tb);
            }
        }

        return new Border
        {
            BorderBrush = BorderDim,
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(6),
            Background = CodeBlockBg,
            Child = grid
        };
    }
}