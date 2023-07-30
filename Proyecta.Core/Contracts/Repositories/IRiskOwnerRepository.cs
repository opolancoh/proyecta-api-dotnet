using Proyecta.Core.Entities;

namespace Proyecta.Core.Contracts.Repositories;

public interface IRiskOwnerRepository : IRepositoryBase<RiskOwner, Guid>
{
    public Task AddRange(IEnumerable<RiskOwner> items);
}