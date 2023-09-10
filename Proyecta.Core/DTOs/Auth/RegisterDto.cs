using System.ComponentModel.DataAnnotations;
using Proyecta.Core.Entities.Validators;

namespace Proyecta.Core.DTOs.Auth;

public record RegisterDto: IValidatableObject
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string? UserName { get; init; }
    public string? Password { get; init; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var validationResult = new List<ValidationResult>();
        validationResult.AddRange(ApplicationUserValidator.ValidateName(FirstName, nameof(FirstName)));
        validationResult.AddRange(ApplicationUserValidator.ValidateName(LastName, nameof(LastName)));
        validationResult.AddRange(ApplicationUserValidator.ValidateUserName(UserName, nameof(UserName)));

        return validationResult;
    }
}