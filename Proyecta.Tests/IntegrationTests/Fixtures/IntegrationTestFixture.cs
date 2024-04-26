using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Proyecta.Core.Contracts.Repositories;
using Proyecta.Core.Contracts.Repositories.Risk;
using Proyecta.Core.Contracts.Services;
using Proyecta.Repository.EntityFramework;
using Proyecta.Repository.EntityFramework.Risk;
using Proyecta.Services;
using Proyecta.Services.Risk;

namespace Proyecta.Tests.IntegrationTests.Fixtures;

public class IntegrationTestFixture : IDisposable
{
    private WebApplicationFactory<Program> Factory { get; }
    public HttpClient Client { get; }
    public ApiDbContext ApiDbContext { get; }
    public JsonSerializerOptions SerializerOptions  { get; }

    public IntegrationTestFixture()
    {
        
        Factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    /* services.AddDbContext<AppDbContext>(options =>
                        options.UseInMemoryDatabase("TestDb")); */
                    var testConnectionString = Environment.GetEnvironmentVariable("PROYECTA_DB_CONNECTION_TEST");
                    if(string.IsNullOrEmpty(testConnectionString))
                        throw new InvalidOperationException($"The environment variable 'PROYECTA_DB_CONNECTION_TEST' is null or empty.");
                    
                    // Override current AuthDbContext
                    var currentAppDbContext = services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<ApiDbContext>));
                    services.Remove(currentAppDbContext!);
                    services.AddDbContext<ApiDbContext>(options => options
                        .UseNpgsql(testConnectionString));
                    // Override current AuthDbContext
                    var currentAuthDbContext = services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<AuthDbContext>));
                    services.Remove(currentAuthDbContext!);
                    services.AddDbContext<AuthDbContext>(options => options
                        .UseNpgsql(testConnectionString));
                    

                    // Register repository and service
                    services.AddScoped<IRiskCategoryRepository, RiskCategoryRepository>();
                    services.AddScoped<IRiskCategoryService, RiskCategoryService>();
                });
            });

        // Create an HttpClient which is setup for the test host
        Client = Factory.CreateClient();

        // Get the instance of AppDbContext from the service provider
        var scope = Factory.Services.CreateScope();
        ApiDbContext = scope.ServiceProvider.GetRequiredService<ApiDbContext>();
        
        SerializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        
        // Authenticate the client
        Authenticate();
    }
    
    public void InitializeDatabase()
    {
        ApiDbContext.Database.EnsureDeleted();
        ApiDbContext.Database.EnsureCreated();
    }

    private void Authenticate()
    {
        var token = GenerateJwtToken();
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
    
    private string GenerateJwtToken()
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, "TestUser"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        // Ensure the secret key is at least 16 characters long
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("PROYECTA_APP_KEY")); // Replace with your secret key

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "PROYECTA_APP", // Replace with your issuer
            audience: "https://localhost:7134", // Replace with your audience
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }


    public void Dispose()
    {
        ApiDbContext.Database.EnsureDeleted();
        Client.Dispose();
        Factory.Dispose();
    }
}
