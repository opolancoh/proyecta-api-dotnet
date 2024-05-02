using Proyecta.Core.Contracts.Repositories.Risk;
using Proyecta.Core.Contracts.Services;
using Proyecta.Core.DTOs;
using Proyecta.Core.DTOs.Risk;
using Proyecta.Core.Entities.Risk;
using Proyecta.Core.Responses;

namespace Proyecta.Services.Risk;

public sealed class RiskCategoryService : IRiskCategoryService
{
    private readonly IRiskCategoryRepository _repository;

    public RiskCategoryService(IRiskCategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<IEnumerable<GenericEntityListDto>>> GetAll()
    {
        var items = await _repository.GetAll();

        return new ApiResponse<IEnumerable<GenericEntityListDto>>
        {
            Success = true,
            Code = "200",
            Data = items
        };
    }

    public async Task<ApiResponse<GenericEntityDetailDto<Guid>>> GetById(Guid id)
    {
        var item = await _repository.GetById(id);

        if (item == null)
        {
            return new ApiResponse<GenericEntityDetailDto<Guid>>
            {
                Success = false,
                Code = "404",
                Message = $"The item with id '{id}' was not found or you don't have permission to access it."
            };
        }

        return new ApiResponse<GenericEntityDetailDto<Guid>>
        {
            Success = true,
            Code = "200",
            Data = item
        };
    }

    public async Task<ApiResponse<ApiCreateResponse<Guid>>> Create(GenericEntityCreateOrUpdateDto item,
        string currentUserId)
    {
        var newItem = MapDtoToEntity(item, currentUserId);

        await _repository.Create(newItem);

        return new ApiResponse<ApiCreateResponse<Guid>>
        {
            Success = true,
            Code = "201",
            Message = "Risk Category created successfully.",
            Data = new ApiCreateResponse<Guid> { Id = newItem.Id }
        };
    }

    public async Task<ApiResponse> Update(Guid id, GenericEntityCreateOrUpdateDto item, string currentUserId)
    {
        var itemToUpdate = MapDtoToEntity(item, currentUserId);
        itemToUpdate.Id = id;

        await _repository.Update(itemToUpdate);

        return new ApiResponse
        {
            Success = true,
            Code = "204",
            Message = "Item updated successfully.",
        };
    }

    public async Task<ApiResponse> Remove(Guid id, string currentUserId)
    {
        await _repository.Remove(id);

        return new ApiResponse
        {
            Success = true,
            Code = "204",
            Message = "Item deleted successfully.",
        };
    }

    public async Task<ApiResponse<IEnumerable<ApiCreateResponse<Guid>>>> AddRange(
        IEnumerable<GenericEntityCreateOrUpdateDto> items,
        string currentUserId)
    {
        var newItems = items.Select(item => MapDtoToEntity(item, currentUserId)).ToList();

        await _repository.AddRange(newItems);

        return new ApiResponse<IEnumerable<ApiCreateResponse<Guid>>>
        {
            Success = true,
            Code = "204",
            Message = "Items added successfully.",
        };
    }

    private RiskCategory MapDtoToEntity(GenericEntityCreateOrUpdateDto item, string currentUserId)
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