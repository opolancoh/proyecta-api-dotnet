using Proyecta.Core.DTOs;
using Proyecta.Core.DTOs.Risk;
using Proyecta.Core.Results;

namespace Proyecta.Core.Contracts.Services;

public interface IRiskTreatmentService: IAuditableServiceBase<ApplicationResult, Guid, RiskTreatmentCreateOrUpdateDto>
{
    public Task<ApplicationResult> AddRange(IEnumerable<RiskTreatmentCreateOrUpdateDto> items, string currentUserId);
}