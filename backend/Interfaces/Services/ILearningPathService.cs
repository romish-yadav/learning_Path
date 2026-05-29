using LearnPath.API.DTOs.Common;
using LearnPath.API.DTOs.LearningPath;
using LearnPath.API.Responses;

namespace LearnPath.API.Interfaces.Services;

public interface ILearningPathService
{
    Task<ApiResponse<LearningPathDetailDto>> GetByIdAsync(Guid id);
    Task<PagedResponse<LearningPathSummaryDto>> GetPublicPathsAsync(PathFilterParams filter);
    Task<PagedResponse<LearningPathSummaryDto>> GetMyPathsAsync(string userId, PaginationParams pagination);
    Task<ApiResponse<LearningPathDetailDto>> CreateAsync(string userId, CreateLearningPathDto dto);
    Task<ApiResponse<LearningPathDetailDto>> UpdateAsync(Guid id, string userId, UpdateLearningPathDto dto);
    Task<ApiResponse<bool>> DeleteAsync(Guid id, string userId);
    Task<ApiResponse<bool>> PublishAsync(Guid id, string userId);

    Task<ApiResponse<ModuleDto>> AddModuleAsync(Guid pathId, string userId, CreateModuleDto dto);
    Task<ApiResponse<ModuleDto>> UpdateModuleAsync(Guid pathId, Guid moduleId, string userId, UpdateModuleDto dto);
    Task<ApiResponse<bool>> DeleteModuleAsync(Guid pathId, Guid moduleId, string userId);
}
