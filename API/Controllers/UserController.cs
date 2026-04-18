using System.Security.Claims;
using API.Models;
using API.Services;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;
    public UsersController(IUserService service) => _service = service;

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var users = await _service.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var user = await _service.GetUserAsync(id);
        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpGet("login/{login}")]
    public async Task<IActionResult> GetByLogin(string login)
    {
        var user = await _service.GetUserByLoginAsync(login);
        if (user == null) return NotFound();
        return Ok(user);
    }
    
    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Update([FromBody] RegRequest dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
        var success = await _service.UpdateUserAsync(userId, dto);

        if (!success)
            return NotFound();

        return NoContent();
    }
    
    [HttpPut("{xp}")]
    [Authorize]
    public async Task<IActionResult> Update(int xp)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        
        var success = await _service.UpdateXpAsync(xp, userId);

        if (!success)
            return NotFound();

        return NoContent();
    }
}