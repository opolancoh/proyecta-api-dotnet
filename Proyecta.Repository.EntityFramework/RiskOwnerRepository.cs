using Microsoft.EntityFrameworkCore;
using Proyecta.Core.Contracts.Repositories;
using Proyecta.Core.Entities;
using Proyecta.Core.Exceptions;

namespace Proyecta.Repository.EntityFramework;

public class RiskOwnerRepository : IRiskOwnerRepository
{
    private readonly AppDbContext _context;
    private readonly DbSet<RiskOwner> _entitySet;

    public RiskOwnerRepository(AppDbContext context)
    {
        _context = context;
        _entitySet = context.RiskOwner;
    }

    public async Task<IEnumerable<RiskOwner>> GetAll()
    {
        return await _entitySet.AsNoTracking().ToListAsync();
    }

    public async Task<RiskOwner?> GetById(Guid id)
    {
        return await _entitySet.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task Create(RiskOwner item)
    {
        _entitySet.Add(item);
        await _context.SaveChangesAsync();
    }

    public async Task Update(RiskOwner item)
    {
        _context.Entry(item).State = EntityState.Modified;
        // _context.Entry(item).Property(x => x.CreatedAt).IsModified = false;

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
        var item = new RiskOwner { Id = id };

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
    
    public async Task AddRange(IEnumerable<RiskOwner> items)
    {
        await _entitySet.AddRangeAsync(items);
        await _context.SaveChangesAsync();
    }
}