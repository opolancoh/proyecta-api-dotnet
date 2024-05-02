using Proyecta.Core.DTOs;
using Proyecta.Core.Entities.Risk;

namespace Proyecta.Core.Contracts.Repositories.Risk;

public interface IRiskOwnerRepository : IIdNameRepository<Guid, RiskOwner, IEnumerable<GenericEntityListDto>,
    GenericEntityDetailDto<Guid>>
{
}