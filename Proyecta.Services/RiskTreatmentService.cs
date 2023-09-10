using Proyecta.Core.Contracts.Repositories;
using Proyecta.Core.Contracts.Services;
using Proyecta.Core.Entities;
using Proyecta.Core.DTOs;
using Proyecta.Core.Results;

namespace Proyecta.Services;

public sealed class RiskTreatmentService : IRiskTreatmentService
{
    private readonly IRiskTreatmentRepository _repository;

    public RiskTreatmentService(IRiskTreatmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApplicationResult> GetAll()
    {
        var items = await _repository.GetAll();

        return new ApplicationResult
        {
            Status = 200,
            D = items
        };
    }

    public async Task<ApplicationResult> GetById(Guid id)
    {
        var item = await _repository.GetById(id);

        if (item == null)
        {
            return new ApplicationResult
            {
                Status = 404,
                Message = $"The entity with id '{id}' doesn't exist in the database."
            };
        }

        return new ApplicationResult
        {
            Status = 200,
            D = item
        };
    }

    public async Task<ApplicationResult> Create(RiskTreatmentCreateOrUpdateDto item)
    {
        var newItem = GetEntity(null, item);
        // newItem.CreatedAt = DateTime.UtcNow;
        // newItem.UpdatedAt = DateTime.UtcNow;

        await _repository.Create(newItem);

        return new ApplicationResult
        {
            Status = 201,
            Message = "User created successfully.",
            D = new { newItem.Id }
        };
    }

    public async Task<ApplicationResult> Update(Guid id, RiskTreatmentCreateOrUpdateDto item)
    {
        var itemToUpdate = GetEntity(id, item);
        // itemToUpdate.UpdatedAt = DateTime.UtcNow;

        await _repository.Update(itemToUpdate);

        return new ApplicationResult
        {
            Status = 204,
            Message = "Item updated successfully.",
        };
    }

    public async Task<ApplicationResult> Remove(Guid id)
    {
        await _repository.Remove(id);

        return new ApplicationResult
        {
            Status = 204,
            Message = "Item deleted successfully.",
        };
    }

    public async Task<ApplicationResult> AddRange(IEnumerable<RiskTreatmentCreateOrUpdateDto> items)
    {
        var newItems = new List<RiskTreatment>();

        foreach (var item in items)
        {
            var newItem = GetEntity(null, item);
            // newItem.CreatedAt = DateTime.UtcNow;
            // newItem.UpdatedAt = DateTime.UtcNow;
            newItems.Add(newItem);
        }

        await _repository.AddRange(newItems);

        return new ApplicationResult
        {
            Status = 204,
            Message = "Items added successfully.",
        };
    }

    private RiskTreatment GetEntity(Guid? id, RiskTreatmentCreateOrUpdateDto item)
    {
        var entity = new RiskTreatment()
        {
            Name = item.Name,
        };

        if (id != null) entity.Id = id.Value;

        return entity;
    }
}