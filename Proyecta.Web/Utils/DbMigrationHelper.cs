using Microsoft.EntityFrameworkCore;

namespace Proyecta.Web.Utils;

public static class DbMigrationHelper
{
    public static IHost MigrateDatabase<T>(this IHost host) where T : DbContext
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<T>();
        var logger = services.GetRequiredService<ILogger<Program>>();
        try
        {
            context.Database.EnsureCreated();
            logger.LogInformation($"Database '{context.Database.GetDbConnection().Database}' was migrated successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                $"An error occurred while migrating the '{context.Database.GetDbConnection().Database}' database.  {ex.Message}");
        }

        return host;
    }
}