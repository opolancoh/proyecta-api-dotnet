namespace Proyecta.Core.DTOs.Risk;

public record RiskDetailDto : RiskListDto
{
    public DateTime CreatedAt { get; set; }
    public IdNameDto<string?> CreatedBy { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    public IdNameDto<string?> UpdatedBy { get; set; }
}