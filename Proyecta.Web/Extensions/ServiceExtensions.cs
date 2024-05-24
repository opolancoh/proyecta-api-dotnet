using System.Text;
using System.Text.Json;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Proyecta.Core.Contracts.Repositories;
using Proyecta.Core.Contracts.Repositories.Risk;
using Proyecta.Core.Contracts.Services;
using Proyecta.Core.DTOs.ApiResponse;
using Proyecta.Core.Entities;
using Proyecta.Core.Entities.Auth;
using Proyecta.Core.Utilities;
using Proyecta.Services;
using Proyecta.Repository.EntityFramework;
using Proyecta.Repository.EntityFramework.Risk;
using Proyecta.Services.Risk;
using Proyecta.Web.Validators;

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
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<IAuthService, AuthService>();
    }

    public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var appDbConnection = CommonHelper.GetEnvironmentVariable("PROYECTA_DB_CONNECTION_API");
#if DEBUG
        Console.WriteLine($"[ConfigureDbContext] appDbConnection:{appDbConnection}");
#endif
        services.AddDbContext<ApiDbContext>(opts =>
        {
            opts.UseNpgsql(appDbConnection);
            opts.EnableSensitiveDataLogging();
        });

        var authDbConnection = CommonHelper.GetEnvironmentVariable("PROYECTA_DB_CONNECTION_AUTH");
#if DEBUG
        Console.WriteLine($"[ConfigureDbContext] authDbConnection:{authDbConnection}");
#endif
        services.AddDbContext<AuthDbContext>(opts =>
        {
            opts.UseNpgsql(authDbConnection);
            opts.EnableSensitiveDataLogging();
        });
    }

    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder
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
        services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
                    foreach (var kvp in context.ModelState)
                    {
                        if (kvp.Value.Errors.Count > 0)
                        {
                            var errorMessages = new List<string>();
                            foreach (var error in kvp.Value.Errors)
                            {
                                errorMessages.Add(error.ErrorMessage);
                            }
                            errors[kvp.Key] = errorMessages;
                        }
                    }

                    var errorResponse = new ApiResponse
                    {
                        Success = false,
                        Code = ApiResponseCode.BadRequest,
                        Message = "One or more validation errors occurred.",
                        Errors = errors
                    };

                    return new OkObjectResult(errorResponse);
                };
            });
    }

    public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
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

    public static void ConfigureFluentValidation(this IServiceCollection services)
    {
        services
            .AddFluentValidationAutoValidation()
            .AddValidatorsFromAssemblyContaining<IdNameAddOrUpdateValidator>();
    }
}