using Proyecta.Core.DTOs;
using Proyecta.Core.Results;

namespace Proyecta.Core.Contracts.Services;

public interface IRiskTreatmentService: IServiceBase<ApplicationResult, Guid, RiskTreatmentCreateOrUpdateDto>
{
    public Task<ApplicationResult> AddRange(IEnumerable<RiskTreatmentCreateOrUpdateDto> items);
}