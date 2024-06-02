using Proyecta.Core.Contracts.Repositories.Risk;
using Proyecta.Core.Contracts.Services;
using Proyecta.Core.DTOs.ApiResponses;
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

    public async Task<ApiResponse> GetAll()
    {
        var items = await _repository.GetAll();

        return new ApiResponse
        {
            Status = ApiStatusResponse.Ok,
            Body = new ApiBody<IEnumerable<IdNameListDto<Guid>>>()
            {
                Data = items
            }
        };
    }

    public async Task<ApiResponse> GetById(Guid id)
    {
        var result = await _repository.GetById(id);

        if (result == null)
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
            Body = new ApiBody<IdNameDetailDto<Guid>>
            {
                Data = result
            }
        };
    }

    public async Task<ApiResponse> Add(IdNameAddOrUpdateDto item, string currentUserId)
    {
        var newItem = MapDtoToEntity(item, currentUserId);

        var result = await _repository.Create(newItem);

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

    public async Task<ApiResponse> Update(Guid id, IdNameAddOrUpdateDto item, string currentUserId)
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

    public async Task<ApiResponse> AddRange(IEnumerable<IdNameAddOrUpdateDto> items, string currentUserId)
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

    private RiskOwner MapDtoToEntity(IdNameAddOrUpdateDto item, string currentUserId)
    {
        var now = DateTime.UtcNow;

        var entity = new RiskOwner
        {
            Name = item.Name!,
            CreatedAt = now,
            CreatedById = currentUserId,
            UpdatedAt = now,
            UpdatedById = currentUserId
        };

        return entity;
    }
}
