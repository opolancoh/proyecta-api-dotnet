using Proyecta.Core.DTOs.Risk;

namespace Proyecta.Core.Contracts.Repositories.Risk;

public interface IRiskRepository
{
    Task<IEnumerable<RiskListDto>> GetAll();
    Task<RiskDetailDto?> GetById(Guid id);
    Task<int> Create(Proyecta.Core.Entities.Risk.Risk item);
    Task<int> Update(Proyecta.Core.Entities.Risk.Risk item);
    Task<int> Remove(Guid id);
    public Task<int> AddRange(IEnumerable<Proyecta.Core.Entities.Risk.Risk> items);
}
