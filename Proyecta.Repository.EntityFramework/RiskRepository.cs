using Microsoft.EntityFrameworkCore;
using Proyecta.Core.Contracts.Repositories;
using Proyecta.Core.DTOs;
using Proyecta.Core.Entities;
using Proyecta.Core.Exceptions;

namespace Proyecta.Repository.EntityFramework;

public class RiskRepository : IRiskRepository
{
    private readonly AppDbContext _context;
    private readonly DbSet<Risk> _entitySet;

    public RiskRepository(AppDbContext context)
    {
        _context = context;
        _entitySet = context.Risks;
    }

    public async Task<IEnumerable<RiskDto>> GetAll()
    {
        return await _entitySet
            .AsNoTracking()
            .Select(
                x => new RiskDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    Category = new KeyValueDto<Guid> { Id = x.Category.Id, Name = x.Category.Name },
                    Type = (int)x.Type,
                    Owner = new KeyValueDto<Guid> { Id = x.Owner.Id, Name = x.Owner.Name },
                    Phase = (int)x.Phase,
                    Manageability = (int)x.Manageability,
                    Treatment = new KeyValueDto<Guid> { Id = x.Treatment.Id, Name = x.Treatment.Name },
                    DateFrom = x.DateFrom,
                    DateTo = x.DateTo,
                    State = x.State,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                })
            .ToListAsync();
    }

    public async Task<RiskDto?> GetById(Guid id)
    {
        return await _entitySet
            .AsNoTracking()
            .Select(x => new RiskDto
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                Category = new KeyValueDto<Guid> { Id = x.Category.Id, Name = x.Category.Name },
                Type = (int)x.Type,
                Owner = new KeyValueDto<Guid> { Id = x.Owner.Id, Name = x.Owner.Name },
                Phase = (int)x.Phase,
                Manageability = (int)x.Manageability,
                Treatment = new KeyValueDto<Guid> { Id = x.Treatment.Id, Name = x.Treatment.Name },
                DateFrom = x.DateFrom,
                DateTo = x.DateTo,
                State = x.State,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            })
            .SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task Create(Risk item)
    {
        _entitySet.Add(item);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Risk item)
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
        var item = new Risk { Id = id };

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

    public async Task AddRange(IEnumerable<Risk> items)
    {
        await _entitySet.AddRangeAsync(items);
        await _context.SaveChangesAsync();
    }
}