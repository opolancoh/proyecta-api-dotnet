using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Proyecta.Core.Contracts.Repositories;
using Proyecta.Core.Contracts.Services;
using Proyecta.Core.Entities;
using Proyecta.Services;
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
        // RiskCategory
        services.AddScoped<IRiskCategoryRepository, RiskCategoryRepository>();
        services.AddScoped<IRiskCategoryService, RiskCategoryService>();
        // RiskOwner
        services.AddScoped<IRiskOwnerRepository, RiskOwnerRepository>();
        services.AddScoped<IRiskOwnerService, RiskOwnerService>();
        // RiskTreatment
        services.AddScoped<IRiskTreatmentRepository, RiskTreatmentRepository>();
        services.AddScoped<IRiskTreatmentService, RiskTreatmentService>();
        // Risk
        services.AddScoped<IRiskRepository, RiskRepository>();
        services.AddScoped<IRiskService, RiskService>();
        // User
        services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
        services.AddScoped<IApplicationUserService, ApplicationUserService>();
        // Auth
        services.AddScoped<IAuthenticationService, AuthenticationService>();
    }

    public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var dbUser = Environment.GetEnvironmentVariable($"PROYECTA_{environment?.ToUpper()}_DB_USER");
        var dbPassword = Environment.GetEnvironmentVariable($"PROYECTA_{environment?.ToUpper()}_DB_PASSWORD");

        var appDbConnection = configuration.GetConnectionString("AppDbConnection");
        var fullAppDbConnection = $"Username={dbUser};Password={dbPassword};{appDbConnection}";
        Console.WriteLine($"[ConfigureDbContext] appDbConnection:{appDbConnection}");
        services.AddDbContext<AppDbContext>(opts =>
        {
            opts.UseNpgsql(fullAppDbConnection);
            opts.EnableSensitiveDataLogging();
        });

        var authDbConnection = configuration.GetConnectionString("AuthDbConnection");
        var fullAuthDbConnection = $"Username={dbUser};Password={dbPassword};{authDbConnection}";
        Console.WriteLine($"[ConfigureDbContext] authDbConnection:{authDbConnection}");
        services.AddDbContext<AuthDbContext>(opts =>
        {
            opts.UseNpgsql(fullAuthDbConnection);
            opts.EnableSensitiveDataLogging();
        });
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

    public static void ConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>(o =>
            {
                o.Password.RequireDigit = true;
                o.Password.RequireLowercase = true;
                o.Password.RequireUppercase = true;
                o.Password.RequireNonAlphanumeric = true;
                o.Password.RequiredLength = 6;
                o.User.RequireUniqueEmail = false;
            })
            .AddEntityFrameworkStores<AuthDbContext>()
            .AddDefaultTokenProviders();
    }

    public static void ConfigureControllers(this IServiceCollection services)
    {
        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
        });
    }

    public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtConfig");
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];
        var secretKey = jwtSettings["Secret"];

        services
            .AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
                };
            });
    }
}