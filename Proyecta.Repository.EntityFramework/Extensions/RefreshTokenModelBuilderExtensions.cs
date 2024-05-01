using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Proyecta.Core.Entities.Auth;
using Proyecta.Core.Utils;

namespace Proyecta.Repository.EntityFramework.Extensions;

public static class RefreshTokenModelBuilderExtensions
{
    public static void ConfigureRefreshTokenEntity(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RefreshToken>()
            .HasKey(x => new { x.UserId, x.Token });
    }
}