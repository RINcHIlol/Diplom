using System.Security.Claims;
using API.DTOs.Profile;
using API.Models;
using API.Services;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _service;
    public CoursesController(ICourseService service) => _service = service;
    [HttpGet]
    public async Task<IActionResult> GetAll()
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

        var courses = await _service.GetCoursesAsync(userId);

        return Ok(courses);
    }
    
    [HttpGet("my")]
    [Authorize]
    public async Task<IActionResult> GetAllForCreator()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var courses = await _service.GetCoursesForCreatorAsync(userId);

        return Ok(courses);
    }
    
    [HttpGet("admin")]
    [Authorize]
    public async Task<IActionResult> GetAllForAdmin()
    {
        var courses = await _service.GetCoursesAsync();

        return Ok(courses);
    }
    
    [HttpGet("{courseId}")]
    [Authorize]
    public async Task<IActionResult> GetCourseById(int courseId)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    
        var course = await _service.GetCourseAsync(courseId);
    
        return Ok(course);
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateUpdateCourseDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var course = await _service.CreateCourseAsync(userId, dto);

        return Ok(course);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, [FromBody] CreateUpdateCourseDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var success = await _service.UpdateCourseAsync(userId, id, dto);

        if (!success)
            return NotFound();

        return NoContent();
    }
    
    [HttpDelete("{courseId}")]
    [Authorize]
    public async Task<IActionResult> Delete([FromRoute] int courseId)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        if (!await _service.IsOwnerAsync(courseId, userId))
            return Forbid();
        
        var success = await _service.DeleteAsync(courseId);

        if (!success)
            return NotFound();

        return NoContent();
    }
}