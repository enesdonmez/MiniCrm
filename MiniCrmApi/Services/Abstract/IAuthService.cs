using MiniCrmApi.DTOs.AuthDtos;

namespace MiniCrmApi.Services.Abstract;

public interface IAuthService
{
    Task<string?> RegisterAsync(RegisterDto dto);
    Task<string?> LoginAsync(LoginDto dto);
}