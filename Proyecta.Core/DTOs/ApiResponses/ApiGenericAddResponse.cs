namespace Proyecta.Core.DTOs.ApiResponses;

public record ApiGenericAddResponse<T>
{
    public required T Id { get; init; }
}
