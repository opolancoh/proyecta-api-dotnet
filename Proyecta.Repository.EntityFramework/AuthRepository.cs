using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Proyecta.Core.Contracts.Repositories;
using Proyecta.Core.DTOs.Auth;
using Proyecta.Core.Entities;
using Proyecta.Core.Entities.Auth;

namespace Proyecta.Repository.EntityFramework;

public class AuthRepository : IAuthRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly AuthDbContext _context;

    public AuthRepository(UserManager<ApplicationUser> userManager,
        AuthDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<ApplicationUser?> GetUserForLogin(LoginDto loginDto)
    {
        var user = await _userManager.FindByNameAsync(loginDto.Username);
        if (user == null)
        {
            return null;
        }

        var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
        return result ? user : null;
    }

    public async Task<RefreshToken?> GetRefreshToken(string userId, string token)
    {
        return await _context.RefreshTokens
            .SingleOrDefaultAsync(x => x.UserId == userId && x.Token == token);
    }

    public async Task<bool> AddRefreshToken(RefreshToken refreshToken)
    {
        _context.RefreshTokens.Add(refreshToken);

        var result = await _context.SaveChangesAsync();

        return result > 0;
    }

    public async Task<bool> RemoveRefreshToken(string userId, string token)
    {
        var refreshToken = new RefreshToken { UserId = userId, Token = token };
        _context.RefreshTokens.Remove(refreshToken);

        var result = await _context.SaveChangesAsync();

        return result > 0;
    }
}