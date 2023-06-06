using System.Text.Json.Serialization;

namespace Proyecta.Core.Models;

public record ApplicationResponse
{
    public bool Succeed { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Message { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? Data { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IList<ApplicationResponseError>? Errors { get; set; }
}

public record ApplicationResponseError
{
    public string Code { get; set; }
    public string Description { get; set; }
}
