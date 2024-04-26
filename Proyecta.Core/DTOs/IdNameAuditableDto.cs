namespace Proyecta.Core.DTOs;

public record IdNameAuditableDto<T>
{
    public required T Id { get; init; }
    public required string Name { get; init; }
    
    public DateTime CreatedAt { get; set; }
    public IdNameDto<string> CreatedBy { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    public IdNameDto<string> UpdatedBy { get; set; }
}