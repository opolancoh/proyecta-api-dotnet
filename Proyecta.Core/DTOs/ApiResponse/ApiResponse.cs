using System.Text.Json.Serialization;

namespace Proyecta.Core.DTOs.ApiResponse;

public record ApiResponse : IApiResponse
{
    public required int Status { get; init; }

    public required ApiBody Body { get; init; }
}

public record ApiResponse<T> : IApiResponse
{
    public required int Status { get; init; }

    public required ApiBody<T> Body { get; init; }

    // Explicitly implement the Body property of IApiResponse
    ApiBody IApiResponse.Body => Body;
}

public record ApiBody
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Message { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, List<string>>? Errors { get; set; }
}

public record ApiBody<T> : ApiBody
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public T? Data { get; init; }
}