using LearnPath.API.DTOs.LearningPath;
using LearnPath.API.Entities;
using LearnPath.API.Interfaces.Repositories;
using LearnPath.API.Interfaces.Services;
using LearnPath.API.Responses;

namespace LearnPath.API.Services.Analytics;

public class ProgressService : IProgressService
{
    private readonly IProgressRepository _progressRepo;
    private readonly IModuleRepository _moduleRepo;

    public ProgressService(IProgressRepository progressRepo, IModuleRepository moduleRepo)
    {
        _progressRepo = progressRepo;
        _moduleRepo = moduleRepo;
    }

    public async Task<ApiResponse<bool>> UpdateProgressAsync(string userId, Guid moduleId, int completionPercentage)
    {
        var progress = await _progressRepo.GetByUserAndModuleAsync(userId, moduleId);
        if (progress == null)
        {
            progress = new Progress
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ModuleId = moduleId,
                Status = ProgressStatus.InProgress,
                CompletionPercentage = completionPercentage,
                StartedAt = DateTime.UtcNow
            };
            await _progressRepo.AddAsync(progress);
        }
        else
        {
            progress.CompletionPercentage = completionPercentage;
            progress.UpdatedAt = DateTime.UtcNow;
            if (completionPercentage >= 100)
            {
                progress.Status = ProgressStatus.Completed;
                progress.CompletedAt = DateTime.UtcNow;
            }
            await _progressRepo.UpdateAsync(progress);
        }

        return ApiResponse<bool>.Ok(true, "Progress updated.");
    }

    public async Task<ApiResponse<bool>> CompleteModuleAsync(string userId, Guid moduleId)
    {
        return await UpdateProgressAsync(userId, moduleId, 100);
    }

    public async Task<ApiResponse<IEnumerable<ModuleDto>>> GetUnlockedModulesAsync(string userId, Guid pathId)
    {
        var modules = await _moduleRepo.GetByPathIdAsync(pathId);
        var progresses = await _progressRepo.GetByUserAndPathAsync(userId, pathId);
        var completedModuleIds = progresses
            .Where(p => p.Status == ProgressStatus.Completed)
            .Select(p => p.ModuleId)
            .ToHashSet();

        var unlocked = modules.Where(m =>
            !m.Prerequisites.Any() ||
            m.Prerequisites.All(d => completedModuleIds.Contains(d.PrerequisiteModuleId)));

        return ApiResponse<IEnumerable<ModuleDto>>.Ok(unlocked.Select(m => new ModuleDto
        {
            Id = m.Id,
            Title = m.Title,
            Description = m.Description,
            Type = m.Type,
            OrderIndex = m.OrderIndex,
            EstimatedMinutes = m.EstimatedMinutes,
            PrerequisiteIds = m.Prerequisites.Select(p => p.PrerequisiteModuleId)
        }));
    }
}
