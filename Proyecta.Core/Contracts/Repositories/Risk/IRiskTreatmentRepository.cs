using Proyecta.Core.DTOs;
using Proyecta.Core.Entities.Risk;

namespace Proyecta.Core.Contracts.Repositories.Risk;

public interface IRiskTreatmentRepository : IRepositoryBase<RiskTreatment, Guid, IEnumerable<IdNameDto<Guid>>,
    IdNameAuditableDto<Guid>>
{
    public Task AddRange(IEnumerable<RiskTreatment> items);
}