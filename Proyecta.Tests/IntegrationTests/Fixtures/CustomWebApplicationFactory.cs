using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Proyecta.Core.DTOs.Auth;
using Proyecta.Core.Utilities;
using Proyecta.Repository.EntityFramework;

namespace Proyecta.Tests.IntegrationTests.Fixtures;

// CustomWebApplicationFactory cannot be abstract
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private IConfiguration Configuration { get; set; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, configBuilder) =>
        {
            context.HostingEnvironment.EnvironmentName = "Test";

            // Add JSON files for app settings
            configBuilder
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: false);

            // Build the configuration and assign to the property
            Configuration = configBuilder.Build();
        });
        
        builder.ConfigureServices(services =>
        {
            // Remove existing registrations of DbContext options
            RemoveExistingDbContextOptions<AuthDbContext>(services);
            RemoveExistingDbContextOptions<ApiDbContext>(services);

            // Add AuthDbContext with a specific test database configuration
            var authTestConnectionString = CommonHelper.GetEnvironmentVariable("PROYECTA_DB_CONNECTION_AUTH_TEST");
            services.AddDbContext<AuthDbContext>(options =>
                options.UseNpgsql(authTestConnectionString));

            // Add ApiDbContext with a specific test database configuration
            var apiTestConnectionString = CommonHelper.GetEnvironmentVariable("PROYECTA_DB_CONNECTION_API_TEST");
            services.AddDbContext<ApiDbContext>(options =>
                options.UseNpgsql(apiTestConnectionString));


            // Build an intermediate service provider
            var serviceProvider = services.BuildServiceProvider();

            using var scope = serviceProvider.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var authDb = scopedServices.GetRequiredService<AuthDbContext>();
            var apiDb = scopedServices.GetRequiredService<ApiDbContext>();
            var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory>>();

            try
            {
                authDb.Database.EnsureDeleted();
                apiDb.Database.EnsureDeleted();

                authDb.Database.EnsureCreated();
                apiDb.Database.EnsureCreated();

                // Optionally seed the database with test data
                // InitializeAuthDbForTests(authDb);
                // InitializeApiDbForTests(apiDb);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred setting up the test databases. Error: {ExceptionMessage}",
                    ex.Message);
            }
        });
        
        builder.ConfigureTestServices(services =>
        {
            // Remove the existing authentication
            var authenticationBuilder = services.AddAuthentication();
            authenticationBuilder.Services.Configure<AuthenticationOptions>(o =>
            {
                o.SchemeMap.Clear();
                ((IList<AuthenticationSchemeBuilder>) o.Schemes).Clear();
            }); 
            services.ConfigureJwtAuthenticationForTests(Configuration);
        });
    }


    /* private void InitializeAuthDbForTests(AuthDbContext db)
    {
        // Add entities for AuthDbContext seeding if necessary
        // db.RiskCategories.Add(new RiskCategory { ... });
        // db.SaveChanges();
    }

    private void InitializeApiDbForTests(ApiDbContext db)
    {
        // Add entities for ApiDbContext seeding if necessary
        // db.RiskCategories.Add(new RiskCategory { ... });
        // db.SaveChanges();
    } */

    public string GenerateAccessTokenForAdministrator()
    {
        return GenerateAccessToken("Administrator");
    }

    private string GenerateAccessToken(string role)
    {
        var issuer = Configuration.GetSection("JwtSettings:Issuer").Value;
        var audience = Configuration.GetSection("JwtSettings:Audience").Value;
        var secret = Configuration.GetSection("JwtSettings:Secret").Value; 
        var expiration = DateTime.UtcNow.AddMinutes(30);
        // Claims generation
        var userRoles = new List<string> { role };
        var claimsInput = new JwtAccessTokenClaimsInputDto
        {
            UserId = $"userId-{role}",
            UserName = $"userName-{role}",
            UserDisplayName = $"userDisplayName-{role}",
            UserRoles = userRoles.ToList()
        };
        var claims = AuthHelper.GetClaims(claimsInput);
        return AuthHelper.GenerateAccessToken(issuer!, audience!, secret!, claims, expiration);
    }

    protected override void ConfigureClient(HttpClient client)
    {
        client.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    private static void RemoveExistingDbContextOptions<TContext>(IServiceCollection services) where TContext : DbContext
    {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<TContext>));
        if (descriptor != null)
        {
            services.Remove(descriptor);
        }
    }
}