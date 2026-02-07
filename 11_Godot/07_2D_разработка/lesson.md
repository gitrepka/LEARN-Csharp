# Тема 11.7: 2D-разработка

## Что ты узнаешь
- TileMap, TileSet, авто-тайлинг (terrain)
- Camera2D: следование, зоны, лимиты
- Parallax (параллакс) фон
- 2D-освещение: PointLight2D, DirectionalLight2D

---

## Объяснение

### TileMap

```csharp
var tilemap = GetNode<TileMapLayer>("TileMapLayer");

// Программная установка тайлов
tilemap.SetCell(new Vector2I(5, 3), sourceId: 0, atlasCoords: new Vector2I(0, 0));

// Получение тайла
Vector2I cell = tilemap.LocalToMap(Position);
TileData? data = tilemap.GetCellTileData(cell);

// Terrain (авто-тайлинг) настраивается в TileSet Inspector
```

### Camera2D

```csharp
public partial class GameCamera : Camera2D
{
    [Export] public NodePath TargetPath { get; set; }
    [Export] public float SmoothSpeed { get; set; } = 5f;
    private Node2D _target = null!;

    public override void _Ready()
    {
        _target = GetNode<Node2D>(TargetPath);
    }

    public override void _Process(double delta)
    {
        GlobalPosition = GlobalPosition.Lerp(_target.GlobalPosition, SmoothSpeed * (float)delta);
    }

    // Тряска камеры
    public async void Shake(float intensity, float duration)
    {
        var original = Offset;
        var timer = GetTree().CreateTimer(duration);

        while (timer.TimeLeft > 0)
        {
            Offset = original + new Vector2(
                (float)GD.RandRange(-intensity, intensity),
                (float)GD.RandRange(-intensity, intensity));
            await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        }
        Offset = original;
    }
}
```

### Parallax

```
ParallaxBackground
├── ParallaxLayer (MotionScale=0.2)  ← далёкий фон (двигается медленно)
│   └── Sprite2D (sky)
├── ParallaxLayer (MotionScale=0.5)  ← средний план
│   └── Sprite2D (mountains)
└── ParallaxLayer (MotionScale=0.8)  ← ближний план
    └── Sprite2D (trees)
```

---

## Мини-упражнения
1. **⭐** TileMap с авто-тайлингом.
2. **⭐** Camera2D с плавным следованием за игроком.
3. **⭐⭐** Camera shake при получении урона.

## Что дальше
Дальше — **3D-разработка** (11.8).
