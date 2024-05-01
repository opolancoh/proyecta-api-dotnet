using System.ComponentModel.DataAnnotations;

namespace Proyecta.Core.DTOs.Auth;

public record ApplicationUserCreateOrUpdateDto : IValidatableObject
{
    [Required] public string? FirstName { get; init; }
    [Required] public string? LastName { get; init; }
    
    [Required] public string? DisplayName { get; init; }
    [Required] public string? UserName { get; init; }
    public string? Password { get; init; }
    public ICollection<string>? Roles { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var validationResult = new List<ValidationResult>();
        // validationResult.AddRange(UserValidator.ValidateUserName(UserName, nameof(UserName)));

        return validationResult;
    }
}