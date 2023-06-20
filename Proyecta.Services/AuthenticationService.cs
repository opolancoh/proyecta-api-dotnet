using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Proyecta.Core.Contracts.Services;
using Proyecta.Core.Entities;
using Proyecta.Core.Entities.DTOs;
using Proyecta.Core.Models;
using Proyecta.Core.Models.Auth;

namespace Proyecta.Services;

public sealed class AuthenticationService : IAuthenticationService
{
    private readonly ILogger<ApplicationUserService> _logger;
    private readonly IApplicationUserService _appUserService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    private ApplicationUser? _user;

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
        $"{nameof(Authenticate)}: Authentication failed. Wrong user name or password.";

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

    public async Task<ApplicationResult> Authenticate(LoginInputModel item)
    {
        _user = await _userManager.FindByNameAsync(item.UserName!);
        if (_user == null)
        {
            _logger.LogWarning(AuthenticationFailedMessage);
            return new ApplicationResult
            {
                Status = 400,
                Message = AuthenticationFailedMessage
            };
        }

        var result = (_user != null && await _userManager.CheckPasswordAsync(_user, item.Password!));
        if (!result)
        {
            _logger.LogWarning(AuthenticationFailedMessage);
            return new ApplicationResult
            {
                Status = 400,
                Message = AuthenticationFailedMessage
            };
        }

        var token = await CreateToken();

        return new ApplicationResult
        {
            Status = 200,
            D = new { Token = token }
        };
    }

    public async Task<string> CreateToken()
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims();
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    private SigningCredentials GetSigningCredentials()
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var secretKey = jwtSettings["SecretKey"];

        var key = Encoding.UTF8.GetBytes(secretKey!);
        var secret = new SymmetricSecurityKey(key);

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private async Task<List<Claim>> GetClaims()
    {
        var claims = new List<Claim>
        {
            // Add user data
            new("sub", _user!.Id),
            new("fullName", $"{_user.FirstName} {_user.LastName}"),
            new("userName", _user!.UserName!),
        };

        // Add user roles
        var roles = await _userManager.GetRolesAsync(_user);
        claims.AddRange(roles.Select(role => new Claim("roles", role)));

        var isAdmin = roles.Contains("Administrator");
        claims.Add(new Claim("isAdmin", isAdmin.ToString(), ClaimValueTypes.Boolean));

        return claims;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];
        var expires = jwtSettings["Expires"];

        var tokenOptions = new JwtSecurityToken
        (
            issuer,
            audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(expires)),
            signingCredentials: signingCredentials
        );

        return tokenOptions;
    }
}