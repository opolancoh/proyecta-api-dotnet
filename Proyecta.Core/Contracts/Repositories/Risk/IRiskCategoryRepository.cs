using Proyecta.Core.DTOs;
using Proyecta.Core.DTOs.IdName;
using Proyecta.Core.Entities.Risk;

namespace Proyecta.Core.Contracts.Repositories.Risk;

public interface IRiskCategoryRepository : IIdNameRepository
<
    Guid,
    RiskCategory,
    IEnumerable<IdNameListDto<Guid>>,
    IdNameDetailDto<Guid>
>
{
}