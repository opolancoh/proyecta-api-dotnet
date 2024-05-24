using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Proyecta.Core.Contracts.Repositories;
using Proyecta.Core.Contracts.Services;
using Proyecta.Core.DTOs.ApiResponse;
using Proyecta.Core.DTOs.Auth;
using Proyecta.Core.Entities;
using Proyecta.Core.Entities.Auth;
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
        $"{nameof(Login)}: Authentication failed.";

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

    public async Task<ApiResponse<TokenDto>> Register(RegisterDto registerDto)
    {
        var newUser = new ApplicationUserAddOrUpdateDto
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            DisplayName = registerDto.DisplayName,
            UserName = registerDto.Username,
            Password = registerDto.Password
        };

        var userCreationResult = await _appUserService.Add(newUser, null!);
        if (userCreationResult.Success)
        {
            return await Login(new LoginDto { Username = newUser.UserName, Password = newUser.Password });
        }

        return new ApiResponse<TokenDto>
        {
            Success = false,
            Code = userCreationResult.Code,
            Message = "There was a problem creating the account.",
            Errors = userCreationResult.Errors
        };
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
                Code = ApiResponseCode.Unauthorized,
                Message = AuthenticationFailedMessage,
                Errors = new Dictionary<string, List<string>>
                {
                    { "_", new List<string>() { "Wrong user name or password." } }
                }
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
            UserId = user.Id,
            UserName = user.UserName ?? "",
            UserDisplayName = user.DisplayName ?? "",
            UserRoles = userRoles.ToList()
        };
        var claims = AuthHelper.GetClaims(claimsInput);
        var accessToken = AuthHelper.GenerateAccessToken(issuer, audience, secret, claims, expiration);

        // Refresh Token
        var refreshToken = AuthHelper.GenerateRefreshToken();
        await _authRepository.AddRefreshToken(new RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            ExpiryDate =
                DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(_configuration.GetSection("JwtSettings:RefreshTokenExpirationInMinutes").Value))
        });

        return new ApiResponse<TokenDto>
        {
            Success = true,
            Code = ApiResponseCode.Ok,
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
                Code = ApiResponseCode.BadRequest,
                Message = "Failed to logout."
            };
        }

        // Remove the refresh token from the database
        await _authRepository.RemoveRefreshToken(accessToken.Subject, tokenDto.RefreshToken);

        return new ApiResponse
        {
            Success = true,
            Code = ApiResponseCode.NoContent,
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
                Code = ApiResponseCode.Unauthorized,
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
                Code = ApiResponseCode.Unauthorized,
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
            Code = ApiResponseCode.Ok,
            Data = new RefreshTokenResponse { AccessToken = newAccessToken }
        };
    }
}