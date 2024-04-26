using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecta.Core.Entities.Risk;

public class Risk : EntityBase
{
    public string Name { get; set; }
    public string Code { get; set; }
    public DateOnly DateFrom { get; set; }
    public DateOnly DateTo { get; set; }
    public Boolean State { get; set; }
    public RiskType Type { get; set; }
    public RiskPhase Phase { get; set; }
    public RiskManageability Manageability { get; set; }

    // Relationships
    // One-to-one relationship
    [ForeignKey("RiskCategory")] public Guid CategoryId { get; set; }
    public RiskCategory Category { get; set; }

    [ForeignKey("RiskOwner")] public Guid OwnerId { get; set; }
    public RiskOwner Owner { get; set; }

    [ForeignKey("RiskTreatment")] public Guid TreatmentId { get; set; }
    public RiskTreatment Treatment { get; set; }
}

public enum RiskType
{
    Common = 1,
    Specific = 2
}

public enum RiskPhase
{
    F1 = 1,
    F2,
    F3,
    F4
}

public enum RiskManageability
{
    Low = 1,
    Medium,
    High
}