using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Proyecta.Core.DTOs.ApiResponse;
using Proyecta.Core.DTOs.IdName;
using Proyecta.Tests.IntegrationTests.Fixtures;

namespace Proyecta.Tests.IntegrationTests.IdName;

public abstract class IdNameBaseIntegrationTestsFailure : IClassFixture<ApiWebApplicationFactory>
{
    protected abstract string BasePath { get; }
    private readonly HttpClient _client;
    private static readonly JsonSerializerOptions JsonSerializerOptions = new() { PropertyNameCaseInsensitive = true };

    protected IdNameBaseIntegrationTestsFailure(ApiWebApplicationFactory factory)
    {
        _client = factory.CreateClient();

        var adminToken = factory.GenerateAccessTokenForAdministrator();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);
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
            JsonSerializer.Deserialize<ApiResponse<IdNameDetailDto<Guid>>>(responseString, JsonSerializerOptions);

        Assert.NotNull(responseContent);
        Assert.False(responseContent.Success);
        Assert.Equal("404", responseContent.Code);
        Assert.Null(responseContent.Data);
        Assert.Null(responseContent.Errors);
    }
}