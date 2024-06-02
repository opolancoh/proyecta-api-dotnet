using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Proyecta.Core.DTOs.ApiResponses;
using Proyecta.Core.DTOs.Auth;
using Proyecta.Tests.IntegrationTests.Fixtures;

namespace Proyecta.Tests.IntegrationTests.Auth;

public class AuthIntegrationTestsSuccess : IClassFixture<AuthWebApplicationFactory>
{
    private const string BasePath = "/api/auth";
    private readonly AuthWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private static readonly JsonSerializerOptions JsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    public AuthIntegrationTestsSuccess(AuthWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task AuthRegister_WithValidData_ReturnsTokens()
    {
        // Arrange
        var newItem = new RegisterDto
        {
            FirstName = "John",
            LastName = "Doe",
            DisplayName = "John Doe",
            Username = "john.doe",
            Password = "Pa$$w0rd"
        };

        // Act
        var response = await _client.PostAsJsonAsync($"{BasePath}/register", newItem);

        // Assert
        var responseContentString = await response.Content.ReadAsStringAsync();
        var responseContentObject =
            JsonSerializer.Deserialize<ApiBody<TokenDto>>(responseContentString, JsonSerializerOptions);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(responseContentObject);
        Assert.True(string.IsNullOrEmpty(responseContentObject.Message));
        Assert.Null(responseContentObject.Errors);
        Assert.False(string.IsNullOrEmpty(responseContentObject.Data!.AccessToken));
        Assert.False(string.IsNullOrEmpty(responseContentObject.Data!.RefreshToken));
    }

    [Fact]
    public async Task AuthLogin_WithValidData_ReturnsTokens()
    {
        // Arrange
        var (user, password) = await _factory.CreateUserAsync();
        var newItem = new { Username = user.UserName, Password = password };

        // Act
        var response = await _client.PostAsJsonAsync($"{BasePath}/login", newItem);

        // Assert
        var responseContentString = await response.Content.ReadAsStringAsync();
        var responseContentObject =
            JsonSerializer.Deserialize<ApiBody<TokenDto>>(responseContentString, JsonSerializerOptions);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(responseContentObject);
        Assert.True(string.IsNullOrEmpty(responseContentObject.Message));
        Assert.Null(responseContentObject.Errors);
        Assert.False(string.IsNullOrEmpty(responseContentObject.Data!.AccessToken));
        Assert.False(string.IsNullOrEmpty(responseContentObject.Data!.RefreshToken));
    }

    [Fact]
    public async Task AuthLogout_WithValidData_ReturnsStatusNoContent()
    {
        // Arrange
        var (user, password) = await _factory.CreateUserAsync();
        var newUser = new { Username = user.UserName, Password = password };
        var loginResponse = await _client.PostAsJsonAsync($"{BasePath}/login", newUser);
        var loginResponseString = await loginResponse.Content.ReadAsStringAsync();
        var loginResponseContent =
            JsonSerializer.Deserialize<ApiBody<TokenDto>>(loginResponseString, JsonSerializerOptions);

        // Act
        var response = await _client.PostAsJsonAsync($"{BasePath}/logout", loginResponseContent!.Data);

        // Assert
        var responseContentString = await response.Content.ReadAsStringAsync();
        var responseContentObject =
            JsonSerializer.Deserialize<ApiBody>(responseContentString, JsonSerializerOptions);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(responseContentObject);
        Assert.False(string.IsNullOrEmpty(responseContentObject.Message));
        Assert.Null(responseContentObject.Errors);
    }

    [Fact]
    public async Task AuthRefreshToken_WithValidData_ReturnsAccessToken()
    {
        // Arrange
        var (user, password) = await _factory.CreateUserAsync();
        var newUser = new { Username = user.UserName, Password = password };
        var loginResponse = await _client.PostAsJsonAsync($"{BasePath}/login", newUser);
        var loginResponseString = await loginResponse.Content.ReadAsStringAsync();
        var loginResponseContent =
            JsonSerializer.Deserialize<ApiBody<TokenDto>>(loginResponseString, JsonSerializerOptions);

        // Act
        var response = await _client.PostAsJsonAsync($"{BasePath}/refresh-token", loginResponseContent!.Data);

        // Assert
        var responseContentString = await response.Content.ReadAsStringAsync();
        var responseContentObject =
            JsonSerializer.Deserialize<ApiBody<RefreshTokenResponse>>(responseContentString, JsonSerializerOptions);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(responseContentObject);
        Assert.True(string.IsNullOrEmpty(responseContentObject.Message));
        Assert.Null(responseContentObject.Errors);
        Assert.False(string.IsNullOrEmpty(responseContentObject.Data!.AccessToken));
    }
}
