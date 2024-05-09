using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using Proyecta.Core.Contracts.Repositories;
using Proyecta.Core.DTOs.Auth;
using Proyecta.Core.Entities.Auth;
using Proyecta.Services;

namespace Proyecta.Tests.UnitTests.Auth;

public class AuthServiceSuccessfulLoginTests : IClassFixture<AuthServiceFixture>
{
    private readonly AuthService _authService;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly Mock<IAuthRepository> _mockAuthRepository;
    private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;

    public AuthServiceSuccessfulLoginTests(AuthServiceFixture fixture)
    {
        _authService = fixture.AuthService;
        _mockConfiguration = fixture.MockConfiguration;
        _mockAuthRepository = fixture.MockAuthRepository;
        _mockUserManager = fixture.MockUserManager;
    }

    [Fact]
    public async Task Login_SuccessfulLoginRoleAdministrator_Returns200AndTokens()
    {
        // Arrange
        const int accessTokenExpirationInMinutes = 15;
        var now = DateTime.UtcNow;
        var loginDto = new LoginDto { Username = "test", Password = "password" };
        var applicationUser = new ApplicationUser
        {
            Id = "d225332b-959c-41b9-9656-3f6e217535fb",
            UserName = "test",
            FirstName = "John",
            LastName = "Doe"
        };
        var applicationUserRoles = new List<string> { "Administrator" };

        _mockConfiguration
            .Setup(a => a.GetSection(It.Is<string>(s => s == "JwtSettings:AccessTokenExpirationInMinutes")))
            .Returns(new ConfigurationSectionMock(accessTokenExpirationInMinutes.ToString()));
        _mockAuthRepository
            .Setup(x => x.GetUserForLogin(It.IsAny<LoginDto>()))
            .ReturnsAsync(applicationUser);
        _mockUserManager
            .Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(applicationUserRoles);
        _mockAuthRepository
            .Setup(x => x.AddRefreshToken(It.IsAny<RefreshToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _authService.Login(loginDto);

        // Assert
        Assert.Equal(200, int.Parse(result.Code));
        Assert.NotNull(result.Data);
        Assert.Null(result.Message);
        Assert.Null(result.Errors);
        // Data
        var tokenData = (TokenDto)result.Data;

        // RefreshToken
        Assert.False(string.IsNullOrEmpty(tokenData.RefreshToken));

        // AccessToken
        var accessToken = tokenData.AccessToken;
        Assert.False(string.IsNullOrEmpty(accessToken));
        Assert.Equal(2, accessToken.Count(c => c == '.'));
        // Decode the token and validate data
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(accessToken);
        // User ID
        var subjectClaim = jwtToken.Claims.First(c => c.Type == "sub");
        Assert.NotNull(subjectClaim);
        Assert.Equal(applicationUser.Id, subjectClaim.Value);
        // Name
        var nameClaim = jwtToken.Claims.First(c => c.Type == "name");
        Assert.NotNull(nameClaim);
        Assert.Equal($"{applicationUser.FirstName} {applicationUser.LastName}", nameClaim.Value);
        // Username
        var usernameClaim = jwtToken.Claims.First(c => c.Type == "username");
        Assert.NotNull(usernameClaim);
        Assert.Equal(applicationUser.UserName, usernameClaim.Value);
        // Roles
        var rolesClaim = jwtToken.Claims.Where(c => c.Type == "roles").Select(c => c.Value).ToList();
        Assert.NotNull(rolesClaim);
        Assert.Equal(applicationUserRoles.Count, rolesClaim.Count);
        Assert.All(applicationUserRoles, role => Assert.Contains(role, rolesClaim));
        // IsAdmin
        var isAdminClaim = jwtToken.Claims.First(c => c.Type == "isAdmin");
        Assert.NotNull(isAdminClaim);
        Assert.Equal(true, bool.Parse(isAdminClaim.Value));
        // Expiration: Convert the claim value (which is a Unix timestamp) to a DateTime
        var expirationClaim = jwtToken.Claims.First(c => c.Type == "exp");
        Assert.NotNull(expirationClaim);
        var unixTimestamp = long.Parse(expirationClaim.Value);
        var expirationDateTime = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).DateTime;
        var timeDifference = expirationDateTime - now;
        var minutesDifference = Math.Round(timeDifference.TotalMinutes);
        Assert.Equal(accessTokenExpirationInMinutes, minutesDifference);
    }

    [Fact]
    public async Task Login_SuccessfulLoginRoleNoAdmin_Returns200AndTokens()
    {
        // Arrange
        const int accessTokenExpirationInMinutes = 15;
        var now = DateTime.UtcNow;
        var loginDto = new LoginDto { Username = "test", Password = "password" };
        var applicationUser = new ApplicationUser
        {
            Id = "d225332b-959c-41b9-9656-3f6e217535fb",
            UserName = "test",
            FirstName = "John",
            LastName = "Doe"
        };
        var applicationUserRoles = new List<string> { "User" };

        _mockConfiguration
            .Setup(a => a.GetSection(It.Is<string>(s => s == "JwtSettings:AccessTokenExpirationInMinutes")))
            .Returns(new ConfigurationSectionMock(accessTokenExpirationInMinutes.ToString()));
        _mockAuthRepository
            .Setup(x => x.GetUserForLogin(It.IsAny<LoginDto>()))
            .ReturnsAsync(applicationUser);
        _mockUserManager
            .Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(applicationUserRoles);
        _mockAuthRepository
            .Setup(x => x.AddRefreshToken(It.IsAny<RefreshToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _authService.Login(loginDto);

        // Assert
        Assert.Equal(200, int.Parse(result.Code));
        Assert.NotNull(result.Data);
        Assert.Null(result.Message);
        Assert.Null(result.Errors);
        // Data
        var tokenData = (TokenDto)result.Data;

        // RefreshToken
        Assert.False(string.IsNullOrEmpty(tokenData.RefreshToken));

        // AccessToken
        var accessToken = tokenData.AccessToken;
        Assert.False(string.IsNullOrEmpty(accessToken));
        Assert.Equal(2, accessToken.Count(c => c == '.'));
        // Decode the token and validate data
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(accessToken);
        // User ID
        var subjectClaim = jwtToken.Claims.First(c => c.Type == "sub");
        Assert.NotNull(subjectClaim);
        Assert.Equal(applicationUser.Id, subjectClaim.Value);
        // Name
        var nameClaim = jwtToken.Claims.First(c => c.Type == "name");
        Assert.NotNull(nameClaim);
        Assert.Equal($"{applicationUser.FirstName} {applicationUser.LastName}", nameClaim.Value);
        // Username
        var usernameClaim = jwtToken.Claims.First(c => c.Type == "username");
        Assert.NotNull(usernameClaim);
        Assert.Equal(applicationUser.UserName, usernameClaim.Value);
        // Roles
        var rolesClaim = jwtToken.Claims.Where(c => c.Type == "roles").Select(c => c.Value).ToList();
        Assert.NotNull(rolesClaim);
        Assert.Equal(applicationUserRoles.Count, rolesClaim.Count);
        Assert.All(applicationUserRoles, role => Assert.Contains(role, rolesClaim));
        // IsAdmin
        var isAdminClaim = jwtToken.Claims.First(c => c.Type == "isAdmin");
        Assert.NotNull(isAdminClaim);
        Assert.Equal(false, bool.Parse(isAdminClaim.Value));
        // Expiration: Convert the claim value (which is a Unix timestamp) to a DateTime
        var expirationClaim = jwtToken.Claims.First(c => c.Type == "exp");
        Assert.NotNull(expirationClaim);
        var unixTimestamp = long.Parse(expirationClaim.Value);
        var expirationDateTime = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).DateTime;
        var timeDifference = expirationDateTime - now;
        var minutesDifference = Math.Round(timeDifference.TotalMinutes);
        Assert.Equal(accessTokenExpirationInMinutes, minutesDifference);
    }

    [Fact]
    public async Task Login_SuccessfulLoginNoRoles_Returns200AndTokens()
    {
        // Arrange
        const int accessTokenExpirationInMinutes = 15;
        var now = DateTime.UtcNow;
        var loginDto = new LoginDto { Username = "test", Password = "password" };
        var applicationUser = new ApplicationUser
        {
            Id = "d225332b-959c-41b9-9656-3f6e217535fb",
            UserName = "test",
            FirstName = "John",
            LastName = "Doe"
        };
        var applicationUserRoles = new List<string>();

        _mockConfiguration
            .Setup(a => a.GetSection(It.Is<string>(s => s == "JwtSettings:AccessTokenExpirationInMinutes")))
            .Returns(new ConfigurationSectionMock(accessTokenExpirationInMinutes.ToString()));
        _mockAuthRepository
            .Setup(x => x.GetUserForLogin(It.IsAny<LoginDto>()))
            .ReturnsAsync(applicationUser);
        _mockUserManager
            .Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(applicationUserRoles);
        _mockAuthRepository
            .Setup(x => x.AddRefreshToken(It.IsAny<RefreshToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _authService.Login(loginDto);

        // Assert
        Assert.Equal(200, int.Parse(result.Code));
        Assert.NotNull(result.Data);
        Assert.Null(result.Message);
        Assert.Null(result.Errors);
        // Data
        var tokenData = (TokenDto)result.Data;

        // RefreshToken
        Assert.False(string.IsNullOrEmpty(tokenData.RefreshToken));

        // AccessToken
        var accessToken = tokenData.AccessToken;
        Assert.False(string.IsNullOrEmpty(accessToken));
        Assert.Equal(2, accessToken.Count(c => c == '.'));
        // Decode the token and validate data
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(accessToken);
        // User ID
        var subjectClaim = jwtToken.Claims.First(c => c.Type == "sub");
        Assert.NotNull(subjectClaim);
        Assert.Equal(applicationUser.Id, subjectClaim.Value);
        // Name
        var nameClaim = jwtToken.Claims.First(c => c.Type == "name");
        Assert.NotNull(nameClaim);
        Assert.Equal($"{applicationUser.FirstName} {applicationUser.LastName}", nameClaim.Value);
        // Username
        var usernameClaim = jwtToken.Claims.First(c => c.Type == "username");
        Assert.NotNull(usernameClaim);
        Assert.Equal(applicationUser.UserName, usernameClaim.Value);
        // Roles
        var rolesClaim = jwtToken.Claims.Where(c => c.Type == "roles").Select(c => c.Value).ToList();
        Assert.NotNull(rolesClaim);
        Assert.Equal(applicationUserRoles.Count, rolesClaim.Count);
        Assert.All(applicationUserRoles, role => Assert.Contains(role, rolesClaim));
        // IsAdmin
        var isAdminClaim = jwtToken.Claims.First(c => c.Type == "isAdmin");
        Assert.NotNull(isAdminClaim);
        Assert.Equal(false, bool.Parse(isAdminClaim.Value));
        // Expiration: Convert the claim value (which is a Unix timestamp) to a DateTime
        var expirationClaim = jwtToken.Claims.First(c => c.Type == "exp");
        Assert.NotNull(expirationClaim);
        var unixTimestamp = long.Parse(expirationClaim.Value);
        var expirationDateTime = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp).DateTime;
        var timeDifference = expirationDateTime - now;
        var minutesDifference = Math.Round(timeDifference.TotalMinutes);
        Assert.Equal(accessTokenExpirationInMinutes, minutesDifference);
    }
}