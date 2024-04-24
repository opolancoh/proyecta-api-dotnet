using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Moq;
using Proyecta.Core.DTOs;
using Proyecta.Core.Entities;
using Proyecta.Core.Results;
using Proyecta.Tests.IntegrationTests.Fixtures;


namespace Proyecta.Tests.IntegrationTests.Risk;

public class RiskCategoryIntegrationTests : IClassFixture<IntegrationTestFixture>
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _serializerOptions;
    private const string BasePath = "/api/riskcategories";


    public RiskCategoryIntegrationTests(IntegrationTestFixture fixture)
    {
        _client = fixture.Client;
        _serializerOptions = fixture.SerializerOptions;
        fixture.InitializeDatabase();
    }

    [Fact]
    public async Task CreateItem_Succeed_ReturnsGenericEntityCreationResult()
    {
        var item = new RiskCategoryCreateOrUpdateDto { Name = "Category 1" };

        var response = await _client.PostAsJsonAsync(BasePath, item);
        var result = await response.Content.ReadFromJsonAsync<ApplicationResult>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(result);
        Assert.Equal((int)HttpStatusCode.Created, int.Parse(result.Code));
        Assert.Equal("Risk Category created successfully.", result.Message);
        // Data
        var dataString = JsonSerializer.Serialize(result.Data, _serializerOptions);
        var dataObject = JsonSerializer.Deserialize<GenericEntityCreationResult>(dataString, _serializerOptions);
        Assert.IsType<Guid>(dataObject!.Id);
    }

    [Fact]
    public async Task AddRange_Succeed_ReturnsNoContent()
    {
        var items = new List<RiskCategoryCreateOrUpdateDto>
        {
            new() { Name = "Category 1" },
            new() { Name = "Category 2" },
            new() { Name = "Category 3" }
        };

        var response = await _client.PostAsJsonAsync($"{BasePath}/add-range", items);
        var result = await response.Content.ReadFromJsonAsync<ApplicationResult>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(result);
        Assert.Equal((int)HttpStatusCode.NoContent, int.Parse(result.Code));
        Assert.Equal("Items added successfully.", result.Message);
        // Data
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task GetAll_Succeed_ReturnsAllItems()
    {
        var items = new List<RiskCategoryCreateOrUpdateDto>
        {
            new() { Name = "Category 1" },
            new() { Name = "Category 2" },
            new() { Name = "Category 3" }
        };
        await _client.PostAsJsonAsync($"{BasePath}/add-range", items);

        var response = await _client.GetAsync(BasePath);
        var result = await response.Content.ReadFromJsonAsync<ApplicationResult>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(result);
        Assert.Equal((int)HttpStatusCode.OK, int.Parse(result.Code));
        Assert.Null(result.Message);
        // Data
        var dataString = JsonSerializer.Serialize(result.Data, _serializerOptions);
        var dataObject = JsonSerializer.Deserialize<List<RiskCategory>>(dataString, _serializerOptions);
        Assert.Equal(items.Count, dataObject!.Count);
        Assert.All(dataObject, item =>
        {
            Assert.NotEqual(Guid.Empty, item.Id);
            Assert.Contains(items, x => x.Name == item.Name);
        });
    }

    [Fact]
    public async Task GetAll_Succeed_ReturnsEmptyList()
    {
        var response = await _client.GetAsync(BasePath);
        var result = await response.Content.ReadFromJsonAsync<ApplicationResult>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(result);
        Assert.Equal((int)HttpStatusCode.OK, int.Parse(result.Code));
        Assert.Null(result.Message);
        // Data
        var dataString = JsonSerializer.Serialize(result.Data, _serializerOptions);
        var dataObject = JsonSerializer.Deserialize<List<RiskCategory>>(dataString, _serializerOptions);
        Assert.Empty(dataObject!);
    }

    [Fact]
    public async Task GetById_Succeed_ReturnsOneItem()
    {
        const string itemName = "Category 1";
        var item = await CreateOneItem(itemName);

        var response = await _client.GetAsync($"{BasePath}/{item!.Id}");
        var result = await response.Content.ReadFromJsonAsync<ApplicationResult>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(result);
        Assert.Equal((int)HttpStatusCode.OK, int.Parse(result.Code));
        // Data
        var dataString = JsonSerializer.Serialize(result.Data, _serializerOptions);
        var dataObject = JsonSerializer.Deserialize<RiskCategory>(dataString, _serializerOptions);
        Assert.Equal(item.Id, dataObject!.Id);
        Assert.Equal(itemName, dataObject!.Name);
    }

    [Fact]
    public async Task GetById_NotSucceed_ReturnsNotFound()
    {
        var id = Guid.NewGuid().ToString();

        var response = await _client.GetAsync($"{BasePath}/{id}");
        var result = await response.Content.ReadFromJsonAsync<ApplicationResult>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(result);
        Assert.Equal((int)HttpStatusCode.NotFound, int.Parse(result.Code));
        Assert.Equal($"The item with id '{id}' was not found or you don't have permission to access it.",
            result.Message);
    }

    [Fact]
    public async Task UpdateItem_Succeed_ReturnsNoContent()
    {
        const string itemName = "Category 1";
        var item = await CreateOneItem(itemName);

        var itemToUpdate = new RiskCategoryCreateOrUpdateDto { Name = "Category 2" };

        var response = await _client.PutAsJsonAsync($"{BasePath}/{item!.Id}", itemToUpdate);
        var result = await response.Content.ReadFromJsonAsync<ApplicationResult>();

        var updatedItemResponse = await _client.GetAsync($"{BasePath}/{item!.Id}");
        var updatedItemResult = await updatedItemResponse.Content.ReadFromJsonAsync<ApplicationResult>();
        var updatedItemDataString = JsonSerializer.Serialize(updatedItemResult!.Data, _serializerOptions);
        var updatedItemDataObject = JsonSerializer.Deserialize<RiskCategory>(updatedItemDataString, _serializerOptions);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(result);
        Assert.Equal((int)HttpStatusCode.NoContent, int.Parse(result.Code));
        Assert.Equal("Item updated successfully.", result.Message);
        // Data
        var dataString = JsonSerializer.Serialize(updatedItemResult.Data, _serializerOptions);
        var dataObject = JsonSerializer.Deserialize<RiskCategory>(dataString, _serializerOptions);
        Assert.Equal(updatedItemDataObject!.Id, dataObject!.Id);
        Assert.Equal(updatedItemDataObject.Name, dataObject!.Name);
    }

    [Fact]
    public async Task UpdateItem_NotSucceed_ReturnsBadRequest()
    {
        const string itemName = "Category 1";
        var item = await CreateOneItem(itemName);

        var itemToUpdate = new RiskCategoryCreateOrUpdateDto { Name = "Category 2*" };

        var response = await _client.PutAsJsonAsync($"{BasePath}/{item!.Id}", itemToUpdate);
        var result = await response.Content.ReadFromJsonAsync<ApplicationResult>();

        var updatedItemResponse = await _client.GetAsync($"{BasePath}/{item!.Id}");
        var updatedItemResult = await updatedItemResponse.Content.ReadFromJsonAsync<ApplicationResult>();
        var updatedItemDataString = JsonSerializer.Serialize(updatedItemResult!.Data, _serializerOptions);
        var updatedItemDataObject = JsonSerializer.Deserialize<RiskCategory>(updatedItemDataString, _serializerOptions);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(result);
        Assert.Equal((int)HttpStatusCode.NoContent, int.Parse(result.Code));
        Assert.Equal("Item updated successfully.", result.Message);
        // Data
        var dataString = JsonSerializer.Serialize(updatedItemResult.Data, _serializerOptions);
        var dataObject = JsonSerializer.Deserialize<RiskCategory>(dataString, _serializerOptions);
        Assert.Equal(updatedItemDataObject!.Id, dataObject!.Id);
        Assert.Equal(updatedItemDataObject.Name, dataObject!.Name);
    }

    private async Task<GenericEntityCreationResult> CreateOneItem(string name)
    {
        var item = new RiskCategoryCreateOrUpdateDto { Name = name };
        var itemResponse = await _client.PostAsJsonAsync(BasePath, item);
        var itemResult = await itemResponse.Content.ReadFromJsonAsync<ApplicationResult>();
        var itemDataString = JsonSerializer.Serialize(itemResult!.Data, _serializerOptions);
        return
            JsonSerializer.Deserialize<GenericEntityCreationResult>(itemDataString, _serializerOptions)!;
    }
}