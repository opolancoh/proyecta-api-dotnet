using Proyecta.Core.DTOs;
using Proyecta.Core.Models;

namespace Proyecta.Core.Contracts.Services;

public interface IRiskService: IServiceBase<ApplicationResult, Guid, RiskCreateOrUpdateDto>
{
    public Task<ApplicationResult> AddRange(IEnumerable<RiskCreateOrUpdateDto> items);
}