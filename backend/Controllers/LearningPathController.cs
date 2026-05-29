using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LearnPath.API.DTOs.Common;
using LearnPath.API.DTOs.LearningPath;
using LearnPath.API.Interfaces.Services;

namespace LearnPath.API.Controllers;

[ApiController]
[Route("api/v1/paths")]
public class LearningPathController : ControllerBase
{
    private readonly ILearningPathService _pathService;

    public LearningPathController(ILearningPathService pathService)
    {
        _pathService = pathService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPublicPaths([FromQuery] PathFilterParams filter)
    {
        var result = await _pathService.GetPublicPathsAsync(filter);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _pathService.GetByIdAsync(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [Authorize]
    [HttpGet("my")]
    public async Task<IActionResult> GetMyPaths([FromQuery] PaginationParams pagination)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _pathService.GetMyPathsAsync(userId, pagination);
        return Ok(result);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLearningPathDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _pathService.CreateAsync(userId, dto);
        return result.Success ? CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result) : BadRequest(result);
    }

    [Authorize]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateLearningPathDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _pathService.UpdateAsync(id, userId, dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _pathService.DeleteAsync(id, userId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [Authorize]
    [HttpPost("{id:guid}/publish")]
    public async Task<IActionResult> Publish(Guid id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _pathService.PublishAsync(id, userId);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [Authorize]
    [HttpPost("{pathId:guid}/modules")]
    public async Task<IActionResult> AddModule(Guid pathId, [FromBody] CreateModuleDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _pathService.AddModuleAsync(pathId, userId, dto);
        return result.Success ? CreatedAtAction(nameof(GetById), new { id = pathId }, result) : BadRequest(result);
    }

    [Authorize]
    [HttpPut("{pathId:guid}/modules/{moduleId:guid}")]
    public async Task<IActionResult> UpdateModule(Guid pathId, Guid moduleId, [FromBody] UpdateModuleDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _pathService.UpdateModuleAsync(pathId, moduleId, userId, dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [Authorize]
    [HttpDelete("{pathId:guid}/modules/{moduleId:guid}")]
    public async Task<IActionResult> DeleteModule(Guid pathId, Guid moduleId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var result = await _pathService.DeleteModuleAsync(pathId, moduleId, userId);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
