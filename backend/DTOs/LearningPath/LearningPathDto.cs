using System.ComponentModel.DataAnnotations;
using LearnPath.API.Entities;

namespace LearnPath.API.DTOs.LearningPath;

public class LearningPathSummaryDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ThumbnailUrl { get; set; }
    public PathDifficulty Difficulty { get; set; }
    public bool IsPublished { get; set; }
    public int EstimatedHours { get; set; }
    public string? Tags { get; set; }
    public int ModuleCount { get; set; }
    public double AverageRating { get; set; }
    public string CreatorName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class LearningPathDetailDto : LearningPathSummaryDto
{
    public string CreatorId { get; set; } = string.Empty;
    public bool IsPublic { get; set; }
    public IEnumerable<ModuleDto> Modules { get; set; } = Enumerable.Empty<ModuleDto>();
    public int RatingCount { get; set; }
}

public class CreateLearningPathDto
{
    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Description { get; set; }

    public string? ThumbnailUrl { get; set; }
    public PathDifficulty Difficulty { get; set; } = PathDifficulty.Beginner;
    public bool IsPublic { get; set; } = true;
    public string? Tags { get; set; }
    public int EstimatedHours { get; set; }
}

public class UpdateLearningPathDto
{
    [MaxLength(200)]
    public string? Title { get; set; }

    [MaxLength(2000)]
    public string? Description { get; set; }

    public string? ThumbnailUrl { get; set; }
    public PathDifficulty? Difficulty { get; set; }
    public bool? IsPublic { get; set; }
    public bool? IsPublished { get; set; }
    public string? Tags { get; set; }
    public int? EstimatedHours { get; set; }
}

public class ModuleDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Content { get; set; }
    public string? ResourceUrl { get; set; }
    public ModuleType Type { get; set; }
    public int OrderIndex { get; set; }
    public int EstimatedMinutes { get; set; }
    public IEnumerable<Guid> PrerequisiteIds { get; set; } = Enumerable.Empty<Guid>();
}

public class CreateModuleDto
{
    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string? Description { get; set; }

    public string? Content { get; set; }
    public string? ResourceUrl { get; set; }
    public ModuleType Type { get; set; } = ModuleType.Lesson;
    public int OrderIndex { get; set; }
    public int EstimatedMinutes { get; set; }
    public IEnumerable<Guid>? PrerequisiteIds { get; set; }
}

public class UpdateModuleDto
{
    [MaxLength(200)]
    public string? Title { get; set; }

    [MaxLength(2000)]
    public string? Description { get; set; }

    public string? Content { get; set; }
    public string? ResourceUrl { get; set; }
    public ModuleType? Type { get; set; }
    public int? OrderIndex { get; set; }
    public int? EstimatedMinutes { get; set; }
    public IEnumerable<Guid>? PrerequisiteIds { get; set; }
}
