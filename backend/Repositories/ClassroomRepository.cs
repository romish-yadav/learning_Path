using Microsoft.EntityFrameworkCore;
using LearnPath.API.Data;
using LearnPath.API.Entities;
using LearnPath.API.Interfaces.Repositories;

namespace LearnPath.API.Repositories;

public class ClassroomRepository : BaseRepository<Classroom>, IClassroomRepository
{
    public ClassroomRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Classroom?> GetWithDetailsAsync(Guid id) =>
        await DbSet
            .Include(c => c.Instructor)
            .Include(c => c.UserClassrooms).ThenInclude(uc => uc.User)
            .Include(c => c.Assignments).ThenInclude(a => a.Submissions)
            .Include(c => c.ClassroomLearningPaths)
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task<Classroom?> GetByJoinCodeAsync(string joinCode) =>
        await DbSet.FirstOrDefaultAsync(c => c.JoinCode == joinCode && c.IsActive);

    public async Task<IEnumerable<Classroom>> GetByUserAsync(string userId, int page, int pageSize) =>
        await DbSet
            .Include(c => c.Instructor)
            .Include(c => c.UserClassrooms)
            .Include(c => c.ClassroomLearningPaths)
            .Where(c => c.UserClassrooms.Any(uc => uc.UserId == userId))
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

    public async Task<IEnumerable<Classroom>> GetByInstructorAsync(string instructorId, int page, int pageSize) =>
        await DbSet
            .Include(c => c.UserClassrooms)
            .Include(c => c.ClassroomLearningPaths)
            .Where(c => c.InstructorId == instructorId)
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
}

public class AssignmentRepository : BaseRepository<Assignment>, IAssignmentRepository
{
    public AssignmentRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Assignment?> GetWithSubmissionsAsync(Guid id) =>
        await DbSet
            .Include(a => a.Submissions).ThenInclude(s => s.Student)
            .FirstOrDefaultAsync(a => a.Id == id);

    public async Task<IEnumerable<Assignment>> GetByClassroomAsync(Guid classroomId) =>
        await DbSet
            .Include(a => a.Submissions)
            .Where(a => a.ClassroomId == classroomId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
}

public class SubmissionRepository : BaseRepository<Submission>, ISubmissionRepository
{
    public SubmissionRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Submission?> GetByStudentAndAssignmentAsync(string studentId, Guid assignmentId) =>
        await DbSet.FirstOrDefaultAsync(s => s.StudentId == studentId && s.AssignmentId == assignmentId);

    public async Task<IEnumerable<Submission>> GetByAssignmentAsync(Guid assignmentId) =>
        await DbSet
            .Include(s => s.Student)
            .Where(s => s.AssignmentId == assignmentId)
            .OrderByDescending(s => s.SubmittedAt)
            .ToListAsync();
}
