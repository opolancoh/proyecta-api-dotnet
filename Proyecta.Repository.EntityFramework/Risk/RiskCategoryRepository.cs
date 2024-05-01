using Microsoft.EntityFrameworkCore;
using Proyecta.Core.Contracts.Repositories.Risk;
using Proyecta.Core.DTOs;
using Proyecta.Core.Entities.Risk;
using Proyecta.Core.Exceptions;

namespace Proyecta.Repository.EntityFramework.Risk;

public class RiskCategoryRepository : IRiskCategoryRepository
{
    private readonly ApiDbContext _context;
    private readonly AuthDbContext _authContext;
    private readonly DbSet<RiskCategory> _entitySet;

    public RiskCategoryRepository(ApiDbContext context, AuthDbContext authContext)
    {
        _context = context;
        _authContext = authContext;
        _entitySet = context.RiskCategory;
    }

    public async Task<IEnumerable<IdNameDto<Guid>>> GetAll()
    {
        return await _entitySet
            .AsNoTracking()
            .Select(x => new IdNameDto<Guid>
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToListAsync();
    }

    public async Task<IdNameAuditableDto<Guid>?> GetById(Guid id)
    {
        var entity = await _entitySet
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new RiskCategory
            {
                Id = x.Id,
                Name = x.Name,
                CreatedAt = x.CreatedAt,
                CreatedById = x.CreatedById,
                UpdatedAt = x.UpdatedAt,
                UpdatedById = x.UpdatedById
            })
            .FirstOrDefaultAsync();

        if (entity == null)
            return null;

        var createdBy = await _authContext.Users
            .Where(x => x.Id == entity.CreatedById)
            .Select(x => x.UserName)
            .FirstOrDefaultAsync();

        var updatedBy = entity.CreatedById == entity.UpdatedById
            ? createdBy
            : await _authContext.Users
                .Where(x => x.Id == entity.UpdatedById)
                .Select(x => x.UserName)
                .FirstOrDefaultAsync();

        return new IdNameAuditableDto<Guid>
        {
            Id = entity.Id,
            Name = entity.Name,
            CreatedAt = entity.CreatedAt,
            CreatedBy = new IdNameDto<string?>
            {
                Id = entity.CreatedById,
                Name = createdBy
            },
            UpdatedAt = entity.UpdatedAt,
            UpdatedBy = new IdNameDto<string?>
            {
                Id = entity.UpdatedById,
                Name = updatedBy
            }
        };
    }

    public async Task Create(RiskCategory item)
    {
        _entitySet.Add(item);
        await _context.SaveChangesAsync();
    }

    public async Task Update(RiskCategory item)
    {
        _context.Entry(item).State = EntityState.Modified;
        _context.Entry(item).Property(x => x.CreatedAt).IsModified = false;
        _context.Entry(item).Property(x => x.CreatedById).IsModified = false;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ItemExists(item.Id))
            {
                throw new EntityNotFoundException<Guid>(item.Id);
            }
            else
            {
                throw;
            }
        }
    }

    public async Task Remove(Guid id)
    {
        var item = new RiskCategory { Id = id };

        _entitySet.Remove(item);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ItemExists(id))
            {
                throw new EntityNotFoundException<Guid>(id);
            }
            else
            {
                throw;
            }
        }
    }

    public async Task<bool> ItemExists(Guid id)
    {
        return await _entitySet.AnyAsync(e => e.Id == id);
    }

    public async Task AddRange(IEnumerable<RiskCategory> items)
    {
        await _entitySet.AddRangeAsync(items);
        await _context.SaveChangesAsync();
    }
}