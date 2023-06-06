using Proyecta.Core.Entities;
using Proyecta.Core.Entities.DTOs;

namespace Proyecta.Core.Contracts.Services;

public interface IRiskService: IServiceBase<Risk, Guid, RiskCreateOrUpdateDto>
{
    public Task AddRange(IEnumerable<RiskCreateOrUpdateDto> items);
}