namespace Proyecta.Core.Contracts.Services;

public interface IServiceBase<TEntity, TKey, in TCreateOrUpdate>
{
    Task<IEnumerable<TEntity>> GetAll();
    Task<TEntity?> GetById(TKey id);
    Task<TKey> Create(TCreateOrUpdate item);
    Task Update(TKey id, TCreateOrUpdate item);
    Task Remove(TKey id);
}