using Microsoft.AspNetCore.Identity;

namespace Proyecta.Services.Helpers;

public static class ErrorResponseHelper
{
    public static Dictionary<string, List<string>> GetIdentityErrors( IEnumerable<IdentityError> errorList)
    {
        var errors = new Dictionary<string, List<string>>();

        foreach (var error in errorList)
        {
            if (errors.TryGetValue(error.Code, out var value))
            {
                value.Add(error.Description);
            }
            else
            {
                errors.Add(error.Code, new List<string> { error.Description });
            }
        }

        return errors;
    }
}
