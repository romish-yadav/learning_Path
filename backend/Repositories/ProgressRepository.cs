using Microsoft.EntityFrameworkCore;
using LearnPath.API.Data;
using LearnPath.API.Entities;
using LearnPath.API.Interfaces.Repositories;

namespace LearnPath.API.Repositories;

public class ProgressRepository : BaseRepository<Progress>, IProgressRepository
{
    public ProgressRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Progress?> GetByUserAndModuleAsync(string userId, Guid moduleId) =>
        await DbSet.FirstOrDefaultAsync(p => p.UserId == userId && p.ModuleId == moduleId);

    public async Task<IEnumerable<Progress>> GetByUserAndPathAsync(string userId, Guid pathId) =>
        await DbSet
            .Include(p => p.Module)
            .Where(p => p.UserId == userId && p.Module.LearningPathId == pathId)
            .ToListAsync();

    public async Task<int> GetCompletedCountByUserAsync(string userId) =>
        await DbSet.CountAsync(p => p.UserId == userId && p.Status == ProgressStatus.Completed);
}
