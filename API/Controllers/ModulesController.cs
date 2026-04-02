using System.Security.Claims;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ModulesController : ControllerBase
{
    private readonly IModulesService _service;
    public ModulesController(IModulesService service) => _service = service;

    [HttpGet("{courseId}")]
    public async Task<IActionResult> GetModulesForCourse(int courseId)
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

        var modules = await _service.GetModulesAsync(courseId, userId);

        return Ok(modules);
    }
}