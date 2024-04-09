using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Proyecta.Core.Entities.Auth;
using Proyecta.Core.Utils;

namespace Proyecta.Repository.EntityFramework.Seed;

public static class AuthModelBuilderExtensions
{
    public static void SeedAuth(this ModelBuilder modelBuilder)
    {
        // Roles
        var roles = new List<IdentityRole>()
        {
            new() { Name = "System", NormalizedName = "SYSTEM" },
            new() { Name = "Administrator", NormalizedName = "ADMINISTRATOR" },
            new() { Name = "Manager", NormalizedName = "MANAGER" },
            new() { Name = "Editor", NormalizedName = "EDITOR" },
            new() { Name = "Viewer", NormalizedName = "VIEWER" }
        };
        modelBuilder.Entity<IdentityRole>().HasData(roles);

        // Users
        var users = new List<ApplicationUser>()
        {
            new()
            {
                UserName = "system",
                NormalizedUserName = "SYSTEM",
                Email = "system@ikobit.com",
                NormalizedEmail = "SYSTEM@IKOBIT.COM"
            },
            new()
            {
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@ikobit.com",
                NormalizedEmail = "ADMIN@IKOBIT.COM"
            },
        };
        // Passwords
        var passwordHasher = new PasswordHasher<ApplicationUser>();

        var systemUser = users.Single(x => x.UserName == "system");
        systemUser.PasswordHash = passwordHasher.HashPassword(systemUser,
            CommonHelper.GetEnvironmentVariable($"PROYECTA_API_USER_SYSTEM_PASSWORD")!);

        var adminUser = users.Single(x => x.UserName == "admin");
        adminUser.PasswordHash = passwordHasher.HashPassword(adminUser,
            CommonHelper.GetEnvironmentVariable($"PROYECTA_API_USER_ADMIN_PASSWORD")!);

        modelBuilder.Entity<ApplicationUser>().HasData(users);

        // User Roles
        var userRoles = new List<IdentityUserRole<string>>();

        var systemRole = roles.Single(x => x.Name == "System");
        userRoles.Add(new IdentityUserRole<string>
        {
            UserId = systemUser.Id,
            RoleId = systemRole.Id
        });

        var adminRole = roles.Single(x => x.Name == "Administrator");
        userRoles.Add(new IdentityUserRole<string>
        {
            UserId = adminUser.Id,
            RoleId = adminRole.Id
        });

        modelBuilder.Entity<IdentityUserRole<string>>().HasData(userRoles);
    }
}