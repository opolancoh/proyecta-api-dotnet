using Proyecta.Core.Contracts.Repositories.Risk;
using Proyecta.Core.Contracts.Services;
using Proyecta.Core.DTOs.ApiResponses;
using Proyecta.Core.DTOs.IdName;
using Proyecta.Core.DTOs.Risk;
using Proyecta.Core.Entities.Risk;

namespace Proyecta.Services.Risk;

public sealed class RiskService : IRiskService
{
    private readonly IRiskRepository _repository;

    public RiskService(IRiskRepository repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse> GetAll()
    {
        var items = await _repository.GetAll();

        return new ApiResponse
        {
            Status = ApiStatusResponse.Ok,
            Body = new ApiBody<IEnumerable<RiskListDto>>()
            {
                Data = items
            }
        };
    }

    public async Task<ApiResponse> GetById(Guid id)
    {
        var item = await _repository.GetById(id);

        if (item == null)
        {
            return new ApiResponse
            {
                Status = ApiStatusResponse.NotFound,
                Body = new ApiBody
                {
                    Message =
                        $"The requested resource with ID '{id}' was not found, or you don't have permission to access it."
                }
            };
        }

        return new ApiResponse
        {
            Status = ApiStatusResponse.Ok,
            Body = new ApiBody<RiskDetailDto>
            {
                Data = item
            }
        };
    }

    public async Task<ApiResponse> Create(RiskAddOrUpdateDto item, string currentUserId)
    {
        var newItem = MapDtoToEntity(item, currentUserId);

        var result = await _repository.Add(newItem);

        if (result == 0)
        {
            return new ApiResponse
            {
                Status = ApiStatusResponse.Conflict,
                Body = new ApiBody<IdNameDetailDto<Guid>>
                {
                    Message = "The resource could not be created, or you do not have permission to create it."
                }
            };
        }

        return new ApiResponse
        {
            Status = ApiStatusResponse.Created,
            Body = new ApiBody<ApiGenericAddResponse<Guid>>
            {
                Data = new ApiGenericAddResponse<Guid> { Id = newItem.Id }
            }
        };
    }

    public async Task<ApiResponse> Update(Guid id, RiskAddOrUpdateDto item, string currentUserId)
    {
        var itemToUpdate = MapDtoToEntity(item, currentUserId);
        itemToUpdate.Id = id;

        var result = await _repository.Update(itemToUpdate);

        if (result == 0)
        {
            return new ApiResponse
            {
                Status = ApiStatusResponse.Conflict,
                Body = new ApiBody<IdNameDetailDto<Guid>>
                {
                    Message =
                        $"The resource with ID '{id}' could not be updated, or you don't have permission to update it."
                }
            };
        }

        return new ApiResponse
        {
            Status = ApiStatusResponse.NoContent,
            Body = new ApiBody
            {
                Message = "The resource was updated successfully."
            }
        };
    }

    public async Task<ApiResponse> Remove(Guid id, string currentUserId)
    {
        var result = await _repository.Remove(id);

        if (result == 0)
        {
            return new ApiResponse
            {
                Status = ApiStatusResponse.Conflict,
                Body = new ApiBody
                {
                    Message =
                        $"The requested resource with ID '{id}' was not found, or you don't have permission to access it."
                }
            };
        }

        return new ApiResponse
        {
            Status = ApiStatusResponse.NoContent,
            Body = new ApiBody
            {
                Message = "The resource was deleted successfully."
            }
        };
    }

    public async Task<ApiResponse> AddRange(IEnumerable<RiskAddOrUpdateDto> items, string currentUserId)
    {
        var newItems = items.Select(item => MapDtoToEntity(item, currentUserId)).ToList();

        var result = await _repository.AddRange(newItems);

        if (result != newItems.Count)
        {
            return new ApiResponse
            {
                Status = ApiStatusResponse.MultiStatus,
                Body = new ApiBody
                {
                    Message = "Some resources could not be processed successfully."
                }
            };
        }

        return new ApiResponse
        {
            Status = ApiStatusResponse.NoContent,
            Body = new ApiBody
            {
                Message = "All resources have been successfully added."
            }
        };
    }

    private Core.Entities.Risk.Risk MapDtoToEntity(RiskAddOrUpdateDto item, string currentUserId)
    {
        var now = DateTime.UtcNow;

        var entity = new Core.Entities.Risk.Risk
        {
            Name = item.Name!,
            Code = item.Code!,
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
