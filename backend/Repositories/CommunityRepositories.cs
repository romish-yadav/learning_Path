using Microsoft.EntityFrameworkCore;
using LearnPath.API.Data;
using LearnPath.API.Entities;
using LearnPath.API.Interfaces.Repositories;

namespace LearnPath.API.Repositories;

public class CommentRepository : BaseRepository<Comment>, ICommentRepository
{
    public CommentRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Comment>> GetByTargetAsync(string targetType, Guid targetId, int page, int pageSize)
    {
        if (!Enum.TryParse<CommentTarget>(targetType, true, out var target))
            return Enumerable.Empty<Comment>();

        return await DbSet
            .Include(c => c.Author)
            .Include(c => c.Replies).ThenInclude(r => r.Author)
            .Where(c => c.TargetType == target && c.TargetId == targetId && c.ParentCommentId == null)
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}

public class RatingRepository : BaseRepository<Rating>, IRatingRepository
{
    public RatingRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Rating?> GetByUserAndPathAsync(string userId, Guid pathId) =>
        await DbSet.FirstOrDefaultAsync(r => r.UserId == userId && r.LearningPathId == pathId);

    public async Task<double> GetAverageByPathAsync(Guid pathId) =>
        await DbSet.Where(r => r.LearningPathId == pathId)
            .Select(r => (double?)r.Score)
            .AverageAsync() ?? 0;

    public async Task<IEnumerable<Rating>> GetByPathAsync(Guid pathId, int page, int pageSize) =>
        await DbSet
            .Include(r => r.User)
            .Where(r => r.LearningPathId == pathId)
            .OrderByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
}

public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
{
    public NotificationRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Notification>> GetByUserAsync(string userId, int page, int pageSize, bool? isRead = null)
    {
        var q = DbSet.Where(n => n.UserId == userId);
        if (isRead.HasValue)
            q = q.Where(n => n.IsRead == isRead.Value);

        return await q
            .OrderByDescending(n => n.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetUnreadCountAsync(string userId) =>
        await DbSet.CountAsync(n => n.UserId == userId && !n.IsRead);

    public async Task MarkAllAsReadAsync(string userId)
    {
        var unread = await DbSet.Where(n => n.UserId == userId && !n.IsRead).ToListAsync();
        foreach (var n in unread)
            n.IsRead = true;
        await Context.SaveChangesAsync();
    }
}

public class CertificateRepository : BaseRepository<Certificate>, ICertificateRepository
{
    public CertificateRepository(ApplicationDbContext context) : base(context) { }

    public async Task<IEnumerable<Certificate>> GetByUserAsync(string userId) =>
        await DbSet
            .Include(c => c.LearningPath)
            .Where(c => c.UserId == userId)
            .OrderByDescending(c => c.IssuedAt)
            .ToListAsync();

    public async Task<Certificate?> GetByUserAndPathAsync(string userId, Guid pathId) =>
        await DbSet.FirstOrDefaultAsync(c => c.UserId == userId && c.LearningPathId == pathId);
}

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdWithDetailsAsync(string userId) =>
        await _context.Users
            .Include(u => u.CreatedPaths)
            .Include(u => u.Progresses)
            .Include(u => u.Certificates)
            .FirstOrDefaultAsync(u => u.Id == userId);

    public async Task<IEnumerable<User>> SearchUsersAsync(string query, int page, int pageSize) =>
        await _context.Users
            .Where(u => u.FirstName.Contains(query) || u.LastName.Contains(query) || u.Email!.Contains(query))
            .OrderBy(u => u.FirstName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
}
