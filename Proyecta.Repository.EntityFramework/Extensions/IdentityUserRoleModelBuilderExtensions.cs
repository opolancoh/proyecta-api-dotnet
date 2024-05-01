using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Proyecta.Repository.EntityFramework.Extensions;

public static class IdentityUserRoleModelBuilderExtensions
{
    public static void ConfigureIdentityUserRoleEntity(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(GetUserRoles());
    }

    private static IEnumerable<IdentityUserRole<string>> GetUserRoles()
    {
        var userRoles = new List<IdentityUserRole<string>>();

        var roles = IdentityRolModelBuilderExtensions.GetRoles();
        var users = ApplicationUserModelBuilderExtensions.GetUsers();

        var systemRole = roles.Single(x => x.NormalizedName == "SYSTEM");
        var systemUser = users.Single(x => x.NormalizedUserName == "SYSTEM");
        userRoles.Add(new IdentityUserRole<string>
        {
            UserId = systemUser.Id,
            RoleId = systemRole.Id
        });

        var adminRole = roles.Single(x => x.NormalizedName == "ADMINISTRATOR");
        var adminUser = users.Single(x => x.NormalizedUserName == "ADMIN");
        userRoles.Add(new IdentityUserRole<string>
        {
            UserId = adminUser.Id,
            RoleId = adminRole.Id
        });

        return userRoles;
    }
}