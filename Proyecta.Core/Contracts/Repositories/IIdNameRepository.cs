namespace Proyecta.Core.Contracts.Repositories;

public interface IIdNameRepository<in TKey, in TEntity, TList, TDetail>
{
    Task<TList> GetAll();
    Task<TDetail?> GetById(TKey id);
    Task<int> Create(TEntity item);
    Task<int> Update(TEntity item);
    Task<int> Remove(TKey id);
    Task<int> AddRange(IEnumerable<TEntity> items);
}