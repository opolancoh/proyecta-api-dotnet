using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Proyecta.Core.Contracts.Services;
using Proyecta.Core.Entities.Auth;
using Proyecta.Core.Entities.DTOs;
using Proyecta.Core.Models;
using Proyecta.Core.Models.Auth;
using Proyecta.Core.Services;

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

    public async Task<ApplicationResponse> Register(ApplicationUserRegisterDto item)
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

    public async Task<ApplicationResponse> Authenticate(LoginInputModel item)
    {
        _user = await _userManager.FindByNameAsync(item.UserName!);
        if (_user == null)
        {
            _logger.LogWarning(AuthenticationFailedMessage);
            return new ApplicationResponse
            {
                Status = 400,
                Message = AuthenticationFailedMessage
            };
        }

        var result = (_user != null && await _userManager.CheckPasswordAsync(_user, item.Password!));
        if (!result)
        {
            _logger.LogWarning(AuthenticationFailedMessage);
            return new ApplicationResponse
            {
                Status = 400,
                Message = AuthenticationFailedMessage
            };
        }

        var token = await CreateToken();

        return new ApplicationResponse
        {
            Status = 200,
            Data = new { Token = token }
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
        var claims = new List<Claim> { new Claim(ClaimTypes.Name, _user.UserName) };

        var roles = await _userManager.GetRolesAsync(_user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

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