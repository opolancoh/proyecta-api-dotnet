using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Proyecta.Core.Entities;

namespace Proyecta.Repository.EntityFramework.Configuration;

public class RiskTreatmentConfiguration : IEntityTypeConfiguration<RiskTreatment>
{
    public void Configure(EntityTypeBuilder<RiskTreatment> builder)
    {
        var data = RiskTreatmentData.Data;
        builder.HasData(data);
    }
}