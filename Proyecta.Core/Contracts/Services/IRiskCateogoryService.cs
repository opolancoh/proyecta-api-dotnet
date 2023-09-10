using Proyecta.Core.DTOs;
using Proyecta.Core.Results;

namespace Proyecta.Core.Contracts.Services;

public interface IRiskCategoryService: IServiceBase<ApplicationResult, Guid, RiskCategoryCreateOrUpdateDto>
{
    public Task<ApplicationResult> AddRange(IEnumerable<RiskCategoryCreateOrUpdateDto> items);
}