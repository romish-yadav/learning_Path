using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LearnPath.API.Data;
using LearnPath.API.DTOs.Auth;
using LearnPath.API.DTOs.Common;
using LearnPath.API.Entities;
using LearnPath.API.Responses;

namespace LearnPath.API.Controllers;

[ApiController]
[Route("api/v1/admin")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly ApplicationDbContext _context;

    public AdminController(UserManager<User> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers([FromQuery] SearchParams search)
    {
        var query = _context.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search.Query))
            query = query.Where(u =>
                u.FirstName.Contains(search.Query) ||
                u.LastName.Contains(search.Query) ||
                u.Email!.Contains(search.Query));

        var total = await query.CountAsync();
        var users = await query
            .OrderBy(u => u.FirstName)
            .Skip((search.Page - 1) * search.PageSize)
            .Take(search.PageSize)
            .ToListAsync();

        var userDtos = new List<UserProfileDto>();
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            userDtos.Add(new UserProfileDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email ?? string.Empty,
                AvatarUrl = user.AvatarUrl,
                Bio = user.Bio,
                Roles = roles
            });
        }

        return Ok(new PagedResponse<UserProfileDto>
        {
            Data = userDtos,
            Page = search.Page,
            PageSize = search.PageSize,
            TotalCount = total
        });
    }

    [HttpPost("users/{userId}/role")]
    public async Task<IActionResult> AssignRole(string userId, [FromBody] string role)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return NotFound(ApiResponse<bool>.Fail("User not found."));

        var result = await _userManager.AddToRoleAsync(user, role);
        return result.Succeeded
            ? Ok(ApiResponse<bool>.Ok(true, $"Role '{role}' assigned."))
            : BadRequest(ApiResponse<bool>.Fail("Failed to assign role.", result.Errors.Select(e => e.Description)));
    }

    [HttpDelete("users/{userId}/role")]
    public async Task<IActionResult> RemoveRole(string userId, [FromBody] string role)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return NotFound(ApiResponse<bool>.Fail("User not found."));

        var result = await _userManager.RemoveFromRoleAsync(user, role);
        return result.Succeeded
            ? Ok(ApiResponse<bool>.Ok(true, $"Role '{role}' removed."))
            : BadRequest(ApiResponse<bool>.Fail("Failed to remove role."));
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        var stats = new
        {
            TotalUsers = await _context.Users.CountAsync(),
            TotalPaths = await _context.LearningPaths.CountAsync(),
            TotalClassrooms = await _context.Classrooms.CountAsync(),
            TotalModules = await _context.Modules.CountAsync(),
            PublishedPaths = await _context.LearningPaths.CountAsync(lp => lp.IsPublished)
        };

        return Ok(ApiResponse<object>.Ok(stats));
    }
}
