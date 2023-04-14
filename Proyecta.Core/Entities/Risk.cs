namespace Proyecta.Core.Entities;

public class Risk
{
    public Guid Id { get; set; }
    public int Code { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public string Type { get; set; }
    public string Owner { get; set; }
    public string Phase { get; set; }
    public string Manageability { get; set; }
    public string Treatment { get; set; }
    public DateOnly DateFrom { get; set; }
    public DateOnly DateTo { get; set; }
    public Boolean State { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}