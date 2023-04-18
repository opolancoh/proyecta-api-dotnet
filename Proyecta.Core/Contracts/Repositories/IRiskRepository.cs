using Proyecta.Core.Entities;
using Proyecta.Core.Entities.DTOs;

namespace Proyecta.Core.Contracts.Repositories;

public interface IRiskRepository : IRepositoryBase<Risk>
{
    public Task AddRange(IEnumerable<Risk> items);
}