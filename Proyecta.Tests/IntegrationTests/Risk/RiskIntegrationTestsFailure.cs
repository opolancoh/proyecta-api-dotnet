using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Proyecta.Core.DTOs.ApiResponse;
using Proyecta.Core.DTOs.Risk;
using Proyecta.Tests.IntegrationTests.Fixtures;

namespace Proyecta.Tests.IntegrationTests.Risk;

public class RiskIntegrationTestsFailure : IClassFixture<ApiWebApplicationFactory>
{
    private const string BasePath = "/api/risks";
    private readonly ApiWebApplicationFactory _factory;
    private readonly HttpClient _client;
    private static readonly JsonSerializerOptions JsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    public RiskIntegrationTestsFailure(ApiWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();

        var adminToken = factory.GenerateAccessTokenForAdministrator();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);
    }
    
    [Fact]
    public async Task Add_WhenNameIsEmpty_ReturnsStatusBadRequest()
    {
        // Arrange
        var (riskCategoryId, riskOwnerId, riskTreatmentId) = await _factory.SetupRiskCreationTest();

        var newItem = _factory.GetValidRiskAddOrUpdateItem(riskCategoryId, riskOwnerId, riskTreatmentId);
        newItem.Name = "";

        // Act
        var response = await _client.PostAsJsonAsync($"{BasePath}", newItem);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseString = await response.Content.ReadAsStringAsync();
        var responseContent =
            JsonSerializer.Deserialize<ApiResponse<ApiResponseGenericAdd<Guid>>>(responseString, JsonSerializerOptions);

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
            JsonSerializer.Deserialize<ApiResponse<RiskDetailDto>>(responseString, JsonSerializerOptions);

        Assert.NotNull(responseContent);
        Assert.False(responseContent.Success);
        Assert.Equal("404", responseContent.Code);
        Assert.Null(responseContent.Data);
        Assert.Null(responseContent.Errors);
    }
}