using Proyecta.Core.Entities;

namespace Proyecta.Core.Contracts.Repositories;

public interface IRiskTreatmentRepository : IRepositoryBase<RiskTreatment, Guid>
{
    public Task AddRange(IEnumerable<RiskTreatment> items);
}