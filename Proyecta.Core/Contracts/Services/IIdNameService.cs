using Proyecta.Core.DTOs;
using Proyecta.Core.DTOs.ApiResponses;
using Proyecta.Core.DTOs.IdName;

namespace Proyecta.Core.Contracts.Services;

public interface IIdNameService<in TKey>
{
    Task<ApiResponse> GetAll();

    Task<ApiResponse> GetById(TKey id);

    Task<ApiResponse> Add(IdNameAddOrUpdateDto item, string currentUserId);

    Task<ApiResponse> Update(TKey id, IdNameAddOrUpdateDto item, string currentUserId);

    Task<ApiResponse> Remove(TKey id, string currentUserId);

    Task<ApiResponse> AddRange(IEnumerable<IdNameAddOrUpdateDto> items, string currentUserId);
}
