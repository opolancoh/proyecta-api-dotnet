using Proyecta.Core.Contracts.Repositories.Risk;
using Proyecta.Core.Contracts.Services;
using Proyecta.Core.DTOs.ApiResponse;
using Proyecta.Core.DTOs.IdName;
using Proyecta.Core.Entities.Risk;

namespace Proyecta.Services.Risk;

public sealed class RiskCategoryService : IRiskCategoryService
{
    private readonly IRiskCategoryRepository _repository;

    public RiskCategoryService(IRiskCategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<IApiResponse> GetAll()
    {
        var items = await _repository.GetAll();

        return new ApiResponse
        {
            Status = ApiResponseStatus.Ok,
            Body = new ApiBody<IEnumerable<IdNameListDto<Guid>>>()
            {
                Data = items
            }
        };
    }

    public async Task<IApiResponse> GetById(Guid id)
    {
        var result = await _repository.GetById(id);

        if (result == null)
        {
            return new ApiResponse
            {
                Status = ApiResponseStatus.NotFound,
                Body = new ApiBody
                {
                    Message =
                        $"The requested resource with ID '{id}' was not found, or you don't have permission to access it."
                }
            };
        }

        return new ApiResponse
        {
            Status = ApiResponseStatus.Ok,
            Body = new ApiBody<IdNameDetailDto<Guid>>
            {
                Data = result
            }
        };
    }

    public async Task<IApiResponse> Add(IdNameAddOrUpdateDto item, string currentUserId)
    {
        var newItem = MapDtoToEntity(item, currentUserId);

        var result = await _repository.Create(newItem);

        if (result == 0)
        {
            return new ApiResponse
            {
                Status = ApiResponseStatus.Conflict,
                Body = new ApiBody<IdNameDetailDto<Guid>>
                {
                    Message = "The resource could not be created, or you do not have permission to create it."
                }
            };
        }

        return new ApiResponse
        {
            Status = ApiResponseStatus.Created,
            Body = new ApiBody<ApiResponseGenericAdd<Guid>>
            {
                Data = new ApiResponseGenericAdd<Guid> { Id = newItem.Id }
            }
        };
    }

    public async Task<IApiResponse> Update(Guid id, IdNameAddOrUpdateDto item, string currentUserId)
    {
        var itemToUpdate = MapDtoToEntity(item, currentUserId);
        itemToUpdate.Id = id;

        var result = await _repository.Update(itemToUpdate);

        if (result == 0)
        {
            return new ApiResponse
            {
                Status = ApiResponseStatus.Conflict,
                Body = new ApiBody<IdNameDetailDto<Guid>>
                {
                    Message =
                        $"The resource with ID '{id}' could not be updated, or you don't have permission to update it."
                }
            };
        }

        return new ApiResponse
        {
            Status = ApiResponseStatus.NoContent,
            Body = new ApiBody
            {
                Message = "The resource was updated successfully."
            }
        };
    }

    public async Task<IApiResponse> Remove(Guid id, string currentUserId)
    {
        var result = await _repository.Remove(id);

        if (result == 0)
        {
            return new ApiResponse
            {
                Status = ApiResponseStatus.Conflict,
                Body = new ApiBody
                {
                    Message =
                        $"The requested resource with ID '{id}' was not found, or you don't have permission to access it."
                }
            };
        }

        return new ApiResponse
        {
            Status = ApiResponseStatus.NoContent,
            Body = new ApiBody
            {
                Message = "The resource was deleted successfully."
            }
        };
    }

    public async Task<IApiResponse> AddRange(IEnumerable<IdNameAddOrUpdateDto> items, string currentUserId)
    {
        var newItems = items.Select(item => MapDtoToEntity(item, currentUserId)).ToList();

        var result = await _repository.AddRange(newItems);

        if (result != newItems.Count)
        {
            return new ApiResponse
            {
                Status = ApiResponseStatus.MultiStatus,
                Body = new ApiBody
                {
                    Message = "Some resources could not be processed successfully."
                }
            };
        }

        return new ApiResponse
        {
            Status = ApiResponseStatus.NoContent,
            Body = new ApiBody
            {
                Message = "All resources have been successfully added."
            }
        };
    }

    private RiskCategory MapDtoToEntity(IdNameAddOrUpdateDto item, string currentUserId)
    {
        var now = DateTime.UtcNow;

        var entity = new RiskCategory
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
