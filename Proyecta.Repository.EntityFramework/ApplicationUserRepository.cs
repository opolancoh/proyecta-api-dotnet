using Microsoft.EntityFrameworkCore;
using Proyecta.Core.Contracts.Repositories;
using Proyecta.Core.Entities;
using Proyecta.Core.Entities.DTOs;
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

    public async Task<IEnumerable<ApplicationUserListDto>> GetUsersWithRoles()
    {
        /* var users = await _context
            .Users
            .Join(
                _context.UserRoles,
                u => u.Id,
                ur => ur.UserId,
                (u, ur) => new
                {
                    u.Id,
                    u.FirstName,
                    u.LastName,
                    u.UserName,
                    u.CreatedAt,
                    u.UpdatedAt,
                    ur.RoleId
                })
            .Join(
                _context.Roles,
                u => u.RoleId,
                r => r.Id,
                (u, r) => new
                {
                    u.Id,
                    u.FirstName,
                    u.LastName,
                    u.UserName,
                    u.CreatedAt,
                    u.UpdatedAt,
                    RoleName = r.Name
                }
            )
            .GroupBy(x => new
            {
                x.Id,
                x.FirstName,
                x.LastName,
                x.UserName,
                x.CreatedAt,
                x.UpdatedAt,
            })
            .Select(x => new ApplicationUserListDto
            {
                Id = x.Key.Id,
                FirstName = x.Key.FirstName,
                LastName = x.Key.LastName,
                UserName = x.Key.UserName!,
                CreatedAt = x.Key.CreatedAt,
                UpdatedAt = x.Key.UpdatedAt,
                Roles = x.Select(y => y.RoleName!)
            })
            .AsNoTracking()
            .ToListAsync(); */

        var users = await (
            from user in _context.Users
            join userRoles in _context.UserRoles on user.Id equals userRoles.UserId
            join role in _context.Roles on userRoles.RoleId equals role.Id
            group role.Name by new
            {
                user.Id,
                user.FirstName,
                user.LastName,
                user.UserName,
            }
            into g
            select new ApplicationUserListDto
            {
                Id = g.Key.Id,
                FirstName = g.Key.FirstName,
                LastName = g.Key.LastName,
                UserName = g.Key.UserName,
                Roles = g.ToList()
            }
        ).AsNoTracking().ToListAsync();

        return users;
    }

    public async Task<ApplicationUserDetailsDto?> GetByIdWithRoles(string id)
    {
        var result = await (
            from user in _context.Users
            where user.Id == id
            join userRoles in _context.UserRoles on user.Id equals userRoles.UserId
            join role in _context.Roles on userRoles.RoleId equals role.Id
            group role.Name by new
            {
                user.Id,
                user.FirstName,
                user.LastName,
                user.UserName,
                user.CreatedAt,
                user.UpdatedAt,
            }
            into g
            select new ApplicationUserDetailsDto
            {
                Id = g.Key.Id,
                FirstName = g.Key.FirstName,
                LastName = g.Key.LastName,
                UserName = g.Key.UserName,
                CreatedAt = g.Key.CreatedAt,
                UpdatedAt = g.Key.UpdatedAt,
                Roles = g.ToList()
            }
        ).AsNoTracking().ToListAsync();

        return result.SingleOrDefault();
    }
}