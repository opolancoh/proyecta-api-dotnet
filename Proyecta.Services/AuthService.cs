using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Proyecta.Core.Contracts.Repositories;
using Proyecta.Core.Contracts.Services;
using Proyecta.Core.DTOs.Auth;
using Proyecta.Core.Entities.Auth;
using Proyecta.Core.Responses;
using Proyecta.Core.Utilities;

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

    public async Task<ApiResponse<ApiCreateResponse<string>>> Register(RegisterDto registerDto, string currentUserId)
    {
        var newUser = new ApplicationUserCreateOrUpdateDto
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            DisplayName = registerDto.DisplayName,
            UserName = registerDto.Username,
            Password = registerDto.Password
        };

        return await _appUserService.Create(newUser, currentUserId);
    }

    public async Task<ApiResponse<TokenDto>> Login(LoginDto loginDto)
    {
        var user = await _authRepository.GetUserForLogin(loginDto);
        if (user == null)
        {
            _logger.LogWarning(AuthenticationFailedMessage);
            return new ApiResponse<TokenDto>
            {
                Success = false,
                Code = "401",
                Message = AuthenticationFailedMessage
            };
        }

        // AccessToken
        var issuer = _configuration.GetSection("JwtSettings:Issuer").Value;
        var audience = _configuration.GetSection("JwtSettings:Audience").Value;
        var secret = _configuration.GetSection("JwtSettings:Secret").Value;
        var expiration =
            DateTime.UtcNow.AddMinutes(
                Convert.ToDouble(_configuration.GetSection("JwtSettings:AccessTokenExpirationInMinutes").Value));
        // Claims generation
        var userRoles = await _userManager.GetRolesAsync(user);
        var claimsInput = new JwtAccessTokenClaimsInputDto
        {
            userId = user.Id,
            userName = user.UserName ?? "",
            userDisplayName = user.DisplayName ?? "",
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
                    Convert.ToDouble(_configuration.GetSection("JwtSettings:RefreshTokenExpirationInMinutes").Value))
        });
        if (!addRefreshTokenResult)
        {
            _logger.LogError("Unable to store the refresh token");
            return new ApiResponse<TokenDto>
            {
                Success = false,
                Code = "401",
                Message = AuthenticationFailedMessage
            };
        }

        return new ApiResponse<TokenDto>
        {
            Success = true,
            Code = "200",
            Data = new TokenDto { AccessToken = accessToken, RefreshToken = refreshToken }
        };
    }

    public async Task<ApiResponse> Logout(TokenDto tokenDto)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");

        // AccessToken
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];
        var secret = jwtSettings["Secret"];
        // Access token validation
        var accessToken = AuthHelper.ValidateJwtToken(tokenDto.AccessToken, issuer, audience, secret, false);
        if (accessToken == null)
        {
            return new ApiResponse
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
            return new ApiResponse
            {
                Success = false,
                Code = "400",
                Message = "Failed to logout."
            };
        }

        return new ApiResponse
        {
            Success = true,
            Code = "204",
            Message = "Logged out successfully."
        };
    }

    public async Task<ApiResponse<RefreshTokenResponse>> RefreshToken(TokenDto tokenDto)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");

        // Access token validation
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];
        var secret = jwtSettings["Secret"];
        var accessToken = AuthHelper.ValidateJwtToken(tokenDto.AccessToken, issuer, audience, secret, false);
        if (accessToken == null)
        {
            return new ApiResponse<RefreshTokenResponse>
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
            return new ApiResponse<RefreshTokenResponse>
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

        return new ApiResponse<RefreshTokenResponse>
        {
            Success = true,
            Code = "200",
            Data = new RefreshTokenResponse { AccessToken = newAccessToken }
        };
    }
}