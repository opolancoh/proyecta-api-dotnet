using Microsoft.AspNetCore.Diagnostics;

namespace Proyecta.Web.Middlewares;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception has occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
        if (contextFeature != null)
        {
            _logger.LogError(
                "[Exception] DateTimeUTC:{DateTimeUtcNow} TraceId:{TraceIdentifier} Path:{Path} Endpoint:{Endpoint} Message:{Message}",
                DateTime.UtcNow, context.TraceIdentifier, contextFeature.Path, contextFeature.Endpoint?.DisplayName,
                contextFeature.Error.Message);
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = 500;

        var response = new
        {
            statusCode = context.Response.StatusCode,
            message = "Internal Server Error.",
            detail = exception.Message // Avoid exposing internal details in production
        };

        return context.Response.WriteAsJsonAsync(response);
    }
}