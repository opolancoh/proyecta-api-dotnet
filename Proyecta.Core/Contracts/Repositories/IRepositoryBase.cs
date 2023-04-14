namespace Proyecta.Core.Contracts.Repositories;

public interface IRepositoryBase<TEntity>
{
    Task<IEnumerable<TEntity>> GetAll();
    Task<TEntity?> GetById(Guid id);
    Task Create(TEntity item);
    Task Update(TEntity item);
    Task Remove(Guid id);
    Task<bool> ItemExists(Guid id);
}