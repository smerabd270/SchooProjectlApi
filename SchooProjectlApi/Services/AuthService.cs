using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using SchooProjectlApi.Data;
using SchooProjectlApi.DTOs;
using SchooProjectlApi.Entities;

namespace SchooProjectlApi.Services;
public class AuthService : IAuthService
{
    private readonly SchoolContext _db;
    private readonly IConfiguration _config;
    private readonly ILogger<AuthService> _logger;

    public AuthService(SchoolContext db, IConfiguration config, ILogger<AuthService> logger)
    {
        _db = db; _config = config; _logger = logger;
    }

    public async Task<(bool Success, string? Message)> RegisterAsync(UserRegisterDto dto)
    {
        if (await _db.Users.AnyAsync(u => u.Username == dto.Username))
            return (false, "Username taken");

        var user = new User
        {
            Username = dto.Username,
            FullName = dto.FullName,
            Role = dto.Role,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        _logger.LogInformation("User {Username} registered with role {Role}", user.Username, user.Role);
        return (true, null);
    }

    public async Task<string?> LoginAsync(UserLoginDto dto)
    {
        var user = await _db.Users.SingleOrDefaultAsync(u => u.Username == dto.Username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        {
            _logger.LogWarning("Failed login for {Username}", dto.Username);
            return null;
        }

        var jwt = _config.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };
        var token = new JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(double.Parse(jwt["ExpireMinutes"] ?? "60")),
            signingCredentials: creds
        );
        var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
        _logger.LogInformation("User {Username} logged in", user.Username);
        return tokenStr;
    }
}
