using System.Security.Claims;
using API.DTOs.Profile;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/modules/{moduleId}/lessons")]
[Authorize]
public class LessonsController : ControllerBase
{
    private readonly ILessonsService _service;
    public LessonsController(ILessonsService service) => _service = service;
    
    [HttpGet]
    public async Task<IActionResult> GetLessonsForModule(int moduleId)
    {
        int? userId = null;

        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null && int.TryParse(claim.Value, out var id))
            {
                userId = id;
            }
        }

        var lessons = await _service.GetLessonsAsync(moduleId, userId);

        return Ok(lessons);
    }
        
    [HttpGet("my")]
    [Authorize]
    public async Task<IActionResult> GetAllForCreator([FromRoute] int moduleId)
    {
        var lessons = await _service.GetLessonsForCreatorAsync(moduleId);

        return Ok(lessons);
    }
    
    [HttpGet("{lessonId}")]
    [Authorize]
    public async Task<IActionResult> GetLessonById([FromRoute] int lessonId)
    {
        var module = await _service.GetLessonAsync(lessonId);
    
        return Ok(module);
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromRoute] int moduleId, [FromBody] CreateUpdateLessonDTO dto)
    {
        var lesson = await _service.CreateLessonAsync(dto, moduleId);

        return Ok(lesson);
    }

    [HttpPut("{lessonId}")]
    [Authorize]
    public async Task<IActionResult> Update([FromRoute] int lessonId, [FromBody] CreateUpdateLessonDTO dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        if (!await _service.IsOwnerAsync(lessonId, userId))
            return Forbid();
        
        var success = await _service.UpdateLessonAsync(lessonId, dto);

        if (!success)
            return NotFound();

        return NoContent();
    }
    
    [HttpDelete("{lessonId}")]
    [Authorize]
    public async Task<IActionResult> Delete([FromRoute] int lessonId)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        if (!await _service.IsOwnerAsync(lessonId, userId))
            return Forbid();
        
        var success = await _service.DeleteAsync(lessonId);

        if (!success)
            return NotFound();

        return NoContent();
    }
}