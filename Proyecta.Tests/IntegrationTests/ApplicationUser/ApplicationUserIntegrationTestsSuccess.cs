using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Proyecta.Core.DTOs.ApiResponse;
using Proyecta.Core.DTOs.Auth;
using Proyecta.Core.DTOs.IdName;
using Proyecta.Tests.IntegrationTests.Fixtures;

namespace Proyecta.Tests.IntegrationTests.ApplicationUser;

public class ApplicationUserIntegrationTestsSuccess : IClassFixture<ApplicationUserWebApplicationFactory>
{
    private const string BasePath = "/api/users";
    private readonly ApplicationUserWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private static readonly JsonSerializerOptions JsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    public ApplicationUserIntegrationTestsSuccess(ApplicationUserWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", factory.AdministratorAccessToken);
    }

    [Fact]
    public async Task Add_WithValidDataAndRoles_ReturnsNewRecordId()
    {
        // Arrange
        var newItem = _factory.GetValidApplicationUserAddOrUpdateDto(new List<string> { "Administrator" });

        // Act
        var response = await _client.PostAsJsonAsync($"{BasePath}", newItem);

        // Assert
        var responseContentString = await response.Content.ReadAsStringAsync();
        var responseContentObject =
            JsonSerializer.Deserialize<ApiBody<ApiResponseGenericAdd<string>>>(responseContentString,
                JsonSerializerOptions);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(responseContentObject);
        Assert.False(string.IsNullOrEmpty(responseContentObject.Message));
        Assert.Null(responseContentObject.Errors);
        Assert.NotNull(responseContentObject.Data);
        Assert.NotEqual(string.Empty, responseContentObject.Data.Id);
    }

    [Fact]
    public async Task Add_WithValidDataAndNoRoles_ReturnsNewRecordId()
    {
        // Arrange
        var newItem = _factory.GetValidApplicationUserAddOrUpdateDto(null);

        // Act
        var response = await _client.PostAsJsonAsync($"{BasePath}", newItem);

        // Assert
        var responseContentString = await response.Content.ReadAsStringAsync();
        var responseContentObject =
            JsonSerializer.Deserialize<ApiBody<ApiResponseGenericAdd<string>>>(responseContentString,
                JsonSerializerOptions);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(responseContentObject);
        Assert.False(string.IsNullOrEmpty(responseContentObject.Message));
        Assert.Null(responseContentObject.Errors);
        Assert.NotNull(responseContentObject.Data);
        Assert.NotEqual(string.Empty, responseContentObject.Data.Id);
    }

    [Fact]
    public async Task GetById_WithValidData_ReturnsExistingRecord()
    {
        // Arrange
        var (user, _) = await _factory.CreateUserAsync();

        // Act
        var response = await _client.GetAsync($"{BasePath}/{user.Id}");

        // Assert
        var responseContentString = await response.Content.ReadAsStringAsync();
        var responseContentObject =
            JsonSerializer.Deserialize<ApiBody<ApplicationUserDetailDto>>(responseContentString, JsonSerializerOptions);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(responseContentObject);
        Assert.True(string.IsNullOrEmpty(responseContentObject.Message));
        Assert.Null(responseContentObject.Errors);
        Assert.NotNull(responseContentObject.Data);
        Assert.Equal(user.Id, responseContentObject.Data.Id);
        Assert.Equal(user.FirstName, responseContentObject.Data.FirstName);
        Assert.Equal(user.LastName, responseContentObject.Data.LastName);
        Assert.Equal(user.DisplayName, responseContentObject.Data.DisplayName);
        Assert.Equal(user.UserName, responseContentObject.Data.UserName);
        Assert.Equal(user.CreatedAt, responseContentObject.Data.CreatedAt);
        Assert.Equal(user.CreatedById, responseContentObject.Data.CreatedBy!.Id);
        Assert.Equal(user.DisplayName, responseContentObject.Data.CreatedBy!.Name);
        Assert.Equal(user.UpdatedAt, responseContentObject.Data.UpdatedAt);
        Assert.Equal(user.UpdatedById, responseContentObject.Data.UpdatedBy!.Id);
        Assert.Equal(user.DisplayName, responseContentObject.Data.UpdatedBy!.Name);
    }

