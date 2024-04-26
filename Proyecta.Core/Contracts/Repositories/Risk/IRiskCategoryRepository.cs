using Proyecta.Core.DTOs;
using Proyecta.Core.Entities.Risk;

namespace Proyecta.Core.Contracts.Repositories.Risk;

public interface
    IRiskCategoryRepository : IRepositoryBase<RiskCategory, Guid, IEnumerable<IdNameDto<Guid>>,
    IdNameAuditableDto<Guid>>
{
    Task AddRange(IEnumerable<RiskCategory> items);
}