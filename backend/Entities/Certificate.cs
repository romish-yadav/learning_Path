namespace LearnPath.API.Entities;

public class Certificate
{
    public Guid Id { get; set; }
    public string CertificateNumber { get; set; } = string.Empty;
    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
    public string? VerificationUrl { get; set; }

    public string UserId { get; set; } = string.Empty;
    public User User { get; set; } = null!;

    public Guid LearningPathId { get; set; }
    public LearningPath LearningPath { get; set; } = null!;
}
