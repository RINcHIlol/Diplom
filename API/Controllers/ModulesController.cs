using System.Security.Claims;
using API.DTOs.Profile;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/courses/{courseId}/modules")]
public class ModulesController : ControllerBase
{
    private readonly IModulesService _service;
    public ModulesController(IModulesService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetModulesForCourse([FromRoute] int courseId)
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
    
    [HttpGet("my")]
    [Authorize]
    public async Task<IActionResult> GetAllForCreator([FromRoute] int courseId)
    {
        var modules = await _service.GetModulesForCreatorAsync(courseId);

        return Ok(modules);
    }
    
    [HttpGet("{moduleId}")]
    [Authorize]
    public async Task<IActionResult> GetModuleById([FromRoute] int moduleId)
    {
        var module = await _service.GetModuleAsync(moduleId);
    
        return Ok(module);
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromRoute] int courseId, [FromBody] CreateUpdateModuleDto dto)
    {
        var course = await _service.CreateModuleAsync(dto, courseId);

        return Ok(course);
    }

    [HttpPut("{moduleId}")]
    [Authorize]
    public async Task<IActionResult> Update([FromRoute] int moduleId, [FromBody] CreateUpdateModuleDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        if (!await _service.IsOwnerAsync(moduleId, userId))
            return Forbid();
        
        var success = await _service.UpdateModuleAsync(moduleId, dto);

        if (!success)
            return NotFound();

        return NoContent();
    }
}