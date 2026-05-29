using LearnPath.API.DTOs.Analytics;
using LearnPath.API.Entities;
using LearnPath.API.Interfaces.Repositories;
using LearnPath.API.Interfaces.Services;
using LearnPath.API.Responses;

namespace LearnPath.API.Services.Analytics;

public class AnalyticsService : IAnalyticsService
{
    private readonly IProgressRepository _progressRepo;
    private readonly ILearningPathRepository _pathRepo;
    private readonly IClassroomRepository _classroomRepo;
    private readonly ICertificateRepository _certRepo;

    public AnalyticsService(
        IProgressRepository progressRepo,
        ILearningPathRepository pathRepo,
        IClassroomRepository classroomRepo,
        ICertificateRepository certRepo)
    {
        _progressRepo = progressRepo;
        _pathRepo = pathRepo;
        _classroomRepo = classroomRepo;
        _certRepo = certRepo;
    }

    public async Task<ApiResponse<DashboardDto>> GetDashboardAsync(string userId)
    {
        var completedCount = await _progressRepo.GetCompletedCountByUserAsync(userId);
        var certs = await _certRepo.GetByUserAsync(userId);

        return ApiResponse<DashboardDto>.Ok(new DashboardDto
        {
            ActivePaths = 0,
            CompletedToday = completedCount,
            StreakDays = 0,
            TotalPoints = completedCount * 10,
            InProgressPaths = Enumerable.Empty<PathProgressDto>(),
            Recommendations = Enumerable.Empty<RecommendationDto>()
        });
    }

    public async Task<ApiResponse<UserAnalyticsDto>> GetUserAnalyticsAsync(string userId)
    {
        var completedModules = await _progressRepo.GetCompletedCountByUserAsync(userId);
        var certs = await _certRepo.GetByUserAsync(userId);

        return ApiResponse<UserAnalyticsDto>.Ok(new UserAnalyticsDto
        {
            ModulesCompleted = completedModules,
            CertificatesEarned = certs.Count(),
            PathProgresses = Enumerable.Empty<PathProgressDto>(),
            RecentActivity = Enumerable.Empty<ActivityDto>()
        });
    }

    public async Task<ApiResponse<ClassroomAnalyticsDto>> GetClassroomAnalyticsAsync(Guid classroomId, string userId)
    {
        var classroom = await _classroomRepo.GetWithDetailsAsync(classroomId);
        if (classroom == null)
            return ApiResponse<ClassroomAnalyticsDto>.Fail("Classroom not found.");
        if (classroom.InstructorId != userId)
            return ApiResponse<ClassroomAnalyticsDto>.Fail("Only the instructor can view classroom analytics.");

        return ApiResponse<ClassroomAnalyticsDto>.Ok(new ClassroomAnalyticsDto
        {
            ClassroomId = classroom.Id,
            ClassroomName = classroom.Name,
            TotalStudents = classroom.UserClassrooms.Count(uc => uc.Role == ClassroomRole.Student),
            AssignmentsCreated = classroom.Assignments.Count,
            SubmissionsReceived = classroom.Assignments.Sum(a => a.Submissions.Count),
            StudentProgresses = Enumerable.Empty<StudentProgressDto>()
        });
    }

    public async Task<ApiResponse<IEnumerable<RecommendationDto>>> GetRecommendationsAsync(string userId)
    {
        var paths = await _pathRepo.GetPublicPathsAsync(1, 5);
        var recommendations = paths.Select(p => new RecommendationDto
        {
            PathId = p.Id,
            PathTitle = p.Title,
            Reason = "Popular learning path",
            ConfidenceScore = 0.8
        });

        return ApiResponse<IEnumerable<RecommendationDto>>.Ok(recommendations);
    }
}
