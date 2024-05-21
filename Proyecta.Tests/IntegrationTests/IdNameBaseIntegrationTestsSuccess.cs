using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Proyecta.Core.DTOs.ApiResponse;
using Proyecta.Core.DTOs.IdName;
using Proyecta.Tests.IntegrationTests.Fixtures;

namespace Proyecta.Tests.IntegrationTests;

public abstract class IdNameBaseIntegrationTestsSuccess : IClassFixture<CustomWebApplicationFactory>
{
    protected abstract string BasePath { get; }
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private static readonly JsonSerializerOptions JsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    protected IdNameBaseIntegrationTestsSuccess(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();

        var adminToken = factory.GenerateAccessTokenForAdministrator();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);
    }

    [Fact]
    public async Task Add_WithValidData_ReturnsNewRecordId()
    {
        // Arrange
        var newItem = _factory.GetValidIdNameAddOrUpdateDtoItem();

        // Act
        var response = await _client.PostAsJsonAsync($"{BasePath}", newItem);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseString = await response.Content.ReadAsStringAsync();
        var responseContent =
            JsonSerializer.Deserialize<ApiResponse<ApiResponseGenericAdd<Guid>>>(responseString, JsonSerializerOptions);

        Assert.NotNull(responseContent);
        Assert.True(responseContent.Success);
        Assert.Equal("201", responseContent.Code);
        // Data
        Assert.NotEqual(Guid.Empty, responseContent.Data!.Id);
    }

    [Fact]
    public async Task GetById_WithValidData_ReturnsExistingRecord()
    {
        // Arrange
        var newItem = _factory.GetValidIdNameAddOrUpdateDtoItem();
        var newItemResponse = await _client.PostAsJsonAsync($"{BasePath}", newItem);
        var newItemResponseContent =
            await newItemResponse.Content.ReadFromJsonAsync<ApiResponse<ApiResponseGenericAdd<Guid>>>();
        var newItemId = newItemResponseContent!.Data!.Id;

        // Act
        var response = await _client.GetAsync($"{BasePath}/{newItemId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseString = await response.Content.ReadAsStringAsync();
        var responseContent =
            JsonSerializer.Deserialize<ApiResponse<IdNameDetailDto<Guid>>>(responseString, JsonSerializerOptions);

        Assert.NotNull(responseContent);
        Assert.True(responseContent.Success);
        Assert.Equal("200", responseContent.Code);
        // Data
        Assert.Equal(newItemId, responseContent.Data!.Id);
        Assert.Equal(newItem.Name, responseContent.Data.Name);
        Assert.True(responseContent.Data.CreatedAt <= DateTime.UtcNow);
        Assert.True(responseContent.Data.UpdatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public async Task Update_WithValidData_ReturnsCode204()
    {
        // Arrange
        var newItem = _factory.GetValidIdNameAddOrUpdateDtoItem();
        var newItemResponse = await _client.PostAsJsonAsync($"{BasePath}", newItem);
        var newItemResponseContent =
            await newItemResponse.Content.ReadFromJsonAsync<ApiResponse<ApiResponseGenericAdd<Guid>>>();
        var newItemId = newItemResponseContent!.Data!.Id;

        var itemToBeUpdated = new IdNameAddOrUpdateDto { Name = _factory.GetValidEntityName() };

        // Act
        var response = await _client.PutAsJsonAsync($"{BasePath}/{newItemId}", itemToBeUpdated);

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
        var newItem = _factory.GetValidIdNameAddOrUpdateDtoItem();
        var newItemResponse = await _client.PostAsJsonAsync($"{BasePath}", newItem);
        var newItemResponseContent =
            await newItemResponse.Content.ReadFromJsonAsync<ApiResponse<ApiResponseGenericAdd<Guid>>>();
        var newItemId = newItemResponseContent!.Data!.Id;

        // Act
        var response = await _client.DeleteAsync($"{BasePath}/{newItemId}");

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
        var newItems = new List<IdNameAddOrUpdateDto>
        {
            new() { Name = _factory.GetValidEntityName() },
            new() { Name = _factory.GetValidEntityName() },
            new() { Name = _factory.GetValidEntityName() },
        };
        await _client.PostAsJsonAsync($"{BasePath}/add-range", newItems);

        // Act
        var response = await _client.GetAsync($"{BasePath}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseString = await response.Content.ReadAsStringAsync();
        var responseContent =
            JsonSerializer.Deserialize<ApiResponse<IEnumerable<IdNameListDto<Guid>>>>(responseString,
                JsonSerializerOptions);

        Assert.NotNull(responseContent);
        Assert.True(responseContent.Success);
        Assert.Equal("200", responseContent.Code);

        // Data
        Assert.NotNull(responseContent.Data);
        Assert.True(newItems.All(x => responseContent.Data.Any(y => y.Name == x.Name)));
    }

    [Fact]
    public async Task AddRange_WithValidData_ReturnsCode204()
    {
        // Arrange
        var newItems = new List<IdNameAddOrUpdateDto>
        {
            new() { Name = _factory.GetValidEntityName() },
            new() { Name = _factory.GetValidEntityName() },
            new() { Name = _factory.GetValidEntityName() },
        };

        // Act
        var response = await _client.PostAsJsonAsync($"{BasePath}/add-range", newItems);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseString = await response.Content.ReadAsStringAsync();
        var responseContent =
            JsonSerializer.Deserialize<ApiResponse<IdNameDetailDto<Guid>>>(responseString, JsonSerializerOptions);

        Assert.NotNull(responseContent);
        Assert.True(responseContent.Success);
        Assert.Equal("204", responseContent.Code);
    }
}