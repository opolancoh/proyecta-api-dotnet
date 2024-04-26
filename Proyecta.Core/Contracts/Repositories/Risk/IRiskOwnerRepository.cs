using Proyecta.Core.DTOs;
using Proyecta.Core.Entities.Risk;

namespace Proyecta.Core.Contracts.Repositories.Risk;

public interface IRiskOwnerRepository : IRepositoryBase<RiskOwner, Guid, IEnumerable<IdNameDto<Guid>>,
    IdNameAuditableDto<Guid>>
{
    public Task AddRange(IEnumerable<RiskOwner> items);
}