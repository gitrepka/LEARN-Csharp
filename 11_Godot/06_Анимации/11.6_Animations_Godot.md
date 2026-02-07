# Тема 11.6: Анимации в Godot

## Что ты узнаешь
- AnimationPlayer: ключевые кадры, Play/Stop/Queue
- AnimationTree: State Machine, Blend Trees
- Tween: программные анимации
- AnimatedSprite2D

---

## Объяснение

### AnimationPlayer

```csharp
var anim = GetNode<AnimationPlayer>("AnimationPlayer");
anim.Play("run");
anim.Play("attack"); // прервёт текущую
anim.Queue("idle");  // после текущей

anim.SpeedScale = 2.0f; // двойная скорость
anim.PlayBackwards("run"); // задом наперёд

// Событие окончания
anim.AnimationFinished += (animName) => GD.Print($"{animName} закончилась");
```

### Tween — программные анимации

```csharp
// Создание Tween
var tween = CreateTween();

// Анимация свойства
tween.TweenProperty(this, "position", new Vector2(500, 300), 1.0f)
    .SetEase(Tween.EaseType.Out)
    .SetTrans(Tween.TransitionType.Cubic);

// Цепочка
tween.TweenProperty(sprite, "modulate:a", 0.0f, 0.5f); // fade out
tween.TweenCallback(Callable.From(QueueFree));           // удалить после

// Параллельно
tween.Parallel().TweenProperty(this, "scale", Vector2.One * 2, 0.3f);
tween.Parallel().TweenProperty(this, "rotation", Mathf.Pi * 2, 0.3f);

// Интервал
tween.TweenInterval(0.5f); // пауза 0.5 сек
tween.TweenCallback(Callable.From(() => GD.Print("Готово!")));

// Зацикливание
tween.SetLoops(); // бесконечно
tween.SetLoops(3); // 3 раза
```

### AnimatedSprite2D

```csharp
var sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
sprite.Play("run");
sprite.Play("idle");
sprite.FlipH = velocity.X < 0; // отзеркалить по горизонтали
sprite.AnimationFinished += () => sprite.Play("idle");
```

---

## Мини-упражнения
1. **⭐** AnimationPlayer: анимации idle и run.
2. **⭐** Tween для движения объекта из точки A в точку B.
3. **⭐⭐** Tween-цепочка: fade in → подождать → fade out → удалить.

## Что дальше
Дальше — **2D-разработка** (11.7).
