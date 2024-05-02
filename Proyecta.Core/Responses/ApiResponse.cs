using System.Text.Json.Serialization;

namespace Proyecta.Core.Responses;

public class ApiResponse
{
    public required bool Success { get; init; }
    public required string Code { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Message { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IList<ApiErrorResponse>? Errors { get; init; }
}

public class ApiResponse<T> : ApiResponse
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public T Data { get; set; }
}

public record ApiErrorResponse
{
    public required string Code { get; init; }
    public required string Description { get; init; }
}