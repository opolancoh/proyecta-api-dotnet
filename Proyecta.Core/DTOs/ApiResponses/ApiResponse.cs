using System.Text.Json.Serialization;

namespace Proyecta.Core.DTOs.ApiResponses;

public record ApiResponse
{
    public required int Status { get; init; }

    public required ApiBody Body { get; init; }
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
