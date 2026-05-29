namespace LearnPath.API.Entities;

public class Assignment
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Instructions { get; set; }
    public DateTime? DueDate { get; set; }
    public int MaxScore { get; set; } = 100;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Guid? ModuleId { get; set; }
    public Module? Module { get; set; }

    public Guid ClassroomId { get; set; }
    public Classroom Classroom { get; set; } = null!;

    public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
}
