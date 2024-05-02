using Proyecta.Core.DTOs;
using Proyecta.Core.Entities.Risk;

namespace Proyecta.Core.Contracts.Repositories.Risk;

public interface IRiskTreatmentRepository : IIdNameRepository<Guid, RiskTreatment, IEnumerable<GenericEntityListDto>,
    GenericEntityDetailDto<Guid>>
{
}