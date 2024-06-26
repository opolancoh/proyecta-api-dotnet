using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Proyecta.Core.DTOs.Auth;
using Proyecta.Core.DTOs.IdName;
using Proyecta.Core.DTOs.Risk;
using Proyecta.Core.Entities.Risk;
using Proyecta.Core.Utilities;
using Proyecta.Repository.EntityFramework;
using Proyecta.Tests.Extensions;

namespace Proyecta.Tests.IntegrationTests.Fixtures;

// CustomWebApplicationFactory cannot be abstract
public class ApiWebApplicationFactory : WebApplicationFactory<Program>, IDisposable
{
    private readonly string _dbName;
    private IServiceScope _scope;
    private ILogger<ApiWebApplicationFactory> _logger;
    private IConfiguration _configuration;
    private ApiDbContext _dbContext;

    public ApiWebApplicationFactory()
    {
        // Use a unique database name for each test run
        _dbName = $"proyecta_db_api_test_{Guid.NewGuid()}";
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

    public string GetValidEntityName()
    {
        return $"Name  (_-.,`'ÁÉÍÓÚáéíóúñÑ) {Guid.NewGuid()} ";
    }

    public string GenerateAccessTokenForAdministrator()
    {
        return GenerateAccessToken("Administrator");
    }
    
    public IdNameAddOrUpdateDto GetValidIdNameAddOrUpdateDtoItem()
    {
        return new IdNameAddOrUpdateDto { Name = GetValidEntityName() };
    }

    public async Task<(Guid RiskCategoryId, Guid RiskOwnerId, Guid RiskTreatmentId)> SetupRiskCreationTest()
    {
        var now = DateTime.UtcNow;
        var userId = "user-id";

        var riskCategory = new RiskCategory
        {
            Id = Guid.NewGuid(), Name = GetValidEntityName(), CreatedAt = now, CreatedById = userId, UpdatedAt = now,
            UpdatedById = userId
        };
        await _dbContext.RiskCategories.AddAsync(riskCategory);

        var riskOwner = new RiskOwner
        {
            Id = Guid.NewGuid(), Name = GetValidEntityName(), CreatedAt = now, CreatedById = userId, UpdatedAt = now,
            UpdatedById = userId
        };
        await _dbContext.RiskOwners.AddAsync(riskOwner);

        var riskTreatment = new RiskTreatment
        {
            Id = Guid.NewGuid(), Name = GetValidEntityName(), CreatedAt = now, CreatedById = userId, UpdatedAt = now,
            UpdatedById = userId
        };
        await _dbContext.RiskTreatments.AddAsync(riskTreatment);

        await _dbContext.SaveChangesAsync();

        return (riskCategory.Id, riskOwner.Id, riskTreatment.Id);
    }
    
    public RiskAddOrUpdateDto GetValidRiskAddOrUpdateItem(Guid categoryId, Guid ownerId, Guid treatmentId)
    {
        return new RiskAddOrUpdateDto()
        {
            Name = GetValidEntityName(),
            Code = "001",
            Category = categoryId,
            Type = 2,
            Owner = ownerId,
            Phase = 3,
            Manageability = 1,
            Treatment = treatmentId,
            DateFrom = DateOnly.Parse("2022-09-28"),
            DateTo = DateOnly.Parse("2022-10-28"),
            State = true
        };
    }

    public async Task<RiskDetailDto> AddRiskRecord()
    {
        var now = DateTime.UtcNow;
        var userId = "user-id";

        var riskCategory = new RiskCategory
        {
            Id = Guid.NewGuid(), Name = GetValidEntityName(), CreatedAt = now, CreatedById = userId, UpdatedAt = now,
            UpdatedById = userId
        };
        await _dbContext.RiskCategories.AddAsync(riskCategory);

        var riskOwner = new RiskOwner
        {
            Id = Guid.NewGuid(), Name = GetValidEntityName(), CreatedAt = now, CreatedById = userId, UpdatedAt = now,
            UpdatedById = userId
        };
        await _dbContext.RiskOwners.AddAsync(riskOwner);

        var riskTreatment = new RiskTreatment
        {
            Id = Guid.NewGuid(), Name = GetValidEntityName(), CreatedAt = now, CreatedById = userId, UpdatedAt = now,
            UpdatedById = userId
        };
        await _dbContext.RiskTreatments.AddAsync(riskTreatment);

        var riskId = Guid.NewGuid();
        var risk = new Core.Entities.Risk.Risk
        {
            Id = riskId,
            Code = "001",
            Name = GetValidEntityName(),
            CategoryId = riskCategory.Id,
            Type = RiskType.Common,
            OwnerId = riskOwner.Id,
            Phase = RiskPhase.F2,
            Manageability = RiskManageability.Medium,
            TreatmentId = riskTreatment.Id,
            DateFrom = DateOnly.Parse("2022-09-28"),
            DateTo = DateOnly.Parse("2022-10-28"),
            State = true,
            CreatedAt = now,
            CreatedById = userId,
            UpdatedAt = now,
            UpdatedById = userId
        };
        await _dbContext.Risks.AddAsync(risk);

        await _dbContext.SaveChangesAsync();

        return new RiskDetailDto
        {
            Id = riskId,
            Code = risk.Code,
            Name = risk.Name,
            Category = new IdNameDto<Guid> { Id = riskCategory.Id, Name = riskCategory.Name },
            Type = (int)risk.Type,
            Owner = new IdNameDto<Guid> { Id = riskOwner.Id, Name = riskOwner.Name },
            Phase = (int)risk.Phase,
            Manageability = (int)risk.Manageability,
            Treatment = new IdNameDto<Guid> { Id = riskTreatment.Id, Name = riskTreatment.Name },
            DateFrom = risk.DateFrom,
            DateTo = risk.DateTo,
            State = risk.State,
            CreatedAt = risk.CreatedAt,
            CreatedBy = new IdNameDto<string> { Id = userId, Name = "" },
            UpdatedAt = risk.UpdatedAt,
            UpdatedBy = new IdNameDto<string> { Id = userId, Name = "" },
        };
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
        RemoveExistingDbContextOptions<ApiDbContext>(services);

        // Add ApiDbContext with a specific test database configuration
        var apiTestConnectionString =
            $"{CommonHelper.GetEnvironmentVariable("PROYECTA_DB_CONNECTION_API_TEST")};Database={_dbName};";
        services.AddDbContext<ApiDbContext>(options => options.UseNpgsql(apiTestConnectionString));

        // Build an intermediate service provider
        var serviceProvider = services.BuildServiceProvider();
        _scope = serviceProvider.CreateScope();
        var scopedServices = _scope.ServiceProvider;
        _dbContext = scopedServices.GetRequiredService<ApiDbContext>();
        _logger = scopedServices.GetRequiredService<ILogger<ApiWebApplicationFactory>>();

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
            await _dbContext.Database.EnsureDeletedAsync();
            await _dbContext.Database.EnsureCreatedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred setting up the test databases. Error: {ExceptionMessage}",
                ex.Message);
        }
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

    private static void RemoveExistingDbContextOptions<TContext>(IServiceCollection services) where TContext : DbContext
    {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<TContext>));
        if (descriptor != null)
        {
            services.Remove(descriptor);
        }
    }
}