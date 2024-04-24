using System.Text.Json.Serialization;

namespace Proyecta.Core.Results;

public record ApplicationResult
{
    
    public required bool Success { get; init; }
    public required string Code { get; init; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? Data { get; init; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Message { get; init; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IList<ApplicationResponseError>? Errors { get; init; }
}

public record ApplicationResponseError
{
    public required string Code { get; set; }
    public required string Description { get; set; }
}
