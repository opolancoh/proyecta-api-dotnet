using Proyecta.Core.Contracts.Repositories.Risk;
using Proyecta.Core.Contracts.Services;
using Proyecta.Core.DTOs.Risk;
using Proyecta.Core.Entities.Risk;
using Proyecta.Core.Responses;

namespace Proyecta.Services.Risk;

public sealed class RiskService : IRiskService
{
    private readonly IRiskRepository _repository;

    public RiskService(IRiskRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<IEnumerable<RiskListDto>>> GetAll()
    {
        var items = await _repository.GetAll();

        return new ApiResponse<IEnumerable<RiskListDto>>
        {
            Success = true,
            Code = ApiResponseCode.OK,
            Data = items
        };
    }

    public async Task<ApiResponse<RiskDetailDto>> GetById(Guid id)
    {
        var item = await _repository.GetById(id);

        if (item == null)
        {
            return new ApiResponse<RiskDetailDto>
            {
                Success = false,
                Code = ApiResponseCode.NotFound,
                Message = $"The entity with id '{id}' doesn't exist in the database."
            };
        }

        return new ApiResponse<RiskDetailDto>
        {
            Success = true,
            Code = ApiResponseCode.OK,
            Data = item
        };
    }

    public async Task<ApiResponse<ApiCreateResponse<Guid>>> Create(RiskCreateOrUpdateDto item, string currentUserId)
    {
        var newItem = MapDtoToEntity(item, currentUserId);

        await _repository.Create(newItem);

        return new ApiResponse<ApiCreateResponse<Guid>>
        {
            Success = true,
            Code = ApiResponseCode.Created,
            Message = "Risk created successfully.",
            Data = new ApiCreateResponse<Guid> { Id = newItem.Id }
        };
    }

    public async Task<ApiResponse> Update(Guid id, RiskCreateOrUpdateDto item, string currentUserId)
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

    public async Task<ApiResponse<IEnumerable<ApiCreateResponse<Guid>>>> AddRange(IEnumerable<RiskCreateOrUpdateDto> items, string currentUserId)
    {
        var newItems = items.Select(item => MapDtoToEntity(item, currentUserId)).ToList();

        await _repository.AddRange(newItems);

        return new ApiResponse<IEnumerable<ApiCreateResponse<Guid>>>
        {
            Success = true,
            Code = ApiResponseCode.NoContent,
            Message = "Items added successfully.",
        };
    }

    private Core.Entities.Risk.Risk MapDtoToEntity(RiskCreateOrUpdateDto item, string currentUserId)
    {
        var now = DateTime.UtcNow;

        var entity = new Core.Entities.Risk.Risk
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
            CreatedAt = now,
            CreatedById = currentUserId,
            UpdatedAt = now,
            UpdatedById = currentUserId
        };

        return entity;
    }
}