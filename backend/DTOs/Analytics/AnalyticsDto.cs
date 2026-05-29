namespace LearnPath.API.DTOs.Analytics;

public class UserAnalyticsDto
{
    public int TotalPathsEnrolled { get; set; }
    public int PathsCompleted { get; set; }
    public int ModulesCompleted { get; set; }
    public int TotalModules { get; set; }
    public double OverallCompletionRate { get; set; }
    public int TotalTimeSpentMinutes { get; set; }
    public int CertificatesEarned { get; set; }
    public IEnumerable<PathProgressDto> PathProgresses { get; set; } = Enumerable.Empty<PathProgressDto>();
    public IEnumerable<ActivityDto> RecentActivity { get; set; } = Enumerable.Empty<ActivityDto>();
}

public class PathProgressDto
{
    public Guid PathId { get; set; }
    public string PathTitle { get; set; } = string.Empty;
    public int CompletedModules { get; set; }
    public int TotalModules { get; set; }
    public double CompletionPercentage { get; set; }
    public DateTime? LastActivityAt { get; set; }
}

public class ActivityDto
{
    public string Description { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public DateTime OccurredAt { get; set; }
}

public class DashboardDto
{
    public int ActivePaths { get; set; }
    public int CompletedToday { get; set; }
    public int StreakDays { get; set; }
    public int TotalPoints { get; set; }
    public IEnumerable<PathProgressDto> InProgressPaths { get; set; } = Enumerable.Empty<PathProgressDto>();
    public IEnumerable<RecommendationDto> Recommendations { get; set; } = Enumerable.Empty<RecommendationDto>();
}

public class RecommendationDto
{
    public Guid PathId { get; set; }
    public string PathTitle { get; set; } = string.Empty;
    public string? Reason { get; set; }
    public double ConfidenceScore { get; set; }
}

public class ClassroomAnalyticsDto
{
    public Guid ClassroomId { get; set; }
    public string ClassroomName { get; set; } = string.Empty;
    public int TotalStudents { get; set; }
    public double AverageCompletionRate { get; set; }
    public int AssignmentsCreated { get; set; }
    public int SubmissionsReceived { get; set; }
    public double AverageScore { get; set; }
    public IEnumerable<StudentProgressDto> StudentProgresses { get; set; } = Enumerable.Empty<StudentProgressDto>();
}

public class StudentProgressDto
{
    public string StudentId { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
    public double CompletionRate { get; set; }
    public double AverageScore { get; set; }
    public int ModulesCompleted { get; set; }
}
