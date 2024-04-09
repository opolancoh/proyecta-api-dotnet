using Proyecta.Core.Contracts.Repositories;
using Proyecta.Core.Contracts.Services;
using Proyecta.Core.Entities;
using Proyecta.Core.DTOs;
using Proyecta.Core.Results;

namespace Proyecta.Services;

public sealed class RiskCategoryService : IRiskCategoryService
{
    private readonly IRiskCategoryRepository _repository;

    public RiskCategoryService(IRiskCategoryRepository repository)
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
                Message = $"The item with id '{id}' was not found or you don't have permission to access it."
            };
        }

        return new ApplicationResult
        {
            Status = 200,
            D = item
        };
    }

    public async Task<ApplicationResult> Create(RiskCategoryCreateOrUpdateDto item)
    {
        var newItem = GetEntity(null, item);
        // newItem.CreatedAt = DateTime.UtcNow;
        // newItem.UpdatedAt = DateTime.UtcNow;

        await _repository.Create(newItem);

        return new ApplicationResult
        {
            Status = 201,
            Message = "Risk Category created successfully.",
            D = new GenericEntityCreationResult { Id = newItem.Id }
        };
    }

    public async Task<ApplicationResult> Update(Guid id, RiskCategoryCreateOrUpdateDto item)
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

    public async Task<ApplicationResult> AddRange(IEnumerable<RiskCategoryCreateOrUpdateDto> items)
    {
        var newItems = items.Select(item => GetEntity(null, item)).ToList();

        await _repository.AddRange(newItems);

        return new ApplicationResult
        {
            Status = 204,
            Message = "Items added successfully.",
        };
    }

    private RiskCategory GetEntity(Guid? id, RiskCategoryCreateOrUpdateDto item)
    {
        var entity = new RiskCategory()
        {
            Name = item.Name,
        };

        if (id != null) entity.Id = id.Value;

        return entity;
    }
}