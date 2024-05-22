using System.Diagnostics.CodeAnalysis;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;

namespace Proyecta.Web.Swagger;

[ExcludeFromCodeCoverage]
public class ExcludeVersionRoutesDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var pathsToRemove = new OpenApiPaths();
        foreach (var path in swaggerDoc.Paths)
        {
            if (!path.Key.Contains("/v{version}/"))
            {
                pathsToRemove.Add(path.Key, path.Value);
            }
        }
        
        swaggerDoc.Paths = pathsToRemove;
    }
}