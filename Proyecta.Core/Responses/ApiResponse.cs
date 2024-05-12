using System.Text.Json.Serialization;

namespace Proyecta.Core.Responses;

public class ApiResponse
{
    public required bool Success { get; init; }
    public required string Code { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Message { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, List<string>>? Errors { get; set; }
}

public class ApiResponse<T> : ApiResponse
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public T? Data { get; set; }
}