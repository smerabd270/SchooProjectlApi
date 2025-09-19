using Microsoft.AspNetCore.Mvc;
using SchooProjectlApi.DTOs;
using SchooProjectlApi.Services;

namespace SchooProjectlApi.Controllers;
[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;
    public AuthController(IAuthService auth) => _auth = auth;

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterDto dto)
    {
        var (success, message) = await _auth.RegisterAsync(dto);
        if (!success) return BadRequest(new { message });
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginDto dto)
    {
        var token = await _auth.LoginAsync(dto);
        if (token == null) return Unauthorized();
        return Ok(new { token });
    }
}
