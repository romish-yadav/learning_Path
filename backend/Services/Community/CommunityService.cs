using Microsoft.AspNetCore.Identity;
using LearnPath.API.DTOs.Common;
using LearnPath.API.DTOs.Community;
using LearnPath.API.Entities;
using LearnPath.API.Interfaces.Repositories;
using LearnPath.API.Interfaces.Services;
using LearnPath.API.Responses;

namespace LearnPath.API.Services.Community;

public class CommunityService : ICommunityService
{
    private readonly ICommentRepository _commentRepo;
    private readonly IRatingRepository _ratingRepo;
    private readonly UserManager<User> _userManager;

    public CommunityService(
        ICommentRepository commentRepo,
        IRatingRepository ratingRepo,
        UserManager<User> userManager)
    {
        _commentRepo = commentRepo;
        _ratingRepo = ratingRepo;
        _userManager = userManager;
    }

    public async Task<PagedResponse<CommentDto>> GetCommentsAsync(string targetType, Guid targetId, PaginationParams pagination)
    {
        var comments = await _commentRepo.GetByTargetAsync(targetType, targetId, pagination.Page, pagination.PageSize);
        return new PagedResponse<CommentDto>
        {
            Data = comments.Select(MapCommentDto),
            Page = pagination.Page,
            PageSize = pagination.PageSize
        };
    }

    public async Task<ApiResponse<CommentDto>> AddCommentAsync(string userId, CreateCommentDto dto)
    {
        if (!Enum.TryParse<CommentTarget>(dto.TargetType, true, out var target))
            return ApiResponse<CommentDto>.Fail("Invalid target type.");

        var comment = new Comment
        {
            Id = Guid.NewGuid(),
            Content = dto.Content,
            TargetType = target,
            TargetId = dto.TargetId,
            ParentCommentId = dto.ParentCommentId,
            AuthorId = userId
        };

        await _commentRepo.AddAsync(comment);
        var user = await _userManager.FindByIdAsync(userId);

        return ApiResponse<CommentDto>.Ok(new CommentDto
        {
            Id = comment.Id,
            Content = comment.Content,
            AuthorId = userId,
            AuthorName = user != null ? $"{user.FirstName} {user.LastName}" : "Unknown",
            CreatedAt = comment.CreatedAt
        }, "Comment added.");
    }

    public async Task<ApiResponse<bool>> DeleteCommentAsync(Guid commentId, string userId)
    {
        var comment = await _commentRepo.GetByIdAsync(commentId);
        if (comment == null)
            return ApiResponse<bool>.Fail("Comment not found.");
        if (comment.AuthorId != userId)
            return ApiResponse<bool>.Fail("You can only delete your own comments.");

        await _commentRepo.DeleteAsync(comment);
        return ApiResponse<bool>.Ok(true, "Comment deleted.");
    }

    public async Task<PagedResponse<RatingDto>> GetRatingsAsync(Guid pathId, PaginationParams pagination)
    {
        var ratings = await _ratingRepo.GetByPathAsync(pathId, pagination.Page, pagination.PageSize);
        return new PagedResponse<RatingDto>
        {
            Data = ratings.Select(r => new RatingDto
            {
                Id = r.Id,
                Score = r.Score,
                Review = r.Review,
                UserName = $"{r.User.FirstName} {r.User.LastName}",
                CreatedAt = r.CreatedAt
            }),
            Page = pagination.Page,
            PageSize = pagination.PageSize
        };
    }

    public async Task<ApiResponse<RatingDto>> AddRatingAsync(string userId, Guid pathId, CreateRatingDto dto)
    {
        var existing = await _ratingRepo.GetByUserAndPathAsync(userId, pathId);
        if (existing != null)
            return ApiResponse<RatingDto>.Fail("You have already rated this learning path.");

        var rating = new Rating
        {
            Id = Guid.NewGuid(),
            Score = dto.Score,
            Review = dto.Review,
            UserId = userId,
            LearningPathId = pathId
        };

        await _ratingRepo.AddAsync(rating);
        var user = await _userManager.FindByIdAsync(userId);

        return ApiResponse<RatingDto>.Ok(new RatingDto
        {
            Id = rating.Id,
            Score = rating.Score,
            Review = rating.Review,
            UserName = user != null ? $"{user.FirstName} {user.LastName}" : "Unknown",
            CreatedAt = rating.CreatedAt
        }, "Rating added.");
    }

    private static CommentDto MapCommentDto(Comment comment) => new()
    {
        Id = comment.Id,
        Content = comment.Content,
        AuthorId = comment.AuthorId,
        AuthorName = $"{comment.Author.FirstName} {comment.Author.LastName}",
        AuthorAvatarUrl = comment.Author.AvatarUrl,
        CreatedAt = comment.CreatedAt,
        Replies = comment.Replies.Select(MapCommentDto)
    };
}

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepo;

    public NotificationService(INotificationRepository notificationRepo)
    {
        _notificationRepo = notificationRepo;
    }

    public async Task<PagedResponse<NotificationDto>> GetNotificationsAsync(string userId, PaginationParams pagination, bool? isRead = null)
    {
        var notifications = await _notificationRepo.GetByUserAsync(userId, pagination.Page, pagination.PageSize, isRead);
        var total = await _notificationRepo.CountAsync(n => n.UserId == userId);

        return new PagedResponse<NotificationDto>
        {
            Data = notifications.Select(n => new NotificationDto
            {
                Id = n.Id,
                Title = n.Title,
                Message = n.Message,
                Type = n.Type.ToString(),
                IsRead = n.IsRead,
                ActionUrl = n.ActionUrl,
                CreatedAt = n.CreatedAt
            }),
            Page = pagination.Page,
            PageSize = pagination.PageSize,
            TotalCount = total
        };
    }

    public async Task<ApiResponse<int>> GetUnreadCountAsync(string userId)
    {
        var count = await _notificationRepo.GetUnreadCountAsync(userId);
        return ApiResponse<int>.Ok(count);
    }

    public async Task<ApiResponse<bool>> MarkAsReadAsync(Guid notificationId, string userId)
    {
        var notification = await _notificationRepo.GetByIdAsync(notificationId);
        if (notification == null || notification.UserId != userId)
            return ApiResponse<bool>.Fail("Notification not found.");

        notification.IsRead = true;
        await _notificationRepo.UpdateAsync(notification);
        return ApiResponse<bool>.Ok(true);
    }

    public async Task<ApiResponse<bool>> MarkAllAsReadAsync(string userId)
    {
        await _notificationRepo.MarkAllAsReadAsync(userId);
        return ApiResponse<bool>.Ok(true, "All notifications marked as read.");
    }

    public async Task CreateNotificationAsync(string userId, string title, string? message, string type, string? actionUrl = null)
    {
        if (!Enum.TryParse<NotificationType>(type, true, out var notificationType))
            notificationType = NotificationType.System;

        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            Title = title,
            Message = message,
            Type = notificationType,
            UserId = userId,
            ActionUrl = actionUrl
        };

        await _notificationRepo.AddAsync(notification);
    }
}
