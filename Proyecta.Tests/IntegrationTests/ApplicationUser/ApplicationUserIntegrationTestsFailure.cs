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
        var responseContentString = await response.Content.ReadAsStringAsync();
        var responseContentObject =
            JsonSerializer.Deserialize<ApiBody<ApiResponseGenericAdd<string>>>(responseContentString,
                JsonSerializerOptions);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(responseContentObject);
        Assert.False(string.IsNullOrEmpty(responseContentObject.Message));
        Assert.Null(responseContentObject.Data);
        Assert.NotNull(responseContentObject.Errors);
    }

    [Fact]
    public async Task GetById_WhenRecordNotExists_ReturnsStatusNotFound()
    {
        // Arrange
        var itemId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"{BasePath}/{itemId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var responseContentString = await response.Content.ReadAsStringAsync();
        var responseContentObject =
            JsonSerializer.Deserialize<ApiBody<ApplicationUserDetailDto>>(responseContentString, JsonSerializerOptions);

        Assert.NotNull(responseContentObject);
        Assert.False(string.IsNullOrEmpty(responseContentObject.Message));
        Assert.Null(responseContentObject.Data);
        Assert.Null(responseContentObject.Errors);
    }

    [Fact]
    public async Task Update_WhenRecordNotExists_ReturnsStatusNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var itemToBeUpdated = new ApplicationUserAddRequest
        {
            FirstName = "New FirstName", LastName = "New LastName", DisplayName = "New DisplayName",
            UserName = "NewUserName", Password = "NewPassword"
        };

        // Act
        var response = await _client.PutAsJsonAsync($"{BasePath}/{userId}", itemToBeUpdated);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var responseContentString = await response.Content.ReadAsStringAsync();
        var responseContentObject = JsonSerializer.Deserialize<ApiBody>(responseContentString, JsonSerializerOptions);

        Assert.NotNull(responseContentObject);
        Assert.False(string.IsNullOrEmpty(responseContentObject.Message));
        Assert.Null(responseContentObject.Errors);
    }

    [Fact]
    public async Task Remove_WhenRecordNotExists_ReturnsStatusNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var response = await _client.DeleteAsync($"{BasePath}/{userId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        var responseContentString = await response.Content.ReadAsStringAsync();
        var responseContentObject = JsonSerializer.Deserialize<ApiBody>(responseContentString, JsonSerializerOptions);

        Assert.NotNull(responseContentObject);
        Assert.False(string.IsNullOrEmpty(responseContentObject.Message));
        Assert.Null(responseContentObject.Errors);
    }

    [Fact]
    public async Task AddRange_WhenNotAllItemsAreAdded_ReturnsStatusMultiStatus()
    {
        // Arrange
        var (user, _) = await _factory.CreateUserAsync();

        var newItem1 = _factory.GetValidApplicationUserAddOrUpdateDto(null);
        newItem1.UserName = user.UserName;

        var newItem2 = _factory.GetValidApplicationUserAddOrUpdateDto(null);
        var newItem3 = _factory.GetValidApplicationUserAddOrUpdateDto(null);

        var newItems = new List<ApplicationUserAddRequest>
        {
            newItem1,
            newItem2,
            newItem3,
        };

        // Act
        var response = await _client.PostAsJsonAsync($"{BasePath}/add-range", newItems);

        // Assert
        Assert.Equal(HttpStatusCode.MultiStatus, response.StatusCode);

        var responseContentString = await response.Content.ReadAsStringAsync();
        var responseContentObject =
            JsonSerializer.Deserialize<ApiBody<IEnumerable<ApiResponseGenericAdd<string>>>>(responseContentString,
                JsonSerializerOptions);

        Assert.NotNull(responseContentObject);
        Assert.False(string.IsNullOrEmpty(responseContentObject.Message));
        Assert.NotNull(responseContentObject.Data);
        Assert.NotNull(responseContentObject.Errors);
    }
}
