using LearnPath.API.DTOs.Common;
using LearnPath.API.DTOs.LearningPath;
using LearnPath.API.Entities;
using LearnPath.API.Interfaces.Repositories;
using LearnPath.API.Interfaces.Services;
using LearnPath.API.Responses;

namespace LearnPath.API.Services.LearningPath;

public class LearningPathService : ILearningPathService
{
    private readonly ILearningPathRepository _pathRepo;
    private readonly IModuleRepository _moduleRepo;
    private readonly Data.ApplicationDbContext _context;

    public LearningPathService(
        ILearningPathRepository pathRepo,
        IModuleRepository moduleRepo,
        Data.ApplicationDbContext context)
    {
        _pathRepo = pathRepo;
        _moduleRepo = moduleRepo;
        _context = context;
    }

    public async Task<ApiResponse<LearningPathDetailDto>> GetByIdAsync(Guid id)
    {
        var path = await _pathRepo.GetWithModulesAsync(id);
        if (path == null)
            return ApiResponse<LearningPathDetailDto>.Fail("Learning path not found.");

        return ApiResponse<LearningPathDetailDto>.Ok(MapToDetailDto(path));
    }

    public async Task<PagedResponse<LearningPathSummaryDto>> GetPublicPathsAsync(PathFilterParams filter)
    {
        var paths = await _pathRepo.GetPublicPathsAsync(filter.Page, filter.PageSize, filter.Query, filter.Difficulty);
        var total = await _pathRepo.GetPublicPathsCountAsync(filter.Query, filter.Difficulty);

        return new PagedResponse<LearningPathSummaryDto>
        {
            Data = paths.Select(MapToSummaryDto),
            Page = filter.Page,
            PageSize = filter.PageSize,
            TotalCount = total
        };
    }

    public async Task<PagedResponse<LearningPathSummaryDto>> GetMyPathsAsync(string userId, PaginationParams pagination)
    {
        var paths = await _pathRepo.GetByCreatorAsync(userId, pagination.Page, pagination.PageSize);
        var total = await _pathRepo.CountAsync(lp => lp.CreatorId == userId);

        return new PagedResponse<LearningPathSummaryDto>
        {
            Data = paths.Select(MapToSummaryDto),
            Page = pagination.Page,
            PageSize = pagination.PageSize,
            TotalCount = total
        };
    }

    public async Task<ApiResponse<LearningPathDetailDto>> CreateAsync(string userId, CreateLearningPathDto dto)
    {
        var path = new Entities.LearningPath
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Description = dto.Description,
            ThumbnailUrl = dto.ThumbnailUrl,
            Difficulty = dto.Difficulty,
            IsPublic = dto.IsPublic,
            Tags = dto.Tags,
            EstimatedHours = dto.EstimatedHours,
            CreatorId = userId
        };

