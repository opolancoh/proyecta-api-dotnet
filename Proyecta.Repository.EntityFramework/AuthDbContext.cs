using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Proyecta.Core.Entities.Auth;
using Proyecta.Repository.EntityFramework.Seed;

namespace Proyecta.Repository.EntityFramework;

public class AuthDbContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public AuthDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RefreshToken>()
            .HasKey(x => new { x.UserId, x.Token });
        modelBuilder.SeedAuth();
        base.OnModelCreating(modelBuilder);
    }
}