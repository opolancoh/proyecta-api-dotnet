using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecta.Core.Entities.Risk;

public class Risk : EntityBase
{
    public string Name { get; init; }
    public string Code { get; init; }
    public DateOnly DateFrom { get; init; }
    public DateOnly DateTo { get; init; }
    public bool State { get; init; }
    public RiskType Type { get; init; }
    public RiskPhase Phase { get; init; }
    public RiskManageability Manageability { get; init; }

    // Relationships
    // One-to-one relationship
    [ForeignKey("RiskCategory")] public Guid CategoryId { get; init; }
    public RiskCategory Category { get; init; }

    [ForeignKey("RiskOwner")] public Guid OwnerId { get; init; }
    public RiskOwner Owner { get; init; }

    [ForeignKey("RiskTreatment")] public Guid TreatmentId { get; init; }
    public RiskTreatment Treatment { get; init; }
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