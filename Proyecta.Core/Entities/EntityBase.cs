using Proyecta.Core.Contracts;

namespace Proyecta.Core.Entities;

public abstract class EntityBase
{
    public Guid Id { get; set; }
    
    // Auditable implementation
    public DateTime CreatedAt { get; set; }
    public string CreatedById { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string UpdatedById { get; set; }
}