namespace LearnPath.API.Entities;

public class ClassroomLearningPath
{
    public Guid ClassroomId { get; set; }
    public Classroom Classroom { get; set; } = null!;

    public Guid LearningPathId { get; set; }
    public LearningPath LearningPath { get; set; } = null!;

    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
}
