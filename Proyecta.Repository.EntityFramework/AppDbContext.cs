using Microsoft.EntityFrameworkCore;
using Proyecta.Core.Entities;
using Proyecta.Repository.EntityFramework.Configuration;

namespace Proyecta.Repository.EntityFramework;

public class AppDbContext: DbContext
{
    public DbSet<Risk> Risks { get; set; }
    public DbSet<RiskCategory> RiskCategory { get; set; }
    public DbSet<RiskOwner> RiskOwner { get; set; }
    public DbSet<RiskTreatment> RiskTreatment { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new RiskCategoryConfiguration());
        modelBuilder.ApplyConfiguration(new RiskOwnerConfiguration());
        modelBuilder.ApplyConfiguration(new RiskTreatmentConfiguration());
        modelBuilder.ApplyConfiguration(new RiskConfiguration());
    }
}