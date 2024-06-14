using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Proyecta.Core.DTOs.Auth;
using Proyecta.Core.Utilities;
using Proyecta.Repository.EntityFramework;
using Proyecta.Tests.Extensions;

namespace Proyecta.Tests.IntegrationTests.Fixtures;

// CustomWebApplicationFactory cannot be abstract
public class ApplicationUserWebApplicationFactory : WebApplicationFactory<Program>, IDisposable
{
    private readonly string _dbName;
    private IServiceScope _scope;
    private ILogger<ApiWebApplicationFactory> _logger;
    private IConfiguration _configuration;
    private AuthDbContext _dbContext;
    public string AdministratorAccessToken;

    public ApplicationUserWebApplicationFactory()
    {
        // Use a unique database name for each test run
        _dbName = $"proyecta_db_auth_test_{Guid.NewGuid()}";
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder
            .ConfigureAppConfiguration(ConfigureAppConfiguration)
            .ConfigureServices(ConfigureServices)
            .ConfigureTestServices(ConfigureTestServices);
    }

    protected override void ConfigureClient(HttpClient client)
    {
        client.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    public ApplicationUserAddRequest GetValidApplicationUserAddOrUpdateDto(List<string>? roles)
    {
        return new ApplicationUserAddRequest
        {
            FirstName = Guid.NewGuid().ToString(),
            LastName = Guid.NewGuid().ToString(),
            DisplayName = Guid.NewGuid().ToString(),
            UserName = Guid.NewGuid().ToString(),
            Password = "Pa$$w0rd",
            Roles = roles
        };
    }

    public async Task<(Core.Entities.ApplicationUser, string)> CreateUserAsync()
    {
        const string password = "Pa$$w0rd";
        var user = await CreateUserAsync($"{Guid.NewGuid()}", $"{Guid.NewGuid()}",
            $"{Guid.NewGuid()}", password);

        return (user, password);
    }

    public new void Dispose()
    {
        try
        {
            _dbContext.Database.EnsureDeleted();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred deleting the test database. Error: {ExceptionMessage}", ex.Message);
        }
        finally
        {
            _dbContext.Dispose();
            _scope.Dispose();
        }

        GC.SuppressFinalize(this);
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
        RemoveExistingDbContextOptions<AuthDbContext>(services);

        // Add ApiDbContext with a specific test database configuration
        var authTestConnectionString =
            $"{CommonHelper.GetEnvironmentVariable("PROYECTA_DB_CONNECTION_AUTH_TEST")};Database={_dbName};";
        services.AddDbContext<AuthDbContext>(options => options.UseNpgsql(authTestConnectionString));

        // Build an intermediate service provider
        var serviceProvider = services.BuildServiceProvider();
        _scope = serviceProvider.CreateScope();
        var scopedServices = _scope.ServiceProvider;
        _dbContext = scopedServices.GetRequiredService<AuthDbContext>();
        _logger = scopedServices.GetRequiredService<ILogger<ApiWebApplicationFactory>>();

        InitializeDatabase().GetAwaiter().GetResult();

        AdministratorAccessToken = GenerateAccessTokenForAdminUserAsync().GetAwaiter().GetResult();
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
            await _dbContext.Database.EnsureDeletedAsync();
            await _dbContext.Database.EnsureCreatedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred setting up the test databases. Error: {ExceptionMessage}",
                ex.Message);
        }
    }

    private async Task<Core.Entities.ApplicationUser> CreateUserAsync(string firstName, string lastName, string username,
        string password)
    {
        var userManager = _scope.ServiceProvider.GetRequiredService<UserManager<Core.Entities.ApplicationUser>>();

        var now = DateTime.UtcNow;
        var userId = Guid.NewGuid().ToString();

        var user = new Core.Entities.ApplicationUser
        {
            Id = userId,
            FirstName = firstName,
            LastName = lastName,
            DisplayName = Guid.NewGuid().ToString(),
            UserName = username,
            CreatedById = userId,
            UpdatedById = userId,
            CreatedAt = now,
            UpdatedAt = now
        };

        var userResult = await userManager.CreateAsync(user, password);
        if (!userResult.Succeeded)
        {
            throw new Exception(
                $"Failed to create test user: {string.Join(", ", userResult.Errors.Select(e => e.Description))}");
        }

        return user;
    }

    private async Task AssignUserToRoleAsync(Core.Entities.ApplicationUser user, string role)
    {
        var userManager = _scope.ServiceProvider.GetRequiredService<UserManager<Core.Entities.ApplicationUser>>();
        var roleManager = _scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        // Ensure the role exists
        IdentityResult? roleResult;
        if (!await roleManager.RoleExistsAsync(role))
        {
            roleResult = await roleManager.CreateAsync(new IdentityRole(role));
            if (!roleResult.Succeeded)
            {
                throw new Exception(
                    $"Failed to create '{role}' role: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
            }
        }

        // Assign the user to the Admin role
        roleResult = await userManager.AddToRoleAsync(user, role);
        if (!roleResult.Succeeded)
        {
            throw new Exception(
                $"Failed to add user to 'Admin' role: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
        }
    }

    private async Task<string> GenerateAccessTokenForAdminUserAsync()
    {
        var user = await CreateUserAsync("Admin", "User", "admin.user", "Pa$$w0rd");

        const string role = "Administrator";
        await AssignUserToRoleAsync(user, role);

        // AccessToken
        var issuer = _configuration.GetSection("JwtSettings:Issuer").Value;
        var audience = _configuration.GetSection("JwtSettings:Audience").Value;
        var secret = _configuration.GetSection("JwtSettings:Secret").Value;
        var expiration =
            DateTime.UtcNow.AddMinutes(
                Convert.ToDouble(_configuration.GetSection("JwtSettings:AccessTokenExpirationInMinutes").Value));
        // Claims generation
        var userRoles = new List<string>() { role };
        var claimsInput = new JwtAccessTokenClaimsInputDto
        {
            UserId = user.Id,
            UserName = user.UserName ?? "",
            UserDisplayName = user.DisplayName,
            UserRoles = userRoles.ToList()
        };
        var claims = AuthHelper.GetClaims(claimsInput);

        return AuthHelper.GenerateAccessToken(issuer!, audience!, secret!, claims, expiration);
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
