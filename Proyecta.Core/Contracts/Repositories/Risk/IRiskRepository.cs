using Proyecta.Core.DTOs.Risk;

namespace Proyecta.Core.Contracts.Repositories.Risk;

public interface IRiskRepository
{
    Task<IEnumerable<RiskListDto>> GetAll();
    Task<RiskDetailDto?> GetById(Guid id);
    Task Create(Proyecta.Core.Entities.Risk.Risk item);
    Task Update(Proyecta.Core.Entities.Risk.Risk item);
    Task Remove(Guid id);
    Task<bool> ItemExists(Guid id);
    public Task AddRange(IEnumerable<Proyecta.Core.Entities.Risk.Risk> items);
}