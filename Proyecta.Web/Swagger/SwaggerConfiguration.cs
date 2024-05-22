using System.Diagnostics.CodeAnalysis;

namespace Proyecta.Web.Swagger;

[ExcludeFromCodeCoverage]
public static class SwaggerConfiguration
{
    public static void ConfigureSwaggerUI(WebApplication app)
    {
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Proyecta API V1");
            c.RoutePrefix = "api-docs"; // Serve Swagger UI at '/api-docs/index.html'
            c.ConfigObject.AdditionalItems["supportedSubmitMethods"] = new string[] { }; // Disables "Try it out" button for all methods
            c.DefaultModelsExpandDepth(-1); // Hides schemas section completely
        });
    }
}