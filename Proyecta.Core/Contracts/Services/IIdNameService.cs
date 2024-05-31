using Proyecta.Core.DTOs;
using Proyecta.Core.DTOs.ApiResponse;
using Proyecta.Core.DTOs.IdName;

namespace Proyecta.Core.Contracts.Services;

public interface IIdNameService<in TKey>
{
    Task<IApiResponse> GetAll();

    Task<IApiResponse> GetById(TKey id);

    Task<IApiResponse> Add(IdNameAddOrUpdateDto item, string currentUserId);

    Task<IApiResponse> Update(TKey id, IdNameAddOrUpdateDto item, string currentUserId);

    Task<IApiResponse> Remove(TKey id, string currentUserId);

    Task<IApiResponse> AddRange(IEnumerable<IdNameAddOrUpdateDto> items, string currentUserId);
}
