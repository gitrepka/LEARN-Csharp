# Тема 15.2: Аутентификация — JWT и OAuth

## Что ты узнаешь
- Аутентификация vs Авторизация
- JWT (JSON Web Token) — структура и использование
- OAuth 2.0 — "Войти через Google"
- ASP.NET Core Identity (обзор)
- Безопасное хранение токенов

---

## Объяснение

### Аутентификация vs Авторизация

```
Аутентификация — КТО ты? (логин/пароль, токен)
Авторизация    — ЧТО тебе разрешено? (роли, права)

Пример:
1. Ты показываешь паспорт на входе → аутентификация
2. Охранник проверяет, пускают ли тебя в VIP → авторизация
```

### JWT — JSON Web Token

```
JWT = три части, разделённые точками:

eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiIxMjMiLCJuYW1lIjoiQm9iIn0.signature
      Header              Payload (Claims)            Подпись

Header:  {"alg": "HS256", "typ": "JWT"}
Payload: {"sub": "123", "name": "Bob", "exp": 1717000000}
Signature: HMACSHA256(header + "." + payload, secret)

⚠️ Payload НЕ зашифрован! Его может прочитать любой (Base64).
   Подпись гарантирует, что данные не подделаны.
```

### Генерация JWT (сервер)

```csharp
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

public class TokenService
{
    private readonly string _secretKey = "super-secret-key-at-least-32-bytes-long!!";

    public string GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role), // "Admin", "User"
            new Claim("level", user.Level.ToString()),
        };

        var token = new JwtSecurityToken(
            issuer: "myapp",
            audience: "myapp",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1), // Токен живёт 1 час
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
```

### Валидация JWT (сервер)

```csharp
// Program.cs — настройка
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "myapp",
            ValidateAudience = true,
            ValidAudience = "myapp",
            ValidateLifetime = true, // Проверяет exp
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("super-secret-key-at-least-32-bytes-long!!")),
        };
    });

builder.Services.AddAuthorization();

// Middleware
app.UseAuthentication();
app.UseAuthorization();

// Защищённый endpoint
app.MapGet("/api/profile", (ClaimsPrincipal user) =>
{
    var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    return Results.Ok(new { UserId = userId });
}).RequireAuthorization();

// Только для Admin
app.MapDelete("/api/users/{id}", (int id) =>
{
    // ...
}).RequireAuthorization(policy => policy.RequireRole("Admin"));
```

### Клиент — отправка JWT

```csharp
// Uno Platform / любой HttpClient
public class ApiClient
{
    private readonly HttpClient _client;
    private string? _token;

    public async Task LoginAsync(string email, string password)
    {
        var response = await _client.PostAsJsonAsync("/api/login",
            new { Email = email, Password = password });

        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
        _token = result!.Token;
    }

    public async Task<Profile?> GetProfileAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/profile");
        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", _token);

        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Profile>();
    }
}
```

### OAuth 2.0 — "Войти через Google/GitHub"

```
Пользователь НЕ даёт тебе свой пароль Google.
Вместо этого:

1. Пользователь нажимает "Войти через Google"
2. Редирект на Google: "Разрешить MyApp доступ к email?"
3. Пользователь соглашается
4. Google даёт MyApp временный код (authorization code)
5. MyApp обменивает код на access token (на сервере!)
6. MyApp запрашивает данные пользователя у Google API
7. MyApp создаёт свой JWT для пользователя

Ты не храниш пароль Google. Ты получаешь только то, что пользователь разрешил.
```

### Refresh Tokens

```
Access Token  — живёт 15 мин - 1 час (короткий)
Refresh Token — живёт дни/недели (длинный, хранится безопасно)

Когда access token истекает:
1. Клиент отправляет refresh token
2. Сервер выдаёт новый access token
3. Пользователю не нужно логиниться заново
```

### Безопасное хранение токенов

```
Uno Platform:
- Windows: Windows.Security.Credentials.PasswordVault
- Android/iOS: Secure Storage (Xamarin.Essentials → MAUI)
- WASM: localStorage (⚠️ уязвимо к XSS)

Godot:
- Шифрованный файл (AES из 15.1)
- НЕ храни в открытом JSON
```

```csharp
// Uno Platform — Secure Storage
using Windows.Security.Credentials;

public class TokenStorage
{
    private const string Resource = "MyApp";

    public void SaveToken(string token)
    {
        var vault = new PasswordVault();
        vault.Add(new PasswordCredential(Resource, "jwt", token));
    }

    public string? GetToken()
    {
        try
        {
            var vault = new PasswordVault();
            var credential = vault.Retrieve(Resource, "jwt");
            credential.RetrievePassword();
            return credential.Password;
        }
        catch { return null; }
    }
}
```

### Input Validation — первая линия обороны

```csharp
// ❌ SQL Injection
$"SELECT * FROM Users WHERE Name = '{userInput}'"
// Если userInput = "'; DROP TABLE Users; --" → всё удалено!

// ✅ Параметризованный запрос
command.CommandText = "SELECT * FROM Users WHERE Name = @name";
command.Parameters.AddWithValue("@name", userInput);

// ❌ XSS
$"<div>{userInput}</div>"
// Если userInput = "<script>alert('hacked')</script>"

// ✅ Экранирование (фреймворки делают автоматически)
// Razor, XAML Binding — автоматически экранируют
```

---

## Мини-упражнения

1. **⭐** Сгенерируй JWT с claims (имя, роль), декодируй на jwt.io.
2. **⭐⭐** Minimal API с JWT: /login выдаёт токен, /profile требует авторизацию.

## Практика для проектов

**Uno Platform:** Менеджер паролей — мастер-пароль хешируется, записи шифруются AES, автоочистка буфера обмена.

**Godot:** Зашифрованные сохранения — AES + HMAC для проверки целостности.

## Что дальше
Модуль 15 завершён! Дальше — **Модуль 16: Оптимизация**.
