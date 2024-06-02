using Proyecta.Core.DTOs.ApiResponses;
using Proyecta.Core.DTOs.Risk;

namespace Proyecta.Core.Contracts.Services;

public interface IRiskService
{
    Task<ApiResponse> GetAll();
    Task<ApiResponse> GetById(Guid id);
    Task<ApiResponse> Create(RiskAddOrUpdateDto item, string currentUserId);
    Task<ApiResponse> Update(Guid id, RiskAddOrUpdateDto item, string currentUserId);
    Task<ApiResponse> Remove(Guid id, string currentUserId);
    public Task<ApiResponse> AddRange(IEnumerable<RiskAddOrUpdateDto> items, string currentUserId);
}
