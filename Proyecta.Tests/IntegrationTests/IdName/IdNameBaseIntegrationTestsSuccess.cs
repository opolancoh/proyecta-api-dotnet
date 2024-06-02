using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Proyecta.Core.DTOs.ApiResponses;
using Proyecta.Core.DTOs.IdName;
using Proyecta.Tests.IntegrationTests.Fixtures;

namespace Proyecta.Tests.IntegrationTests.IdName;

public abstract class IdNameBaseIntegrationTestsSuccess : IClassFixture<ApiWebApplicationFactory>
{
    protected abstract string BasePath { get; }
    private readonly ApiWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private static readonly JsonSerializerOptions JsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    protected IdNameBaseIntegrationTestsSuccess(ApiWebApplicationFactory factory)
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
        var responseContentString = await response.Content.ReadAsStringAsync();
        var responseContentObject =
            JsonSerializer.Deserialize<ApiBody<ApiGenericAddResponse<Guid>>>(responseContentString, JsonSerializerOptions);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(responseContentObject);
        Assert.True(string.IsNullOrEmpty(responseContentObject.Message));
        Assert.Null(responseContentObject.Errors);
        Assert.NotEqual(Guid.Empty, responseContentObject.Data!.Id);
    }

    [Fact]
    public async Task GetById_WithValidData_ReturnsExistingRecord()
    {
        // Arrange
        var newItem = _factory.GetValidIdNameAddOrUpdateDtoItem();
        var newItemResponse = await _client.PostAsJsonAsync($"{BasePath}", newItem);
        var newItemResponseContent =
            await newItemResponse.Content.ReadFromJsonAsync<ApiBody<ApiGenericAddResponse<Guid>>>();
        var newItemId = newItemResponseContent!.Data!.Id;

        // Act
        var response = await _client.GetAsync($"{BasePath}/{newItemId}");

        // Assert
        var responseContentString = await response.Content.ReadAsStringAsync();
        var responseContentObject =
            JsonSerializer.Deserialize<ApiBody<IdNameDetailDto<Guid>>>(responseContentString, JsonSerializerOptions);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(responseContentObject);
        Assert.True(string.IsNullOrEmpty(responseContentObject.Message));
        Assert.Null(responseContentObject.Errors);
        Assert.Equal(newItemId, responseContentObject.Data!.Id);
        Assert.Equal(newItem.Name, responseContentObject.Data.Name);
        Assert.True(responseContentObject.Data.CreatedAt <= DateTime.UtcNow);
        Assert.True(responseContentObject.Data.UpdatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public async Task Update_WithValidData_ReturnsStatusNoContent()
    {
        // Arrange
        var newItem = _factory.GetValidIdNameAddOrUpdateDtoItem();
        var newItemResponse = await _client.PostAsJsonAsync($"{BasePath}", newItem);
        var newItemResponseContent =
            await newItemResponse.Content.ReadFromJsonAsync<ApiBody<ApiGenericAddResponse<Guid>>>();
        var newItemId = newItemResponseContent!.Data!.Id;

        var itemToBeUpdated = new IdNameAddOrUpdateDto { Name = _factory.GetValidEntityName() };

        // Act
        var response = await _client.PutAsJsonAsync($"{BasePath}/{newItemId}", itemToBeUpdated);

        // Assert


        var responseContentString = await response.Content.ReadAsStringAsync();
        var responseContentObject =  JsonSerializer.Deserialize<ApiBody>(responseContentString, JsonSerializerOptions);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.NotNull(responseContentObject);
        Assert.False(string.IsNullOrEmpty(responseContentObject.Message));
        Assert.Null(responseContentObject.Errors);
    }

    [Fact]
    public async Task Remove_WithValidData_ReturnsStatusNoContent()
    {
        // Arrange
        var newItem = _factory.GetValidIdNameAddOrUpdateDtoItem();
        var newItemResponse = await _client.PostAsJsonAsync($"{BasePath}", newItem);
        var newItemResponseContent =
            await newItemResponse.Content.ReadFromJsonAsync<ApiBody<ApiGenericAddResponse<Guid>>>();
        var newItemId = newItemResponseContent!.Data!.Id;

        // Act
        var response = await _client.DeleteAsync($"{BasePath}/{newItemId}");

        // Assert
        var responseContentString = await response.Content.ReadAsStringAsync();
        var responseContentObject =  JsonSerializer.Deserialize<ApiBody>(responseContentString, JsonSerializerOptions);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.NotNull(responseContentObject);
        Assert.False(string.IsNullOrEmpty(responseContentObject.Message));
        Assert.Null(responseContentObject.Errors);
    }

    [Fact]
    public async Task GetAll_WithValidData_ReturnsStatusOk()
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
        var responseContentString = await response.Content.ReadAsStringAsync();
        var responseContentObject =
            JsonSerializer.Deserialize<ApiBody<IEnumerable<IdNameListDto<Guid>>>>(responseContentString,
                JsonSerializerOptions);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(responseContentObject);
        Assert.True(string.IsNullOrEmpty(responseContentObject.Message));
        Assert.Null(responseContentObject.Errors);
        Assert.NotNull(responseContentObject.Data);
        Assert.True(newItems.All(x => responseContentObject.Data.Any(y => y.Name == x.Name)));
    }

    [Fact]
    public async Task AddRange_WithValidData_ReturnsStatusNoContent()
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
        var responseContentString = await response.Content.ReadAsStringAsync();
        var responseContentObject =
            JsonSerializer.Deserialize<ApiBody>(responseContentString, JsonSerializerOptions);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.NotNull(responseContentObject);
        Assert.False(string.IsNullOrEmpty(responseContentObject.Message));
        Assert.Null(responseContentObject.Errors);
    }
}
