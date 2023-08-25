using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Proyecta.Core.Entities;
using Proyecta.Repository.EntityFramework.Configuration;
using Proyecta.Repository.EntityFramework.Seed;

namespace Proyecta.Repository.EntityFramework;

public class AuthDbContext : IdentityDbContext<ApplicationUser>
{
    public AuthDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.SeedAuth();
        base.OnModelCreating(modelBuilder);
    }
}