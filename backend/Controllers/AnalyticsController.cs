using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LearnPath.API.Interfaces.Services;

namespace LearnPath.API.Controllers;

[ApiController]
[Route("api/v1/analytics")]
[Authorize]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsService _analyticsService;
    private readonly IProgressService _progressService;

    public AnalyticsController(IAnalyticsService analyticsService, IProgressService progressService)
    {
        _analyticsService = analyticsService;
        _progressService = progressService;
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _analyticsService.GetDashboardAsync(userId);
        return Ok(result);
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetUserAnalytics()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _analyticsService.GetUserAnalyticsAsync(userId);
        return Ok(result);
    }

    [HttpGet("classroom/{classroomId:guid}")]
    public async Task<IActionResult> GetClassroomAnalytics(Guid classroomId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _analyticsService.GetClassroomAnalyticsAsync(classroomId, userId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("recommendations")]
    public async Task<IActionResult> GetRecommendations()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _analyticsService.GetRecommendationsAsync(userId);
        return Ok(result);
    }

    [HttpPost("progress/{moduleId:guid}")]
    public async Task<IActionResult> UpdateProgress(Guid moduleId, [FromQuery] int percentage = 100)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _progressService.UpdateProgressAsync(userId, moduleId, percentage);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost("progress/{moduleId:guid}/complete")]
    public async Task<IActionResult> CompleteModule(Guid moduleId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _progressService.CompleteModuleAsync(userId, moduleId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("progress/{pathId:guid}/unlocked")]
    public async Task<IActionResult> GetUnlockedModules(Guid pathId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _progressService.GetUnlockedModulesAsync(userId, pathId);
        return Ok(result);
    }
}
