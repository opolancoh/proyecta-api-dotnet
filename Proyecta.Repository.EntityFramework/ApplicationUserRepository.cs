using Microsoft.EntityFrameworkCore;
using Proyecta.Core.Contracts.Repositories;
using Proyecta.Core.DTOs.Auth;
using Proyecta.Core.Entities.Auth;
using Proyecta.Core.Exceptions;

namespace Proyecta.Repository.EntityFramework;

public class ApplicationUserRepository : IApplicationUserRepository
{
    private readonly AuthDbContext _context;
    private readonly DbSet<ApplicationUser> _entitySet;

    public ApplicationUserRepository(AuthDbContext context)
    {
        _context = context;
        _entitySet = context.Users;
    }

    public async Task<IEnumerable<ApplicationUser>> GetAll()
    {
        return await _entitySet.AsNoTracking().ToListAsync();
    }

    public async Task<ApplicationUser?> GetById(string id)
    {
        return await _entitySet.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task Create(ApplicationUser item)
    {
        _entitySet.Add(item);
        await _context.SaveChangesAsync();
    }

    public async Task Update(ApplicationUser item)
    {
        _context.Entry(item).State = EntityState.Modified;
        _context.Entry(item).Property(x => x.CreatedAt).IsModified = false;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ItemExists(item.Id))
            {
                throw new EntityNotFoundException<string>(item.Id);
            }
            else
            {
                throw;
            }
        }
    }

    public async Task Remove(string id)
    {
        var item = new ApplicationUser { Id = id };

        _entitySet.Remove(item);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ItemExists(id))
            {
                throw new EntityNotFoundException<string>(id);
            }
            else
            {
                throw;
            }
        }
    }

    public async Task<bool> ItemExists(string id)
    {
        return await _entitySet.AnyAsync(e => e.Id == id);
    }

    public async Task AddRange(IEnumerable<ApplicationUser> items)
    {
        await _entitySet.AddRangeAsync(items);
        await _context.SaveChangesAsync();
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
                    x.UserName,
                })
                .Select(x => new ApplicationUserListDto
                {
                    Id = x.Key.Id,
                    FirstName = x.Key.FirstName,
                    LastName = x.Key.LastName,
                    UserName = x.Key.UserName,
                    Roles = (x.Count() == 1 && x.FirstOrDefault()?.Role == null)
                        ? new List<string>()
                        : x.Select(r => r.Role).ToList()!
                });

        return resultWithListOfRoles;
    }

    public async Task<ApplicationUserDetailsDto?> GetByIdWithRoles(string id)
    {
        var result = await (
            from u in _context.Users
            join ur in _context.UserRoles on u.Id equals ur.UserId into userRolesGroup
            from ur in userRolesGroup.DefaultIfEmpty()
            join r in _context.Roles on ur.RoleId equals r.Id into rolesGroup
            from r in rolesGroup.DefaultIfEmpty()
            where u.Id == id
            select new
            {
                u.Id,
                u.FirstName,
                u.LastName,
                u.UserName,
                u.CreatedAt,
                u.UpdatedAt,
                Role = r.Name
            }).AsNoTracking().ToListAsync();

        var resultWithListOfRoles =
            result
                .GroupBy(x => new
                {
                    x.Id,
                    x.FirstName,
                    x.LastName,
                    x.UserName,
                    x.CreatedAt,
                    x.UpdatedAt
                })
                .Select(x => new ApplicationUserDetailsDto
                {
                    Id = x.Key.Id,
                    FirstName = x.Key.FirstName,
                    LastName = x.Key.LastName,
                    UserName = x.Key.UserName,
                    CreatedAt = x.Key.CreatedAt,
                    UpdatedAt = x.Key.UpdatedAt,
                    Roles = (x.Count() == 1 && x.FirstOrDefault()?.Role == null)
                        ? new List<string>()
                        : x.Select(r => r.Role).ToList()!
                });

        return resultWithListOfRoles.SingleOrDefault();
    }
}