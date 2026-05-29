using Microsoft.EntityFrameworkCore;
using LearnPath.API.Data;
using LearnPath.API.Entities;
using LearnPath.API.Interfaces.Repositories;

namespace LearnPath.API.Repositories;

public class ModuleRepository : BaseRepository<Module>, IModuleRepository
{
    public ModuleRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Module>> GetByPathIdAsync(Guid pathId) =>
        await DbSet
            .Include(m => m.Prerequisites)
            .Where(m => m.LearningPathId == pathId)
            .OrderBy(m => m.OrderIndex)
            .ToListAsync();

    public async Task<Module?> GetWithDependenciesAsync(Guid moduleId) =>
        await DbSet
            .Include(m => m.Prerequisites)
                .ThenInclude(d => d.PrerequisiteModule)
            .Include(m => m.Dependents)
                .ThenInclude(d => d.Module)
            .FirstOrDefaultAsync(m => m.Id == moduleId);
}
