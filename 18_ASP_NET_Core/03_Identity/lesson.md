# Тема 18.3: ASP.NET Core Identity

## Что ты узнаешь
- ASP.NET Core Identity — система аутентификации
- Регистрация, логин, JWT-токены
- Роли, Claims, Policies
- Авторизация endpoint-ов

---

## Объяснение

### ЗАЧЕМ?

Каждое приложение с пользователями нуждается в: регистрации, логине, "забыл пароль", ролях (admin/user). **Identity** — готовая система от Microsoft. Не нужно писать с нуля.

### Настройка Identity

```csharp
// dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore

// User модель
public class AppUser : IdentityUser
{
    public string DisplayName { get; set; } = "";
    public int Level { get; set; }
}

// DbContext
public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }
}

// Program.cs
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=app.db"));

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireUppercase = true;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();
```

### Регистрация и логин (JWT)

```csharp
// Модели
record RegisterRequest(string Email, string DisplayName, string Password);
record LoginRequest(string Email, string Password);
record AuthResponse(string Token, string DisplayName);

// Endpoints
var auth = app.MapGroup("/api/auth").WithTags("Authentication");

auth.MapPost("/register", async (
    RegisterRequest request,
    UserManager<AppUser> userManager,
    TokenService tokenService) =>
{
    var user = new AppUser
    {
        Email = request.Email,
        UserName = request.Email,
        DisplayName = request.DisplayName
    };

    var result = await userManager.CreateAsync(user, request.Password);

    if (!result.Succeeded)
        return Results.BadRequest(result.Errors);

    await userManager.AddToRoleAsync(user, "User");
    var token = tokenService.GenerateToken(user);

    return Results.Ok(new AuthResponse(token, user.DisplayName));
});

auth.MapPost("/login", async (
    LoginRequest request,
    UserManager<AppUser> userManager,
    TokenService tokenService) =>
{
    var user = await userManager.FindByEmailAsync(request.Email);
    if (user is null)
        return Results.Unauthorized();

    var isValid = await userManager.CheckPasswordAsync(user, request.Password);
    if (!isValid)
        return Results.Unauthorized();

    var token = tokenService.GenerateToken(user);
    return Results.Ok(new AuthResponse(token, user.DisplayName));
});
```

### Роли, Claims, Policies

```csharp
// Роли — группы прав
await userManager.AddToRoleAsync(user, "Admin");
await userManager.AddToRoleAsync(user, "Moderator");

// Claims — конкретные утверждения о пользователе
await userManager.AddClaimAsync(user, new Claim("permission", "delete_posts"));
await userManager.AddClaimAsync(user, new Claim("subscription", "premium"));

// Policies — правила авторизации
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));

    options.AddPolicy("PremiumUser", policy =>
        policy.RequireClaim("subscription", "premium"));

    options.AddPolicy("CanDeletePosts", policy =>
        policy.RequireClaim("permission", "delete_posts"));

    options.AddPolicy("MinLevel10", policy =>
        policy.RequireAssertion(context =>
        {
            var levelClaim = context.User.FindFirst("level")?.Value;
            return int.TryParse(levelClaim, out int level) && level >= 10;
        }));
});

// Применение
app.MapDelete("/api/posts/{id}", (int id) => { /* ... */ })
    .RequireAuthorization("CanDeletePosts");

app.MapGet("/api/admin/dashboard", () => { /* ... */ })
    .RequireAuthorization("AdminOnly");

app.MapGet("/api/premium/content", () => { /* ... */ })
    .RequireAuthorization("PremiumUser");
```

### Получение текущего пользователя

```csharp
app.MapGet("/api/me", async (
    ClaimsPrincipal claims,
    UserManager<AppUser> userManager) =>
{
    var userId = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    if (userId is null) return Results.Unauthorized();

    var user = await userManager.FindByIdAsync(userId);
    return Results.Ok(new
    {
        user!.DisplayName,
        user.Email,
        user.Level
    });
}).RequireAuthorization();
```

### CORS (для доступа из Uno WASM/Godot)

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMyApp", policy =>
    {
        policy.WithOrigins("https://myapp.com", "http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // для SignalR
    });
});

app.UseCors("AllowMyApp");
```

---

## Мини-упражнения

1. **⭐** Настрой Identity, создай endpoint /register и /login.
2. **⭐⭐** Добавь роли (User, Admin), создай endpoint только для Admin.
3. **⭐⭐** Подключи Uno Platform клиент: логин → сохранение токена → запрос профиля.

## Что дальше
Дальше — **Background Services** (18.4).
