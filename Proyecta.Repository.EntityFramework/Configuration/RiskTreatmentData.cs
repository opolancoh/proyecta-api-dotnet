using Proyecta.Core.Entities;

namespace Proyecta.Repository.EntityFramework.Configuration;

public static class RiskTreatmentData
{
    public static List<RiskTreatment> Data =>
        new()
        {
            new() { Id = Guid.Parse("7d07c966-cdc6-4ce5-8792-5c46f9b3f734"), Name = "Transferir" },
            new() { Id = Guid.Parse("cb60e8aa-bb46-4ac3-bc37-963440d202fa"), Name = "Mitigar" },
            new() { Id = Guid.Parse("f0e6cd1e-cd39-44f3-929b-2e036802e231"), Name = "Aceptar" }
        };
}