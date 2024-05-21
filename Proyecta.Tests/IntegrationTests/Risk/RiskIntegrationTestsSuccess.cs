using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Proyecta.Core.DTOs.ApiResponse;
using Proyecta.Core.DTOs.IdName;
using Proyecta.Core.DTOs.Risk;
using Proyecta.Tests.IntegrationTests.Fixtures;

namespace Proyecta.Tests.IntegrationTests.Risk;

public class RiskIntegrationTestsSuccess : IClassFixture<CustomWebApplicationFactory>
{
    private const string BasePath = "/api/risks";
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private static readonly JsonSerializerOptions JsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    public RiskIntegrationTestsSuccess(CustomWebApplicationFactory factory)
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
        var newItem = await _factory.AddRiskRecord();

        // Act
        var response = await _client.GetAsync($"{BasePath}/{newItem.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseString = await response.Content.ReadAsStringAsync();
        var responseContent =
            JsonSerializer.Deserialize<ApiResponse<RiskDetailDto>>(responseString, JsonSerializerOptions);

        Assert.NotNull(responseContent);
        Assert.True(responseContent.Success);
        Assert.Equal("200", responseContent.Code);
        // Data
        Assert.Equal(newItem.Id, responseContent.Data!.Id);
        Assert.Equal(newItem.Code, responseContent.Data.Code);
        Assert.Equal(newItem.Name, responseContent.Data.Name);
        Assert.Equal(newItem.Category.Id, responseContent.Data.Category.Id);
        Assert.Equal(newItem.Category.Name, responseContent.Data.Category.Name);
        Assert.Equal(newItem.Type, responseContent.Data.Type);
        Assert.Equal(newItem.Owner.Id, responseContent.Data.Owner.Id);
        Assert.Equal(newItem.Owner.Name, responseContent.Data.Owner.Name);
        Assert.Equal(newItem.Phase, responseContent.Data.Phase);
        Assert.Equal(newItem.Manageability, responseContent.Data.Manageability);
        Assert.Equal(newItem.Treatment.Id, responseContent.Data.Treatment.Id);
        Assert.Equal(newItem.Treatment.Name, responseContent.Data.Treatment.Name);
        Assert.Equal(newItem.DateFrom, responseContent.Data.DateFrom);
        Assert.Equal(newItem.DateTo, responseContent.Data.DateTo);
        Assert.Equal(newItem.State, responseContent.Data.State);
        Assert.Equal(newItem.CreatedAt, responseContent.Data.CreatedAt);
        Assert.Equal(newItem.CreatedBy.Id, responseContent.Data.CreatedBy.Id);
        Assert.Equal(newItem.UpdatedAt, responseContent.Data.UpdatedAt);
        Assert.Equal(newItem.UpdatedBy.Id, responseContent.Data.UpdatedBy.Id);
    }

    [Fact]
    public async Task Update_WithValidData_ReturnsCode204()
    {
        // Arrange
        var newItem = await _factory.AddRiskRecord();

        var itemToBeUpdated =
            _factory.GetValidRiskAddOrUpdateItem(newItem.Category.Id, newItem.Owner.Id, newItem.Treatment.Id);

        // Act
        var response = await _client.PutAsJsonAsync($"{BasePath}/{newItem.Id}", itemToBeUpdated);

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
        var newItem = await _factory.AddRiskRecord();

        // Act
        var response = await _client.DeleteAsync($"{BasePath}/{newItem.Id}");

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
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseString = await response.Content.ReadAsStringAsync();
        var responseContent =
            JsonSerializer.Deserialize<ApiResponse<IEnumerable<RiskListDto>>>(responseString,
                JsonSerializerOptions);

        Assert.NotNull(responseContent);
        Assert.True(responseContent.Success);
        Assert.Equal("200", responseContent.Code);

        // Data
        Assert.NotNull(responseContent.Data);
        Assert.True(newItems.All(x => responseContent.Data.Any(y => y.Id == x.Id && y.Name == x.Name)));
    }

    [Fact]
    public async Task AddRange_WithValidData_ReturnsCode204()
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
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseString = await response.Content.ReadAsStringAsync();
        var responseContent =
            JsonSerializer.Deserialize<ApiResponse<IdNameDetailDto<Guid>>>(responseString, JsonSerializerOptions);

        Assert.NotNull(responseContent);
        Assert.True(responseContent.Success);
        Assert.Equal("204", responseContent.Code);
    }
}