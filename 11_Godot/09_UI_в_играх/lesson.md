# Тема 11.9: UI в играх (Godot Control)

## Что ты узнаешь
- Control-узлы: Label, Button, ProgressBar, TextureRect
- Anchor и Layout — позиционирование UI
- CanvasLayer — UI поверх игры
- Темы (Theme) в Godot

---

## Объяснение

```csharp
// HUD — UI поверх игры
public partial class HUD : CanvasLayer
{
    private Label _scoreLabel = null!;
    private ProgressBar _healthBar = null!;

    public override void _Ready()
    {
        _scoreLabel = GetNode<Label>("ScoreLabel");
        _healthBar = GetNode<ProgressBar>("HealthBar");
    }

    public void UpdateScore(int score)
    {
        _scoreLabel.Text = $"Score: {score}";
    }

    public void UpdateHealth(float current, float max)
    {
        _healthBar.MaxValue = max;
        _healthBar.Value = current;
    }

    // Анимированное появление текста
    public async void ShowMessage(string text, float duration = 2f)
    {
        var label = GetNode<Label>("MessageLabel");
        label.Text = text;
        label.Visible = true;

        var tween = CreateTween();
        tween.TweenProperty(label, "modulate:a", 1.0f, 0.3f);
        tween.TweenInterval(duration);
        tween.TweenProperty(label, "modulate:a", 0.0f, 0.3f);
        tween.TweenCallback(Callable.From(() => label.Visible = false));
    }
}
```

### Anchors и Layout

```
Anchors определяют КАК элемент прикреплён к родителю:
- Top-Left: HUD-элементы (здоровье)
- Top-Right: счёт, мини-карта
- Bottom-Center: панель способностей
- Full Rect: фоновые панели, меню
- Center: диалоги, всплывающие окна
```

### Godot Theme

Темы в Godot задают стиль всех Control-узлов: цвета, шрифты, размеры.

```csharp
// Программная установка темы
var theme = GD.Load<Theme>("res://themes/dark_theme.tres");
GetTree().Root.Theme = theme;
```

---

## Мини-упражнения
1. **⭐** HUD: здоровье (ProgressBar) + счёт (Label).
2. **⭐** Меню паузы с кнопками Resume, Restart, Quit.
3. **⭐⭐** Всплывающий текст урона (анимация Tween).

## Что дальше
Дальше — **аудио** (11.10).
