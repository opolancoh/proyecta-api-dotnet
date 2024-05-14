using Proyecta.Core.DTOs.ApiResponse;
using Proyecta.Core.DTOs.Risk;

namespace Proyecta.Core.Contracts.Services;

public interface IRiskService
{
    Task<ApiResponse<IEnumerable<RiskListDto>>> GetAll();
    Task<ApiResponse<RiskDetailDto>> GetById(Guid id);
    Task<ApiResponse<ApiResponseGenericAdd<Guid>>> Create(RiskAddOrUpdateDto item, string currentUserId);
    Task<ApiResponse> Update(Guid id, RiskAddOrUpdateDto item, string currentUserId);
    Task<ApiResponse> Remove(Guid id, string currentUserId);
    public Task<ApiResponse<IEnumerable<ApiResponseGenericAdd<Guid>>>> AddRange(IEnumerable<RiskAddOrUpdateDto> items,
        string currentUserId);
}