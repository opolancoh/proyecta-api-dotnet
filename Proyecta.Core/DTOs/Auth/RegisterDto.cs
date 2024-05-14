using System.ComponentModel.DataAnnotations;
using Proyecta.Core.Entities.Validators;

namespace Proyecta.Core.DTOs.Auth;

public record RegisterDto : IValidatableObject
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string DisplayName { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var validationResult = new List<ValidationResult>();
        validationResult.AddRange(ApplicationUserValidator.ValidateName(FirstName, nameof(FirstName)));
        validationResult.AddRange(ApplicationUserValidator.ValidateName(LastName, nameof(LastName)));
        validationResult.AddRange(ApplicationUserValidator.ValidateName(DisplayName, nameof(DisplayName)));
        validationResult.AddRange(ApplicationUserValidator.ValidateUserName(Username, nameof(Username)));

        return validationResult;
    }
}