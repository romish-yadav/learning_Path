using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using LearnPath.API.Authentication.Jwt;
using LearnPath.API.Data;
using LearnPath.API.DTOs.Auth;
using LearnPath.API.Entities;
using LearnPath.API.Interfaces.Services;
using LearnPath.API.Responses;

namespace LearnPath.API.Services.Auth;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtTokenService _jwtService;
    private readonly JwtSettings _jwtSettings;
    private readonly ApplicationDbContext _context;

    public AuthService(
        UserManager<User> userManager,
        IJwtTokenService jwtService,
        IOptions<JwtSettings> jwtSettings,
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _jwtService = jwtService;
        _jwtSettings = jwtSettings.Value;
        _context = context;
    }

    public async Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterDto dto)
    {
        var existingUser = await _userManager.FindByEmailAsync(dto.Email);
        if (existingUser != null)
            return ApiResponse<AuthResponseDto>.Fail("A user with this email already exists.");

        var user = new User
        {
            UserName = dto.Email,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            return ApiResponse<AuthResponseDto>.Fail("Registration failed.", result.Errors.Select(e => e.Description));

        await _userManager.AddToRoleAsync(user, "Learner");
        return await GenerateAuthResponse(user);
    }

    public async Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            return ApiResponse<AuthResponseDto>.Fail("Invalid email or password.");

        return await GenerateAuthResponse(user);
    }

    public async Task<ApiResponse<AuthResponseDto>> RefreshTokenAsync(RefreshTokenDto dto)
    {
        var principal = _jwtService.GetPrincipalFromExpiredToken(dto.Token);
        if (principal == null)
            return ApiResponse<AuthResponseDto>.Fail("Invalid token.");

        var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return ApiResponse<AuthResponseDto>.Fail("Invalid token.");

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return ApiResponse<AuthResponseDto>.Fail("User not found.");

        var storedToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == dto.RefreshToken && rt.UserId == userId);

        if (storedToken == null || !storedToken.IsActive)
            return ApiResponse<AuthResponseDto>.Fail("Invalid or expired refresh token.");

        storedToken.RevokedAt = DateTime.UtcNow;
        var newRefreshToken = _jwtService.GenerateRefreshToken();
        storedToken.ReplacedByToken = newRefreshToken;

        _context.RefreshTokens.Add(new RefreshToken
        {
            Token = newRefreshToken,
            UserId = userId,
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays)
        });

        await _context.SaveChangesAsync();

        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = _jwtService.GenerateAccessToken(user, roles);

        return ApiResponse<AuthResponseDto>.Ok(new AuthResponseDto
        {
            Token = accessToken,
            RefreshToken = newRefreshToken,
            Expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
            User = MapToProfile(user, roles)
        });
    }

    public async Task<ApiResponse<bool>> RevokeTokenAsync(string userId)
    {
        var tokens = await _context.RefreshTokens
            .Where(rt => rt.UserId == userId && rt.RevokedAt == null)
            .ToListAsync();

        foreach (var token in tokens)
            token.RevokedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return ApiResponse<bool>.Ok(true, "All tokens revoked.");
    }

    public async Task<ApiResponse<UserProfileDto>> GetProfileAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return ApiResponse<UserProfileDto>.Fail("User not found.");

        var roles = await _userManager.GetRolesAsync(user);
        return ApiResponse<UserProfileDto>.Ok(MapToProfile(user, roles));
    }

    public async Task<ApiResponse<UserProfileDto>> UpdateProfileAsync(string userId, UpdateProfileDto dto)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return ApiResponse<UserProfileDto>.Fail("User not found.");

        if (dto.FirstName != null) user.FirstName = dto.FirstName;
        if (dto.LastName != null) user.LastName = dto.LastName;
        if (dto.AvatarUrl != null) user.AvatarUrl = dto.AvatarUrl;
        if (dto.Bio != null) user.Bio = dto.Bio;
        user.UpdatedAt = DateTime.UtcNow;

        await _userManager.UpdateAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        return ApiResponse<UserProfileDto>.Ok(MapToProfile(user, roles), "Profile updated.");
    }

    public async Task<ApiResponse<bool>> ChangePasswordAsync(string userId, ChangePasswordDto dto)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return ApiResponse<bool>.Fail("User not found.");

        var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
        if (!result.Succeeded)
            return ApiResponse<bool>.Fail("Password change failed.", result.Errors.Select(e => e.Description));

        return ApiResponse<bool>.Ok(true, "Password changed successfully.");
    }

    private async Task<ApiResponse<AuthResponseDto>> GenerateAuthResponse(User user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = _jwtService.GenerateAccessToken(user, roles);
        var refreshToken = _jwtService.GenerateRefreshToken();

        _context.RefreshTokens.Add(new RefreshToken
        {
            Token = refreshToken,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays)
        });
        await _context.SaveChangesAsync();

        return ApiResponse<AuthResponseDto>.Ok(new AuthResponseDto
        {
            Token = accessToken,
            RefreshToken = refreshToken,
            Expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
            User = MapToProfile(user, roles)
        });
    }

    private static UserProfileDto MapToProfile(User user, IEnumerable<string> roles) => new()
    {
        Id = user.Id,
        FirstName = user.FirstName,
        LastName = user.LastName,
        Email = user.Email ?? string.Empty,
        AvatarUrl = user.AvatarUrl,
        Bio = user.Bio,
        Roles = roles
    };
}
