using LearnPath.API.DTOs.Auth;
using LearnPath.API.Responses;

namespace LearnPath.API.Interfaces.Services;

public interface IAuthService
{
    Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterDto dto);
    Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto dto);
    Task<ApiResponse<AuthResponseDto>> RefreshTokenAsync(RefreshTokenDto dto);
    Task<ApiResponse<bool>> RevokeTokenAsync(string userId);
    Task<ApiResponse<UserProfileDto>> GetProfileAsync(string userId);
    Task<ApiResponse<UserProfileDto>> UpdateProfileAsync(string userId, UpdateProfileDto dto);
    Task<ApiResponse<bool>> ChangePasswordAsync(string userId, ChangePasswordDto dto);
}
