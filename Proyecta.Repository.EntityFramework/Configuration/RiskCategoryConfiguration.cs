using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Proyecta.Core.Entities;

namespace Proyecta.Repository.EntityFramework.Configuration;

public class RiskCategoryConfiguration : IEntityTypeConfiguration<RiskCategory>
{
    public void Configure(EntityTypeBuilder<RiskCategory> builder)
    {
        var data = RiskCategoryData.Data;
        builder.HasData(data);
    }
}