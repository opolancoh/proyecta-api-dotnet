using Proyecta.Core.Entities;

namespace Proyecta.Core.Contracts.Repositories;

public interface IRiskRepository : IRepositoryBase<Risk, Guid>
{
    public Task AddRange(IEnumerable<Risk> items);
}