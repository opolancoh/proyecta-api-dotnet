using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Proyecta.Core.Contracts.Services;
using Proyecta.Core.Entities;
using Proyecta.Core.DTOs;
using Proyecta.Core.Exceptions;
using Proyecta.Core.Models;
using Proyecta.Core.Models.Auth;

namespace Proyecta.Services;

public sealed class AuthenticationService : IAuthenticationService
{
    private readonly ILogger<ApplicationUserService> _logger;
    private readonly IApplicationUserService _appUserService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    // private ApplicationUser? _user;

    public AuthenticationService(
        ILogger<ApplicationUserService> logger,
        IApplicationUserService appUserService,
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration
    )
    {
        _logger = logger;
        _appUserService = appUserService;
        _userManager = userManager;
        _configuration = configuration;
    }

    private string AuthenticationFailedMessage =>
        $"{nameof(Login)}: Authentication failed. Wrong user name or password.";

    public async Task<ApplicationResult> Register(RegisterInputModel item)
    {
        var newUser = new ApplicationUserCreateOrUpdateDto
        {
            FirstName = item.FirstName,
            LastName = item.LastName,
            UserName = item.UserName,
            Password = item.Password
        };

        return await _appUserService.Create(newUser);
    }

    public async Task<ApplicationResult> Login(LoginInputModel item)
    {
        // Username validation
        var user = await _userManager.FindByNameAsync(item.UserName!);
        if (user == null)
        {
            _logger.LogWarning(AuthenticationFailedMessage);
            return new ApplicationResult
            {
                Status = 401,
                Message = AuthenticationFailedMessage
            };
        }

        // Password validation
        var result = await _userManager.CheckPasswordAsync(user, item.Password!);
        if (!result)
        {
            _logger.LogWarning(AuthenticationFailedMessage);
            return new ApplicationResult
            {
                Status = 401,
                Message = AuthenticationFailedMessage
            };
        }

        // Token generation
        var token = await GenerateTokens(user);

        return new ApplicationResult
        {
            Status = 200,
            D = new { token.AccessToken, token.RefreshToken }
        };
    }
    
    public async Task<ApplicationResult> RefreshToken(AuthTokenInputModel item)
    {
        var principal = GetPrincipalFromExpiredToken(item.AccessToken!);
        var username = principal.Identity.Name;

        var user = _userManager.Users.SingleOrDefault(u => u.UserName == username);
        if (user == null || user.RefreshToken != item.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return new ApplicationResult
            {
                Status = 400,
                Message = "Token is not valid."
            };
        }

        var newJwtToken = await GenerateAccessToken(user);
        var newRefreshToken = GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        await _userManager.UpdateAsync(user);

        return new ApplicationResult
        {
            Status = 200,
            D = new { AccessToken = newJwtToken, RefreshToken = newRefreshToken }
        };
    }
    
    public async Task<ApplicationResult> RevokeToken(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
        {
            return new ApplicationResult
            {
                Status = 400,
                Message = "Username is not valid."
            };
        }

        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = null;
        await _userManager.UpdateAsync(user);

        return new ApplicationResult
        {
            Status = 204
        };
    }

    private async Task<AuthTokenInputModel> GenerateTokens(ApplicationUser user)
    {
        // Access token creation
        var accessToken = await GenerateAccessToken(user);

        // Refresh token creation
        var refreshToken = GenerateRefreshToken();

        // Update user with refresh token data
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime =
            DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["JwtConfig:RefreshTokenExpirationInDays"]));
        await _userManager.UpdateAsync(user);

        return new AuthTokenInputModel { AccessToken = accessToken, RefreshToken = refreshToken };
    }
    
    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Secret"]!)),
            ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }

    private async Task<IEnumerable<Claim>> GetClaims(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            // Add user data
            new("sub", user!.Id),
            new("name", $"{user.FirstName} {user.LastName}"),
            new(ClaimTypes.Name, user.UserName!)
        };

        // Add user roles
        var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim("roles", role)));

        var isAdmin = roles.Contains("Administrator");
        claims.Add(new Claim("isAdmin", isAdmin.ToString(), ClaimValueTypes.Boolean));

        return claims;
    }

    private async Task<string> GenerateAccessToken(ApplicationUser user)
    {
        var jwtSettings = _configuration.GetSection("JwtConfig");

        // Access token creation
        // Signing Credentials
        var key = Encoding.UTF8.GetBytes(jwtSettings["Secret"]!);
        var secret = new SymmetricSecurityKey(key);
        var signingCredentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        // Claims
        var claims = await GetClaims(user);
        // Access Token
        var token = new JwtSecurityToken
        (
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["AccessTokenExpirationInMinutes"])),
            signingCredentials: signingCredentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}