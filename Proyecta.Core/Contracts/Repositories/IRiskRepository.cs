using Proyecta.Core.DTOs;
using Proyecta.Core.Entities;

namespace Proyecta.Core.Contracts.Repositories;

public interface IRiskRepository 
{
    Task<IEnumerable<RiskDto>> GetAll();
    Task<RiskDto?> GetById(Guid id);
    Task Create(Risk item);
    Task Update(Risk item);
    Task Remove(Guid id);
    Task<bool> ItemExists(Guid id);
    public Task AddRange(IEnumerable<Risk> items);
}