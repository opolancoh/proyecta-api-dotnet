using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Proyecta.Core.Entities;

namespace Proyecta.Repository.EntityFramework.Configuration;

public class RiskOwnerConfiguration : IEntityTypeConfiguration<RiskOwner>
{
    public void Configure(EntityTypeBuilder<RiskOwner> builder)
    {
        var data = RiskOwnerData.Data;
        builder.HasData(data);
    }
}