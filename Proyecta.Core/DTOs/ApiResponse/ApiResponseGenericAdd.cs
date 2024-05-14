namespace Proyecta.Core.DTOs.ApiResponse;

public record ApiResponseGenericAdd<T>
{
    public required T Id { get; set; }
}