namespace Proyecta.Core.DTOs.IdName;

public record IdNameDto<T>
{
    public required T Id { get; init; }
    public required string Name { get; set; }
}