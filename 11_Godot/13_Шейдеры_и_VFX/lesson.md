# Тема 11.13: Шейдеры и VFX

## Что ты узнаешь
- GPUParticles2D/3D — системы частиц
- Godot Shading Language (основы)
- Управление шейдерами из C#
- Постобработка

---

## Объяснение

### GPUParticles2D

```csharp
public partial class ExplosionEffect : GPUParticles2D
{
    public override void _Ready()
    {
        Emitting = true;
        OneShot = true;
        // Удалить после проигрывания
        Finished += QueueFree;
    }
}

// Спавн эффекта
public void SpawnExplosion(Vector2 position)
{
    var scene = GD.Load<PackedScene>("res://effects/explosion.tscn");
    var effect = scene.Instantiate<GPUParticles2D>();
    effect.GlobalPosition = position;
    GetTree().Root.AddChild(effect);
}
```

### ShaderMaterial из C#

```csharp
// Установка параметров шейдера
var material = sprite.Material as ShaderMaterial;
material?.SetShaderParameter("flash_color", new Color(1, 0, 0));
material?.SetShaderParameter("flash_intensity", 1.0f);

// Flash-эффект при получении урона
public async void FlashWhite()
{
    var mat = _sprite.Material as ShaderMaterial;
    mat?.SetShaderParameter("flash_amount", 1.0f);
    await ToSignal(GetTree().CreateTimer(0.1), SceneTreeTimer.SignalName.Timeout);
    mat?.SetShaderParameter("flash_amount", 0.0f);
}
```

### Простой шейдер (Godot Shading Language)

```glsl
// flash.gdshader
shader_type canvas_item;

uniform float flash_amount : hint_range(0.0, 1.0) = 0.0;
uniform vec4 flash_color : source_color = vec4(1.0, 1.0, 1.0, 1.0);

void fragment() {
    vec4 tex = texture(TEXTURE, UV);
    COLOR = mix(tex, flash_color, flash_amount);
    COLOR.a = tex.a;
}
```

---

## Мини-упражнения
1. **⭐** GPUParticles2D: взрыв при смерти врага.
2. **⭐** Flash-эффект через ShaderMaterial из C#.
3. **⭐⭐** Простой шейдер для outline-эффекта.

## Что дальше
Дальше — **Game Feel** (11.14).
