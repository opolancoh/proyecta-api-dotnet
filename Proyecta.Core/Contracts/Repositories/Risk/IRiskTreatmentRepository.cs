using Proyecta.Core.DTOs;
using Proyecta.Core.DTOs.IdName;
using Proyecta.Core.Entities.Risk;

namespace Proyecta.Core.Contracts.Repositories.Risk;

public interface IRiskTreatmentRepository : IIdNameRepository<Guid, RiskTreatment, IEnumerable<IdNameListDto<Guid>>,
    IdNameDetailDto<Guid>>
{
}