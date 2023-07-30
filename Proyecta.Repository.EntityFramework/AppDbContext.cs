using Microsoft.EntityFrameworkCore;
using Proyecta.Core.Entities;

namespace Proyecta.Repository.EntityFramework;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Risk> Risks { get; set; }
    public DbSet<RiskCategory> RiskCategory { get; set; }
    public DbSet<RiskOwner> RiskOwner { get; set; }
    public DbSet<RiskTreatment> RiskTreatment { get; set; }
}