using System.ComponentModel.DataAnnotations;

namespace LearnPath.API.DTOs.Community;

public class CommentDto
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public string AuthorId { get; set; } = string.Empty;
    public string AuthorName { get; set; } = string.Empty;
    public string? AuthorAvatarUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public IEnumerable<CommentDto> Replies { get; set; } = Enumerable.Empty<CommentDto>();
}

public class CreateCommentDto
{
    [Required, MaxLength(5000)]
    public string Content { get; set; } = string.Empty;

    [Required]
    public string TargetType { get; set; } = string.Empty;

    [Required]
    public Guid TargetId { get; set; }

    public Guid? ParentCommentId { get; set; }
}

public class RatingDto
{
    public Guid Id { get; set; }
    public int Score { get; set; }
    public string? Review { get; set; }
    public string UserName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateRatingDto
{
    [Required, Range(1, 5)]
    public int Score { get; set; }

    [MaxLength(2000)]
    public string? Review { get; set; }
}

public class NotificationDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Message { get; set; }
    public string Type { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public string? ActionUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}
