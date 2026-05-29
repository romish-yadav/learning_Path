using System.ComponentModel.DataAnnotations;

namespace LearnPath.API.DTOs.Classroom;

public class ClassroomSummaryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public int StudentCount { get; set; }
    public int PathCount { get; set; }
    public string InstructorName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class ClassroomDetailDto : ClassroomSummaryDto
{
    public string? JoinCode { get; set; }
    public string InstructorId { get; set; } = string.Empty;
    public IEnumerable<ClassroomMemberDto> Members { get; set; } = Enumerable.Empty<ClassroomMemberDto>();
    public IEnumerable<AssignmentSummaryDto> Assignments { get; set; } = Enumerable.Empty<AssignmentSummaryDto>();
}

public class ClassroomMemberDto
{
    public string UserId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public string Role { get; set; } = string.Empty;
    public DateTime JoinedAt { get; set; }
}

public class CreateClassroomDto
{
    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }
}

public class UpdateClassroomDto
{
    [MaxLength(200)]
    public string? Name { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    public bool? IsActive { get; set; }
}

public class JoinClassroomDto
{
    [Required]
    public string JoinCode { get; set; } = string.Empty;
}

public class AssignmentSummaryDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public int MaxScore { get; set; }
    public int SubmissionCount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class AssignmentDetailDto : AssignmentSummaryDto
{
    public string? Instructions { get; set; }
    public Guid? ModuleId { get; set; }
    public Guid ClassroomId { get; set; }
    public IEnumerable<SubmissionDto> Submissions { get; set; } = Enumerable.Empty<SubmissionDto>();
}

public class CreateAssignmentDto
{
    [Required, MaxLength(300)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Description { get; set; }

    public string? Instructions { get; set; }
    public DateTime? DueDate { get; set; }
    public int MaxScore { get; set; } = 100;
    public Guid? ModuleId { get; set; }
}

public class SubmissionDto
{
    public Guid Id { get; set; }
    public string? Content { get; set; }
    public string? FileUrl { get; set; }
    public int? Score { get; set; }
    public string? Feedback { get; set; }
    public string Status { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
    public DateTime SubmittedAt { get; set; }
    public DateTime? GradedAt { get; set; }
}

public class CreateSubmissionDto
{
    public string? Content { get; set; }
    public string? FileUrl { get; set; }
}

public class GradeSubmissionDto
{
    [Required, Range(0, 100)]
    public int Score { get; set; }

    [MaxLength(2000)]
    public string? Feedback { get; set; }
}
