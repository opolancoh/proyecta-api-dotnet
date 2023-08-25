using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Proyecta.Core.Entities;

namespace Proyecta.Repository.EntityFramework.Configuration;

public class RiskConfiguration : IEntityTypeConfiguration<Risk>
{
    public void Configure(EntityTypeBuilder<Risk> builder)
    {
        var data = RiskData.GetData();
        builder.HasData(data);
    }
}