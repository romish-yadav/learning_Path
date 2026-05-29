using LearnPath.API.DTOs.Classroom;
using LearnPath.API.DTOs.Common;
using Microsoft.EntityFrameworkCore;
using LearnPath.API.Entities;
using LearnPath.API.Interfaces.Repositories;
using LearnPath.API.Interfaces.Services;
using LearnPath.API.Responses;

namespace LearnPath.API.Services.Classroom;

public class ClassroomService : IClassroomService
{
    private readonly IClassroomRepository _classroomRepo;
    private readonly IAssignmentRepository _assignmentRepo;
    private readonly ISubmissionRepository _submissionRepo;
    private readonly Data.ApplicationDbContext _context;

    public ClassroomService(
        IClassroomRepository classroomRepo,
        IAssignmentRepository assignmentRepo,
        ISubmissionRepository submissionRepo,
        Data.ApplicationDbContext context)
    {
        _classroomRepo = classroomRepo;
        _assignmentRepo = assignmentRepo;
        _submissionRepo = submissionRepo;
        _context = context;
    }

    public async Task<ApiResponse<ClassroomDetailDto>> GetByIdAsync(Guid id, string userId)
    {
        var classroom = await _classroomRepo.GetWithDetailsAsync(id);
        if (classroom == null)
            return ApiResponse<ClassroomDetailDto>.Fail("Classroom not found.");

        var isMember = classroom.InstructorId == userId ||
                       classroom.UserClassrooms.Any(uc => uc.UserId == userId);
        if (!isMember)
            return ApiResponse<ClassroomDetailDto>.Fail("You are not a member of this classroom.");

        return ApiResponse<ClassroomDetailDto>.Ok(MapToDetailDto(classroom));
    }

    public async Task<PagedResponse<ClassroomSummaryDto>> GetMyClassroomsAsync(string userId, PaginationParams pagination)
    {
        var classrooms = await _classroomRepo.GetByUserAsync(userId, pagination.Page, pagination.PageSize);
        return new PagedResponse<ClassroomSummaryDto>
        {
            Data = classrooms.Select(MapToSummaryDto),
            Page = pagination.Page,
            PageSize = pagination.PageSize,
            TotalCount = classrooms.Count()
        };
    }

    public async Task<ApiResponse<ClassroomDetailDto>> CreateAsync(string userId, CreateClassroomDto dto)
    {
        var classroom = new Entities.Classroom
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            InstructorId = userId,
            JoinCode = GenerateJoinCode()
        };

        await _classroomRepo.AddAsync(classroom);

        _context.UserClassrooms.Add(new UserClassroom
        {
            UserId = userId,
            ClassroomId = classroom.Id,
            Role = ClassroomRole.Instructor
        });
        await _context.SaveChangesAsync();

