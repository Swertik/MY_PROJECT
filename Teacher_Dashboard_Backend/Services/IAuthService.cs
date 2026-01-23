using Teacher_Dashboard_Backend.DTOs;
using Teacher_Dashboard_Backend.Models;

namespace Teacher_Dashboard_Backend.Services;

public interface IAuthService
{
    Task<AuthResponseDto?> LoginAsync(LoginDto loginDto);
    Task<AuthResponseDto?> RegisterAsync(RegisterDto registerDto);
    Task<bool> UserExistsAsync(string username);
    string GenerateJwtToken(User user);
}