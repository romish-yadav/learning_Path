using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LearnPath.API.DTOs.Classroom;
using LearnPath.API.DTOs.Common;
using LearnPath.API.Interfaces.Services;

namespace LearnPath.API.Controllers;

[ApiController]
[Route("api/v1/classrooms")]
[Authorize]
public class ClassroomController : ControllerBase
{
    private readonly IClassroomService _classroomService;

    public ClassroomController(IClassroomService classroomService)
    {
        _classroomService = classroomService;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _classroomService.GetByIdAsync(id, userId);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMyClassrooms([FromQuery] PaginationParams pagination)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _classroomService.GetMyClassroomsAsync(userId, pagination);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateClassroomDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _classroomService.CreateAsync(userId, dto);
        return result.Success ? CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result) : BadRequest(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateClassroomDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _classroomService.UpdateAsync(id, userId, dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _classroomService.DeleteAsync(id, userId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost("join")]
    public async Task<IActionResult> Join([FromBody] JoinClassroomDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _classroomService.JoinAsync(userId, dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost("{id:guid}/leave")]
    public async Task<IActionResult> Leave(Guid id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _classroomService.LeaveAsync(id, userId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost("{classroomId:guid}/paths/{pathId:guid}")]
    public async Task<IActionResult> AssignPath(Guid classroomId, Guid pathId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _classroomService.AssignPathAsync(classroomId, pathId, userId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost("{classroomId:guid}/assignments")]
    public async Task<IActionResult> CreateAssignment(Guid classroomId, [FromBody] CreateAssignmentDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _classroomService.CreateAssignmentAsync(classroomId, userId, dto);
        return result.Success ? CreatedAtAction(nameof(GetById), new { id = classroomId }, result) : BadRequest(result);
    }

    [HttpPost("assignments/{assignmentId:guid}/submit")]
    public async Task<IActionResult> Submit(Guid assignmentId, [FromBody] CreateSubmissionDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _classroomService.SubmitAssignmentAsync(assignmentId, userId, dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost("submissions/{submissionId:guid}/grade")]
    public async Task<IActionResult> Grade(Guid submissionId, [FromBody] GradeSubmissionDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _classroomService.GradeSubmissionAsync(submissionId, userId, dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
