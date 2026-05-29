namespace LearnPath.API.Entities;

public class Submission
{
    public Guid Id { get; set; }
    public string? Content { get; set; }
    public string? FileUrl { get; set; }
    public int? Score { get; set; }
    public string? Feedback { get; set; }
    public SubmissionStatus Status { get; set; } = SubmissionStatus.Submitted;
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    public DateTime? GradedAt { get; set; }

    public string StudentId { get; set; } = string.Empty;
    public User Student { get; set; } = null!;

    public Guid AssignmentId { get; set; }
    public Assignment Assignment { get; set; } = null!;
}

public enum SubmissionStatus
{
    Submitted,
    UnderReview,
    Graded,
    Returned
}
