using Microsoft.AspNetCore.Mvc;
using Teacher_Dashboard_Backend.DTOs;
using Teacher_Dashboard_Backend.Services;

namespace Teacher_Dashboard_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _authService.LoginAsync(loginDto);
        
        if (result == null)
        {
            return Unauthorized(new { message = "Неверное имя пользователя или пароль" });
        }

        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (await _authService.UserExistsAsync(registerDto.Username))
        {
            return BadRequest(new { message = "Пользователь с таким именем уже существует" });
        }

        var result = await _authService.RegisterAsync(registerDto);
        
        if (result == null)
        {
            return BadRequest(new { message = "Ошибка при регистрации пользователя" });
        }

        return Ok(result);
    }
}