using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Proyecta.Core.Entities.Auth;
using Proyecta.Core.Utils;

namespace Proyecta.Repository.EntityFramework.Extensions;

public static class IdentityRolModelBuilderExtensions
{
    public static void ConfigureIdentityRoleEntity(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdentityRole>().HasData(GetRoles());
    }

    public static IEnumerable<IdentityRole> GetRoles()
    {
        return new List<IdentityRole>()
        {
            new()
            {
                Id = Guid.Parse("e11ec141-7c48-4332-a7a7-ce02b8a6cc01").ToString(),
                Name = "System",
                NormalizedName = "SYSTEM"
            },
            new()
            {
                Id = Guid.Parse("0204fa00-56a1-44a2-8430-54caa05c4d2a").ToString(),
                Name = "Administrator",
                NormalizedName = "ADMINISTRATOR"
            },
            new()
            {
                Id = Guid.Parse("0f9f7ff1-2aa5-4b89-b90d-ee0b211869f3").ToString(),
                Name = "Manager",
                NormalizedName = "MANAGER"
            },
            new()
            {
                Id = Guid.Parse("005668af-483d-4ff0-9fe8-947f3ca7f28f").ToString(),
                Name = "Editor",
                NormalizedName = "EDITOR"
            },
            new()
            {
                Id = Guid.Parse("3176d541-8299-4a5f-a2db-4fdc64c5d7d9").ToString(),
                Name = "Viewer",
                NormalizedName = "VIEWER"
            }
        };
    }
}