namespace Proyecta.Core.Contracts;

public interface IAuditableEntity
{
    DateTime CreatedAt { get; set; }
    string CreatedById { get; set; }
    
    DateTime UpdatedAt { get; set; }
    string UpdatedById { get; set; }
}