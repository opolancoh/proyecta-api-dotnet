using Proyecta.Core.DTOs.Auth;
using Proyecta.Core.Responses;

namespace Proyecta.Core.Contracts.Services;

public interface IAuthService
{
    Task<ApiResponse<ApiCreateResponse<string>>> Register(RegisterDto registerDto, string currentUserId);
    Task<ApiResponse<TokenDto>> Login(LoginDto loginDto);
    Task<ApiResponse> Logout(TokenDto tokenDto);
    Task<ApiResponse<RefreshTokenResponse>> RefreshToken(TokenDto tokenDto);
}