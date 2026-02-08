# Тема 13.4: Мультиплеер в Godot

## Что ты узнаешь
- ENet MultiplayerPeer
- RPC (Remote Procedure Calls)
- Синхронизация состояния: MultiplayerSynchronizer
- Authority (кто управляет объектом)

---

## Объяснение

### Базовый мультиплеер

```csharp
public partial class Lobby : Node
{
    private ENetMultiplayerPeer _peer = new();

    // Создать сервер
    public void HostGame(int port = 9999)
    {
        _peer.CreateServer(port, maxClients: 4);
        Multiplayer.MultiplayerPeer = _peer;
        GD.Print("Сервер запущен!");
    }

    // Подключиться к серверу
    public void JoinGame(string address = "127.0.0.1", int port = 9999)
    {
        _peer.CreateClient(address, port);
        Multiplayer.MultiplayerPeer = _peer;
    }

    public override void _Ready()
    {
        Multiplayer.PeerConnected += id => GD.Print($"Игрок {id} подключился");
        Multiplayer.PeerDisconnected += id => GD.Print($"Игрок {id} отключился");
        Multiplayer.ConnectedToServer += () => GD.Print("Подключён к серверу!");
    }
}
```

### RPC — вызов методов на других клиентах

```csharp
public partial class Player : CharacterBody2D
{
    // Вызывается на ВСЕХ клиентах
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true)]
    public void TakeDamage(int amount)
    {
        Health -= amount;
        GD.Print($"Игрок {Name} получил {amount} урона");
    }

    // Вызов
    public void Attack(Player target)
    {
        target.Rpc(MethodName.TakeDamage, 10); // вызовет TakeDamage на всех клиентах
    }
}
```

### MultiplayerSynchronizer — автосинхронизация

```
В сцене:
Player (CharacterBody2D)
├── MultiplayerSynchronizer
│   Replication Config:
│   - position: Always
│   - velocity: Always
│   - health: OnChange
├── Sprite2D
└── CollisionShape2D
```

---

## Мини-упражнения
1. **⭐** Host + Join: два клиента подключаются.
2. **⭐⭐** RPC: синхронизация позиции игроков.

## Что дальше
Модуль 13 завершён! Дальше — **Модуль 14: Тестирование**.
