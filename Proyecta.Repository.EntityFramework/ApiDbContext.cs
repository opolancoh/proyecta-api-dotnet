using Microsoft.EntityFrameworkCore;
using Proyecta.Core.Entities.Risk;

namespace Proyecta.Repository.EntityFramework;

public class ApiDbContext : DbContext
{
    public DbSet<Core.Entities.Risk.Risk> Risks { get; set; }
    public DbSet<RiskCategory> RiskCategory { get; set; }
    public DbSet<RiskOwner> RiskOwner { get; set; }
    public DbSet<RiskTreatment> RiskTreatment { get; set; }

    public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
    {
    }
}