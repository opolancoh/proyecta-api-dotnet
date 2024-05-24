using Microsoft.EntityFrameworkCore;
using Proyecta.Core.Entities.Risk;

namespace Proyecta.Repository.EntityFramework;

public class ApiDbContext : DbContext
{
    public DbSet<Core.Entities.Risk.Risk> Risks { get; set; }
    public DbSet<RiskCategory> RiskCategories { get; set; }
    public DbSet<RiskOwner> RiskOwners { get; set; }
    public DbSet<RiskTreatment> RiskTreatments { get; set; }

    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
    {
    }
}