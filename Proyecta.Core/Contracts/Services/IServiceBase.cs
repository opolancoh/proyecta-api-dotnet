namespace Proyecta.Core.Contracts.Services;

public interface IServiceBase<TEntity, in TCreateOrUpdate>
{
    Task<IEnumerable<TEntity>> GetAll();
    Task<TEntity?> GetById(Guid id);
    Task<Guid> Create(TCreateOrUpdate item);
    Task Update(Guid id, TCreateOrUpdate item);
    Task Remove(Guid id);
}