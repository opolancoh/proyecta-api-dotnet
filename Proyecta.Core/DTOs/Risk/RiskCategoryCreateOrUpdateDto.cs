using System.ComponentModel.DataAnnotations;
using Proyecta.Core.Entities.Validators;

namespace Proyecta.Core.DTOs.Risk;

public record RiskCategoryCreateOrUpdateDto // : IValidatableObject
{
    public string Name { get; set; }


    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var validationResult = new List<ValidationResult>();
        validationResult.AddRange(TextValidator.ValidateName(Name, nameof(Name)));

        return validationResult;
    }
}