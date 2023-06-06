using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Proyecta.Core.Helpers;

namespace Proyecta.Core.Entities.Validators;

public static class UserValidator
{
    public static IEnumerable<ValidationResult> ValidateUserName(string value, string fieldName)
    {
        if (!Regex.IsMatch(value, TextHelper.RegExp.NameRegExp))
        {
            yield return new ValidationResult(
                $"The {fieldName} field is invalid.", new[] { fieldName });
        }
    }
}