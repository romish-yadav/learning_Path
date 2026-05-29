using LearnPath.API.DTOs.Classroom;
using LearnPath.API.DTOs.Common;
using LearnPath.API.Responses;

namespace LearnPath.API.Interfaces.Services;

public interface IClassroomService
{
    Task<ApiResponse<ClassroomDetailDto>> GetByIdAsync(Guid id, string userId);
    Task<PagedResponse<ClassroomSummaryDto>> GetMyClassroomsAsync(string userId, PaginationParams pagination);
    Task<ApiResponse<ClassroomDetailDto>> CreateAsync(string userId, CreateClassroomDto dto);
    Task<ApiResponse<ClassroomDetailDto>> UpdateAsync(Guid id, string userId, UpdateClassroomDto dto);
    Task<ApiResponse<bool>> DeleteAsync(Guid id, string userId);
    Task<ApiResponse<bool>> JoinAsync(string userId, JoinClassroomDto dto);
    Task<ApiResponse<bool>> LeaveAsync(Guid id, string userId);
    Task<ApiResponse<bool>> AssignPathAsync(Guid classroomId, Guid pathId, string userId);

    Task<ApiResponse<AssignmentDetailDto>> CreateAssignmentAsync(Guid classroomId, string userId, CreateAssignmentDto dto);
    Task<ApiResponse<SubmissionDto>> SubmitAssignmentAsync(Guid assignmentId, string userId, CreateSubmissionDto dto);
    Task<ApiResponse<SubmissionDto>> GradeSubmissionAsync(Guid submissionId, string userId, GradeSubmissionDto dto);
}
