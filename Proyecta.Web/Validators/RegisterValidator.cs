using FluentValidation;
using Proyecta.Core.DTOs.Auth;
using Proyecta.Core.Utilities;

namespace Proyecta.Web.Validators;

public class RegisterValidator : AbstractValidator<RegisterDto>
{
    public RegisterValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage(ValidationMessages.RequiredField)
            .Matches(TextHelper.RegExp.NameRegExp).WithMessage(ValidationMessages.InvalidField);
        
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage(ValidationMessages.RequiredField)
            .Matches(TextHelper.RegExp.NameRegExp).WithMessage(ValidationMessages.InvalidField);
        
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage(ValidationMessages.RequiredField)
            .Matches(TextHelper.RegExp.UserNameRegExp).WithMessage(ValidationMessages.InvalidField);
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(ValidationMessages.RequiredField);
    }
}