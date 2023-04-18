using Proyecta.Core.Contracts.Repositories;
using Proyecta.Core.Contracts.Services;
using Proyecta.Core.Entities;
using Proyecta.Core.Entities.DTOs;

namespace Proyecta.Core.Services;

public sealed class RiskService : IRiskService
{
    private readonly IRiskRepository _repository;

    public RiskService(IRiskRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Risk>> GetAll()
    {
        return await _repository.GetAll();
    }

    public async Task<Risk?> GetById(Guid id)
    {
        return await _repository.GetById(id);
    }

    public async Task<Guid> Create(RiskCreateOrUpdateDto item)
    {
        var newItem = GetEntity(null, item);
        newItem.CreatedAt = DateTime.UtcNow;
        newItem.UpdatedAt = DateTime.UtcNow;

        await _repository.Create(newItem);

        return newItem.Id;
    }

    public async Task Update(Guid id, RiskCreateOrUpdateDto item)
    {
        var itemToUpdate = GetEntity(id, item);
        itemToUpdate.UpdatedAt = DateTime.UtcNow;

        await _repository.Update(itemToUpdate);
    }

    public async Task Remove(Guid id)
    {
        await _repository.Remove(id);
    }
    
    public async Task AddRange(IEnumerable<RiskCreateOrUpdateDto> items)
    {
        var newItems = new List<Risk>();
        
        foreach (var item in items)
        {
            var newItem = GetEntity(null, item);
            newItem.CreatedAt = DateTime.UtcNow;
            newItem.UpdatedAt = DateTime.UtcNow;
            newItems.Add(newItem);
        }

        await _repository.AddRange(newItems);
    }

    private Risk GetEntity(Guid? id, RiskCreateOrUpdateDto item)
    {
        var entity = new Risk()
        {
            Name = item.Name,
            Code = item.Code,
            Category = item.Category,
            Type = item.Type,
            Owner = item.Owner,
            Phase = item.Phase,
            Manageability = item.Manageability,
            Treatment = item.Treatment,
            DateFrom = item.DateFrom,
            DateTo = item.DateTo,
            State = item.State,
        };

        if (id != null) entity.Id = id.Value;

        return entity;
    }
}