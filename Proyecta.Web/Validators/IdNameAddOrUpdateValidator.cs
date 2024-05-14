using FluentValidation;
using Proyecta.Core.DTOs.IdName;
using Proyecta.Core.Utilities;

namespace Proyecta.Web.Validators;

public class IdNameAddOrUpdateValidator : AbstractValidator<IdNameAddOrUpdateDto>
{
    public IdNameAddOrUpdateValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(ValidationMessages.RequiredField)
            .Matches(TextHelper.RegExp.NameRegExp).WithMessage(ValidationMessages.InvalidField);
    }
}