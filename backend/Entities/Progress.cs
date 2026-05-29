namespace LearnPath.API.Entities;

public class Progress
{
    public Guid Id { get; set; }
    public ProgressStatus Status { get; set; } = ProgressStatus.NotStarted;
    public int CompletionPercentage { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public string UserId { get; set; } = string.Empty;
    public User User { get; set; } = null!;

    public Guid ModuleId { get; set; }
    public Module Module { get; set; } = null!;
}

public enum ProgressStatus
{
    NotStarted,
    InProgress,
    Completed,
    Skipped
}
