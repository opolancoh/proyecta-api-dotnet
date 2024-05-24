using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Proyecta.Core.Entities;
using Proyecta.Core.Entities.Auth;
using Proyecta.Repository.EntityFramework.Configurations;

namespace Proyecta.Repository.EntityFramework;

public class AuthDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Identity
        modelBuilder.ApplyConfiguration(new IdentityRolConfiguration());
        modelBuilder.ApplyConfiguration(new ApplicationUserConfiguration());
        modelBuilder.ApplyConfiguration(new IdentityUserRoleConfiguration());
        //
        modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
    }
}