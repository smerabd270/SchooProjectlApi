using SchooProjectlApi.DTOs;
namespace SchooProjectlApi.Services;
public interface IAuthService
{
    Task<(bool Success, string? Message)> RegisterAsync(UserRegisterDto dto);
    Task<string?> LoginAsync(UserLoginDto dto);
}
