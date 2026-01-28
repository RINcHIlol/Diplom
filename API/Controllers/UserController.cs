using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;
    public UsersController(IUserService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _service.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var user = await _service.GetUserAsync(id);
        if (user == null) return NotFound();
        return Ok(user);
    }
    
    [HttpGet("{login}")]
    public async Task<IActionResult> GetByLogin(string login)
    {
        var user = await _service.GetUserByLoginAsync(login);
        if (user == null) return NotFound();
        return Ok(user);
    }
}