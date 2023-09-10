using System.Text.Json.Serialization;

namespace Proyecta.Core.Results;

public record ApplicationResult
{
    public int Status { get; set; }
    
    // Data
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? D { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Message { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IList<ApplicationResponseError>? Errors { get; set; }
}

public record ApplicationResponseError
{
    public string Code { get; set; }
    public string Description { get; set; }
}
