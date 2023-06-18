using System.Net;
using System.Text;
using System.Text.Json;
using Proyecta.Core.Entities;
using Proyecta.Core.Entities.DTOs;
using Proyecta.Tests.Helpers;
using Proyecta.Tests.IntegrationTests.Fixtures;

namespace Proyecta.Tests.IntegrationTests;

[Collection("SharedContext")]
public class RiskIntegrationTests
{
    private readonly JsonSerializerOptions _serializerOptions;
    private readonly HttpClient _httpClient;
    private const string BasePath = "/api/risks";

    public RiskIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _httpClient = factory.CreateClient();

        _serializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    #region GetAll

    [Fact]
    public async Task GeAll_ShouldReturnAllItems()
    {
        var response = await _httpClient.GetAsync($"{BasePath}");
        var payloadString = await response.Content.ReadAsStringAsync();
        var payloadObject = JsonSerializer.Deserialize<List<Risk>>(payloadString, _serializerOptions);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(payloadObject?.Count >= DbHelper.GetRisks.Count);
    }

    #endregion

    #region GeById

    [Theory]
    [MemberData(nameof(ItemIds))]
    public async Task GeById_ShouldReturnOnlyOneItem(Guid itemId)
    {
        var existingItem = DbHelper.GetRisks.SingleOrDefault(x => x.Id == itemId);

        var response = await _httpClient.GetAsync($"{BasePath}/{existingItem?.Id}");
        var payloadString = await response.Content.ReadAsStringAsync();
        var payloadObject = JsonSerializer.Deserialize<Risk>(payloadString, _serializerOptions);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(existingItem?.Code, payloadObject?.Code);
        Assert.Equal(existingItem?.Name, payloadObject?.Name);
        Assert.Equal(existingItem?.Category, payloadObject?.Category);
        Assert.Equal(existingItem?.Owner, payloadObject?.Owner);
        Assert.Equal(existingItem?.Phase, payloadObject?.Phase);
        Assert.Equal(existingItem?.Manageability, payloadObject?.Manageability);
        Assert.Equal(existingItem?.Treatment, payloadObject?.Treatment);
        Assert.Equal(existingItem?.DateFrom, payloadObject?.DateFrom);
        Assert.Equal(existingItem?.DateTo, payloadObject?.DateTo);
        Assert.Equal(existingItem?.State, payloadObject?.State);
        Assert.Equal(existingItem?.CreatedAt.Date, payloadObject?.CreatedAt.Date);
        Assert.Equal(existingItem?.UpdatedAt.Date, payloadObject?.UpdatedAt.Date);
    }

