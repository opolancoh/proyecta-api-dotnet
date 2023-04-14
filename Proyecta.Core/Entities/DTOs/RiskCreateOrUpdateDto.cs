using System.ComponentModel.DataAnnotations;
using Proyecta.Core.Entities.Validators;

namespace Proyecta.Core.Entities.DTOs;

public record RiskCreateOrUpdateDto : IValidatableObject
{
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

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var validationResult = new List<ValidationResult>();
        validationResult.AddRange(RiskValidator.ValidateTitle(Name, nameof(Name)));

        return validationResult;
    }
}