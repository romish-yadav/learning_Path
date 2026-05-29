namespace LearnPath.API.Entities;

public class Rating
{
    public Guid Id { get; set; }
    public int Score { get; set; }
    public string? Review { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string UserId { get; set; } = string.Empty;
    public User User { get; set; } = null!;

    public Guid LearningPathId { get; set; }
    public LearningPath LearningPath { get; set; } = null!;
}
