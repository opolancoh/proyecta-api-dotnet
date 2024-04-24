using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Proyecta.Core.Contracts.Repositories;
using Proyecta.Core.Contracts.Services;
using Proyecta.Core.DTOs.Auth;
using Proyecta.Core.Entities.Auth;
using Proyecta.Core.Results;
using Proyecta.Core.Utils;

namespace Proyecta.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IAuthRepository _authRepository;
    private readonly IApplicationUserService _appUserService;

    private string AuthenticationFailedMessage =>
        $"{nameof(Login)}: Authentication failed. Wrong user name or password.";

    public AuthService(
        IConfiguration configuration,
        ILogger<AuthService> logger,
        UserManager<ApplicationUser> userManager,
        IAuthRepository authRepository,
        IApplicationUserService appUserService
    )
    {
        _configuration = configuration;
        _logger = logger;
        _userManager = userManager;
        _authRepository = authRepository;
        _appUserService = appUserService;
    }

    public async Task<ApplicationResult> Register(RegisterDto registerDto)
    {
        var newUser = new ApplicationUserCreateOrUpdateDto
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            UserName = registerDto.UserName,
            Password = registerDto.Password
        };

        return await _appUserService.Create(newUser);
    }

    public async Task<ApplicationResult> Login(LoginDto loginDto)
    {
        var user = await _authRepository.GetUserForLogin(loginDto);
        if (user == null)
        {
            _logger.LogWarning(AuthenticationFailedMessage);
            return new ApplicationResult
            {
                Success = false,
                Code = "401",
                Message = AuthenticationFailedMessage
            };
        }

        // AccessToken
        var issuer = _configuration.GetSection("JwtConfig:Issuer").Value;
        var audience = _configuration.GetSection("JwtConfig:Audience").Value;
        var secret = _configuration.GetSection("JwtConfig:Secret").Value;
        var expiration =
            DateTime.UtcNow.AddMinutes(
                Convert.ToDouble(_configuration.GetSection("JwtConfig:AccessTokenExpirationInMinutes").Value));
        // Claims generation
        var userRoles = await _userManager.GetRolesAsync(user);
        var claimsInput = new JwtAccessTokenClaimsInputDto
        {
            userId = user.Id,
            userFirstName = user.FirstName ?? "",
            userLastName = user.LastName ?? "",
            userName = user.UserName ?? "",
            userRoles = userRoles.ToList()
        };
        var claims = AuthHelper.GetClaims(claimsInput);
        var accessToken = AuthHelper.GenerateAccessToken(issuer, audience, secret, claims, expiration);

        // Refresh Token
        var refreshToken = AuthHelper.GenerateRefreshToken();
        var addRefreshTokenResult = await _authRepository.AddRefreshToken(new RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            ExpiryDate =
                DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(_configuration.GetSection("JwtConfig:RefreshTokenExpirationInMinutes").Value))
        });
        if (!addRefreshTokenResult)
        {
            _logger.LogError("Unable to store the refresh token");
            return new ApplicationResult
            {
                Success = false,
                Code = "401",
                Message = AuthenticationFailedMessage
            };
        }

        return new ApplicationResult
        {
            Success = true,
            Code = "200",
            Data = new TokenDto { AccessToken = accessToken, RefreshToken = refreshToken }
        };
    }

    public async Task<ApplicationResult> Logout(TokenDto tokenDto)
    {
        var jwtSettings = _configuration.GetSection("JwtConfig");

        // AccessToken
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];
        var secret = jwtSettings["Secret"];
        // Access token validation
        var accessToken = AuthHelper.ValidateJwtToken(tokenDto.AccessToken, issuer, audience, secret, false);
        if (accessToken == null)
        {
            return new ApplicationResult
            {
                Success = false,
                Code = "400",
                Message = "Failed to logout."
            };
        }

        // Remove the refresh token from the database
        var removeRefreshTokenResult =
            await _authRepository.RemoveRefreshToken(accessToken.Subject, tokenDto.RefreshToken);
        if (!removeRefreshTokenResult)
        {
            _logger.LogError("Unable to remove the refresh token");
            return new ApplicationResult
            {
                Success = false,
                Code = "400",
                Message = "Failed to logout."
            };
        }

        return new ApplicationResult
        {
            Success = true,
            Code = "204",
            Message = "Logged out successfully."
        };
    }

    public async Task<ApplicationResult> RefreshToken(TokenDto tokenDto)
    {
        var jwtSettings = _configuration.GetSection("JwtConfig");

        // Access token validation
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];
        var secret = jwtSettings["Secret"];
        var accessToken = AuthHelper.ValidateJwtToken(tokenDto.AccessToken, issuer, audience, secret, false);
        if (accessToken == null)
        {
            return new ApplicationResult
            {
                Success = false,
                Code = "401",
                Message = "Invalid access token."
            };
        }

        // Refresh token validation
        var refreshTokenFromDb = await _authRepository.GetRefreshToken(accessToken.Subject, tokenDto.RefreshToken);
        if (refreshTokenFromDb == null || refreshTokenFromDb.ExpiryDate < DateTime.UtcNow)
        {
            return new ApplicationResult
            {
                Success = false,
                Code = "401",
                Message = "Refresh Token is not valid."
            };
        }

        // New access token generation
        var expirationInMinutes = Convert.ToDouble(jwtSettings["AccessTokenExpirationInMinutes"]);
        var expiration = DateTime.UtcNow.AddMinutes(expirationInMinutes);
        var newAccessToken = AuthHelper.UpdateTokenExpiration(accessToken, expiration, secret);

        return new ApplicationResult
        {
            Success = true,
            Code = "200",
            Data = new { AccessToken = newAccessToken }
        };
    }
}