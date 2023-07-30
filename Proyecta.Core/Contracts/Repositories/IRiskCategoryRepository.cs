using Proyecta.Core.Entities;

namespace Proyecta.Core.Contracts.Repositories;

public interface IRiskCategoryRepository : IRepositoryBase<RiskCategory, Guid>
{
    public Task AddRange(IEnumerable<RiskCategory> items);
}