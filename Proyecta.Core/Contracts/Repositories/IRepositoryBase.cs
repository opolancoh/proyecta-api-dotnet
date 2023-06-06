namespace Proyecta.Core.Contracts.Repositories;

public interface IRepositoryBase<TEntity, in TKey>
{
    Task<IEnumerable<TEntity>> GetAll();
    Task<TEntity?> GetById(TKey id);
    Task Create(TEntity item);
    Task Update(TEntity item);
    Task Remove(TKey id);
    Task<bool> ItemExists(TKey id);
}