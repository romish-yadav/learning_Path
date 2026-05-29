using LearnPath.API.DTOs.Analytics;
using LearnPath.API.Responses;

namespace LearnPath.API.Interfaces.Services;

public interface IAnalyticsService
{
    Task<ApiResponse<DashboardDto>> GetDashboardAsync(string userId);
    Task<ApiResponse<UserAnalyticsDto>> GetUserAnalyticsAsync(string userId);
    Task<ApiResponse<ClassroomAnalyticsDto>> GetClassroomAnalyticsAsync(Guid classroomId, string userId);
    Task<ApiResponse<IEnumerable<RecommendationDto>>> GetRecommendationsAsync(string userId);
}

public interface IProgressService
{
    Task<ApiResponse<bool>> UpdateProgressAsync(string userId, Guid moduleId, int completionPercentage);
    Task<ApiResponse<bool>> CompleteModuleAsync(string userId, Guid moduleId);
    Task<ApiResponse<IEnumerable<DTOs.LearningPath.ModuleDto>>> GetUnlockedModulesAsync(string userId, Guid pathId);
}
