using Microsoft.EntityFrameworkCore;
using Proyecta.Core.Contracts.Repositories.Risk;
using Proyecta.Core.DTOs.IdName;
using Proyecta.Core.DTOs.Risk;
using Proyecta.Core.Exceptions;

namespace Proyecta.Repository.EntityFramework.Risk;

public class RiskRepository : IRiskRepository
{
    private readonly ApiDbContext _context;
    private readonly AuthDbContext _authContext;
    private readonly DbSet<Core.Entities.Risk.Risk> _entitySet;

    public RiskRepository(ApiDbContext context, AuthDbContext authContext)
    {
        _context = context;
        _authContext = authContext;
        _entitySet = context.Risks;
    }

    public async Task<IEnumerable<RiskListDto>> GetAll()
    {
        return await _entitySet
            .AsNoTracking()
            .Select(
                x => new RiskListDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    Category = new IdNameDto<Guid> { Id = x.Category.Id, Name = x.Category.Name },
                    Type = (int)x.Type,
                    Owner = new IdNameDto<Guid> { Id = x.Owner.Id, Name = x.Owner.Name },
                    Phase = (int)x.Phase,
                    Manageability = (int)x.Manageability,
                    Treatment = new IdNameDto<Guid> { Id = x.Treatment.Id, Name = x.Treatment.Name },
                    DateFrom = x.DateFrom,
                    DateTo = x.DateTo,
                    State = x.State
                })
            .ToListAsync();
    }

    public async Task<RiskDetailDto?> GetById(Guid id)
    {
        // Todo: Create a single query in the DB
        var item = await _entitySet
            .AsNoTracking()
            .Select(x => new RiskDetailDto
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                Category = new IdNameDto<Guid> { Id = x.Category.Id, Name = x.Category.Name },
                Type = (int)x.Type,
                Owner = new IdNameDto<Guid> { Id = x.Owner.Id, Name = x.Owner.Name },
                Phase = (int)x.Phase,
                Manageability = (int)x.Manageability,
                Treatment = new IdNameDto<Guid> { Id = x.Treatment.Id, Name = x.Treatment.Name },
                DateFrom = x.DateFrom,
                DateTo = x.DateTo,
                State = x.State,
                CreatedAt = x.CreatedAt,
                CreatedBy = new IdNameDto<string> { Id = x.CreatedById, Name = "" },
                UpdatedAt = x.UpdatedAt,
                UpdatedBy = new IdNameDto<string> { Id = x.UpdatedById, Name = "" },
            })
            .SingleOrDefaultAsync(x => x.Id == id);

        if (item == null)
            return null;

        var createdByName = await _authContext.Users
            .Where(x => x.Id == item.CreatedBy.Id)
            .Select(x => x.DisplayName)
            .FirstOrDefaultAsync();

        var updatedByName = item.CreatedBy.Id == item.UpdatedBy.Id
            ? createdByName
            : await _authContext.Users
                .Where(x => x.Id == item.UpdatedBy.Id)
                .Select(x => x.DisplayName)
                .FirstOrDefaultAsync();

        item.CreatedBy.Name = createdByName;
        item.UpdatedBy.Name = updatedByName;

        return item;
    }

    public async Task Create(Core.Entities.Risk.Risk item)
    {
        _entitySet.Add(item);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Core.Entities.Risk.Risk item)
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
        var item = new Core.Entities.Risk.Risk { Id = id };

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

    public async Task AddRange(IEnumerable<Core.Entities.Risk.Risk> items)
    {
        await _entitySet.AddRangeAsync(items);
        await _context.SaveChangesAsync();
    }
}