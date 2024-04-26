using Proyecta.Core.DTOs;
using Proyecta.Core.DTOs.Risk;
using Proyecta.Core.Results;

namespace Proyecta.Core.Contracts.Services;

public interface IRiskOwnerService: IAuditableServiceBase<ApplicationResult, Guid, RiskOwnerCreateOrUpdateDto>
{
    public Task<ApplicationResult> AddRange(IEnumerable<RiskOwnerCreateOrUpdateDto> items, string currentUserId);
}