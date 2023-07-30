using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecta.Core.Entities;

public class RiskTreatment
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}