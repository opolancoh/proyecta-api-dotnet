namespace Proyecta.Core.DTOs;

public record RiskDto 
{
    public Guid Id { get; set; }
    public int Code { get; set; }
    public string Name { get; set; }
    public KeyValueDto<Guid> Category { get; set; }
    public int Type { get; set; }
    public KeyValueDto<Guid> Owner { get; set; }
    public int Phase { get; set; }
    public int Manageability { get; set; }
    public KeyValueDto<Guid> Treatment { get; set; }
    public DateOnly DateFrom { get; set; }
    public DateOnly DateTo { get; set; }
    public Boolean State { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}