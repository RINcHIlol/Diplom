using System.Security.Claims;
using API.DTOs.Profile;
using API.Services.Interfaces;
using diplom.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/tasks")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly ITasksService _service;

    public TasksController(ITasksService service)
    {
        _service = service;
    }

    [HttpGet("lesson/{lessonId}")]
    public async Task<IActionResult> GetByLesson(int lessonId)
    {
        var tasks = await _service.GetTasksAsync(lessonId);
        return Ok(tasks);
    }

    [HttpGet("lesson/{lessonId}/completed")]
    public async Task<ActionResult> GetCompletedTasks(int lessonId)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
        var isCompleted = await _service.GetLessonCompletedAsync(lessonId, userId);
        
        return Ok(isCompleted);
    }

    [HttpPost("submit")]
    public async Task<IActionResult> Submit([FromBody] SubmitDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
        var result = await _service.Submit(dto, userId);

        return Ok(new { isCorrect = result });
    }
}