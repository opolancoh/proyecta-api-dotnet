using Proyecta.Core.DTOs.Auth;
using Proyecta.Core.Entities;
using Proyecta.Core.Entities.Auth;

namespace Proyecta.Core.Contracts.Repositories;

public interface IAuthRepository
{
    Task<ApplicationUser?> GetUserForLogin(LoginDto loginDto);
    Task<RefreshToken?> GetRefreshToken(string userId, string token);
    Task<bool> AddRefreshToken(RefreshToken refreshToken);
    Task<bool> RemoveRefreshToken(string userId, string token);
    
    
}