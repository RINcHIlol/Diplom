using System.Security.Claims;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LessonsController : ControllerBase
{
    private readonly ILessonsService _service;
    public LessonsController(ILessonsService service) => _service = service;
    
    [HttpGet("{moduleId}")]
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
}