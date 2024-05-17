using Proyecta.Core.Entities.Risk;

namespace Proyecta.Core.DTOs.Risk;

public record RiskAddOrUpdateDto
{
    public string? Name { get; set; }
    public string? Code { get; set; }
    public Guid Category { get; set; }
    public int Type { get; set; }
    public Guid Owner { get; set; }
    public int Phase { get; set; }
    public int Manageability { get; set; }
    public Guid Treatment { get; set; }
    public DateOnly DateFrom { get; set; }
    public DateOnly DateTo { get; set; }
    public bool State { get; set; }
}