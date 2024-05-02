using Proyecta.Core.DTOs.Risk;
using Proyecta.Core.Responses;

namespace Proyecta.Core.Contracts.Services;

public interface IRiskService
{
    Task<ApiResponse<IEnumerable<RiskListDto>>> GetAll();
    Task<ApiResponse<RiskListDto>> GetById(Guid id);
    Task<ApiResponse<ApiCreateResponse<Guid>>> Create(RiskCreateOrUpdateDto item, string currentUserId);
    Task<ApiResponse> Update(Guid id, RiskCreateOrUpdateDto item, string currentUserId);
    Task<ApiResponse> Remove(Guid id, string currentUserId);
    public Task<ApiResponse<IEnumerable<ApiCreateResponse<Guid>>>> AddRange(IEnumerable<RiskCreateOrUpdateDto> items,
        string currentUserId);
}