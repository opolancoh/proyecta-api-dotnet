using Proyecta.Core.DTOs.IdName;

namespace Proyecta.Core.DTOs.Risk;

public record RiskListDto 
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public IdNameDto<Guid> Category { get; set; }
    public int Type { get; set; }
    public IdNameDto<Guid> Owner { get; set; }
    public int Phase { get; set; }
    public int Manageability { get; set; }
    public IdNameDto<Guid> Treatment { get; set; }
    public DateOnly DateFrom { get; set; }
    public DateOnly DateTo { get; set; }
    public Boolean State { get; set; }
}