using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Proyecta.Core.Entities;

namespace Proyecta.Repository.EntityFramework.Seed;

public static class AuthModelBuilderExtensions
{
    public static void SeedAuth(this ModelBuilder modelBuilder)
    {
        // Roles
        var roles = new List<IdentityRole>()
        {
            new() { Name = "Administrator", NormalizedName = "ADMINISTRATOR" },
            new() { Name = "Manager", NormalizedName = "MANAGER" }
        };
        modelBuilder.Entity<IdentityRole>().HasData(roles);

        // Users
        var users = new List<ApplicationUser>()
        {
            new()
            {
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@ikobit.com",
                NormalizedEmail = "ADMIN@IKOBIT.COM"
            },
            new()
            {
                UserName = "manager",
                NormalizedUserName = "MANAGER",
                Email = "manager@ikobit.com",
                NormalizedEmail = "MANAGER@IKOBIT.COM"
            },
        };
        // Passwords
        var passwordHasher = new PasswordHasher<ApplicationUser>();
        
        var adminUser = users.Single(x => x.UserName == "admin");
        adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "AdminPa$$w0rd");

        var managerUser = users.Single(x => x.UserName == "manager");
        managerUser.PasswordHash = passwordHasher.HashPassword(managerUser, "ManPa$$w0rd");
        
        modelBuilder.Entity<ApplicationUser>().HasData(users);

        // User Roles
        var userRoles = new List<IdentityUserRole<string>>();

        var adminRole = roles.Single(x => x.Name == "Administrator");
        userRoles.Add(new IdentityUserRole<string>
        {
            UserId = adminUser.Id,
            RoleId = adminRole.Id
        });

        var managerRole = roles.Single(x => x.Name == "Manager");
        userRoles.Add(new IdentityUserRole<string>
        {
            UserId = managerUser.Id,
            RoleId = managerRole.Id
        });

        modelBuilder.Entity<IdentityUserRole<string>>().HasData(userRoles);
    }
}