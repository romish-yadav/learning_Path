namespace LearnPath.API.Entities;

public class Module
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Content { get; set; }
    public string? ResourceUrl { get; set; }
    public ModuleType Type { get; set; } = ModuleType.Lesson;
    public int OrderIndex { get; set; }
    public int EstimatedMinutes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Guid LearningPathId { get; set; }
    public LearningPath LearningPath { get; set; } = null!;

    public ICollection<ModuleDependency> Prerequisites { get; set; } = new List<ModuleDependency>();
    public ICollection<ModuleDependency> Dependents { get; set; } = new List<ModuleDependency>();
    public ICollection<Progress> Progresses { get; set; } = new List<Progress>();
    public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
}

public enum ModuleType
{
    Lesson,
    Quiz,
    Assignment,
    Video,
    Article,
    Project
}
