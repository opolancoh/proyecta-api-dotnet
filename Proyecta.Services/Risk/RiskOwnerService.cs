using Proyecta.Core.Contracts.Repositories.Risk;
using Proyecta.Core.Contracts.Services;
using Proyecta.Core.DTOs;
using Proyecta.Core.DTOs.ApiResponse;
using Proyecta.Core.DTOs.IdName;
using Proyecta.Core.Entities.Risk;

namespace Proyecta.Services.Risk;

public sealed class RiskOwnerService : IRiskOwnerService
{
    private readonly IRiskOwnerRepository _repository;

    public RiskOwnerService(IRiskOwnerRepository repository)
    {
        _repository = repository;
    }

     public async Task<ApiResponse<IEnumerable<IdNameListDto<Guid>>>> GetAll()
    {
        var items = await _repository.GetAll();

        return new ApiResponse<IEnumerable<IdNameListDto<Guid>>>
        {
            Success = true,
            Code = ApiResponseCode.Ok,
            Data = items
        };
    }

    public async Task<ApiResponse<IdNameDetailDto<Guid>>> GetById(Guid id)
    {
        var item = await _repository.GetById(id);

        if (item == null)
        {
            return new ApiResponse<IdNameDetailDto<Guid>>
            {
                Success = false,
                Code = ApiResponseCode.NotFound,
                Message = $"The item with id '{id}' was not found or you don't have permission to access it."
            };
        }

        return new ApiResponse<IdNameDetailDto<Guid>>
        {
            Success = true,
            Code = ApiResponseCode.Ok,
            Data = item
        };
    }

    public async Task<ApiResponse<ApiResponseGenericAdd<Guid>>> Create(IdNameAddOrUpdateDto item, string currentUserId)
    {
        var newItem = MapDtoToEntity(item, currentUserId);

        await _repository.Create(newItem);

        return new ApiResponse<ApiResponseGenericAdd<Guid>>
        {
            Success = true,
            Code = ApiResponseCode.Created,
            Message = "Risk Category created successfully.",
            Data = new ApiResponseGenericAdd<Guid> { Id = newItem.Id }
        };
    }

    public async Task<ApiResponse> Update(Guid id, IdNameAddOrUpdateDto item, string currentUserId)
    {
        var itemToUpdate = MapDtoToEntity(item, currentUserId);
        itemToUpdate.Id = id;

        await _repository.Update(itemToUpdate);

        return new ApiResponse
        {
            Success = true,
            Code = ApiResponseCode.NoContent,
            Message = "Item updated successfully.",
        };
    }

    public async Task<ApiResponse> Remove(Guid id, string currentUserId)
    {
        await _repository.Remove(id);

        return new ApiResponse
        {
            Success = true,
            Code = ApiResponseCode.NoContent,
            Message = "Item deleted successfully.",
        };
    }

    public async Task<ApiResponse<IEnumerable<ApiResponseGenericAdd<Guid>>>> AddRange(IEnumerable<IdNameAddOrUpdateDto> items,
        string currentUserId)
    {
        var newItems = items.Select(item => MapDtoToEntity(item, currentUserId)).ToList();

        await _repository.AddRange(newItems);

        return new ApiResponse<IEnumerable<ApiResponseGenericAdd<Guid>>>
        {
            Success = true,
            Code = ApiResponseCode.NoContent,
            Message = "Items added successfully.",
        };
    }
    
    private RiskOwner MapDtoToEntity(IdNameAddOrUpdateDto item, string currentUserId)
    {
        var now = DateTime.UtcNow;

        var entity = new RiskOwner
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