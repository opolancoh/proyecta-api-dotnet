using Proyecta.Core.DTOs;
using Proyecta.Core.DTOs.Risk;
using Proyecta.Core.Results;

namespace Proyecta.Core.Contracts.Services;

public interface IRiskCategoryService: IAuditableServiceBase<ApplicationResult, Guid, RiskCategoryCreateOrUpdateDto>
{
    public Task<ApplicationResult> AddRange(IEnumerable<RiskCategoryCreateOrUpdateDto> items, string currentUserId);
}