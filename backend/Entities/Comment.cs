namespace LearnPath.API.Entities;

public class Comment
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public CommentTarget TargetType { get; set; }
    public Guid TargetId { get; set; }
    public Guid? ParentCommentId { get; set; }
    public Comment? ParentComment { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public string AuthorId { get; set; } = string.Empty;
    public User Author { get; set; } = null!;

    public ICollection<Comment> Replies { get; set; } = new List<Comment>();
}

public enum CommentTarget
{
    LearningPath,
    Module,
    Assignment,
    Community
}
