using System.Linq.Expressions;

namespace LearnPath.API.Interfaces.Repositories;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
}

public interface IUserRepository
{
    Task<Entities.User?> GetByIdWithDetailsAsync(string userId);
    Task<IEnumerable<Entities.User>> SearchUsersAsync(string query, int page, int pageSize);
}

public interface ILearningPathRepository : IRepository<Entities.LearningPath>
{
    Task<Entities.LearningPath?> GetWithModulesAsync(Guid id);
    Task<IEnumerable<Entities.LearningPath>> GetPublicPathsAsync(int page, int pageSize, string? query = null, string? difficulty = null);
    Task<IEnumerable<Entities.LearningPath>> GetByCreatorAsync(string userId, int page, int pageSize);
    Task<int> GetPublicPathsCountAsync(string? query = null, string? difficulty = null);
}

public interface IModuleRepository : IRepository<Entities.Module>
{
    Task<IEnumerable<Entities.Module>> GetByPathIdAsync(Guid pathId);
    Task<Entities.Module?> GetWithDependenciesAsync(Guid moduleId);
}

public interface IProgressRepository : IRepository<Entities.Progress>
{
    Task<Entities.Progress?> GetByUserAndModuleAsync(string userId, Guid moduleId);
    Task<IEnumerable<Entities.Progress>> GetByUserAndPathAsync(string userId, Guid pathId);
    Task<int> GetCompletedCountByUserAsync(string userId);
}

public interface IClassroomRepository : IRepository<Entities.Classroom>
{
    Task<Entities.Classroom?> GetWithDetailsAsync(Guid id);
    Task<Entities.Classroom?> GetByJoinCodeAsync(string joinCode);
    Task<IEnumerable<Entities.Classroom>> GetByUserAsync(string userId, int page, int pageSize);
    Task<IEnumerable<Entities.Classroom>> GetByInstructorAsync(string instructorId, int page, int pageSize);
}

public interface IAssignmentRepository : IRepository<Entities.Assignment>
{
    Task<Entities.Assignment?> GetWithSubmissionsAsync(Guid id);
    Task<IEnumerable<Entities.Assignment>> GetByClassroomAsync(Guid classroomId);
}

public interface ISubmissionRepository : IRepository<Entities.Submission>
{
    Task<Entities.Submission?> GetByStudentAndAssignmentAsync(string studentId, Guid assignmentId);
    Task<IEnumerable<Entities.Submission>> GetByAssignmentAsync(Guid assignmentId);
}

public interface ICommentRepository : IRepository<Entities.Comment>
{
    Task<IEnumerable<Entities.Comment>> GetByTargetAsync(string targetType, Guid targetId, int page, int pageSize);
}

public interface IRatingRepository : IRepository<Entities.Rating>
{
    Task<Entities.Rating?> GetByUserAndPathAsync(string userId, Guid pathId);
    Task<double> GetAverageByPathAsync(Guid pathId);
    Task<IEnumerable<Entities.Rating>> GetByPathAsync(Guid pathId, int page, int pageSize);
}

public interface INotificationRepository : IRepository<Entities.Notification>
{
    Task<IEnumerable<Entities.Notification>> GetByUserAsync(string userId, int page, int pageSize, bool? isRead = null);
    Task<int> GetUnreadCountAsync(string userId);
    Task MarkAllAsReadAsync(string userId);
}

public interface ICertificateRepository : IRepository<Entities.Certificate>
{
    Task<IEnumerable<Entities.Certificate>> GetByUserAsync(string userId);
    Task<Entities.Certificate?> GetByUserAndPathAsync(string userId, Guid pathId);
}
