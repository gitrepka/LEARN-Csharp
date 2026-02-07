# Тема 11.16: Viewport и рендеринг

## Что ты узнаешь
- SubViewport для рендера в текстуру
- Split-screen (разделённый экран)
- Мини-карта
- Постобработка через Viewport

---

## Объяснение

```csharp
// SubViewport — рендер в текстуру
// Сцена:
// SubViewportContainer
//   └── SubViewport
//       └── Camera2D (камера мини-карты)
//           └── ... (содержимое)

// Мини-карта
public partial class Minimap : SubViewportContainer
{
    private Camera2D _minimapCamera = null!;
    private Node2D _player = null!;

    public override void _Ready()
    {
        _minimapCamera = GetNode<Camera2D>("SubViewport/MinimapCamera");
        _player = GetTree().GetFirstNodeInGroup("player") as Node2D;
    }

    public override void _Process(double delta)
    {
        if (_player != null)
            _minimapCamera.GlobalPosition = _player.GlobalPosition;
    }
}
```

### Split-screen

```
HSplitContainer (или 2 SubViewportContainer)
├── SubViewportContainer
│   └── SubViewport (World=SharedWorld)
│       └── Camera2D (следует за Player1)
└── SubViewportContainer
    └── SubViewport (World=SharedWorld)
        └── Camera2D (следует за Player2)
```

---

## Мини-упражнения
1. **⭐** SubViewport для мини-карты.
2. **⭐⭐** Split-screen для двух игроков.

## Что дальше
Дальше — **локализация** (11.17).
