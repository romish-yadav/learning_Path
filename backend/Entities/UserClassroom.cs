namespace LearnPath.API.Entities;

public class UserClassroom
{
    public string UserId { get; set; } = string.Empty;
    public User User { get; set; } = null!;

    public Guid ClassroomId { get; set; }
    public Classroom Classroom { get; set; } = null!;

    public ClassroomRole Role { get; set; } = ClassroomRole.Student;
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
}

public enum ClassroomRole
{
    Student,
    Instructor,
    Assistant
}
