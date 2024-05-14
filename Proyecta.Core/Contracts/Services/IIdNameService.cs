using Proyecta.Core.DTOs;
using Proyecta.Core.DTOs.ApiResponse;
using Proyecta.Core.DTOs.IdName;

namespace Proyecta.Core.Contracts.Services;

public interface IIdNameService<TKey>
{
    Task<ApiResponse<IEnumerable<IdNameListDto<Guid>>>> GetAll();
    
    Task<ApiResponse<IdNameDetailDto<TKey>>> GetById(TKey id);
    
    Task<ApiResponse<ApiResponseGenericAdd<TKey>>> Create(IdNameAddOrUpdateDto item, string currentUserId);
    
    Task<ApiResponse> Update(TKey id, IdNameAddOrUpdateDto item, string currentUserId);
    
    Task<ApiResponse> Remove(TKey id, string currentUserId);
    
    Task<ApiResponse<IEnumerable<ApiResponseGenericAdd<TKey>>>> AddRange(
        IEnumerable<IdNameAddOrUpdateDto> items,
        string currentUserId);
}