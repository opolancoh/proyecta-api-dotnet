using Proyecta.Core.DTOs;
using Proyecta.Core.DTOs.Risk;
using Proyecta.Core.Results;

namespace Proyecta.Core.Contracts.Services;

public interface IRiskService : IAuditableServiceBase<ApplicationResult, Guid, RiskCreateOrUpdateDto>
{
    public Task<ApplicationResult> AddRange(IEnumerable<RiskCreateOrUpdateDto> items, string currentUserId);
}