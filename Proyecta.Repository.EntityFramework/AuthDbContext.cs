using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Proyecta.Core.Entities.Auth;
using Proyecta.Repository.EntityFramework.Extensions;

namespace Proyecta.Repository.EntityFramework;

public class AuthDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public AuthDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Identity
        modelBuilder.ConfigureIdentityRoleEntity();
        modelBuilder.ConfigureApplicationUserEntity();
        modelBuilder.ConfigureIdentityUserRoleEntity();
        //
        modelBuilder.ConfigureRefreshTokenEntity();
    }
}