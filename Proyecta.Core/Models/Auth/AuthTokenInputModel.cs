using System.ComponentModel.DataAnnotations;

namespace Proyecta.Core.Models.Auth;

public record AuthTokenInputModel : IValidatableObject
{
    public string? AccessToken { get; init; }
    public string? RefreshToken { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var validationResult = new List<ValidationResult>();
        
        if (AccessToken == null)
            validationResult.Add(new ValidationResult($"{nameof(AccessToken)} is required.",
                new[] { nameof(AccessToken) }));

        if (RefreshToken == null)
            validationResult.Add(new ValidationResult($"{nameof(RefreshToken)} is required.",
                new[] { nameof(RefreshToken) }));

        return validationResult;
    }
}