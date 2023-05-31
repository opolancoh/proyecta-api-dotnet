using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Proyecta.Web.Models;

namespace Proyecta.Web.Extensions;

public static class ApplicationBuilderExtensions
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger logger)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    logger.LogError(
                        $"[Exception] DateTimeUTC:{DateTime.UtcNow} TraceId:{context.TraceIdentifier} Path:{contextFeature.Path} Endpoint:{contextFeature.Endpoint?.DisplayName} Message:{contextFeature.Error.Message}");
                    await context.Response.WriteAsync(new ErrorDetails()
                    {
                        TraceId = context.TraceIdentifier,
                        StatusCode = context.Response.StatusCode,
                        Message = "Internal Server Error."
                    }.ToString());
                }
            });
        });
    }
}