using Proyecta.Core.DTOs.ApiResponse;
using Proyecta.Core.DTOs.Auth;

namespace Proyecta.Core.Contracts.Services;

public interface IAuthService
{
    Task<IApiResponse> Register(RegisterDto registerDto);
    Task<IApiResponse> Login(LoginDto loginDto);
    Task<IApiResponse> Logout(TokenDto tokenDto);
    Task<IApiResponse> RefreshToken(TokenDto tokenDto);
}