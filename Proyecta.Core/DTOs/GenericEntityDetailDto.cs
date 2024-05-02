namespace Proyecta.Core.DTOs;

public record GenericEntityDetailDto<T>
{
    public required T Id { get; set; }
    public required string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public IdNameDto<string> CreatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public IdNameDto<string> UpdatedBy { get; set; }
}