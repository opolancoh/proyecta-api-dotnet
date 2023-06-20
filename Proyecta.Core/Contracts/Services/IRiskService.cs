using Proyecta.Core.Entities;
using Proyecta.Core.Entities.DTOs;
using Proyecta.Core.Models;

namespace Proyecta.Core.Contracts.Services;

public interface IRiskService: IServiceBase<ApplicationResult, Guid, RiskCreateOrUpdateDto>
{
    public Task<ApplicationResult> AddRange(IEnumerable<RiskCreateOrUpdateDto> items);
}