using Proyecta.Core.DTOs;
using Proyecta.Core.Models;

namespace Proyecta.Core.Contracts.Services;

public interface IRiskTreatmentService: IServiceBase<ApplicationResult, Guid, RiskTreatmentCreateOrUpdateDto>
{
    public Task<ApplicationResult> AddRange(IEnumerable<RiskTreatmentCreateOrUpdateDto> items);
}