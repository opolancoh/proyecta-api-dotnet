using Proyecta.Core.DTOs;
using Proyecta.Core.Entities.Risk;

namespace Proyecta.Core.Contracts.Repositories.Risk;

public interface IRiskCategoryRepository : IIdNameRepository<Guid, RiskCategory, IEnumerable<GenericEntityListDto>,
    GenericEntityDetailDto<Guid>>
{
}