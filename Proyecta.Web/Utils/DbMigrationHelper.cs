using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace Proyecta.Web.Utils;

[ExcludeFromCodeCoverage]
public static class DbMigrationHelper
{
    public static IHost MigrateDatabase<T>(this IHost host) where T : DbContext
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<T>();
        var logger = services.GetRequiredService<ILogger<Program>>();
        var connection = context.Database.GetDbConnection();

        try
        {
            logger.LogInformation("[MigrateDatabase] Attempting to migrate '{Database}' database on host '{Host}'",
                connection.Database, connection.DataSource);
            context.Database.EnsureCreated();
            // Get lists of migrations
            var appliedMigrations = context.Database.GetAppliedMigrations();
            var migrations = appliedMigrations as string[] ?? appliedMigrations.ToArray();
            if (migrations.Any())
            {
                foreach (var migration in migrations)
                {
                    logger.LogInformation("[MigrateDatabase] Applied migration: '{Migration}'", migration);
                }
            }
            else
            {
                logger.LogInformation("[MigrateDatabase] No migrations were applied to '{Database}' database.",
                    connection.Database);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex,
                "[MigrateDatabase] An error occurred while migrating the '{Database}' DB.  {Message}",
                connection.Database, ex.Message);
        }

        return host;
    }
}