# Тема 11.14: Game Feel (Ощущения от игры)

## Что ты узнаешь
- Juice: screen shake, hitstop, flash
- Easing и timing
- Feedback loop: визуальный + звуковой + тактильный отклик

---

## Объяснение

### ЗАЧЕМ?
Два одинаковых по механике платформера. Один "чувствуется" на 10 из 10, другой на 3. Разница — Game Feel: мелкие эффекты, которые делают игру "сочной".

### Screen Shake

```csharp
public async void ShakeCamera(float intensity = 5f, float duration = 0.2f)
{
    var camera = GetViewport().GetCamera2D();
    if (camera == null) return;

    var originalOffset = camera.Offset;
    float elapsed = 0;

    while (elapsed < duration)
    {
        float strength = intensity * (1 - elapsed / duration); // затухание
        camera.Offset = originalOffset + new Vector2(
            (float)GD.RandRange(-strength, strength),
            (float)GD.RandRange(-strength, strength));
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        elapsed += (float)GetProcessDeltaTime();
    }
    camera.Offset = originalOffset;
}
```

### Hitstop (заморозка кадра)

```csharp
public async void Hitstop(float duration = 0.05f)
{
    Engine.TimeScale = 0.01;
    await ToSignal(GetTree().CreateTimer(duration * 0.01), SceneTreeTimer.SignalName.Timeout);
    Engine.TimeScale = 1.0;
}
```

### Комбо при ударе

```csharp
public void OnHitEnemy(Enemy enemy, int damage)
{
    // 1. Визуально
    enemy.FlashWhite();                  // мигание
    SpawnDamageNumber(damage, enemy.Position); // числа урона
    SpawnHitParticles(enemy.Position);   // частицы

    // 2. Камера
    ShakeCamera(damage > 50 ? 10f : 3f); // тряска пропорционально урону

    // 3. Время
    if (damage > 50) Hitstop(0.08f);     // заморозка при сильном ударе

    // 4. Звук
    _audioManager.PlaySFX(_hitSound);

    // 5. Отдача
    enemy.ApplyKnockback((enemy.Position - Position).Normalized() * 200);
}
```

---

## Мини-упражнения
1. **⭐** Screen shake при столкновении.
2. **⭐** Hitstop (замедление времени) при ударе.
3. **⭐⭐** Комбо: flash + shake + particles + sound.

## Что дальше
Дальше — **ресурсы и данные** (11.15).
