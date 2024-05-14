using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Proyecta.Core.DTOs.ApiResponse;
using Proyecta.Core.DTOs.IdName;
using Proyecta.Tests.IntegrationTests.Fixtures;

namespace Proyecta.Tests.IntegrationTests.Risk;

public class RiskCategoryIntegrationTests : IClassFixture<CustomWebApplicationFactory>
{
    private const string BasePath = "/api/risk-categories";

    private readonly HttpClient _client;

    public RiskCategoryIntegrationTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();

        var adminToken = factory.GenerateAccessTokenForAdministrator();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);
    }

    [Fact]
    public async Task Add_WithValidData_ReturnsNewRecordId()
    {
        // Arrange
        var newItem = new IdNameAddOrUpdateDto { Name = "Category 1" };

        // Act
        var response = await _client.PostAsJsonAsync($"{BasePath}", newItem);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseContent = await response.Content.ReadFromJsonAsync<ApiResponse<ApiResponseGenericAdd<Guid>>>();

        Assert.NotNull(responseContent);
        Assert.True(responseContent.Success);
        Assert.Equal("201", responseContent.Code);
        // Data
        Assert.NotEqual(Guid.Empty, responseContent.Data.Id);
    }

    [Fact]
    public async Task GetById_WithValidData_ReturnsExistingRecord()
    {
        // Arrange
        var newItem = new IdNameAddOrUpdateDto { Name = "Category 1" };
        var newItemResponse = await _client.PostAsJsonAsync($"{BasePath}", newItem);
        var newItemResponseContent =
            await newItemResponse.Content.ReadFromJsonAsync<ApiResponse<ApiResponseGenericAdd<Guid>>>();
        var newItemId = newItemResponseContent!.Data.Id;

        // Act
        var response = await _client.GetAsync($"{BasePath}/{newItemId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseContent = await response.Content.ReadFromJsonAsync<ApiResponse<IdNameDetailDto<Guid>>>();

        Assert.NotNull(responseContent);
        Assert.True(responseContent.Success);
        Assert.Equal("200", responseContent.Code);
        // Data
        Assert.Equal(newItemId, responseContent.Data.Id);
        Assert.Equal(newItem.Name, responseContent.Data.Name);
        Assert.True(responseContent.Data.CreatedAt <= DateTime.UtcNow);
        Assert.True(responseContent.Data.UpdatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public async Task Update_WithValidData_ReturnsCode204()
    {
        // Arrange
        var newItem = new IdNameAddOrUpdateDto { Name = "Category 1" };
        var newItemResponse = await _client.PostAsJsonAsync($"{BasePath}", newItem);
        var newItemResponseContent =
            await newItemResponse.Content.ReadFromJsonAsync<ApiResponse<ApiResponseGenericAdd<Guid>>>();
        var newItemId = newItemResponseContent!.Data.Id;

        var itemToBeUpdated = new IdNameAddOrUpdateDto { Name = "Category 2" };

        // Act
        var response = await _client.PutAsJsonAsync($"{BasePath}/{newItemId}", itemToBeUpdated);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseContent = await response.Content.ReadFromJsonAsync<ApiResponse>();

        Assert.NotNull(responseContent);
        Assert.True(responseContent.Success);
        Assert.Equal("204", responseContent.Code);
    }

    [Fact]
    public async Task Remove_WithValidData_ReturnsCode204()
    {
        // Arrange
        var newItem = new IdNameAddOrUpdateDto { Name = "Category 1" };
        var newItemResponse = await _client.PostAsJsonAsync($"{BasePath}", newItem);
        var newItemResponseContent =
            await newItemResponse.Content.ReadFromJsonAsync<ApiResponse<ApiResponseGenericAdd<Guid>>>();
        var newItemId = newItemResponseContent!.Data.Id;

        // Act
        var response = await _client.DeleteAsync($"{BasePath}/{newItemId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseContent = await response.Content.ReadFromJsonAsync<ApiResponse>();

        Assert.NotNull(responseContent);
        Assert.True(responseContent.Success);
        Assert.Equal("204", responseContent.Code);
    }

    [Fact]
    public async Task AddRange_WithValidData_ReturnsCode204()
    {
        // Arrange
        var newItems = new List<IdNameAddOrUpdateDto>
        {
            new() { Name = "Item 1" },
            new() { Name = "Item 2" },
            new() { Name = "Item 3" },
        };

        // Act
        var response = await _client.PostAsJsonAsync($"{BasePath}/add-range", newItems);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseContent = await response.Content.ReadFromJsonAsync<ApiResponse<IdNameDetailDto<Guid>>>();

        Assert.NotNull(responseContent);
        Assert.True(responseContent.Success);
        Assert.Equal("204", responseContent.Code);
    }

    [Fact]
    public async Task GetAll_WithValidData_ReturnsCode204()
    {
        // Arrange
        var newItems = new List<IdNameAddOrUpdateDto>
        {
            new() { Name = "Item 1" },
            new() { Name = "Item 2" },
            new() { Name = "Item 3" },
        };
        await _client.PostAsJsonAsync($"{BasePath}/add-range", newItems);

        // Act
        var response = await _client.GetAsync($"{BasePath}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseContent =
            await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<IdNameListDto<Guid>>>>();

        Assert.NotNull(responseContent);
        Assert.True(responseContent.Success);
        Assert.Equal("200", responseContent.Code);
        // Data
        foreach (var item in responseContent.Data)
        {
            Assert.NotEqual(Guid.Empty, item.Id);
            Assert.Contains(newItems, x => x.Name == item.Name);
            Assert.True(item.CreatedAt <= DateTime.UtcNow);
            Assert.True(item.UpdatedAt <= DateTime.UtcNow);
        }
    }
}