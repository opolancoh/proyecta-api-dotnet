namespace Proyecta.Core.Contracts.Repositories;

public interface IRepositoryBase<in TEntity, in TKey, TEntityListDto, TEntityDetailDto>
{
    Task<TEntityListDto> GetAll();
    Task<TEntityDetailDto> GetById(TKey id);
    Task Create(TEntity item);
    Task Update(TEntity item);
    Task Remove(TKey id);
    Task<bool> ItemExists(TKey id);
}