using Proyecta.Repository.EntityFramework;
using Proyecta.Tests.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Proyecta.Tests.IntegrationTests.Fixtures;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var connectionString =
                "Server=localhost; Database=proyecta_db_test; Username=postgres; Password=My@Passw0rd;";
            // Entity Framework DbContext
            // Remove current
            var entityFrameworkDbContext = services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (entityFrameworkDbContext != null) services.Remove(entityFrameworkDbContext);
            // Add a new one
            services.AddDbContext<AppDbContext>(options => options
                .UseNpgsql(connectionString));

            //
            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<AppDbContext>();
            var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory<TProgram>>>();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            // Don't update/remove this initial data
            var risks = DbHelper.GetRisks;
            db.Risks?.AddRange(risks);
            db.SaveChanges();

            logger.LogError("All data was saved successfully"); 
        });
    }
}