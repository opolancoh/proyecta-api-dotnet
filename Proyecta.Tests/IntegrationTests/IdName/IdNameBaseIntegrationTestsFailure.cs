using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Proyecta.Core.DTOs.ApiResponses;
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
        var responseContentString = await response.Content.ReadAsStringAsync();
        var responseContentObject =
            JsonSerializer.Deserialize<ApiBody>(responseContentString, JsonSerializerOptions);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.NotNull(responseContentObject);
        Assert.False(string.IsNullOrEmpty(responseContentObject.Message));
        Assert.Null(responseContentObject.Errors);
    }
}
