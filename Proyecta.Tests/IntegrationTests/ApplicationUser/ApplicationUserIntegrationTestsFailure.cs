using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Proyecta.Core.DTOs.ApiResponse;
using Proyecta.Core.DTOs.Auth;
using Proyecta.Tests.IntegrationTests.Fixtures;

namespace Proyecta.Tests.IntegrationTests.ApplicationUser;

public class ApplicationUserIntegrationTestsFailure : IClassFixture<AuthWebApplicationFactory>
{
    private const string BasePath = "/api/users";
    private readonly HttpClient _client;
    private readonly AuthWebApplicationFactory _factory;
    private static readonly JsonSerializerOptions JsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    public ApplicationUserIntegrationTestsFailure(AuthWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", factory.AdministratorAccessToken);
    }

    [Fact]
    public async Task Add_WhenUsernameExists_ReturnsStatusBadRequest()
    {
        // Arrange
        var (user, _) = await _factory.CreateUserAsync();
        var newItem = _factory.GetValidApplicationUserAddOrUpdateDto(null);
        newItem.UserName = user.UserName;

        // Act
        var response = await _client.PostAsJsonAsync($"{BasePath}", newItem);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseString = await response.Content.ReadAsStringAsync();
        var responseContent =
            JsonSerializer.Deserialize<ApiResponse<ApiResponseGenericAdd<string>>>(responseString,
                JsonSerializerOptions);

        Assert.NotNull(responseContent);
        Assert.False(responseContent.Success);
        Assert.Equal("400", responseContent.Code);
        Assert.Null(responseContent.Data);
        Assert.NotNull(responseContent.Errors);
    }

    [Fact]
    public async Task GetById_WhenRecordNotExists_ReturnsStatusNotFound()
    {
        // Arrange
        var itemId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"{BasePath}/{itemId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseString = await response.Content.ReadAsStringAsync();
        var responseContent =
            JsonSerializer.Deserialize<ApiResponse<ApplicationUserDetailDto>>(responseString, JsonSerializerOptions);

        Assert.NotNull(responseContent);
        Assert.False(responseContent.Success);
        Assert.Equal("404", responseContent.Code);
        Assert.Null(responseContent.Data);
        Assert.Null(responseContent.Errors);
    }
    
    [Fact]
    public async Task Update_WhenRecordNotExists_ReturnsStatusNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var itemToBeUpdated = new ApplicationUserAddOrUpdateDto
        {
            FirstName = "New FirstName", LastName = "New LastName", DisplayName = "New DisplayName",
            UserName = "NewUserName", Password = "NewPassword"
        };

        // Act
        var response = await _client.PutAsJsonAsync($"{BasePath}/{userId}", itemToBeUpdated);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseString = await response.Content.ReadAsStringAsync();
        var responseContent = JsonSerializer.Deserialize<ApiResponse>(responseString, JsonSerializerOptions);

        Assert.NotNull(responseContent);
        Assert.False(responseContent.Success);
        Assert.Equal("404", responseContent.Code);
        Assert.Null(responseContent.Errors);
    }
    
    [Fact]
    public async Task Remove_WhenRecordNotExists_ReturnsStatusNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var response = await _client.DeleteAsync($"{BasePath}/{userId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseString = await response.Content.ReadAsStringAsync();
        var responseContent = JsonSerializer.Deserialize<ApiResponse>(responseString, JsonSerializerOptions);

        Assert.NotNull(responseContent);
        Assert.False(responseContent.Success);
        Assert.Equal("404", responseContent.Code);
        Assert.Null(responseContent.Errors);
    }
    
    [Fact]
    public async Task AddRange_WhenNotAllItemsAreAdded_ReturnsStatusAccepted()
    {
        // Arrange
        var (user, _) = await _factory.CreateUserAsync();
        
        var newItem1 = _factory.GetValidApplicationUserAddOrUpdateDto(null);
        newItem1.UserName = user.UserName;
        
        var newItem2 = _factory.GetValidApplicationUserAddOrUpdateDto(null);
        var newItem3 = _factory.GetValidApplicationUserAddOrUpdateDto(null);

        var newItems = new List<ApplicationUserAddOrUpdateDto>
        {
            newItem1,
            newItem2,
            newItem3,
        };

        // Act
        var response = await _client.PostAsJsonAsync($"{BasePath}/add-range", newItems);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseString = await response.Content.ReadAsStringAsync();
        var responseContent =
            JsonSerializer.Deserialize<ApiResponse<IEnumerable<ApiResponseGenericAdd<string>>>>(responseString,
                JsonSerializerOptions);

        Assert.NotNull(responseContent);
        Assert.False(responseContent.Success);
        Assert.Equal("202", responseContent.Code);
        Assert.NotNull(responseContent.Data);
        Assert.NotNull(responseContent.Errors);
    }
}