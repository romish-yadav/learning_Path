using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LearnPath.API.DTOs.Common;
using LearnPath.API.DTOs.Community;
using LearnPath.API.Interfaces.Services;

namespace LearnPath.API.Controllers;

[ApiController]
[Route("api/v1/community")]
public class CommunityController : ControllerBase
{
    private readonly ICommunityService _communityService;
    private readonly INotificationService _notificationService;

    public CommunityController(ICommunityService communityService, INotificationService notificationService)
    {
        _communityService = communityService;
        _notificationService = notificationService;
    }

    [HttpGet("comments/{targetType}/{targetId:guid}")]
    public async Task<IActionResult> GetComments(string targetType, Guid targetId, [FromQuery] PaginationParams pagination)
    {
        var result = await _communityService.GetCommentsAsync(targetType, targetId, pagination);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("comments")]
    public async Task<IActionResult> AddComment([FromBody] CreateCommentDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _communityService.AddCommentAsync(userId, dto);
        return result.Success ? CreatedAtAction(nameof(GetComments), new { targetType = dto.TargetType, targetId = dto.TargetId }, result) : BadRequest(result);
    }

    [Authorize]
    [HttpDelete("comments/{id:guid}")]
    public async Task<IActionResult> DeleteComment(Guid id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _communityService.DeleteCommentAsync(id, userId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpGet("ratings/{pathId:guid}")]
    public async Task<IActionResult> GetRatings(Guid pathId, [FromQuery] PaginationParams pagination)
    {
        var result = await _communityService.GetRatingsAsync(pathId, pagination);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("ratings/{pathId:guid}")]
    public async Task<IActionResult> AddRating(Guid pathId, [FromBody] CreateRatingDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _communityService.AddRatingAsync(userId, pathId, dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [Authorize]
    [HttpGet("notifications")]
    public async Task<IActionResult> GetNotifications([FromQuery] PaginationParams pagination, [FromQuery] bool? isRead = null)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _notificationService.GetNotificationsAsync(userId, pagination, isRead);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("notifications/unread-count")]
    public async Task<IActionResult> GetUnreadCount()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _notificationService.GetUnreadCountAsync(userId);
        return Ok(result);
    }

    [Authorize]
    [HttpPut("notifications/{id:guid}/read")]
    public async Task<IActionResult> MarkAsRead(Guid id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _notificationService.MarkAsReadAsync(id, userId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [Authorize]
    [HttpPut("notifications/read-all")]
    public async Task<IActionResult> MarkAllAsRead()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _notificationService.MarkAllAsReadAsync(userId);
        return Ok(result);
    }
}
