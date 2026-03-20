using System.Security.Claims;
using System.Text;
using API.Models;
using API.Services;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RegController : ControllerBase
{
    private readonly IUserService _userService;
    public RegController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Reg([FromBody] RegRequest request)
    {
        var newUser = new User()
        {
            Login = request.Login,
            PasswordHash = request.Password,
            Email = request.Email,
            CreatedAt = DateTime.UtcNow,
            RoleId = 1,
            Xp = 0
        };
        
        if (string.IsNullOrWhiteSpace(request.Login) ||
            string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new { message = "Заполните все поля." });
        }
        
        var existingByEmail = await _userService.GetUserByEmailAsync(request.Email);
        if (existingByEmail != null)
        {
            return Conflict(new { message = "Пользователь с такой почтой уже существует." });   
        }
        var existingByLogin = await _userService.GetUserByLoginAsync(request.Login); 
        if (existingByLogin != null) 
        {
            return Conflict(new { message = "Пользователь с таким логином уже существует." });
        }
        
        await _userService.AddUserAsync(newUser);
        return Ok();
    }
}

