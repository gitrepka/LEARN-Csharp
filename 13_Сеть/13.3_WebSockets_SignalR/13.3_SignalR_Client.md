# Тема 13.3: WebSockets и SignalR

## Что ты узнаешь
- WebSockets — двустороннее соединение
- SignalR — реальное время в .NET
- Хабы, группы, потоковая передача

---

## Объяснение

### ЗАЧЕМ?
REST — запрос/ответ. А если нужно **мгновенно** получать данные? Чат, уведомления, мультиплеер. **SignalR** — "звонок", не "письмо".

### SignalR Server

```csharp
// Hub — серверный обработчик
class ChatHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        // Отправить ВСЕМ подключённым клиентам
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public async Task JoinRoom(string room)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, room);
        await Clients.Group(room).SendAsync("ReceiveMessage", "System", $"{Context.ConnectionId} joined {room}");
    }
}

// Регистрация
builder.Services.AddSignalR();
app.MapHub<ChatHub>("/chathub");
```

### SignalR Client (Uno Platform)

```csharp
using Microsoft.AspNetCore.SignalR.Client;

var connection = new HubConnectionBuilder()
    .WithUrl("https://localhost:5001/chathub")
    .WithAutomaticReconnect()
    .Build();

connection.On<string, string>("ReceiveMessage", (user, message) =>
{
    // Обновить UI
    Messages.Add($"{user}: {message}");
});

await connection.StartAsync();
await connection.InvokeAsync("SendMessage", "Алиса", "Привет!");
```

---

## Мини-упражнения
1. **⭐** Простой SignalR Hub + клиент.
2. **⭐⭐** Чат с комнатами (Groups).

## Что дальше
Дальше — **мультиплеер в Godot** (13.4).
