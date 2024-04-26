using Proyecta.Core.Contracts.Repositories.Risk;
using Proyecta.Core.Contracts.Services;
using Proyecta.Core.DTOs;
using Proyecta.Core.DTOs.Risk;
using Proyecta.Core.Entities.Risk;
using Proyecta.Core.Results;

namespace Proyecta.Services.Risk;

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
                Message = $"The item with id '{id}' was not found or you don't have permission to access it."
            };
        }

        return new ApplicationResult
        {
            Success = true,
            Code = "200",
            Data = item
        };
    }

    public async Task<ApplicationResult> Create(RiskCategoryCreateOrUpdateDto item, string currentUserId)
    {
        var newItem = MapDtoToEntity(item, currentUserId);

        await _repository.Create(newItem);

        return new ApplicationResult
        {
            Success = true,
            Code = "201",
            Message = "Risk Category created successfully.",
            Data = new GenericEntityCreationResult { Id = newItem.Id }
        };
    }

    public async Task<ApplicationResult> Update(Guid id, RiskCategoryCreateOrUpdateDto item, string currentUserId)
    {
        var itemToUpdate = MapDtoToEntity(item, currentUserId);
        itemToUpdate.Id = id;

        await _repository.Update(itemToUpdate);

        return new ApplicationResult
        {
            Success = true,
            Code = "204",
            Message = "Item updated successfully.",
        };
    }

    public async Task<ApplicationResult> Remove(Guid id, string currentUserId)
    {
        await _repository.Remove(id);

        return new ApplicationResult
        {
            Success = true,
            Code = "204",
            Message = "Item deleted successfully.",
        };
    }

    public async Task<ApplicationResult> AddRange(IEnumerable<RiskCategoryCreateOrUpdateDto> items,
        string currentUserId)
    {
        var newItems = items.Select(item => MapDtoToEntity(item, currentUserId)).ToList();

        await _repository.AddRange(newItems);

        return new ApplicationResult
        {
            Success = true,
            Code = "204",
            Message = "Items added successfully.",
        };
    }
    
    private RiskCategory MapDtoToEntity(RiskCategoryCreateOrUpdateDto item, string currentUserId)
    {
        var now = DateTime.UtcNow;

        var entity = new RiskCategory
        {
            Name = item.Name,
            CreatedAt = now,
            CreatedById = currentUserId,
            UpdatedAt = now,
            UpdatedById = currentUserId
        };

        return entity;
    }
}