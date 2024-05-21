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
public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IDisposable
{
    private readonly string _apiDatabaseName;
    private IServiceScope _scope;
    private ApiDbContext _apiDb;
    private ILogger<CustomWebApplicationFactory> _logger;
    private IConfiguration _configuration;

    public CustomWebApplicationFactory()
    {
        // Use a unique database name for each test run
        _apiDatabaseName = $"proyecta_db_api_test_{Guid.NewGuid()}";
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder
            .ConfigureAppConfiguration(ConfigureAppConfiguration)
            .ConfigureServices(ConfigureServices)
            .ConfigureTestServices(ConfigureTestServices);
    }

    private void ConfigureAppConfiguration(WebHostBuilderContext context, IConfigurationBuilder configBuilder)
    {
        context.HostingEnvironment.EnvironmentName = "Test";

        // Add JSON files for app settings
        configBuilder.AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: false);

        // Build the configuration and assign to the property
        _configuration = configBuilder.Build();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        // Remove existing registrations of DbContext options
        RemoveExistingDbContextOptions<ApiDbContext>(services);

        // Add ApiDbContext with a specific test database configuration
        var apiTestConnectionString =
            $"{CommonHelper.GetEnvironmentVariable("PROYECTA_DB_CONNECTION_API_TEST")};Database={_apiDatabaseName};";
        services.AddDbContext<ApiDbContext>(options => options.UseNpgsql(apiTestConnectionString));

        // Build an intermediate service provider
        var serviceProvider = services.BuildServiceProvider();
        _scope = serviceProvider.CreateScope();
        var scopedServices = _scope.ServiceProvider;
        _apiDb = scopedServices.GetRequiredService<ApiDbContext>();
        _logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory>>();

        InitializeDatabase().GetAwaiter().GetResult();
    }

    private void ConfigureTestServices(IServiceCollection services)
    {
        // Remove the existing authentication
        var authenticationBuilder = services.AddAuthentication();
        authenticationBuilder.Services.Configure<AuthenticationOptions>(o =>
        {
            o.SchemeMap.Clear();
            ((IList<AuthenticationSchemeBuilder>)o.Schemes).Clear();
        });
        services.ConfigureJwtAuthenticationForTests(_configuration);
    }

    private async Task InitializeDatabase()
    {
        try
        {
            await _apiDb.Database.EnsureDeletedAsync();
            await _apiDb.Database.EnsureCreatedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred setting up the test databases. Error: {ExceptionMessage}",
                ex.Message);
        }
    }

    public string GenerateAccessTokenForAdministrator()
    {
        return GenerateAccessToken("Administrator");
    }

    private string GenerateAccessToken(string role)
    {
        var issuer = _configuration.GetSection("JwtSettings:Issuer").Value;
        var audience = _configuration.GetSection("JwtSettings:Audience").Value;
        var secret = _configuration.GetSection("JwtSettings:Secret").Value;
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

    public new void Dispose()
    {
        try
        {
            _apiDb.Database.EnsureDeleted();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred deleting the test database. Error: {ExceptionMessage}", ex.Message);
        }
        finally
        {
            _apiDb.Dispose();
            _scope.Dispose();
        }

        GC.SuppressFinalize(this);
    }
}