        var created = await _classroomRepo.GetWithDetailsAsync(classroom.Id);
        return ApiResponse<ClassroomDetailDto>.Ok(MapToDetailDto(created!), "Classroom created.");
    }

    public async Task<ApiResponse<ClassroomDetailDto>> UpdateAsync(Guid id, string userId, UpdateClassroomDto dto)
    {
        var classroom = await _classroomRepo.GetByIdAsync(id);
        if (classroom == null)
            return ApiResponse<ClassroomDetailDto>.Fail("Classroom not found.");
        if (classroom.InstructorId != userId)
            return ApiResponse<ClassroomDetailDto>.Fail("Only the instructor can update the classroom.");

        if (dto.Name != null) classroom.Name = dto.Name;
        if (dto.Description != null) classroom.Description = dto.Description;
        if (dto.IsActive.HasValue) classroom.IsActive = dto.IsActive.Value;
        classroom.UpdatedAt = DateTime.UtcNow;

        await _classroomRepo.UpdateAsync(classroom);
        var updated = await _classroomRepo.GetWithDetailsAsync(id);
        return ApiResponse<ClassroomDetailDto>.Ok(MapToDetailDto(updated!), "Classroom updated.");
    }

    public async Task<ApiResponse<bool>> DeleteAsync(Guid id, string userId)
    {
        var classroom = await _classroomRepo.GetByIdAsync(id);
        if (classroom == null)
            return ApiResponse<bool>.Fail("Classroom not found.");
        if (classroom.InstructorId != userId)
            return ApiResponse<bool>.Fail("Only the instructor can delete the classroom.");

        await _classroomRepo.DeleteAsync(classroom);
        return ApiResponse<bool>.Ok(true, "Classroom deleted.");
    }

    public async Task<ApiResponse<bool>> JoinAsync(string userId, JoinClassroomDto dto)
    {
        var classroom = await _classroomRepo.GetByJoinCodeAsync(dto.JoinCode);
        if (classroom == null)
            return ApiResponse<bool>.Fail("Invalid join code.");

        var alreadyMember = await _context.UserClassrooms
            .AnyAsync(uc => uc.UserId == userId && uc.ClassroomId == classroom.Id);
        if (alreadyMember)
            return ApiResponse<bool>.Fail("You are already a member of this classroom.");

        _context.UserClassrooms.Add(new UserClassroom
        {
            UserId = userId,
            ClassroomId = classroom.Id,
            Role = ClassroomRole.Student
        });
        await _context.SaveChangesAsync();
        return ApiResponse<bool>.Ok(true, "Joined classroom successfully.");
    }

    public async Task<ApiResponse<bool>> LeaveAsync(Guid id, string userId)
    {
        var membership = await _context.UserClassrooms
            .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.ClassroomId == id);
        if (membership == null)
            return ApiResponse<bool>.Fail("You are not a member of this classroom.");

        _context.UserClassrooms.Remove(membership);
        await _context.SaveChangesAsync();
        return ApiResponse<bool>.Ok(true, "Left classroom.");
    }

    public async Task<ApiResponse<bool>> AssignPathAsync(Guid classroomId, Guid pathId, string userId)
    {
        var classroom = await _classroomRepo.GetByIdAsync(classroomId);
        if (classroom == null)
            return ApiResponse<bool>.Fail("Classroom not found.");
        if (classroom.InstructorId != userId)
            return ApiResponse<bool>.Fail("Only the instructor can assign paths.");

        _context.ClassroomLearningPaths.Add(new ClassroomLearningPath
        {
            ClassroomId = classroomId,
            LearningPathId = pathId
        });
        await _context.SaveChangesAsync();
        return ApiResponse<bool>.Ok(true, "Path assigned to classroom.");
    }

    public async Task<ApiResponse<AssignmentDetailDto>> CreateAssignmentAsync(Guid classroomId, string userId, CreateAssignmentDto dto)
    {
        var classroom = await _classroomRepo.GetByIdAsync(classroomId);
        if (classroom == null)
            return ApiResponse<AssignmentDetailDto>.Fail("Classroom not found.");
        if (classroom.InstructorId != userId)
            return ApiResponse<AssignmentDetailDto>.Fail("Only the instructor can create assignments.");

        var assignment = new Assignment
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Description = dto.Description,
            Instructions = dto.Instructions,
            DueDate = dto.DueDate,
            MaxScore = dto.MaxScore,
            ModuleId = dto.ModuleId,
            ClassroomId = classroomId
        };

        await _assignmentRepo.AddAsync(assignment);
        return ApiResponse<AssignmentDetailDto>.Ok(new AssignmentDetailDto
        {
            Id = assignment.Id,
            Title = assignment.Title,
            Description = assignment.Description,
            Instructions = assignment.Instructions,
            DueDate = assignment.DueDate,
            MaxScore = assignment.MaxScore,
            ModuleId = assignment.ModuleId,
            ClassroomId = assignment.ClassroomId,
            CreatedAt = assignment.CreatedAt
        }, "Assignment created.");
    }

    public async Task<ApiResponse<SubmissionDto>> SubmitAssignmentAsync(Guid assignmentId, string userId, CreateSubmissionDto dto)
    {
        var assignment = await _assignmentRepo.GetByIdAsync(assignmentId);
        if (assignment == null)
            return ApiResponse<SubmissionDto>.Fail("Assignment not found.");

        var existing = await _submissionRepo.GetByStudentAndAssignmentAsync(userId, assignmentId);
        if (existing != null)
            return ApiResponse<SubmissionDto>.Fail("You have already submitted this assignment.");

        var submission = new Submission
        {
            Id = Guid.NewGuid(),
            Content = dto.Content,
            FileUrl = dto.FileUrl,
            StudentId = userId,
            AssignmentId = assignmentId
        };

        await _submissionRepo.AddAsync(submission);
        return ApiResponse<SubmissionDto>.Ok(new SubmissionDto
        {
            Id = submission.Id,
            Content = submission.Content,
            FileUrl = submission.FileUrl,
            Status = submission.Status.ToString(),
            SubmittedAt = submission.SubmittedAt
        }, "Assignment submitted.");
    }

    public async Task<ApiResponse<SubmissionDto>> GradeSubmissionAsync(Guid submissionId, string userId, GradeSubmissionDto dto)
    {
        var submission = await _submissionRepo.GetByIdAsync(submissionId);
        if (submission == null)
            return ApiResponse<SubmissionDto>.Fail("Submission not found.");

        var assignment = await _assignmentRepo.GetByIdAsync(submission.AssignmentId);
        if (assignment == null)
            return ApiResponse<SubmissionDto>.Fail("Assignment not found.");

        var classroom = await _classroomRepo.GetByIdAsync(assignment.ClassroomId);
        if (classroom == null || classroom.InstructorId != userId)
            return ApiResponse<SubmissionDto>.Fail("Only the instructor can grade submissions.");

        submission.Score = dto.Score;
        submission.Feedback = dto.Feedback;
        submission.Status = SubmissionStatus.Graded;
        submission.GradedAt = DateTime.UtcNow;

        await _submissionRepo.UpdateAsync(submission);
        return ApiResponse<SubmissionDto>.Ok(new SubmissionDto
        {
            Id = submission.Id,
            Content = submission.Content,
            Score = submission.Score,
            Feedback = submission.Feedback,
            Status = submission.Status.ToString(),
            SubmittedAt = submission.SubmittedAt,
            GradedAt = submission.GradedAt
        }, "Submission graded.");
    }

    private static string GenerateJoinCode() =>
        Guid.NewGuid().ToString("N")[..8].ToUpper();

    private static ClassroomSummaryDto MapToSummaryDto(Entities.Classroom classroom) => new()
    {
        Id = classroom.Id,
        Name = classroom.Name,
        Description = classroom.Description,
        IsActive = classroom.IsActive,
        StudentCount = classroom.UserClassrooms.Count(uc => uc.Role == ClassroomRole.Student),
        PathCount = classroom.ClassroomLearningPaths.Count,
        InstructorName = classroom.Instructor != null
            ? $"{classroom.Instructor.FirstName} {classroom.Instructor.LastName}"
            : string.Empty,
        CreatedAt = classroom.CreatedAt
    };

    private static ClassroomDetailDto MapToDetailDto(Entities.Classroom classroom) => new()
    {
        Id = classroom.Id,
        Name = classroom.Name,
        Description = classroom.Description,
        IsActive = classroom.IsActive,
        JoinCode = classroom.JoinCode,
        InstructorId = classroom.InstructorId,
        StudentCount = classroom.UserClassrooms.Count(uc => uc.Role == ClassroomRole.Student),
        PathCount = classroom.ClassroomLearningPaths.Count,
        InstructorName = classroom.Instructor != null
            ? $"{classroom.Instructor.FirstName} {classroom.Instructor.LastName}"
            : string.Empty,
        CreatedAt = classroom.CreatedAt,
        Members = classroom.UserClassrooms.Select(uc => new ClassroomMemberDto
        {
            UserId = uc.UserId,
            Name = $"{uc.User.FirstName} {uc.User.LastName}",
            Email = uc.User.Email ?? string.Empty,
            AvatarUrl = uc.User.AvatarUrl,
            Role = uc.Role.ToString(),
            JoinedAt = uc.JoinedAt
        }),
        Assignments = classroom.Assignments.Select(a => new AssignmentSummaryDto
        {
            Id = a.Id,
            Title = a.Title,
            Description = a.Description,
            DueDate = a.DueDate,
            MaxScore = a.MaxScore,
            SubmissionCount = a.Submissions.Count,
            CreatedAt = a.CreatedAt
        })
    };

}
