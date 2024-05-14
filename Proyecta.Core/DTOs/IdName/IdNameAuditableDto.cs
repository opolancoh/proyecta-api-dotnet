namespace Proyecta.Core.DTOs.IdName;

public record IdNameAuditableDto<T> : IdNameDto<T>
{
    public DateTime CreatedAt { get; set; }
    public IdNameDto<string?> CreatedBy { get; set; }

    public DateTime UpdatedAt { get; set; }
    public IdNameDto<string?> UpdatedBy { get; set; }
}