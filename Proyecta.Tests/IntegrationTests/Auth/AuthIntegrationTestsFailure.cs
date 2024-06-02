using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Proyecta.Core.DTOs.ApiResponses;
using Proyecta.Core.DTOs.Auth;
using Proyecta.Tests.IntegrationTests.Fixtures;

namespace Proyecta.Tests.IntegrationTests.Auth;

public class AuthIntegrationTestsFailure : IClassFixture<AuthWebApplicationFactory>
{
    private const string BasePath = "/api/auth";
    private readonly AuthWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private static readonly JsonSerializerOptions JsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    public AuthIntegrationTestsFailure(AuthWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task AuthRegister_WhenUsernameExists_ReturnsBadRequest()
    {
        // Arrange
        var (user, _) = await _factory.CreateUserAsync();

        var newItem = new RegisterDto
        {
            FirstName = "John",
            LastName = "Doe",
            DisplayName = "John Doe",
            Username = user.UserName!,
            Password = "Pa$$w0rd"
        };

        // Act
        var response = await _client.PostAsJsonAsync($"{BasePath}/register", newItem);

        // Assert
        var responseContentString = await response.Content.ReadAsStringAsync();
        var responseContentObject =
            JsonSerializer.Deserialize<ApiBody>(responseContentString, JsonSerializerOptions);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(responseContentObject);
        Assert.False(string.IsNullOrEmpty(responseContentObject.Message));
        Assert.NotNull(responseContentObject.Errors);
    }

    [Fact]
    public async Task AuthLogin_WhenUsernameNotExists_ReturnsStatusUnauthorized()
    {
        // Arrange
        var (username, password) = ("no_existing_user", "Pa$$w0rd");
        var newItem = new { Username = username, Password = password };

        // Act
        var response = await _client.PostAsJsonAsync($"{BasePath}/login", newItem);

        // Assert
        var responseContentString = await response.Content.ReadAsStringAsync();
        var responseContentObject =
            JsonSerializer.Deserialize<ApiBody>(responseContentString, JsonSerializerOptions);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.NotNull(responseContentObject);
        Assert.False(string.IsNullOrEmpty(responseContentObject.Message));
        Assert.NotNull(responseContentObject.Errors);
    }

    [Fact]
    public async Task AuthLogout_WhenAccessTokenIsNotValid_ReturnsStatusBadRequest()
    {
        // Arrange
        var request = new TokenDto { AccessToken = "AccessToken", RefreshToken = "RefreshToken" };

        // Act
        var response = await _client.PostAsJsonAsync($"{BasePath}/logout", request);

        // Assert
        var responseContentString = await response.Content.ReadAsStringAsync();
        var responseContentObject =
            JsonSerializer.Deserialize<ApiBody>(responseContentString, JsonSerializerOptions);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(responseContentObject);
        Assert.False(string.IsNullOrEmpty(responseContentObject.Message));
        Assert.Null(responseContentObject.Errors);
    }

    [Fact]
    public async Task AuthRefreshToken_WhenAccessTokenIsNotValid_ReturnsStatusUnauthorized()
    {
        // Arrange
        var request = new TokenDto { AccessToken = "AccessToken", RefreshToken = "RefreshToken" };

        // Act
        var response = await _client.PostAsJsonAsync($"{BasePath}/refresh-token", request);

        // Assert
        var responseContentString = await response.Content.ReadAsStringAsync();
        var responseContentObject =
            JsonSerializer.Deserialize<ApiBody>(responseContentString, JsonSerializerOptions);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.NotNull(responseContentObject);
        Assert.False(string.IsNullOrEmpty(responseContentObject.Message));
        Assert.Null(responseContentObject.Errors);
    }

    [Fact]
    public async Task AuthRefreshToken_WhenRefreshTokenNotExists_ReturnsStatusUnauthorized()
    {
        // Arrange
        var (user, password) = await _factory.CreateUserAsync();
        var newUser = new { Username = user.UserName, Password = password };
        var loginResponse = await _client.PostAsJsonAsync($"{BasePath}/login", newUser);
        var loginResponseString = await loginResponse.Content.ReadAsStringAsync();
        var loginResponseContent =
            JsonSerializer.Deserialize<ApiBody<TokenDto>>(loginResponseString, JsonSerializerOptions);
        var request = new TokenDto
            { AccessToken = loginResponseContent!.Data!.AccessToken, RefreshToken = "RefreshToken" };

        // Act
        var response = await _client.PostAsJsonAsync($"{BasePath}/refresh-token", request);

        // Assert
        var responseContentString = await response.Content.ReadAsStringAsync();
        var responseContentObject =
            JsonSerializer.Deserialize<ApiBody<RefreshTokenResponse>>(responseContentString, JsonSerializerOptions);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.NotNull(responseContentObject);
        Assert.False(string.IsNullOrEmpty(responseContentObject.Message));
        Assert.Null(responseContentObject.Errors);
    }
}
