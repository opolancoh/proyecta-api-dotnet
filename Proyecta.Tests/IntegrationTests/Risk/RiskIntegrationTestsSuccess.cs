using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Proyecta.Core.DTOs.ApiResponses;
using Proyecta.Core.DTOs.IdName;
using Proyecta.Core.DTOs.Risk;
using Proyecta.Tests.IntegrationTests.Fixtures;

namespace Proyecta.Tests.IntegrationTests.Risk;

public class RiskIntegrationTestsSuccess : IClassFixture<ApiWebApplicationFactory>
{
    private const string BasePath = "/api/risks";
    private readonly ApiWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private static readonly JsonSerializerOptions JsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    public RiskIntegrationTestsSuccess(ApiWebApplicationFactory factory)
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
        var (riskCategoryId, riskOwnerId, riskTreatmentId) = await _factory.SetupRiskCreationTest();

        var newItem = _factory.GetValidRiskAddOrUpdateItem(riskCategoryId, riskOwnerId, riskTreatmentId);

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
        var newItem = await _factory.AddRiskRecord();

        // Act
        var response = await _client.GetAsync($"{BasePath}/{newItem.Id}");

        // Assert
        var responseContentString = await response.Content.ReadAsStringAsync();
        var responseContentObject =
            JsonSerializer.Deserialize<ApiBody<RiskDetailDto>>(responseContentString, JsonSerializerOptions);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(responseContentObject);
        Assert.True(string.IsNullOrEmpty(responseContentObject.Message));
        Assert.Null(responseContentObject.Errors);
        Assert.NotNull(responseContentObject.Data);
        Assert.Equal(newItem.Id, responseContentObject.Data!.Id);
        Assert.Equal(newItem.Code, responseContentObject.Data.Code);
        Assert.Equal(newItem.Name, responseContentObject.Data.Name);
        Assert.Equal(newItem.Category.Id, responseContentObject.Data.Category.Id);
        Assert.Equal(newItem.Category.Name, responseContentObject.Data.Category.Name);
        Assert.Equal(newItem.Type, responseContentObject.Data.Type);
        Assert.Equal(newItem.Owner.Id, responseContentObject.Data.Owner.Id);
        Assert.Equal(newItem.Owner.Name, responseContentObject.Data.Owner.Name);
        Assert.Equal(newItem.Phase, responseContentObject.Data.Phase);
        Assert.Equal(newItem.Manageability, responseContentObject.Data.Manageability);
        Assert.Equal(newItem.Treatment.Id, responseContentObject.Data.Treatment.Id);
        Assert.Equal(newItem.Treatment.Name, responseContentObject.Data.Treatment.Name);
        Assert.Equal(newItem.DateFrom, responseContentObject.Data.DateFrom);
        Assert.Equal(newItem.DateTo, responseContentObject.Data.DateTo);
        Assert.Equal(newItem.State, responseContentObject.Data.State);
        Assert.Equal(newItem.CreatedAt, responseContentObject.Data.CreatedAt);
        Assert.Equal(newItem.CreatedBy.Id, responseContentObject.Data.CreatedBy.Id);
        Assert.Equal(newItem.UpdatedAt, responseContentObject.Data.UpdatedAt);
        Assert.Equal(newItem.UpdatedBy.Id, responseContentObject.Data.UpdatedBy.Id);
    }

    [Fact]
    public async Task Update_WithValidData_ReturnsStatusNoContent()
    {
        // Arrange
        var newItem = await _factory.AddRiskRecord();

        var itemToBeUpdated =
            _factory.GetValidRiskAddOrUpdateItem(newItem.Category.Id, newItem.Owner.Id, newItem.Treatment.Id);

        // Act
        var response = await _client.PutAsJsonAsync($"{BasePath}/{newItem.Id}", itemToBeUpdated);

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
        var newItem = await _factory.AddRiskRecord();

        // Act
        var response = await _client.DeleteAsync($"{BasePath}/{newItem.Id}");

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
        var newItem1 = await _factory.AddRiskRecord();
        var newItem2 = await _factory.AddRiskRecord();
        var newItem3 = await _factory.AddRiskRecord();

        var newItems = new List<IdNameDto<Guid>>
        {
            new() { Id = newItem1.Id, Name = newItem1.Name },
            new() { Id = newItem2.Id, Name = newItem2.Name },
            new() { Id = newItem3.Id, Name = newItem3.Name },
        };

        // Act
        var response = await _client.GetAsync($"{BasePath}");

        // Assert
        var responseContentString = await response.Content.ReadAsStringAsync();
        var responseContentObject =
            JsonSerializer.Deserialize<ApiBody<IEnumerable<RiskListDto>>>(responseContentString,
                JsonSerializerOptions);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(responseContentObject);
        Assert.True(string.IsNullOrEmpty(responseContentObject.Message));
        Assert.Null(responseContentObject.Errors);
        Assert.NotNull(responseContentObject.Data);
        Assert.True(newItems.All(x => responseContentObject.Data.Any(y => y.Id == x.Id && y.Name == x.Name)));
    }

    [Fact]
    public async Task AddRange_WithValidData_ReturnsStatusNoContent()
    {
        // Arrange
        var newItem1 = await _factory.AddRiskRecord();
        var newItem2 = await _factory.AddRiskRecord();
        var newItem3 = await _factory.AddRiskRecord();

        var newItems = new List<RiskAddOrUpdateDto>
        {
            _factory.GetValidRiskAddOrUpdateItem(newItem1.Category.Id, newItem1.Owner.Id, newItem1.Treatment.Id),
            _factory.GetValidRiskAddOrUpdateItem(newItem2.Category.Id, newItem2.Owner.Id, newItem2.Treatment.Id),
            _factory.GetValidRiskAddOrUpdateItem(newItem3.Category.Id, newItem3.Owner.Id, newItem3.Treatment.Id),
        };

        // Act
        var response = await _client.PostAsJsonAsync($"{BasePath}/add-range", newItems);

        // Assert
        var responseContentString = await response.Content.ReadAsStringAsync();
        var responseContentObject =
            JsonSerializer.Deserialize<ApiBody<IdNameDetailDto<Guid>>>(responseContentString, JsonSerializerOptions);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.NotNull(responseContentObject);
        Assert.False(string.IsNullOrEmpty(responseContentObject.Message));
        Assert.Null(responseContentObject.Errors);
    }
}