        await _pathRepo.AddAsync(path);
        var created = await _pathRepo.GetWithModulesAsync(path.Id);
        return ApiResponse<LearningPathDetailDto>.Ok(MapToDetailDto(created!), "Learning path created.");
    }

    public async Task<ApiResponse<LearningPathDetailDto>> UpdateAsync(Guid id, string userId, UpdateLearningPathDto dto)
    {
        var path = await _pathRepo.GetByIdAsync(id);
        if (path == null)
            return ApiResponse<LearningPathDetailDto>.Fail("Learning path not found.");
        if (path.CreatorId != userId)
            return ApiResponse<LearningPathDetailDto>.Fail("You can only update your own learning paths.");

        if (dto.Title != null) path.Title = dto.Title;
        if (dto.Description != null) path.Description = dto.Description;
        if (dto.ThumbnailUrl != null) path.ThumbnailUrl = dto.ThumbnailUrl;
        if (dto.Difficulty.HasValue) path.Difficulty = dto.Difficulty.Value;
        if (dto.IsPublic.HasValue) path.IsPublic = dto.IsPublic.Value;
        if (dto.IsPublished.HasValue) path.IsPublished = dto.IsPublished.Value;
        if (dto.Tags != null) path.Tags = dto.Tags;
        if (dto.EstimatedHours.HasValue) path.EstimatedHours = dto.EstimatedHours.Value;
        path.UpdatedAt = DateTime.UtcNow;

        await _pathRepo.UpdateAsync(path);
        var updated = await _pathRepo.GetWithModulesAsync(id);
        return ApiResponse<LearningPathDetailDto>.Ok(MapToDetailDto(updated!), "Learning path updated.");
    }

    public async Task<ApiResponse<bool>> DeleteAsync(Guid id, string userId)
    {
        var path = await _pathRepo.GetByIdAsync(id);
        if (path == null)
            return ApiResponse<bool>.Fail("Learning path not found.");
        if (path.CreatorId != userId)
            return ApiResponse<bool>.Fail("You can only delete your own learning paths.");

        await _pathRepo.DeleteAsync(path);
        return ApiResponse<bool>.Ok(true, "Learning path deleted.");
    }

    public async Task<ApiResponse<bool>> PublishAsync(Guid id, string userId)
    {
        var path = await _pathRepo.GetWithModulesAsync(id);
        if (path == null)
            return ApiResponse<bool>.Fail("Learning path not found.");
        if (path.CreatorId != userId)
            return ApiResponse<bool>.Fail("You can only publish your own learning paths.");
        if (!path.Modules.Any())
            return ApiResponse<bool>.Fail("Cannot publish a learning path with no modules.");

        path.IsPublished = true;
        path.UpdatedAt = DateTime.UtcNow;
        await _pathRepo.UpdateAsync(path);
        return ApiResponse<bool>.Ok(true, "Learning path published.");
    }

    public async Task<ApiResponse<ModuleDto>> AddModuleAsync(Guid pathId, string userId, CreateModuleDto dto)
    {
        var path = await _pathRepo.GetByIdAsync(pathId);
        if (path == null)
            return ApiResponse<ModuleDto>.Fail("Learning path not found.");
        if (path.CreatorId != userId)
            return ApiResponse<ModuleDto>.Fail("You can only add modules to your own learning paths.");

        var module = new Module
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Description = dto.Description,
            Content = dto.Content,
            ResourceUrl = dto.ResourceUrl,
            Type = dto.Type,
            OrderIndex = dto.OrderIndex,
            EstimatedMinutes = dto.EstimatedMinutes,
            LearningPathId = pathId
        };

        await _moduleRepo.AddAsync(module);

        if (dto.PrerequisiteIds?.Any() == true)
        {
            foreach (var prereqId in dto.PrerequisiteIds)
            {
                _context.ModuleDependencies.Add(new ModuleDependency
                {
                    Id = Guid.NewGuid(),
                    ModuleId = module.Id,
                    PrerequisiteModuleId = prereqId
                });
            }
            await _context.SaveChangesAsync();
        }

        return ApiResponse<ModuleDto>.Ok(MapToModuleDto(module), "Module added.");
    }

    public async Task<ApiResponse<ModuleDto>> UpdateModuleAsync(Guid pathId, Guid moduleId, string userId, UpdateModuleDto dto)
    {
        var path = await _pathRepo.GetByIdAsync(pathId);
        if (path == null)
            return ApiResponse<ModuleDto>.Fail("Learning path not found.");
        if (path.CreatorId != userId)
            return ApiResponse<ModuleDto>.Fail("You can only update modules in your own learning paths.");

        var module = await _moduleRepo.GetByIdAsync(moduleId);
        if (module == null || module.LearningPathId != pathId)
            return ApiResponse<ModuleDto>.Fail("Module not found in this learning path.");

        if (dto.Title != null) module.Title = dto.Title;
        if (dto.Description != null) module.Description = dto.Description;
        if (dto.Content != null) module.Content = dto.Content;
        if (dto.ResourceUrl != null) module.ResourceUrl = dto.ResourceUrl;
        if (dto.Type.HasValue) module.Type = dto.Type.Value;
        if (dto.OrderIndex.HasValue) module.OrderIndex = dto.OrderIndex.Value;
        if (dto.EstimatedMinutes.HasValue) module.EstimatedMinutes = dto.EstimatedMinutes.Value;
        module.UpdatedAt = DateTime.UtcNow;

        await _moduleRepo.UpdateAsync(module);
        return ApiResponse<ModuleDto>.Ok(MapToModuleDto(module), "Module updated.");
    }

    public async Task<ApiResponse<bool>> DeleteModuleAsync(Guid pathId, Guid moduleId, string userId)
    {
        var path = await _pathRepo.GetByIdAsync(pathId);
        if (path == null)
            return ApiResponse<bool>.Fail("Learning path not found.");
        if (path.CreatorId != userId)
            return ApiResponse<bool>.Fail("You can only delete modules from your own learning paths.");

        var module = await _moduleRepo.GetByIdAsync(moduleId);
        if (module == null || module.LearningPathId != pathId)
            return ApiResponse<bool>.Fail("Module not found in this learning path.");

        await _moduleRepo.DeleteAsync(module);
        return ApiResponse<bool>.Ok(true, "Module deleted.");
    }

    private static LearningPathSummaryDto MapToSummaryDto(Entities.LearningPath path) => new()
    {
        Id = path.Id,
        Title = path.Title,
        Description = path.Description,
        ThumbnailUrl = path.ThumbnailUrl,
        Difficulty = path.Difficulty,
        IsPublished = path.IsPublished,
        EstimatedHours = path.EstimatedHours,
        Tags = path.Tags,
        ModuleCount = path.Modules.Count,
        AverageRating = path.Ratings.Any() ? path.Ratings.Average(r => r.Score) : 0,
        CreatorName = $"{path.Creator.FirstName} {path.Creator.LastName}",
        CreatedAt = path.CreatedAt
    };

    private static LearningPathDetailDto MapToDetailDto(Entities.LearningPath path) => new()
    {
        Id = path.Id,
        Title = path.Title,
        Description = path.Description,
        ThumbnailUrl = path.ThumbnailUrl,
        Difficulty = path.Difficulty,
        IsPublished = path.IsPublished,
        IsPublic = path.IsPublic,
        EstimatedHours = path.EstimatedHours,
        Tags = path.Tags,
        ModuleCount = path.Modules.Count,
        AverageRating = path.Ratings.Any() ? path.Ratings.Average(r => r.Score) : 0,
        CreatorName = $"{path.Creator.FirstName} {path.Creator.LastName}",
        CreatorId = path.CreatorId,
        CreatedAt = path.CreatedAt,
        RatingCount = path.Ratings.Count,
        Modules = path.Modules.Select(MapToModuleDto)
    };

    private static ModuleDto MapToModuleDto(Module module) => new()
    {
        Id = module.Id,
        Title = module.Title,
        Description = module.Description,
        Content = module.Content,
        ResourceUrl = module.ResourceUrl,
        Type = module.Type,
        OrderIndex = module.OrderIndex,
        EstimatedMinutes = module.EstimatedMinutes,
        PrerequisiteIds = module.Prerequisites.Select(p => p.PrerequisiteModuleId)
    };
}
