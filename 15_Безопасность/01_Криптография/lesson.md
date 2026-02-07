# Тема 15.1: Криптография

## Что ты узнаешь
- Хеширование: SHA256, HMAC, PBKDF2
- Симметричное шифрование: AES
- Асимметричное шифрование: RSA (обзор)
- Безопасное хранение данных
- OWASP Top 10 — главные уязвимости

---

## Объяснение

### ЗАЧЕМ?

Пароли хранят **хешами**, не открытым текстом. Данные шифруют, чтобы даже при утечке файла злоумышленник ничего не прочитал. Это не "потом добавим" — это **с самого начала**.

### Хеширование — необратимое преобразование

```csharp
using System.Security.Cryptography;
using System.Text;

// SHA256 — для проверки целостности (НЕ для паролей!)
byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes("Hello"));
string hexHash = Convert.ToHexString(hash);
// "185F8DB32271FE25F561A6FC938B2E264306EC304EDA518007D1764826381969"

// Один и тот же вход → всегда один хеш
// Невозможно из хеша получить исходные данные
// Малейшее изменение → совершенно другой хеш
```

### Хеширование паролей — PBKDF2 (правильно!)

```csharp
// ❌ НИКОГДА не храни пароли так:
string password = "mypassword";           // открытый текст
string md5 = ComputeMD5(password);        // MD5 — сломан
string sha = ComputeSHA256(password);     // SHA без соли — rainbow tables

// ✅ Правильно — PBKDF2 с солью
public static class PasswordHasher
{
    private const int SaltSize = 16;    // 128 бит
    private const int HashSize = 32;    // 256 бит
    private const int Iterations = 100_000;

    public static string Hash(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            Iterations,
            HashAlgorithmName.SHA256,
            HashSize);

        // Сохраняем: итерации + соль + хеш (всё в одной строке)
        return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }

    public static bool Verify(string password, string storedHash)
    {
        string[] parts = storedHash.Split('.');
        int iterations = int.Parse(parts[0]);
        byte[] salt = Convert.FromBase64String(parts[1]);
        byte[] hash = Convert.FromBase64String(parts[2]);

        byte[] testHash = Rfc2898DeriveBytes.Pbkdf2(
            password, salt, iterations, HashAlgorithmName.SHA256, hash.Length);

        // Сравнение за постоянное время (защита от timing attack)
        return CryptographicOperations.FixedTimeEquals(hash, testHash);
    }
}

// Использование
string hashed = PasswordHasher.Hash("MyP@ssw0rd");
bool isValid = PasswordHasher.Verify("MyP@ssw0rd", hashed); // true
bool isWrong = PasswordHasher.Verify("wrong", hashed);       // false
```

### AES — симметричное шифрование

```csharp
// Один ключ для шифрования И расшифровки
public static class AesEncryptor
{
    // Шифрование
    public static (byte[] ciphertext, byte[] iv) Encrypt(string plaintext, byte[] key)
    {
        using var aes = Aes.Create();
        aes.Key = key;
        aes.GenerateIV(); // Случайный IV для каждого шифрования!

        using var encryptor = aes.CreateEncryptor();
        byte[] plainBytes = Encoding.UTF8.GetBytes(plaintext);
        byte[] cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

        return (cipherBytes, aes.IV);
    }

    // Расшифровка
    public static string Decrypt(byte[] ciphertext, byte[] key, byte[] iv)
    {
        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor();
        byte[] plainBytes = decryptor.TransformFinalBlock(ciphertext, 0, ciphertext.Length);

        return Encoding.UTF8.GetString(plainBytes);
    }
}

// Использование
byte[] key = RandomNumberGenerator.GetBytes(32); // 256 бит
var (encrypted, iv) = AesEncryptor.Encrypt("Секретные данные", key);
string decrypted = AesEncryptor.Decrypt(encrypted, key, iv);
```

### HMAC — проверка целостности + аутентичность

```csharp
// "Это сообщение действительно от меня и не было изменено"
byte[] key = RandomNumberGenerator.GetBytes(32);
byte[] data = Encoding.UTF8.GetBytes("save_data_here");
byte[] mac = HMACSHA256.HashData(key, data);

// Проверка
byte[] newMac = HMACSHA256.HashData(key, data);
bool isValid = CryptographicOperations.FixedTimeEquals(mac, newMac); // true
```

### Шифрование сохранений в Godot

```csharp
public partial class SaveManager : Node
{
    private byte[] _key = null!;

    public override void _Ready()
    {
        // Ключ из пароля (для игрового сохранения)
        _key = Rfc2898DeriveBytes.Pbkdf2(
            "game-secret-key", // В реальности — из конфига
            salt: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16],
            iterations: 10_000,
            HashAlgorithmName.SHA256,
            outputLength: 32);
    }

    public void SaveEncrypted(GameData data)
    {
        string json = JsonSerializer.Serialize(data);
        var (cipher, iv) = AesEncryptor.Encrypt(json, _key);

        // Сохраняем IV + данные
        string path = ProjectSettings.GlobalizePath("user://save.dat");
        using var file = File.Create(path);
        file.Write(iv);
        file.Write(cipher);
    }
}
```

### OWASP Top 10 — знай врага

```
1. Injection (SQL, Command)     → Параметризованные запросы!
2. Broken Authentication        → PBKDF2, 2FA, rate limiting
3. Sensitive Data Exposure      → Шифрование, HTTPS
4. XML External Entities (XXE)  → Отключи DTD
5. Broken Access Control        → Проверяй права на сервере
6. Security Misconfiguration    → Не оставляй дефолты
7. XSS (Cross-Site Scripting)   → Экранируй вывод
8. Insecure Deserialization     → Не десериализуй из недоверенных источников
9. Known Vulnerabilities        → Обновляй зависимости
10. Insufficient Logging        → Логируй подозрительное
```

### Главные правила

```
✅ ВСЕГДА:
- Хешируй пароли (PBKDF2/bcrypt/Argon2)
- Генерируй случайные соли и IV
- Используй CryptographicOperations.FixedTimeEquals
- Храни ключи отдельно от данных
- Используй HTTPS

❌ НИКОГДА:
- Не храни пароли открытым текстом
- Не используй MD5/SHA1 для паролей
- Не хардкодь ключи в коде
- Не изобретай свою криптографию
- Не используй ECB режим AES
```

---

## Мини-упражнения

1. **⭐** Захешируй пароль через PBKDF2, проверь правильный и неправильный.
2. **⭐⭐** Зашифруй и расшифруй строку через AES. Проверь, что неправильный ключ не работает.
3. **⭐⭐** HMAC: подпиши файл сохранения, проверь что изменение данных ломает подпись.

## Что дальше
Дальше — **аутентификация: JWT и OAuth** (15.2).
