using Proyecta.Core.Entities;
using Proyecta.Core.Entities.Risk;

namespace Proyecta.Tests.Helpers;

public static class DbHelper
{
    public static Guid RiskId1 = new Guid("01692cba-f59b-4f02-9ee2-fcbbb04a7b29");
    public static Guid RiskId2 = new Guid("9c6ad8fd-1837-4b64-ab84-5a42de5b8529");
    public static Guid RiskId3 = new Guid("cff5860a-3c38-4d0d-966c-770b1bfaeb92");
    public static Guid RiskId4 = new Guid("d5c8df05-64be-4d36-8ca7-461242542873");
    public static Guid RiskId5 = new Guid("dae1cc58-84ed-4e5f-a8ad-e7714fbea7ac");

    // {{review}}
    public static List<Risk> GetRisks = new List<Risk>();
    /* public static List<Risk> GetRisks =>
        new List<Risk>
        {
            new()
            {
                Id = RiskId1,
                Code = 101,
                Name = "Risk 101",
                Category = "Category 101",
                Type = "Type 101",
                Owner = "Owner 101",
                Phase = "Phase 101",
                Manageability = "Manageability 101",
                Treatment = "Treatment 101",
                DateFrom = new DateOnly(2023, 04, 13),
                DateTo = new DateOnly(2023, 05, 13),
                State = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = RiskId2,
                Code = 102,
                Name = "Risk 102",
                Category = "Category 102",
                Type = "Type 102",
                Owner = "Owner 102",
                Phase = "Phase 102",
                Manageability = "Manageability 102",
                Treatment = "Treatment 102",
                DateFrom = new DateOnly(2023, 04, 14),
                DateTo = new DateOnly(2023, 05, 14),
                State = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = RiskId3,
                Code = 103,
                Name = "Risk 103",
                Category = "Category 103",
                Type = "Type 103",
                Owner = "Owner 103",
                Phase = "Phase 103",
                Manageability = "Manageability 103",
                Treatment = "Treatment 103",
                DateFrom = new DateOnly(2023, 04, 15),
                DateTo = new DateOnly(2023, 05, 15),
                State = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = RiskId4,
                Code = 104,
                Name = "Risk 104",
                Category = "Category 104",
                Type = "Type 104",
                Owner = "Owner 104",
                Phase = "Phase 104",
                Manageability = "Manageability 104",
                Treatment = "Treatment 104",
                DateFrom = new DateOnly(2023, 04, 16),
                DateTo = new DateOnly(2023, 05, 16),
                State = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = RiskId5,
                Code = 105,
                Name = "Risk 105",
                Category = "Category 105",
                Type = "Type 105",
                Owner = "Owner 105",
                Phase = "Phase 105",
                Manageability = "Manageability 105",
                Treatment = "Treatment 105",
                DateFrom = new DateOnly(2023, 04, 17),
                DateTo = new DateOnly(2023, 05, 17),
                State = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
        }; */
}