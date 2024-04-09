using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Proyecta.Web.Filters;

public class CustomValidationAttribute : ActionFilterAttribute
{
    private readonly string _propertyName;

    public CustomValidationAttribute(string propertyName)
    {
        _propertyName = propertyName;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ActionArguments.TryGetValue(_propertyName, out var value))
        {
            if (true)
            {
                // Validation failed, return a custom result
                context.Result = new BadRequestObjectResult(new
                {
                    Message = "Custom validation failed",
                    // Add any additional information you want in the response
                });
            }
        }

        base.OnActionExecuting(context);
    }
}
