using Proyecta.Core.DTOs.ApiResponse;
using Proyecta.Core.DTOs.Auth;

namespace Proyecta.Core.Contracts.Services;

public interface IAuthService
{
    Task<ApiResponse<TokenDto>> Register(RegisterDto registerDto);
    Task<ApiResponse<TokenDto>> Login(LoginDto loginDto);
    Task<ApiResponse> Logout(TokenDto tokenDto);
    Task<ApiResponse<RefreshTokenResponse>> RefreshToken(TokenDto tokenDto);
}