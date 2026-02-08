# Тема 18.2: SignalR — реальное время

## Что ты узнаешь
- SignalR: Hubs, Clients, Groups
- Потоковая передача данных (Streaming)
- Подключение из Uno Platform
- Подключение из Godot (для лидерборда)

---

## Объяснение

### ЗАЧЕМ?

REST — "запрос → ответ". Клиент спрашивает, сервер отвечает. А если нужно, чтобы сервер **сам** отправлял данные? Чат, уведомления, лидерборд, мультиплеер — **SignalR**.

### Сервер — Hub

```csharp
// Hubs/GameHub.cs
using Microsoft.AspNetCore.SignalR;

public class GameHub : Hub
{
    // Клиент вызывает → сервер рассылает всем
    public async Task SendChatMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    // Группы — как комнаты
    public async Task JoinRoom(string roomName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        await Clients.Group(roomName).SendAsync("ReceiveMessage",
            "System", $"{Context.ConnectionId} зашёл в {roomName}");
    }

    public async Task LeaveRoom(string roomName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
    }

    // Отправить конкретному клиенту
    public async Task SendDirectMessage(string connectionId, string message)
    {
        await Clients.Client(connectionId).SendAsync("ReceiveDirectMessage", message);
    }

    // Отправить всем КРОМЕ отправителя
    public async Task UpdatePosition(float x, float y)
    {
        await Clients.Others.SendAsync("PlayerMoved",
            Context.ConnectionId, x, y);
    }

    // События подключения/отключения
    public override async Task OnConnectedAsync()
    {
        await Clients.All.SendAsync("PlayerJoined", Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await Clients.All.SendAsync("PlayerLeft", Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }
}

// Program.cs
builder.Services.AddSignalR();
app.MapHub<GameHub>("/gamehub");
```

### Типизированный Hub (лучше)

```csharp
// Интерфейс клиентских методов
public interface IGameClient
{
    Task ReceiveMessage(string user, string message);
    Task PlayerMoved(string playerId, float x, float y);
    Task PlayerJoined(string playerId);
    Task PlayerLeft(string playerId);
    Task ScoreUpdated(string playerId, int score);
}

// Типизированный Hub — компилятор проверяет имена методов
public class GameHub : Hub<IGameClient>
{
    public async Task SendChatMessage(string user, string message)
    {
        // ✅ Компилятор проверит, что ReceiveMessage существует
        await Clients.All.ReceiveMessage(user, message);

        // ❌ Ошибка компиляции, если метод не в IGameClient
        // await Clients.All.NonExistentMethod();
    }

    public async Task UpdateScore(int score)
    {
        await Clients.All.ScoreUpdated(Context.ConnectionId, score);
    }
}
```

### Streaming — потоковая передача

```csharp
// Сервер отправляет данные потоком
public class DataHub : Hub
{
    // Сервер → Клиент (server-to-client streaming)
    public async IAsyncEnumerable<int> Counter(
        int count,
        int delay,
        [EnumeratorCancellation] CancellationToken ct)
    {
        for (int i = 0; i < count; i++)
        {
            ct.ThrowIfCancellationRequested();
            yield return i;
            await Task.Delay(delay, ct);
        }
    }
}

// Клиент подписывается на стрим:
// await foreach (var item in connection.StreamAsync<int>("Counter", 10, 500))
//     Console.WriteLine(item);
```

### Клиент Uno Platform

```csharp
// dotnet add package Microsoft.AspNetCore.SignalR.Client

public class GameService : IDisposable
{
    private HubConnection _connection = null!;

    public event Action<string, string>? MessageReceived;
    public event Action<string, float, float>? PlayerMoved;

    public async Task ConnectAsync(string url)
    {
        _connection = new HubConnectionBuilder()
            .WithUrl(url)
            .WithAutomaticReconnect() // переподключение при разрыве
            .Build();

        // Подписка на серверные события
        _connection.On<string, string>("ReceiveMessage", (user, msg) =>
            MessageReceived?.Invoke(user, msg));

        _connection.On<string, float, float>("PlayerMoved", (id, x, y) =>
            PlayerMoved?.Invoke(id, x, y));

        _connection.Reconnecting += error =>
        {
            // UI: "Переподключение..."
            return Task.CompletedTask;
        };

        _connection.Reconnected += connectionId =>
        {
            // UI: "Подключено!"
            return Task.CompletedTask;
        };

        await _connection.StartAsync();
    }

    public async Task SendMessageAsync(string user, string message)
    {
        await _connection.InvokeAsync("SendChatMessage", user, message);
    }

    public void Dispose() => _connection?.DisposeAsync();
}
```

### Клиент Godot (для лидерборда)

```csharp
public partial class LeaderboardManager : Node
{
    private HubConnection _connection = null!;

    [Signal] public delegate void ScoreUpdatedEventHandler(string playerId, int score);

    public override async void _Ready()
    {
        _connection = new HubConnectionBuilder()
            .WithUrl("https://myserver.com/gamehub")
            .Build();

        _connection.On<string, int>("ScoreUpdated", (id, score) =>
        {
            // Вызываем в основном потоке Godot
            CallDeferred(MethodName.OnScoreUpdated, id, score);
        });

        await _connection.StartAsync();
    }

    private void OnScoreUpdated(string playerId, int score)
    {
        EmitSignal(SignalName.ScoreUpdated, playerId, score);
    }

    public async Task SubmitScoreAsync(int score)
    {
        await _connection.InvokeAsync("UpdateScore", score);
    }

    public override void _ExitTree()
    {
        _connection?.DisposeAsync();
    }
}
```

---

## Мини-упражнения

1. **⭐** Создай SignalR Hub для чата. Клиент отправляет сообщение → все получают.
2. **⭐⭐** Добавь Groups: комнаты чата, сообщения только внутри комнаты.

## Что дальше
Дальше — **Identity и авторизация** (18.3).
