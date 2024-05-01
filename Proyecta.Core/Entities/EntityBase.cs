using Proyecta.Core.Contracts;

namespace Proyecta.Core.Entities;

public abstract class EntityBase: IAuditableEntity
{
    public Guid Id { get; set; }
    
    // IAuditableEntity implementation
    public DateTime CreatedAt { get; set; }
    public string? CreatedById { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? UpdatedById { get; set; }
}