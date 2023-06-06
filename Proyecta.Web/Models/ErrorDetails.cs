using System.Text.Json;

namespace Proyecta.Web.Models;

public class ErrorDetails
{
    
    public string TraceId { get; set; }
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public string Timestamp => DateTime.UtcNow.ToLongDateString();

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}