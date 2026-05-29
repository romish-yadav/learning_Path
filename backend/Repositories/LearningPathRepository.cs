using Microsoft.EntityFrameworkCore;
using LearnPath.API.Data;
using LearnPath.API.Interfaces.Repositories;

namespace LearnPath.API.Repositories;

public class LearningPathRepository : BaseRepository<Entities.LearningPath>, ILearningPathRepository
{
    public LearningPathRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Entities.LearningPath?> GetWithModulesAsync(Guid id) =>
        await DbSet
            .Include(lp => lp.Creator)
            .Include(lp => lp.Modules.OrderBy(m => m.OrderIndex))
                .ThenInclude(m => m.Prerequisites)
            .Include(lp => lp.Ratings)
            .FirstOrDefaultAsync(lp => lp.Id == id);

    public async Task<IEnumerable<Entities.LearningPath>> GetPublicPathsAsync(
        int page, int pageSize, string? query = null, string? difficulty = null)
    {
        var q = DbSet
            .Include(lp => lp.Creator)
            .Include(lp => lp.Modules)
            .Include(lp => lp.Ratings)
            .Where(lp => lp.IsPublished && lp.IsPublic);

        if (!string.IsNullOrWhiteSpace(query))
            q = q.Where(lp => lp.Title.Contains(query) || (lp.Description != null && lp.Description.Contains(query)));

        if (!string.IsNullOrWhiteSpace(difficulty) && Enum.TryParse<Entities.PathDifficulty>(difficulty, true, out var d))
            q = q.Where(lp => lp.Difficulty == d);

        return await q
            .OrderByDescending(lp => lp.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<Entities.LearningPath>> GetByCreatorAsync(string userId, int page, int pageSize) =>
        await DbSet
            .Include(lp => lp.Creator)
            .Include(lp => lp.Modules)
            .Include(lp => lp.Ratings)
            .Where(lp => lp.CreatorId == userId)
            .OrderByDescending(lp => lp.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

    public async Task<int> GetPublicPathsCountAsync(string? query = null, string? difficulty = null)
    {
        var q = DbSet.Where(lp => lp.IsPublished && lp.IsPublic);

        if (!string.IsNullOrWhiteSpace(query))
            q = q.Where(lp => lp.Title.Contains(query) || (lp.Description != null && lp.Description.Contains(query)));

        if (!string.IsNullOrWhiteSpace(difficulty) && Enum.TryParse<Entities.PathDifficulty>(difficulty, true, out var d))
            q = q.Where(lp => lp.Difficulty == d);

        return await q.CountAsync();
    }
}
