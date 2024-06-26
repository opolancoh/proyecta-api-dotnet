using Microsoft.EntityFrameworkCore;
using Proyecta.Core.Contracts.Repositories;
using Proyecta.Core.DTOs.Auth;
using Proyecta.Core.DTOs.IdName;

namespace Proyecta.Repository.EntityFramework;

public class ApplicationUserRepository : IApplicationUserRepository
{
    private readonly AuthDbContext _context;

    public ApplicationUserRepository(AuthDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ApplicationUserListDto>> GetAllWithRoles()
    {
        var result = await (from u in _context.Users
            join ur in _context.UserRoles on u.Id equals ur.UserId into userRolesGroup
            from ur in userRolesGroup.DefaultIfEmpty()
            join r in _context.Roles on ur.RoleId equals r.Id into rolesGroup
            from r in rolesGroup.DefaultIfEmpty()
            select new
            {
                u.Id,
                u.FirstName,
                u.LastName,
                u.DisplayName,
                u.UserName,
                Role = r.Name
            }).AsNoTracking().ToListAsync();

        var resultWithListOfRoles =
            result
                .GroupBy(x => new
                {
                    x.Id,
                    x.FirstName,
                    x.LastName,
                    x.DisplayName,
                    x.UserName,
                })
                .Select(x => new ApplicationUserListDto
                {
                    Id = x.Key.Id,
                    FirstName = x.Key.FirstName,
                    LastName = x.Key.LastName,
                    DisplayName = x.Key.DisplayName,
                    UserName = x.Key.UserName,
                    Roles = (x.Count() == 1 && x.FirstOrDefault()?.Role == null)
                        ? new List<string>()
                        : x.Select(r => r.Role).ToList()
                });

        return resultWithListOfRoles;
    }

    public async Task<ApplicationUserDetailDto?> GetByIdWithRoles(string userId)
    {
        var user = await _context.Users
            .Where(u => u.Id == userId)
            .Select(u => new ApplicationUserDetailDto
            {
                Id = u.Id,
                UserName = u.UserName!,
                FirstName = u.FirstName,
                LastName = u.LastName,
                DisplayName = u.DisplayName,
                CreatedAt = u.CreatedAt,
                CreatedBy = u.CreatedBy == null
                    ? null
                    : new IdNameDto<string>
                    {
                        Id = u.CreatedBy.Id,
                        Name = u.CreatedBy.DisplayName
                    },
                UpdatedAt = u.UpdatedAt,
                UpdatedBy = u.UpdatedBy == null
                    ? null
                    : new IdNameDto<string>
                    {
                        Id = u.UpdatedBy.Id,
                        Name = u.UpdatedBy.DisplayName
                    },
            })
            .FirstOrDefaultAsync();

        if (user == null) return null;

        var userRoles = await _context.UserRoles
            .Where(ur => ur.UserId == userId)
            .Join(_context.Roles, // Join UserRoles with Roles
                ur => ur.RoleId, // Key from UserRoles
                r => r.Id, // Key from Roles
                (ur, r) => new IdNameDto<string> { Id = r.Id, Name = r.Name! }) // Result selector
            .ToListAsync();

        user.Roles = userRoles;

        return user;
    }
}