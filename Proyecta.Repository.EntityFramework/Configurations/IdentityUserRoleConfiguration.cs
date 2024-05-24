using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Proyecta.Repository.EntityFramework.Configurations;

public class IdentityUserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        builder.HasData(GetUserRoles());
    }

    private static IEnumerable<IdentityUserRole<string>> GetUserRoles()
    {
        var userRoles = new List<IdentityUserRole<string>>();

        var roles = IdentityRolConfiguration.GetRoles();
        var users = ApplicationUserConfiguration.GetUsers();

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