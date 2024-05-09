using Microsoft.EntityFrameworkCore;
using Proyecta.Core.Entities.Auth;

namespace Proyecta.Repository.EntityFramework.Extensions;

public static class RefreshTokenModelBuilderExtensions
{
    public static void ConfigureRefreshTokenEntity(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RefreshToken>()
            .HasKey(x => new { x.UserId, x.Token });
    }
}