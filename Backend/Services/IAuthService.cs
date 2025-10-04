using Backend.Models.DTOs;
using Backend.Models.Domain;

namespace Backend.Services;

public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(LoginRequestDto loginRequest);
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto registerRequest);
    Task<AuthResponseDto> RefreshTokenAsync(string refreshToken);
    Task<AuthResponseDto> GoogleLoginAsync(string googleToken);
    Task<User?> ValidateGoogleTokenAsync(string googleToken);
    Task<bool> ChangePasswordAsync(int userId, ChangePasswordRequestDto changePasswordRequest);
    Task<bool> LogoutAsync(string refreshToken);
    string GenerateJwtToken(User user);
    string GenerateRefreshToken();
}
