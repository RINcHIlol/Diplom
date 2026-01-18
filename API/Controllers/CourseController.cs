using API.Models;
using API.Services;
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
        var courses = await _service.GetAllCoursesAsync();
        return Ok(courses);
    }

    // [HttpGet("{id}")]
    // public async Task<IActionResult> Get(int id)
    // {
    //     var course = await _service.GetCourseAsync(id);
    //     if (course == null) return NotFound();
    //     return Ok(course);
    // }
    //
    // [HttpPost]
    // public async Task<IActionResult> Create(Course course)
    // {
    //     await _service.AddCourseAsync(course);
    //     return CreatedAtAction(nameof(Get), new { id = course.Id }, course);
    // }
}