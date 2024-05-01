using Proyecta.Core.DTOs;
using Proyecta.Core.DTOs.Auth;
using Proyecta.Core.Results;

namespace Proyecta.Core.Contracts.Services;

public interface IAuthService
{
    Task<ApplicationResult> Register(RegisterDto registerDto, string currentUserId);
    Task<ApplicationResult> Login(LoginDto loginDto);
    Task<ApplicationResult> Logout(TokenDto tokenDto);
    Task<ApplicationResult> RefreshToken(TokenDto tokenDto);
}