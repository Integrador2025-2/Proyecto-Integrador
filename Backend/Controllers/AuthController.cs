using Backend.Models.DTOs;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Registra un nuevo usuario
    /// </summary>
    /// <param name="registerRequest">Datos de registro</param>
    /// <returns>Token de autenticación y datos del usuario</returns>
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterRequestDto registerRequest)
    {
        try
        {
            var result = await _authService.RegisterAsync(registerRequest);
            _logger.LogInformation("Usuario registrado exitosamente: {Email}", registerRequest.Email);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Error en registro: {Message}", ex.Message);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado durante el registro");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Inicia el login y envía el código 2FA al email
    /// </summary>
    /// <param name="loginRequest">Credenciales de login</param>
    /// <returns>Token temporal y destino enmascarado</returns>
    [HttpPost("login/init")]
    public async Task<ActionResult<TwoFactorInitResponseDto>> LoginInit([FromBody] LoginRequestDto loginRequest)
    {
        try
        {
            var result = await _authService.InitiateTwoFactorAsync(loginRequest);
            _logger.LogInformation("2FA iniciado para usuario: {Email}", loginRequest.Email);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Inicio de sesión fallido para {Email}: {Message}", loginRequest.Email, ex.Message);
            return Unauthorized(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado durante el inicio de 2FA");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Verifica el código 2FA y emite el JWT
    /// </summary>
    /// <param name="verifyRequest">Token temporal y código</param>
    /// <returns>Token de autenticación y datos del usuario</returns>
    [HttpPost("2fa/verify")]
    public async Task<ActionResult<AuthResponseDto>> VerifyTwoFactor([FromBody] TwoFactorVerifyRequestDto verifyRequest)
    {
        try
        {
            var result = await _authService.VerifyTwoFactorAsync(verifyRequest);
            _logger.LogInformation("2FA verificado correctamente");
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Verificación 2FA fallida: {Message}", ex.Message);
            return Unauthorized(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado durante la verificación 2FA");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Inicia sesión con Google OAuth
    /// </summary>
    /// <param name="googleAuthRequest">Token de Google</param>
    /// <returns>Token de autenticación y datos del usuario</returns>
    [HttpPost("google-login")]
    public async Task<ActionResult<AuthResponseDto>> GoogleLogin([FromBody] GoogleAuthRequestDto googleAuthRequest)
    {
        try
        {
            var result = await _authService.GoogleLoginAsync(googleAuthRequest.GoogleToken);
            _logger.LogInformation("Login con Google exitoso");
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Login con Google fallido: {Message}", ex.Message);
            return Unauthorized(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado durante el login con Google");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Renueva el token de acceso usando el refresh token
    /// </summary>
    /// <param name="refreshTokenRequest">Refresh token</param>
    /// <returns>Nuevo token de autenticación</returns>
    [HttpPost("refresh-token")]
    public async Task<ActionResult<AuthResponseDto>> RefreshToken([FromBody] RefreshTokenRequestDto refreshTokenRequest)
    {
        try
        {
            var result = await _authService.RefreshTokenAsync(refreshTokenRequest.RefreshToken);
            _logger.LogInformation("Token renovado exitosamente");
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Renovación de token fallida: {Message}", ex.Message);
            return Unauthorized(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado durante la renovación del token");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Cierra sesión y invalida el refresh token
    /// </summary>
    /// <param name="refreshTokenRequest">Refresh token a invalidar</param>
    /// <returns>Resultado del logout</returns>
    [HttpPost("logout")]
    [Authorize]
    public async Task<ActionResult> Logout([FromBody] RefreshTokenRequestDto refreshTokenRequest)
    {
        try
        {
            var success = await _authService.LogoutAsync(refreshTokenRequest.RefreshToken);
            if (success)
            {
                _logger.LogInformation("Logout exitoso para usuario: {UserId}", GetCurrentUserId());
                return Ok(new { message = "Logout exitoso" });
            }
            return BadRequest("Token de actualización no válido");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado durante el logout");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Cambia la contraseña del usuario autenticado
    /// </summary>
    /// <param name="changePasswordRequest">Datos para cambio de contraseña</param>
    /// <returns>Resultado del cambio de contraseña</returns>
    [HttpPost("change-password")]
    [Authorize]
    public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordRequestDto changePasswordRequest)
    {
        try
        {
            var userId = GetCurrentUserId();
            var success = await _authService.ChangePasswordAsync(userId, changePasswordRequest);
            
            if (success)
            {
                _logger.LogInformation("Contraseña cambiada exitosamente para usuario: {UserId}", userId);
                return Ok(new { message = "Contraseña cambiada exitosamente" });
            }
            
            return BadRequest("No se pudo cambiar la contraseña");
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Cambio de contraseña fallido: {Message}", ex.Message);
            return Unauthorized(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Cambio de contraseña fallido: {Message}", ex.Message);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado durante el cambio de contraseña");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene la información del usuario autenticado
    /// </summary>
    /// <returns>Datos del usuario actual</returns>
    [HttpGet("me")]
    [Authorize]
    public ActionResult<UserDto> GetCurrentUser()
    {
        try
        {
            var userId = GetCurrentUserId();
            var email = GetCurrentUserEmail();
            var name = GetCurrentUserName();
            var roleId = GetCurrentUserRoleId();
            var roleName = GetCurrentUserRoleName();
            var provider = GetCurrentUserProvider();

            var userDto = new UserDto
            {
                Id = userId,
                Email = email,
                FirstName = name.Split(' ').FirstOrDefault() ?? "",
                LastName = name.Split(' ').Skip(1).FirstOrDefault() ?? "",
                FullName = name,
                RoleId = roleId,
                RoleName = roleName,
                Provider = provider
            };

            return Ok(userDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo información del usuario actual");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene la URL para autenticación con Google (para el frontend)
    /// </summary>
    /// <returns>URL de autenticación con Google</returns>
    [HttpGet("google-auth-url")]
    public ActionResult GetGoogleAuthUrl()
    {
        try
        {
            // Esta URL debería ser configurada en el frontend
            var clientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID") ?? "your-google-client-id";
            var redirectUri = Environment.GetEnvironmentVariable("GOOGLE_REDIRECT_URI") ?? "http://localhost:3000/auth/google/callback";
            
            var authUrl = $"https://accounts.google.com/o/oauth2/v2/auth?" +
                         $"client_id={clientId}&" +
                         $"redirect_uri={Uri.EscapeDataString(redirectUri)}&" +
                         $"scope=email profile&" +
                         $"response_type=code&" +
                         $"access_type=offline";

            return Ok(new { authUrl });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generando URL de Google Auth");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    #region Helper Methods

    private int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(userIdClaim, out var userId) ? userId : 0;
    }

    private string GetCurrentUserEmail()
    {
        return User.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
    }

    private string GetCurrentUserName()
    {
        return User.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
    }

    private int GetCurrentUserRoleId()
    {
        var roleIdClaim = User.FindFirst("role_id")?.Value;
        return int.TryParse(roleIdClaim, out var roleId) ? roleId : 0;
    }

    private string GetCurrentUserRoleName()
    {
        return User.FindFirst("role_name")?.Value ?? string.Empty;
    }

    private string GetCurrentUserProvider()
    {
        return User.FindFirst("provider")?.Value ?? "local";
    }

    #endregion
}
