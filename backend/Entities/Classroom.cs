namespace LearnPath.API.Entities;

public class Classroom
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? JoinCode { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public string InstructorId { get; set; } = string.Empty;
    public User Instructor { get; set; } = null!;

    public ICollection<UserClassroom> UserClassrooms { get; set; } = new List<UserClassroom>();
    public ICollection<ClassroomLearningPath> ClassroomLearningPaths { get; set; } = new List<ClassroomLearningPath>();
    public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
}
