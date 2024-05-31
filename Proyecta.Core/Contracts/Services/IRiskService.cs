using Proyecta.Core.DTOs.ApiResponse;
using Proyecta.Core.DTOs.Risk;

namespace Proyecta.Core.Contracts.Services;

public interface IRiskService
{
    Task<IApiResponse> GetAll();
    Task<IApiResponse> GetById(Guid id);
    Task<IApiResponse> Create(RiskAddOrUpdateDto item, string currentUserId);
    Task<IApiResponse> Update(Guid id, RiskAddOrUpdateDto item, string currentUserId);
    Task<IApiResponse> Remove(Guid id, string currentUserId);
    public Task<IApiResponse> AddRange(IEnumerable<RiskAddOrUpdateDto> items, string currentUserId);
}
