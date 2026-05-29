using System.Security.Claims;

namespace LearnPath.API.Helpers;

public static class TokenHelper
{
    public static string? GetUserId(ClaimsPrincipal user) =>
        user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    public static string? GetUserEmail(ClaimsPrincipal user) =>
        user.FindFirst(ClaimTypes.Email)?.Value;

    public static IEnumerable<string> GetUserRoles(ClaimsPrincipal user) =>
        user.FindAll(ClaimTypes.Role).Select(c => c.Value);

    public static bool IsInRole(ClaimsPrincipal user, string role) =>
        user.IsInRole(role);
}
