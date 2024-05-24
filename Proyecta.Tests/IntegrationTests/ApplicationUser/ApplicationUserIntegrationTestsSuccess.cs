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
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseString = await response.Content.ReadAsStringAsync();
        var responseContent =
            JsonSerializer.Deserialize<ApiResponse<ApiResponseGenericAdd<string>>>(responseString,
                JsonSerializerOptions);

        Assert.NotNull(responseContent);
        Assert.True(responseContent.Success);
        Assert.Equal("201", responseContent.Code);
        // Data
        Assert.NotEqual(string.Empty, responseContent.Data!.Id);
    }

    [Fact]
    public async Task Add_WithValidDataAndNoRoles_ReturnsNewRecordId()
    {
        // Arrange
        var newItem = _factory.GetValidApplicationUserAddOrUpdateDto(null);

        // Act
        var response = await _client.PostAsJsonAsync($"{BasePath}", newItem);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseString = await response.Content.ReadAsStringAsync();
        var responseContent =
            JsonSerializer.Deserialize<ApiResponse<ApiResponseGenericAdd<string>>>(responseString,
                JsonSerializerOptions);

        Assert.NotNull(responseContent);
        Assert.True(responseContent.Success);
        Assert.Equal("201", responseContent.Code);
        // Data
        Assert.NotEqual(string.Empty, responseContent.Data!.Id);
    }

    [Fact]
    public async Task GetById_WithValidData_ReturnsExistingRecord()
    {
        // Arrange
        var (user, _) = await _factory.CreateUserAsync();

        // Act
        var response = await _client.GetAsync($"{BasePath}/{user.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseString = await response.Content.ReadAsStringAsync();
        var responseContent =
            JsonSerializer.Deserialize<ApiResponse<ApplicationUserDetailDto>>(responseString, JsonSerializerOptions);

        Assert.NotNull(responseContent);
        Assert.True(responseContent.Success);
        Assert.Equal("200", responseContent.Code);
        // Data
        Assert.Equal(user.Id, responseContent.Data!.Id);
        Assert.Equal(user.FirstName, responseContent.Data!.FirstName);
        Assert.Equal(user.LastName, responseContent.Data!.LastName);
        Assert.Equal(user.DisplayName, responseContent.Data!.DisplayName);
        Assert.Equal(user.UserName, responseContent.Data!.UserName);
        Assert.Equal(user.CreatedAt, responseContent.Data!.CreatedAt);
        Assert.Equal(user.CreatedById, responseContent.Data!.CreatedBy!.Id);
        Assert.Equal(user.DisplayName, responseContent.Data!.CreatedBy!.Name);
        Assert.Equal(user.UpdatedAt, responseContent.Data!.UpdatedAt);
        Assert.Equal(user.UpdatedById, responseContent.Data!.UpdatedBy!.Id);
        Assert.Equal(user.DisplayName, responseContent.Data!.UpdatedBy!.Name);
    }

    [Fact]
    public async Task Update_WithValidDataAndRoles_ReturnsCode204()
    {
        // Arrange
        var (user, _) = await _factory.CreateUserAsync();

        var itemToBeUpdated = new ApplicationUserAddOrUpdateDto
        {
            FirstName = "New FirstName", LastName = "New LastName", DisplayName = "New DisplayName",
            UserName = Guid.NewGuid().ToString(), Password = "NewPassword", Roles = new List<string> { "Administrator" }
        };

        // Act
        var response = await _client.PutAsJsonAsync($"{BasePath}/{user.Id}", itemToBeUpdated);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseString = await response.Content.ReadAsStringAsync();
        var responseContent = JsonSerializer.Deserialize<ApiResponse>(responseString, JsonSerializerOptions);

        Assert.NotNull(responseContent);
        Assert.True(responseContent.Success);
        Assert.Equal("204", responseContent.Code);
    }

    [Fact]
    public async Task Update_WithValidDataAndNoRoles_ReturnsCode204()
    {
        // Arrange
        var (user, _) = await _factory.CreateUserAsync();

        var itemToBeUpdated = new ApplicationUserAddOrUpdateDto
        {
            FirstName = "New FirstName", LastName = "New LastName", DisplayName = "New DisplayName",
            UserName = Guid.NewGuid().ToString(), Password = "NewPassword"
        };

        // Act
        var response = await _client.PutAsJsonAsync($"{BasePath}/{user.Id}", itemToBeUpdated);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseString = await response.Content.ReadAsStringAsync();
        var responseContent = JsonSerializer.Deserialize<ApiResponse>(responseString, JsonSerializerOptions);

        Assert.NotNull(responseContent);
        Assert.True(responseContent.Success);
        Assert.Equal("204", responseContent.Code);
    }

    [Fact]
    public async Task Remove_WithValidData_ReturnsCode204()
    {
        // Arrange
        var (user, _) = await _factory.CreateUserAsync();

        // Act
        var response = await _client.DeleteAsync($"{BasePath}/{user.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseString = await response.Content.ReadAsStringAsync();
        var responseContent = JsonSerializer.Deserialize<ApiResponse>(responseString, JsonSerializerOptions);

        Assert.NotNull(responseContent);
        Assert.True(responseContent.Success);
        Assert.Equal("204", responseContent.Code);
    }

    [Fact]
    public async Task GetAll_WithValidData_ReturnsCode200()
    {
        // Arrange
        var (user1, _) = await _factory.CreateUserAsync();
        var (user2, _) = await _factory.CreateUserAsync();
        var (user3, _) = await _factory.CreateUserAsync();

        var newItems = new List<IdNameDto<string>>
        {
            new() { Id = user1.Id, Name = user1.UserName },
            new() { Id = user2.Id, Name = user2.UserName },
            new() { Id = user3.Id, Name = user3.UserName },
        };

        // Act
        var response = await _client.GetAsync($"{BasePath}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseString = await response.Content.ReadAsStringAsync();
        var responseContent =
            JsonSerializer.Deserialize<ApiResponse<IEnumerable<ApplicationUserListDto>>>(responseString,
                JsonSerializerOptions);

        Assert.NotNull(responseContent);
        Assert.True(responseContent.Success);
        Assert.Equal("200", responseContent.Code);

        // Data
        Assert.NotNull(responseContent.Data);
        Assert.True(newItems.All(x => responseContent.Data.Any(y => y.Id == x.Id)));
    }

    [Fact]
    public async Task AddRange_WithValidData_ReturnsCode204()
    {
        // Arrange
        var newItem1 = _factory.GetValidApplicationUserAddOrUpdateDto(null);
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
        Assert.True(responseContent.Success);
        Assert.Equal("204", responseContent.Code);
    }
}