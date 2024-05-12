using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Proyecta.Core.Responses;

public static class ApiResponseFactory
{
    public static ApiResponse CreateErrorResponse(string code, string message, IEnumerable<IdentityError> identityErrors, ILogger logger)
    {
        var errors = new Dictionary<string, List<string>>();
        foreach (var error in identityErrors)
        {
            logger.LogError("Error: Code: {Code}, Description: {Description}", error.Code, error.Description);

            if (!errors.ContainsKey(error.Code))
            {
                errors[error.Code] = new List<string>();
            }
            errors[error.Code].Add(error.Description);
        }

        return new ApiResponse
        {
            Success = false,
            Code = code,
            Message = message,
            Errors = errors
        };
    }
}