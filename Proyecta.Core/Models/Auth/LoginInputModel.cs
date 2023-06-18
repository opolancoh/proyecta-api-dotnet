using System.ComponentModel.DataAnnotations;

namespace Proyecta.Core.Models.Auth;

public record LoginInputModel : IValidatableObject
{
    public string? UserName { get; init; }
    public string? Password { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var validationResult = new List<ValidationResult>();
        if (UserName == null)
            validationResult.Add(new ValidationResult($"{nameof(UserName)} is required.", new[] { nameof(UserName) }));
        if (Password == null)
            validationResult.Add(new ValidationResult($"{nameof(Password)} is required.", new[] { nameof(Password) }));
        
        return validationResult;
    }
}