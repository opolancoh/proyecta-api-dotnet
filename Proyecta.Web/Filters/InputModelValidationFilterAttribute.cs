using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Proyecta.Web.Filters;

public class InputModelValidationFilterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ModelState.IsValid) return;
        
        var errors = context.ModelState
            .Where(e => e.Value!.Errors.Count > 0)
            .Select(e => new 
            {
                Name = e.Key,
                Message = e.Value!.Errors.First().ErrorMessage
            }).ToArray();

        var response = new 
        {
            Status = "Validation Failed",
            Errors = errors
        };

        context.Result = new ObjectResult(response) 
        { 
            StatusCode = StatusCodes.Status422UnprocessableEntity // Set your custom status code here
        };
    }
}
