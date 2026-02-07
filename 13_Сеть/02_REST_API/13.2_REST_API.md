# Тема 13.2: REST API

## Что ты узнаешь
- REST принципы: ресурсы, HTTP-методы, статус-коды
- Создание REST API (ASP.NET Core Minimal API)
- Потребление REST API из клиента

---

## Объяснение

### REST — стиль архитектуры API

```
GET    /api/players        → получить список
GET    /api/players/1      → получить игрока с id=1
POST   /api/players        → создать нового
PUT    /api/players/1      → обновить полностью
PATCH  /api/players/1      → обновить частично
DELETE /api/players/1      → удалить

Статус-коды:
200 OK          — успех
201 Created     — создано
204 No Content  — успех без тела
400 Bad Request — плохой запрос
401 Unauthorized — не авторизован
404 Not Found   — не найдено
500 Server Error — ошибка сервера
```

### Minimal API (ASP.NET Core)

```csharp
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<Player> players = [new(1, "Алиса", 15), new(2, "Боб", 10)];

app.MapGet("/api/players", () => players);
app.MapGet("/api/players/{id}", (int id) =>
    players.FirstOrDefault(p => p.Id == id) is Player p
        ? Results.Ok(p) : Results.NotFound());
app.MapPost("/api/players", (Player player) => {
    players.Add(player);
    return Results.Created($"/api/players/{player.Id}", player);
});
app.MapDelete("/api/players/{id}", (int id) => {
    players.RemoveAll(p => p.Id == id);
    return Results.NoContent();
});

app.Run();
record Player(int Id, string Name, int Level);
```

---

## Мини-упражнения
1. **⭐** Создай Minimal API с CRUD для одной сущности.
2. **⭐** Клиент: HttpClient для потребления этого API.

## Что дальше
Дальше — **WebSockets и SignalR** (13.3).
