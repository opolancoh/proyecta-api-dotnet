using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Proyecta.Core.DTOs.ApiResponse;
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
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseString = await response.Content.ReadAsStringAsync();
        var responseContent =
            JsonSerializer.Deserialize<ApiResponse<TokenDto>>(responseString, JsonSerializerOptions);

        Assert.NotNull(responseContent);
        Assert.True(responseContent.Success);
        Assert.Equal("200", responseContent.Code);
        // Data
        Assert.False(string.IsNullOrEmpty(responseContent.Data!.AccessToken));
        Assert.False(string.IsNullOrEmpty(responseContent.Data!.RefreshToken));
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
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseString = await response.Content.ReadAsStringAsync();
        var responseContent =
            JsonSerializer.Deserialize<ApiResponse<TokenDto>>(responseString, JsonSerializerOptions);

        Assert.NotNull(responseContent);
        Assert.True(responseContent.Success);
        Assert.Equal("200", responseContent.Code);
        // Data
        Assert.False(string.IsNullOrEmpty(responseContent.Data!.AccessToken));
        Assert.False(string.IsNullOrEmpty(responseContent.Data!.RefreshToken));
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
            JsonSerializer.Deserialize<ApiResponse<TokenDto>>(loginResponseString, JsonSerializerOptions);

        // Act
        var response = await _client.PostAsJsonAsync($"{BasePath}/logout", loginResponseContent!.Data);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseString = await response.Content.ReadAsStringAsync();
        var responseContent =
            JsonSerializer.Deserialize<ApiResponse>(responseString, JsonSerializerOptions);

        Assert.NotNull(responseContent);
        Assert.True(responseContent.Success);
        Assert.Equal("204", responseContent.Code);
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
            JsonSerializer.Deserialize<ApiResponse<TokenDto>>(loginResponseString, JsonSerializerOptions);

        // Act
        var response = await _client.PostAsJsonAsync($"{BasePath}/refresh-token", loginResponseContent!.Data);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseString = await response.Content.ReadAsStringAsync();
        var responseContent =
            JsonSerializer.Deserialize<ApiResponse<RefreshTokenResponse>>(responseString, JsonSerializerOptions);

        Assert.NotNull(responseContent);
        Assert.True(responseContent.Success);
        Assert.Equal("200", responseContent.Code);
        // Data
        Assert.False(string.IsNullOrEmpty(responseContent.Data!.AccessToken));
    }
}