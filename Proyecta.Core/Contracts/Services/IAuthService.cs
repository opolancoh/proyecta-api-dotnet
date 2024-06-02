using Proyecta.Core.DTOs.ApiResponses;
using Proyecta.Core.DTOs.Auth;

namespace Proyecta.Core.Contracts.Services;

public interface IAuthService
{
    Task<ApiResponse> Register(RegisterDto registerDto);
    Task<ApiResponse> Login(LoginDto loginDto);
    Task<ApiResponse> Logout(TokenDto tokenDto);
    Task<ApiResponse> RefreshToken(TokenDto tokenDto);
}
