namespace LearnPath.API.Entities;

public class LearningPath
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ThumbnailUrl { get; set; }
    public PathDifficulty Difficulty { get; set; } = PathDifficulty.Beginner;
    public bool IsPublished { get; set; }
    public bool IsPublic { get; set; } = true;
    public string? Tags { get; set; }
    public int EstimatedHours { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public string CreatorId { get; set; } = string.Empty;
    public User Creator { get; set; } = null!;

    public ICollection<Module> Modules { get; set; } = new List<Module>();
    public ICollection<ClassroomLearningPath> ClassroomLearningPaths { get; set; } = new List<ClassroomLearningPath>();
    public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    public ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();
}

public enum PathDifficulty
{
    Beginner,
    Intermediate,
    Advanced,
    Expert
}
