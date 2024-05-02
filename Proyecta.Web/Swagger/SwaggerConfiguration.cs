namespace Proyecta.Web.Swagger;

public static class SwaggerConfiguration
{
    public static void ConfigureSwaggerUI(WebApplication app)
    {
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Proyecta API V1");
            c.RoutePrefix = "api-reference"; // Serve Swagger UI at '/api-reference/index.html'
            c.ConfigObject.AdditionalItems["supportedSubmitMethods"] = new string[] { }; // Disables "Try it out" button for all methods
            c.DefaultModelsExpandDepth(-1); // Hides schemas section completely
        });
    }
}