    [Fact]
    public async Task GeById_ShouldReturnNotFoundWhenIdNotExists()
    {
        var itemId = new Guid();

        var response = await _httpClient.GetAsync($"{BasePath}/{itemId}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GeById_ShouldReturnBadRequestWhenIdIsNotValid()
    {
        const string itemId = "not-valid-id";

        var response = await _httpClient.GetAsync($"{BasePath}/{itemId}");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    #endregion

    #region Create

    [Fact]
    public async Task Create_ShouldCreateAnItem()
    {
        var newItem = new
        {
            Code = 201,
            Name = "Risk 201",
            Category = "Category 201",
            Type = "Type 201",
            Owner = "Owner 201",
            Phase = "Phase 201",
            Manageability = "Manageability 201",
            Treatment = "Treatment 201",
            DateFrom = new DateOnly(2023, 04, 13),
            DateTo = new DateOnly(2023, 05, 13),
            State = true
        };
        var payload = JsonSerializer.Serialize(newItem, _serializerOptions);
        HttpContent httpContent = new StringContent(payload, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"{BasePath}", httpContent);
        var locationHeader = response.Headers.GetValues("Location").FirstOrDefault();
        var newItemId = locationHeader?.Split('/').Last().Split('?').First();

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotEqual(Guid.Empty, new Guid(newItemId!));
    }

    [Theory]
    [MemberData(nameof(MissingRequiredFieldsBase))]
    [MemberData(nameof(MissingRequiredFieldsForCreating))]
    public async Task Create_ShouldReturnBadRequestWhenMissingRequiredFields(string[] expectedCollection,
        object payloadObject)
    {
        var payload = JsonSerializer.Serialize(payloadObject, _serializerOptions);
        HttpContent httpContent = new StringContent(payload, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"{BasePath}", httpContent);
        var payloadString = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.All(expectedCollection, expected => Assert.Contains(expected, payloadString));
    }

    [Theory]
    [MemberData(nameof(InvalidFields))]
    public async Task Create_ShouldReturnBadRequestWhenFieldsAreInvalid(string[] expectedCollection,
        object payloadObject)
    {
        var payload = JsonSerializer.Serialize(payloadObject, _serializerOptions);
        HttpContent httpContent = new StringContent(payload, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"{BasePath}", httpContent);
        var payloadString = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.All(expectedCollection, expected => Assert.Contains(expected, payloadString));
    }

    [Fact]
    public async Task Create_ShouldCreateManyItems()
    {
        var newItems = DbHelper.GetRisks;
        var payload = JsonSerializer.Serialize(newItems, _serializerOptions);
        HttpContent httpContent = new StringContent(payload, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"{BasePath}/add-range", httpContent);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
    
    #endregion
    
    #region Update

    [Fact]
    public async Task Update_ShouldUpdateAnItem()
    {
        // Create a new item
        var newItem = new
        {
            Code = 501,
            Name = "Risk 501",
            Category = "Category 501",
            Type = "Type 501",
            Owner = "Owner 501",
            Phase = "Phase 501",
            Manageability = "Manageability 501",
            Treatment = "Treatment 501",
            DateFrom = new DateOnly(2023, 04, 13),
            DateTo = new DateOnly(2023, 05, 13),
            State = true
        };
        var newItemPayload = JsonSerializer.Serialize(newItem, _serializerOptions);
        var newItemHttpContent = new StringContent(newItemPayload, Encoding.UTF8, "application/json");
        var newItemResponse = await _httpClient.PostAsync($"{BasePath}", newItemHttpContent);

        var locationHeader = newItemResponse.Headers.GetValues("Location").FirstOrDefault();
        var newItemId = locationHeader?.Split('/').Last().Split('?').First();

        // Update the created item
        var itemToUpdate = new
        {
            Id = newItemId,
            Code = 502,
            Name = "Risk 502",
            Category = "Category 502",
            Type = "Type 502",
            Owner = "Owner 502",
            Phase = "Phase 502",
            Manageability = "Manageability 502",
            Treatment = "Treatment 502",
            DateFrom = new DateOnly(2023, 04, 14),
            DateTo = new DateOnly(2023, 05, 14),
            State = false
        };
        var payload = JsonSerializer.Serialize(itemToUpdate, _serializerOptions);
        var httpContent = new StringContent(payload, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync($"{BasePath}/{newItemId}", httpContent);

        // Ensure the item has been changed getting the item from the DB
        var updatedItemResponse = await _httpClient.GetAsync($"{BasePath}/{newItemId}");
        var updatedItemPayloadString = await updatedItemResponse.Content.ReadAsStringAsync();
        var updatedItemPayloadObject =
            JsonSerializer.Deserialize<Risk>(updatedItemPayloadString, _serializerOptions);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.NotEqual(Guid.Empty, updatedItemPayloadObject?.Id);
        Assert.Equal(itemToUpdate?.Code, updatedItemPayloadObject?.Code);
        Assert.Equal(itemToUpdate?.Name, updatedItemPayloadObject?.Name);
        Assert.Equal(itemToUpdate?.Category, updatedItemPayloadObject?.Category);
        Assert.Equal(itemToUpdate?.Owner, updatedItemPayloadObject?.Owner);
        Assert.Equal(itemToUpdate?.Phase, updatedItemPayloadObject?.Phase);
        Assert.Equal(itemToUpdate?.Manageability, updatedItemPayloadObject?.Manageability);
        Assert.Equal(itemToUpdate?.Treatment, updatedItemPayloadObject?.Treatment);
        Assert.Equal(itemToUpdate?.DateFrom, updatedItemPayloadObject?.DateFrom);
        Assert.Equal(itemToUpdate?.DateTo, updatedItemPayloadObject?.DateTo);
        Assert.Equal(itemToUpdate?.State, updatedItemPayloadObject?.State);
    }

    [Fact]
    public async Task Update_ShouldReturnNotFoundWhenIdNotExists()
    {
        var itemId = new Guid();
        var itemToUpdate = new
        {
            Code = 501,
            Name = "Risk 501",
            Category = "Category 501",
            Type = "Type 501",
            Owner = "Owner 501",
            Phase = "Phase 501",
            Manageability = "Manageability 501",
            Treatment = "Treatment 501",
            DateFrom = new DateOnly(2023, 04, 13),
            DateTo = new DateOnly(2023, 05, 13),
            State = true
        };

        var payload = JsonSerializer.Serialize(itemToUpdate, _serializerOptions);
        var httpContent = new StringContent(payload, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync($"{BasePath}/{itemId}", httpContent);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Theory]
    [MemberData(nameof(MissingRequiredFieldsBase))]
    [MemberData(nameof(MissingRequiredFieldsForUpdating))]
    public async Task Update_ShouldReturnBadRequestWhenMissingRequiredFields(string[] expectedCollection,
        object payloadObject)
    {
        var payload = JsonSerializer.Serialize(payloadObject, _serializerOptions);
        HttpContent httpContent = new StringContent(payload, Encoding.UTF8, "application/json");

        var itemId = new Guid();
        var response = await _httpClient.PutAsync($"{BasePath}/{itemId}", httpContent);
        var payloadString = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.All(expectedCollection, expected => Assert.Contains(expected, payloadString));
    }

    [Theory]
    [MemberData(nameof(InvalidFields))]
    public async Task Update_ShouldReturnBadRequestWhenFieldsAreInvalid(string[] expectedCollection,
        object payloadObject)
    {
        var payload = JsonSerializer.Serialize(payloadObject, _serializerOptions);
        HttpContent httpContent = new StringContent(payload, Encoding.UTF8, "application/json");

        var itemId = new Guid();
        var response = await _httpClient.PutAsync($"{BasePath}/{itemId.ToString()}", httpContent);
        var payloadString = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.All(expectedCollection, expected => Assert.Contains(expected, payloadString));
    }

    #endregion

    #region Remove

    [Fact]
    public async Task Remove_ShouldRemoveOnlyOneItem()
    {
        // Create a new item
        var newItem = new
        {
            Code = 601,
            Name = "Risk 601",
            Category = "Category 601",
            Type = "Type 601",
            Owner = "Owner 601",
            Phase = "Phase 601",
            Manageability = "Manageability 601",
            Treatment = "Treatment 601",
            DateFrom = new DateOnly(2023, 04, 13),
            DateTo = new DateOnly(2023, 05, 13),
            State = true
        };
        var newItemPayload = JsonSerializer.Serialize(newItem, _serializerOptions);
        var newItemHttpContent = new StringContent(newItemPayload, Encoding.UTF8, "application/json");
        var newItemResponse = await _httpClient.PostAsync($"{BasePath}", newItemHttpContent);

        var locationHeader = newItemResponse.Headers.GetValues("Location").FirstOrDefault();
        var newItemId = locationHeader?.Split('/').Last();

        // Remove the created item
        var response = await _httpClient.DeleteAsync($"{BasePath}/{newItemId}");

        // Ensure the item has been deleted trying to get the item from the DB
        var deletedItemResponse = await _httpClient.GetAsync($"{BasePath}/{newItemId}");

        Assert.Equal(HttpStatusCode.Created, newItemResponse.StatusCode);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, deletedItemResponse.StatusCode);
    }

    [Fact]
    public async Task Remove_ShouldReturnNotFoundWhenIdNotExists()
    {
        var itemId = new Guid();
        var response = await _httpClient.DeleteAsync($"{BasePath}/{itemId}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Remove_ShouldReturnBadRequestWhenIdIsNotValid()
    {
        const string itemId = "not-valid-id";
        var response = await _httpClient.DeleteAsync($"{BasePath}/{itemId}");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    #endregion

    public static TheoryData<string[], object> MissingRequiredFieldsBase => new()
    {
        {
            new[]
            {
                "The Name field is required.",
                "The Category field is required.",
                "The Type field is required.",
                "The Owner field is required.",
                "The Phase field is required.",
                "The Manageability field is required.",
                "The Treatment field is required.",
            },
            new { }
        },
        {
            new[]
            {
                "The Name field is required.",
                "The Category field is required.",
                "The Owner field is required.",
                "The Phase field is required."
            },
            new
            {
                Manageability = "Manageability 201",
                Treatment = "Treatment 201",
                DateFrom = new DateOnly(2023, 04, 13),
                DateTo = new DateOnly(2023, 05, 13),
                State = true
            }
        },
        {
            new[]
            {
                "The Manageability field is required.",
                "The Treatment field is required.",
            },
            new
            {
                Code = 201,
                Name = "Risk 201",
                Category = "Category 201",
                Type = "Type 201",
                Owner = "Owner 201",
                Phase = "Phase 201",
            }
        }
    };

    public static TheoryData<string[], object> MissingRequiredFieldsForCreating => new()
    {
    };

    public static TheoryData<string[], object> MissingRequiredFieldsForUpdating => new()
    {
    };

    public static TheoryData<string[], object> InvalidFields => new()
    {
        {
            new[] { "The Name field is invalid." }, new
            {
                Id = DbHelper.RiskId1,
                Code = 401,
                Name = "Risk 201 *",
                Category = "Category 401",
                Type = "Type 401",
                Owner = "Owner 401",
                Phase = "Phase 401",
                Manageability = "Manageability 401",
                Treatment = "Treatment 401",
                DateFrom = new DateOnly(2023, 04, 13),
                DateTo = new DateOnly(2023, 05, 13),
                State = true
            }
        },
    };

    public static TheoryData<Guid> ItemIds => new()
    {
        { DbHelper.RiskId1 },
        { DbHelper.RiskId2 },
        { DbHelper.RiskId3 },
        { DbHelper.RiskId4 },
        { DbHelper.RiskId5 },
    };
}