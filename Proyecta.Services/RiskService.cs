using Proyecta.Core.Contracts.Repositories;
using Proyecta.Core.Contracts.Services;
using Proyecta.Core.Entities;
using Proyecta.Core.DTOs;
using Proyecta.Core.Results;

namespace Proyecta.Services;

public sealed class RiskService : IRiskService
{
    private readonly IRiskRepository _repository;

    public RiskService(IRiskRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApplicationResult> GetAll()
    {
        var items = await _repository.GetAll();

        return new ApplicationResult
        {
            Success = true,
            Code = "200",
            Data = items
        };
    }

    public async Task<ApplicationResult> GetById(Guid id)
    {
        var item = await _repository.GetById(id);

        if (item == null)
        {
            return new ApplicationResult
            {
                Success = false,
                Code = "404",
                Message = $"The entity with id '{id}' doesn't exist in the database."
            };
        }

        return new ApplicationResult
        {
            Success = true,
            Code = "200",
            Data = item
        };
    }

    public async Task<ApplicationResult> Create(RiskCreateOrUpdateDto item)
    {
        var newItem = GetEntity(null, item);
        newItem.CreatedAt = DateTime.UtcNow;
        newItem.UpdatedAt = DateTime.UtcNow;

        await _repository.Create(newItem);

        return new ApplicationResult
        {
            Success = true,
            Code = "201",
            Message = "Risk created successfully.",
            Data = new { newItem.Id }
        };
    }

    public async Task<ApplicationResult> Update(Guid id, RiskCreateOrUpdateDto item)
    {
        var itemToUpdate = GetEntity(id, item);
        itemToUpdate.UpdatedAt = DateTime.UtcNow;

        await _repository.Update(itemToUpdate);

        return new ApplicationResult
        {
            Success = true,
            Code = "204",
            Message = "Item updated successfully.",
        };
    }

    public async Task<ApplicationResult> Remove(Guid id)
    {
        await _repository.Remove(id);

        return new ApplicationResult
        {
            Success = true,
            Code = "204",
            Message = "Item deleted successfully.",
        };
    }

    public async Task<ApplicationResult> AddRange(IEnumerable<RiskCreateOrUpdateDto> items)
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

        return new ApplicationResult
        {
            Success = true,
            Code = "204",
            Message = "Items added successfully.",
        };
    }

    private Risk GetEntity(Guid? id, RiskCreateOrUpdateDto item)
    {
        var entity = new Risk()
        {
            Name = item.Name,
            Code = item.Code,
            CategoryId = item.Category,
            Type = (RiskType)item.Type,
            OwnerId = item.Owner,
            Phase = (RiskPhase)item.Phase,
            Manageability = (RiskManageability)item.Manageability,
            TreatmentId = item.Treatment,
            DateFrom = item.DateFrom,
            DateTo = item.DateTo,
            State = item.State,
        };

        if (id != null) entity.Id = id.Value;

        return entity;
    }
}