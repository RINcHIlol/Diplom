using System.Security.Claims;
using API.Models;
using API.Services;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _service;
    public CoursesController(ICourseService service) => _service = service;

    // [HttpGet]
    // public async Task<IActionResult> GetAll(User? user)
    // {
    //     var courses = await _service.GetCoursesAsync(user);
    //     return Ok(courses);
    // }
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
}