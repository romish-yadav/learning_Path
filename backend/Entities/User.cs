using Microsoft.AspNetCore.Identity;

namespace LearnPath.API.Entities;

public class User : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public string? Bio { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<LearningPath> CreatedPaths { get; set; } = new List<LearningPath>();
    public ICollection<Progress> Progresses { get; set; } = new List<Progress>();
    public ICollection<UserClassroom> UserClassrooms { get; set; } = new List<UserClassroom>();
    public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
