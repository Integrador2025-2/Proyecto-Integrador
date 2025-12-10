using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using Backend.Models.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Backend.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;
    private readonly IEmailService _emailService;

    // Redis para almacenamiento de refresh tokens
    private readonly StackExchange.Redis.IDatabase _redisDb;

    public AuthService(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IConfiguration configuration,
        ILogger<AuthService> logger,
        StackExchange.Redis.IConnectionMultiplexer redis,
        IEmailService emailService)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _configuration = configuration;
        _logger = logger;
        _redisDb = redis.GetDatabase();
        _emailService = emailService;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto loginRequest)
    {
        var user = await _userRepository.GetByEmailAsync(loginRequest.Email);
        
        if (user == null || !user.IsActive)
        {
            throw new UnauthorizedAccessException("Credenciales inválidas");
        }

        if (user.Provider != "local")
        {
            throw new UnauthorizedAccessException("Este usuario se registró con Google. Use el login con Google.");
        }

        if (!BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Credenciales inválidas");
        }

        // Forzamos 2FA por email: generamos código y token de 2FA, enviamos correo
        var twoFactorToken = GenerateTwoFactorToken();
        var code = GenerateNumericCode(6);

        var twoFaInfo = new TwoFactorInfo
        {
            UserId = user.Id,
            Code = code,
            ExpiresAt = DateTime.UtcNow.AddMinutes(10),
            Attempts = 0
        };
        await _redisDb.StringSetAsync($"twofa:{twoFactorToken}", JsonSerializer.Serialize(twoFaInfo), TimeSpan.FromMinutes(10));

        var masked = MaskEmail(user.Email);
        var subject = "Tu código de verificación";
        var body = $"<p>Tu código de verificación es <strong>{code}</strong>. Expira en 10 minutos.</p>";
        try
        {
            await _emailService.SendEmailAsync(user.Email, subject, body);
        }
        catch (Exception ex)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
            if (env.Equals("Development", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning(ex, "Fallo al enviar correo 2FA en Development. Exponiendo código en logs para pruebas: {Code}", code);
            }
            else
            {
                throw;
            }
        }

        // Lanzamos excepción controlada para que el controlador pueda responder adecuadamente si usa LoginAsync directo
        throw new UnauthorizedAccessException("2FA requerido");
    }

    public async Task<TwoFactorInitResponseDto> InitiateTwoFactorAsync(LoginRequestDto loginRequest)
    {
        var user = await _userRepository.GetByEmailAsync(loginRequest.Email);
        if (user == null || !user.IsActive)
        {
            throw new UnauthorizedAccessException("Credenciales inválidas");
        }
        if (user.Provider != "local")
        {
            throw new UnauthorizedAccessException("Este usuario se registró con Google. Use el login con Google.");
        }
        if (!BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Credenciales inválidas");
        }

        var twoFactorToken = GenerateTwoFactorToken();
        var code = GenerateNumericCode(6);
        var twoFaInfo = new TwoFactorInfo
        {
            UserId = user.Id,
            Code = code,
            ExpiresAt = DateTime.UtcNow.AddMinutes(10),
            Attempts = 0
        };
        await _redisDb.StringSetAsync($"twofa:{twoFactorToken}", JsonSerializer.Serialize(twoFaInfo), TimeSpan.FromMinutes(10));

        var masked = MaskEmail(user.Email);
        var subject = "Tu código de verificación";
        var body = $"<p>Tu código de verificación es <strong>{code}</strong>. Expira en 10 minutos.</p>";
        try
        {
            await _emailService.SendEmailAsync(user.Email, subject, body);
        }
        catch (Exception ex)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
            if (env.Equals("Development", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning(ex, "Fallo al enviar correo 2FA en Development. Exponiendo código en logs para pruebas: {Code}", code);
            }
            else
            {
                throw;
            }
        }

        return new TwoFactorInitResponseDto
        {
            TwoFactorRequired = true,
            TwoFactorToken = twoFactorToken,
            DeliveryChannel = "email",
            MaskedDestination = masked
        };
    }

    public async Task<AuthResponseDto> VerifyTwoFactorAsync(TwoFactorVerifyRequestDto verifyRequest)
    {
        var key = $"twofa:{verifyRequest.TwoFactorToken}";
        var json = await _redisDb.StringGetAsync(key);
        if (string.IsNullOrEmpty(json))
        {
            throw new UnauthorizedAccessException("Código inválido o expirado");
        }
        var info = JsonSerializer.Deserialize<TwoFactorInfo>(json!);
        if (info == null || info.ExpiresAt < DateTime.UtcNow)
        {
            await _redisDb.KeyDeleteAsync(key);
            throw new UnauthorizedAccessException("Código inválido o expirado");
        }
        if (!string.Equals(info.Code, verifyRequest.Code, StringComparison.Ordinal))
        {
            info.Attempts += 1;
            // Persist attempt count but keep original expiry
            var ttl = await _redisDb.KeyTimeToLiveAsync(key) ?? TimeSpan.FromMinutes(10);
            await _redisDb.StringSetAsync(key, JsonSerializer.Serialize(info), ttl);
            throw new UnauthorizedAccessException("Código incorrecto");
        }

        // Código correcto: emitir tokens y limpiar estado 2FA
        var user = await _userRepository.GetByIdAsync(info.UserId);
        if (user == null || !user.IsActive)
        {
            await _redisDb.KeyDeleteAsync(key);
            throw new UnauthorizedAccessException("Usuario no válido");
        }

        var token = GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken();
        var expiresAt = DateTime.UtcNow.AddMinutes(GetJwtExpirationMinutes());

        var tokenInfo = new RefreshTokenInfo
        {
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow
        };
        await _redisDb.StringSetAsync($"refresh_token:{refreshToken}", JsonSerializer.Serialize(tokenInfo), TimeSpan.FromDays(7));
        await _redisDb.KeyDeleteAsync(key);

        return new AuthResponseDto
        {
            Token = token,
            RefreshToken = refreshToken,
            ExpiresAt = expiresAt,
            User = new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                RoleId = user.RoleId,
                RoleName = user.Role?.Name ?? string.Empty,
                Provider = user.Provider,
                ProfilePictureUrl = user.ProfilePictureUrl
            }
        };
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto registerRequest)
    {
        // Verificar si el email ya existe
        var existingUser = await _userRepository.GetByEmailAsync(registerRequest.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("El email ya está registrado");
        }

        // Verificar si el rol existe
        var role = await _roleRepository.GetByIdAsync(registerRequest.RoleId);
        if (role == null || !role.IsActive)
        {
            throw new InvalidOperationException("El rol especificado no existe o no está activo");
        }

        var user = new User
        {
            FirstName = registerRequest.FirstName,
            LastName = registerRequest.LastName,
            Email = registerRequest.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password),
            RoleId = registerRequest.RoleId,
            Provider = "local",
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        var createdUser = await _userRepository.CreateAsync(user);

        var token = GenerateJwtToken(createdUser);
        var refreshToken = GenerateRefreshToken();
        var expiresAt = DateTime.UtcNow.AddMinutes(GetJwtExpirationMinutes());

        // Store refresh token en Redis
        try
        {
            var tokenInfo = new RefreshTokenInfo
            {
                UserId = createdUser.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow
            };
            _redisDb.StringSet($"refresh_token:{refreshToken}", System.Text.Json.JsonSerializer.Serialize(tokenInfo), TimeSpan.FromDays(7));
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error guardando refresh token en Redis, continuando sin él");
        }

        return new AuthResponseDto
        {
            Token = token,
            RefreshToken = refreshToken,
            ExpiresAt = expiresAt,
            User = new UserDto
            {
                Id = createdUser.Id,
                FirstName = createdUser.FirstName,
                LastName = createdUser.LastName,
                Email = createdUser.Email,
                IsActive = createdUser.IsActive,
                CreatedAt = createdUser.CreatedAt,
                UpdatedAt = createdUser.UpdatedAt,
                RoleId = createdUser.RoleId,
                RoleName = createdUser.Role?.Name ?? role.Name,
                Provider = createdUser.Provider,
                ProfilePictureUrl = createdUser.ProfilePictureUrl
            }
        };
    }

    public async Task<AuthResponseDto> GoogleLoginAsync(string googleToken)
    {
        var user = await ValidateGoogleTokenAsync(googleToken);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Token de Google inválido");
        }

        var token = GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken();
        var expiresAt = DateTime.UtcNow.AddMinutes(GetJwtExpirationMinutes());

        // Store refresh token en Redis
        var tokenInfo = new RefreshTokenInfo
        {
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow
        };
        _redisDb.StringSet($"refresh_token:{refreshToken}", System.Text.Json.JsonSerializer.Serialize(tokenInfo), TimeSpan.FromDays(7));

        return new AuthResponseDto
        {
            Token = token,
            RefreshToken = refreshToken,
            ExpiresAt = expiresAt,
            User = new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                RoleId = user.RoleId,
                RoleName = user.Role?.Name ?? string.Empty,
                Provider = user.Provider,
                ProfilePictureUrl = user.ProfilePictureUrl
            }
        };
    }

    public async Task<User?> ValidateGoogleTokenAsync(string googleToken)
    {
        try
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"https://www.googleapis.com/oauth2/v2/userinfo?access_token={googleToken}");
            
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            var googleUser = JsonSerializer.Deserialize<GoogleUserInfo>(json);

            if (googleUser == null)
            {
                return null;
            }

            // Buscar usuario existente por GoogleId o Email
            var existingUser = await _userRepository.GetByGoogleIdAsync(googleUser.Id) 
                             ?? await _userRepository.GetByEmailAsync(googleUser.Email);

            if (existingUser != null)
            {
                // Actualizar información si es necesario
                if (existingUser.GoogleId != googleUser.Id)
                {
                    existingUser.GoogleId = googleUser.Id;
                    existingUser.Provider = "google";
                    await _userRepository.UpdateAsync(existingUser);
                }
                return existingUser;
            }

            // Crear nuevo usuario
            var defaultRole = await _roleRepository.GetByIdAsync(2); // Rol "Usuario" por defecto
            if (defaultRole == null)
            {
                throw new InvalidOperationException("No se encontró el rol por defecto");
            }

            var newUser = new User
            {
                FirstName = googleUser.GivenName,
                LastName = googleUser.FamilyName,
                Email = googleUser.Email,
                GoogleId = googleUser.Id,
                Provider = "google",
                ProfilePictureUrl = googleUser.Picture,
                RoleId = defaultRole.Id,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _userRepository.CreateAsync(newUser);
            return newUser;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating Google token");
            return null;
        }
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
    {
        // Obtener refresh token de Redis
        var tokenInfoJson = await _redisDb.StringGetAsync($"refresh_token:{refreshToken}");
        if (string.IsNullOrEmpty(tokenInfoJson))
        {
            throw new UnauthorizedAccessException("Token de actualización inválido");
        }
        var tokenInfo = System.Text.Json.JsonSerializer.Deserialize<RefreshTokenInfo>(tokenInfoJson!);
        if (tokenInfo == null || tokenInfo.ExpiresAt < DateTime.UtcNow)
        {
            await _redisDb.KeyDeleteAsync($"refresh_token:{refreshToken}");
            throw new UnauthorizedAccessException("Token de actualización expirado");
        }
        var user = await _userRepository.GetByIdAsync(tokenInfo.UserId);
        if (user == null || !user.IsActive)
        {
            await _redisDb.KeyDeleteAsync($"refresh_token:{refreshToken}");
            throw new UnauthorizedAccessException("Usuario no encontrado o inactivo");
        }
        // Generate new tokens
        var newToken = GenerateJwtToken(user);
        var newRefreshToken = GenerateRefreshToken();
        var expiresAt = DateTime.UtcNow.AddMinutes(GetJwtExpirationMinutes());
        // Remove old refresh token and store new one
        await _redisDb.KeyDeleteAsync($"refresh_token:{refreshToken}");
        var newTokenInfo = new RefreshTokenInfo
        {
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow
        };
        await _redisDb.StringSetAsync($"refresh_token:{newRefreshToken}", System.Text.Json.JsonSerializer.Serialize(newTokenInfo), TimeSpan.FromDays(7));
        return new AuthResponseDto
        {
            Token = newToken,
            RefreshToken = newRefreshToken,
            ExpiresAt = expiresAt,
            User = new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                RoleId = user.RoleId,
                RoleName = user.Role?.Name ?? string.Empty,
                Provider = user.Provider,
                ProfilePictureUrl = user.ProfilePictureUrl
            }
        };
    }

    public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordRequestDto changePasswordRequest)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null || !user.IsActive)
        {
            return false;
        }

        if (user.Provider != "local")
        {
            throw new InvalidOperationException("No se puede cambiar la contraseña para usuarios de Google");
        }

        if (!BCrypt.Net.BCrypt.Verify(changePasswordRequest.CurrentPassword, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("La contraseña actual es incorrecta");
        }

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(changePasswordRequest.NewPassword);
        user.UpdatedAt = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user);
        return true;
    }

    public async Task<bool> LogoutAsync(string refreshToken)
    {
        return await _redisDb.KeyDeleteAsync($"refresh_token:{refreshToken}");
    }

    public string GenerateJwtToken(User user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? "your-super-secret-key-that-is-at-least-32-characters-long";
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.FullName),
            new("role_id", user.RoleId.ToString()),
            new("role_name", user.Role?.Name ?? ""),
            new("provider", user.Provider ?? "local")
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"] ?? "ProyectoIntegrador",
            audience: jwtSettings["Audience"] ?? "ProyectoIntegrador",
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(GetJwtExpirationMinutes()),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomBytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    private int GetJwtExpirationMinutes()
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        return int.TryParse(jwtSettings["ExpirationMinutes"], out var minutes) ? minutes : 60;
    }

    private class RefreshTokenInfo
    {
        public int UserId { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    private class GoogleUserInfo
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string GivenName { get; set; } = string.Empty;
        public string FamilyName { get; set; } = string.Empty;
        public string Picture { get; set; } = string.Empty;
    }

    private class TwoFactorInfo
    {
        public int UserId { get; set; }
        public string Code { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public int Attempts { get; set; }
    }

    private static string GenerateTwoFactorToken()
    {
        var bytes = new byte[24];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }

    private static string GenerateNumericCode(int length)
    {
        var rng = RandomNumberGenerator.Create();
        var digits = new char[length];
        var buffer = new byte[1];
        for (int i = 0; i < length; i++)
        {
            do { rng.GetBytes(buffer); } while (buffer[0] >= 250);
            digits[i] = (char)('0' + (buffer[0] % 10));
        }
        return new string(digits);
    }

    private static string MaskEmail(string email)
    {
        var atIndex = email.IndexOf('@');
        if (atIndex <= 1) return "***";
        var name = email.Substring(0, atIndex);
        var domain = email.Substring(atIndex);
        var visible = Math.Min(2, name.Length);
        return name.Substring(0, visible) + new string('*', Math.Max(0, name.Length - visible)) + domain;
    }

    /// <summary>
    /// [SOLO DESARROLLO] Login directo sin 2FA para pruebas
    /// </summary>
    public async Task<AuthResponseDto> DevLoginAsync(LoginRequestDto loginRequest)
    {
        var user = await _userRepository.GetByEmailAsync(loginRequest.Email);
        
        if (user == null || !user.IsActive)
        {
            throw new UnauthorizedAccessException("Credenciales inválidas");
        }

        if (user.Provider != "local")
        {
            throw new UnauthorizedAccessException("Este usuario se registró con Google. Use el login con Google.");
        }

        if (!BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Credenciales inválidas");
        }

        // Generar JWT directamente sin 2FA
        var token = GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken();

        var refreshTokenInfo = new RefreshTokenInfo
        {
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow
        };

        await _redisDb.StringSetAsync(
            $"refresh_token:{refreshToken}", 
            JsonSerializer.Serialize(refreshTokenInfo), 
            TimeSpan.FromDays(7)
        );

        _logger.LogInformation("Dev login exitoso para usuario {UserId}", user.Id);

        return new AuthResponseDto
        {
            Token = token,
            RefreshToken = refreshToken,
            User = new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                IsActive = user.IsActive,
                RoleId = user.RoleId,
                RoleName = user.Role?.Name ?? "",
                Provider = user.Provider ?? "local"
            }
        };
    }
}
