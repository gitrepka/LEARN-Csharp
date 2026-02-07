# Тема 11.10: Аудио

## Что ты узнаешь
- AudioStreamPlayer / 2D / 3D
- AudioBus, эффекты
- Программное управление звуком

---

## Объяснение

```csharp
public partial class AudioManager : Node
{
    [Export] public AudioStream HitSound { get; set; } = null!;
    [Export] public AudioStream Music { get; set; } = null!;

    private AudioStreamPlayer _musicPlayer = null!;
    private AudioStreamPlayer _sfxPlayer = null!;

    public override void _Ready()
    {
        _musicPlayer = GetNode<AudioStreamPlayer>("MusicPlayer");
        _sfxPlayer = GetNode<AudioStreamPlayer>("SFXPlayer");
    }

    public void PlayMusic(AudioStream stream)
    {
        _musicPlayer.Stream = stream;
        _musicPlayer.Play();
    }

    public void PlaySFX(AudioStream stream)
    {
        // Для множественных звуков одновременно — создаём новый player
        var player = new AudioStreamPlayer();
        AddChild(player);
        player.Stream = stream;
        player.Bus = "SFX";
        player.Play();
        player.Finished += player.QueueFree; // удалить после проигрывания
    }

    public void SetMusicVolume(float volumeDb)
    {
        AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Music"), volumeDb);
    }
}
```

### AudioStreamPlayer2D — позиционный звук

```csharp
// Звук зависит от расстояния до Listener
var explosion = GetNode<AudioStreamPlayer2D>("ExplosionSound");
explosion.GlobalPosition = explosionPosition;
explosion.Play();
```

---

## Мини-упражнения
1. **⭐** Фоновая музыка + звук при нажатии кнопки.
2. **⭐** AudioStreamPlayer2D для позиционного звука.

## Что дальше
Дальше — **AI и навигация** (11.11).
