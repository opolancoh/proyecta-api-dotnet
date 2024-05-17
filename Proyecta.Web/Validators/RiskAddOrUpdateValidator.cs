using FluentValidation;
using Proyecta.Core.DTOs.Risk;
using Proyecta.Core.Entities.Risk;
using Proyecta.Core.Utilities;

namespace Proyecta.Web.Validators;

public class RiskAddOrUpdateValidator : AbstractValidator<RiskAddOrUpdateDto>
{
    public RiskAddOrUpdateValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(ValidationMessages.RequiredField)
            .Matches(TextHelper.RegExp.NameRegExp).WithMessage(ValidationMessages.InvalidField);

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage(ValidationMessages.RequiredField)
            .Matches(@"^[a-zA-Z0-9_\-\+\.\ ]*$").WithMessage(ValidationMessages.InvalidField);

        RuleFor(x => x.Category)
            .NotEmpty().WithMessage(ValidationMessages.RequiredField);

        RuleFor(x => x.Type)
            .Must(BeAValidEnumValue<RiskType>).WithMessage(ValidationMessages.InvalidField);

        RuleFor(x => x.Owner)
            .NotEmpty().WithMessage(ValidationMessages.RequiredField);

        RuleFor(x => x.Phase)
            .Must(BeAValidEnumValue<RiskPhase>).WithMessage(ValidationMessages.InvalidField);

        RuleFor(x => x.Manageability)
            .Must(BeAValidEnumValue<RiskManageability>).WithMessage(ValidationMessages.InvalidField);

        RuleFor(x => x.Treatment)
            .NotEmpty().WithMessage(ValidationMessages.RequiredField);

        RuleFor(x => x.DateFrom)
            .LessThanOrEqualTo(x => x.DateTo).WithMessage("DateFrom must be less than or equal to DateTo.");

        RuleFor(x => x.DateTo)
            .GreaterThanOrEqualTo(x => x.DateFrom).WithMessage("DateTo must be greater than or equal to DateFrom.");

        RuleFor(x => x.State)
            .NotNull().WithMessage(ValidationMessages.RequiredField);
    }

    private bool BeAValidEnumValue<TEnum>(int value) where TEnum : struct, Enum
    {
        return Enum.IsDefined(typeof(TEnum), value);
    }
}