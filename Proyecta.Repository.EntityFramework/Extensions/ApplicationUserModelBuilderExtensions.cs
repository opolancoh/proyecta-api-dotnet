using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Proyecta.Core.Entities;
using Proyecta.Core.Entities.Auth;
using Proyecta.Core.Utilities;

namespace Proyecta.Repository.EntityFramework.Extensions;

public static class ApplicationUserModelBuilderExtensions
{
    public static void ConfigureApplicationUserEntity(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(c => c.FirstName)
                .IsRequired()
                .HasMaxLength(60);

            entity.Property(c => c.LastName)
                .IsRequired()
                .HasMaxLength(60);

            entity.Property(c => c.DisplayName)
                .IsRequired()
                .HasMaxLength(40);

            // Configure the CreatedBy relationship as one-to-many: one user can create many users
            entity.HasOne(u => u.CreatedBy) // ApplicationUser has one creator
                .WithMany() // The creator can have many created users
                .HasForeignKey(u => u.CreatedById) // Foreign key in ApplicationUser pointing to the creator
                .OnDelete(DeleteBehavior.NoAction) // Prevent cascade delete
                .IsRequired(); // Assuming the creator is not required

            // Configure the UpdatedBy relationship as one-to-many: one user can update many users
            entity.HasOne(u => u.UpdatedBy) // ApplicationUser has one updater
                .WithMany() // The updater can have many updated users
                .HasForeignKey(u => u.UpdatedById) // Foreign key in ApplicationUser pointing to the updater
                .OnDelete(DeleteBehavior.NoAction) // Prevent cascade delete
                .IsRequired();
        });

        var users = GetUsers();
        modelBuilder.Entity<ApplicationUser>().HasData(users);
    }

    public static IEnumerable<ApplicationUser> GetUsers()
    {
        var now = DateTime.UtcNow;
        var userSystemId = Guid.Parse("174ebfd6-f9bf-491b-b598-7587d9bf106d").ToString();
        var userAdminId = Guid.Parse("1c0bbe23-61b6-4cd0-a429-d606dac4d04c").ToString();
        var users = new List<ApplicationUser>()
        {
            new()
            {
                Id = userSystemId,
                UserName = "system",
                FirstName = "System",
                LastName = "User",
                DisplayName = "System",
                NormalizedUserName = "SYSTEM",
                Email = "system@ikobit.com",
                NormalizedEmail = "SYSTEM@IKOBIT.COM",
                CreatedAt = now,
                CreatedById = userSystemId,
                UpdatedAt = now,
                UpdatedById = userSystemId,
            },
            new()
            {
                Id = userAdminId,
                UserName = "admin",
                FirstName = "Admin",
                LastName = "User",
                DisplayName = "Admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@ikobit.com",
                NormalizedEmail = "ADMIN@IKOBIT.COM",
                CreatedAt = now,
                CreatedById = userSystemId,
                UpdatedAt = now,
                UpdatedById = userSystemId,
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

        return users;
    }
}