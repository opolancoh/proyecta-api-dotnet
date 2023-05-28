using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Proyecta.Core.Contracts.Repositories;
using Proyecta.Core.Contracts.Services;
using Proyecta.Core.Services;
using Proyecta.Repository.EntityFramework;

namespace Proyecta.Web.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        });
    }

    public static void ConfigurePersistenceServices(this IServiceCollection services)
    {
        services.AddScoped<IRiskRepository, RiskRepository>();

        services.AddScoped<IRiskService, RiskService>();
    }

    public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var dbConnection = configuration.GetConnectionString("DbConnection");

        services.AddDbContext<AppDbContext>(opts => opts.UseNpgsql(dbConnection));
    }

    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                    {
                        policy
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    }
                );
            }
        );
    }
}