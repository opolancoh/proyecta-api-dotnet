using Proyecta.Core.DTOs;
using Proyecta.Core.Responses;

namespace Proyecta.Core.Contracts.Services;

public interface IIdNameService<TKey>
{
    Task<ApiResponse<IEnumerable<GenericEntityListDto>>> GetAll();
    
    Task<ApiResponse<GenericEntityDetailDto<TKey>>> GetById(TKey id);
    
    Task<ApiResponse<ApiCreateResponse<TKey>>> Create(GenericEntityCreateOrUpdateDto item, string currentUserId);
    
    Task<ApiResponse> Update(TKey id, GenericEntityCreateOrUpdateDto item, string currentUserId);
    
    Task<ApiResponse> Remove(TKey id, string currentUserId);
    
    Task<ApiResponse<IEnumerable<ApiCreateResponse<TKey>>>> AddRange(
        IEnumerable<GenericEntityCreateOrUpdateDto> items,
        string currentUserId);
}