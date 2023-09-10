using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Proyecta.Core.Contracts.Repositories;
using Proyecta.Core.DTOs.Auth;
using Proyecta.Core.Entities.Auth;

namespace Proyecta.Repository.EntityFramework;

public class AuthRepository : IAuthRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly AuthDbContext _context;

    public AuthRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
        AuthDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }

    public async Task<ApplicationUser?> GetUserForLogin(LoginDto loginDto)
    {
        var user = await _userManager.FindByNameAsync(loginDto.Username!);
        if (user == null)
        {
            return null;
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password!, false);
        return result.Succeeded ? user : null;
    }

    public async Task<RefreshToken?> GetRefreshToken(string userId, string token)
    {
        return await _context.RefreshTokens
            .SingleOrDefaultAsync(x => x.UserId == userId && x.Token == token);
    }

    public async Task<bool> AddRefreshToken(RefreshToken refreshToken)
    {
        _context.RefreshTokens.Add(refreshToken);
        try
        {
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<bool> RemoveRefreshToken(string userId, string token)
    {
        var refreshToken = new RefreshToken { UserId = userId, Token = token };
        _context.RefreshTokens.Remove(refreshToken);
        try
        {
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}