    [Fact]
    public async Task Update_WithValidDataAndRoles_ReturnsStatusNoContent()
    {
        // Arrange
        var (user, _) = await _factory.CreateUserAsync();

        var itemToBeUpdated = new ApplicationUserAddRequest
        {
            FirstName = "New FirstName", LastName = "New LastName", DisplayName = "New DisplayName",
            UserName = Guid.NewGuid().ToString(), Password = "NewPassword", Roles = new List<string> { "Administrator" }
        };

        // Act
        var response = await _client.PutAsJsonAsync($"{BasePath}/{user.Id}", itemToBeUpdated);

        // Assert
        var responseContentString = await response.Content.ReadAsStringAsync();
        var responseContentObject =
            JsonSerializer.Deserialize<ApiBody>(responseContentString, JsonSerializerOptions);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.NotNull(responseContentObject);
        Assert.False(string.IsNullOrEmpty(responseContentObject.Message));
        Assert.Null(responseContentObject.Errors);
    }

    [Fact]
    public async Task Update_WithValidDataAndNoRoles_ReturnsStatusNoContent()
    {
        // Arrange
        var (user, _) = await _factory.CreateUserAsync();

        var itemToBeUpdated = new ApplicationUserAddRequest
        {
            FirstName = "New FirstName", LastName = "New LastName", DisplayName = "New DisplayName",
            UserName = Guid.NewGuid().ToString(), Password = "NewPassword"
        };

        // Act
        var response = await _client.PutAsJsonAsync($"{BasePath}/{user.Id}", itemToBeUpdated);

        // Assert
        var responseContentString = await response.Content.ReadAsStringAsync();
        var responseContentObject =
            JsonSerializer.Deserialize<ApiBody>(responseContentString, JsonSerializerOptions);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.NotNull(responseContentObject);
        Assert.False(string.IsNullOrEmpty(responseContentObject.Message));
        Assert.Null(responseContentObject.Errors);
    }

    [Fact]
    public async Task Remove_WithValidData_ReturnsStatusNoContent()
    {
        // Arrange
        var (user, _) = await _factory.CreateUserAsync();

        // Act
        var response = await _client.DeleteAsync($"{BasePath}/{user.Id}");

        // Assert
        var responseContentString = await response.Content.ReadAsStringAsync();
        var responseContentObject =
            JsonSerializer.Deserialize<ApiBody>(responseContentString, JsonSerializerOptions);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.NotNull(responseContentObject);
        Assert.False(string.IsNullOrEmpty(responseContentObject.Message));
        Assert.Null(responseContentObject.Errors);
    }

    [Fact]
    public async Task GetAll_WithValidData_ReturnsStatusOk()
    {
        // Arrange
        var (user1, _) = await _factory.CreateUserAsync();
        var (user2, _) = await _factory.CreateUserAsync();
        var (user3, _) = await _factory.CreateUserAsync();

        var newItems = new List<IdNameDto<string>>
        {
            new() { Id = user1.Id, Name = user1.UserName! },
            new() { Id = user2.Id, Name = user2.UserName! },
            new() { Id = user3.Id, Name = user3.UserName! },
        };

        // Act
        var response = await _client.GetAsync($"{BasePath}");

        // Assert
        var responseContentString = await response.Content.ReadAsStringAsync();
        var responseContentObject =
            JsonSerializer.Deserialize<ApiBody<IEnumerable<ApplicationUserListDto>>>(responseContentString,
                JsonSerializerOptions);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(responseContentObject);
        Assert.True(string.IsNullOrEmpty(responseContentObject.Message));
        Assert.Null(responseContentObject.Errors);
        Assert.NotNull(responseContentObject.Data);
        Assert.True(newItems.All(x => responseContentObject.Data.Any(y => y.Id == x.Id)));
    }

    [Fact]
    public async Task AddRange_WithValidData_ReturnsStatusNoContent()
    {
        // Arrange
        var newItem1 = _factory.GetValidApplicationUserAddOrUpdateDto(null);
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
        var responseContentString = await response.Content.ReadAsStringAsync();
        var responseContentObject =
            JsonSerializer.Deserialize<ApiBody<IEnumerable<ApiResponseGenericAdd<string>>>>(responseContentString,
                JsonSerializerOptions);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.NotNull(responseContentObject);
        Assert.False(string.IsNullOrEmpty(responseContentObject.Message));
        Assert.Null(responseContentObject.Errors);
        Assert.NotNull(responseContentObject.Data);
    }
}
