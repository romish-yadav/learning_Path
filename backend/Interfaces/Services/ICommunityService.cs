using LearnPath.API.DTOs.Common;
using LearnPath.API.DTOs.Community;
using LearnPath.API.Responses;

namespace LearnPath.API.Interfaces.Services;

public interface ICommunityService
{
    Task<PagedResponse<CommentDto>> GetCommentsAsync(string targetType, Guid targetId, PaginationParams pagination);
    Task<ApiResponse<CommentDto>> AddCommentAsync(string userId, CreateCommentDto dto);
    Task<ApiResponse<bool>> DeleteCommentAsync(Guid commentId, string userId);

    Task<PagedResponse<RatingDto>> GetRatingsAsync(Guid pathId, PaginationParams pagination);
    Task<ApiResponse<RatingDto>> AddRatingAsync(string userId, Guid pathId, CreateRatingDto dto);
}

public interface INotificationService
{
    Task<PagedResponse<NotificationDto>> GetNotificationsAsync(string userId, PaginationParams pagination, bool? isRead = null);
    Task<ApiResponse<int>> GetUnreadCountAsync(string userId);
    Task<ApiResponse<bool>> MarkAsReadAsync(Guid notificationId, string userId);
    Task<ApiResponse<bool>> MarkAllAsReadAsync(string userId);
    Task CreateNotificationAsync(string userId, string title, string? message, string type, string? actionUrl = null);
}
