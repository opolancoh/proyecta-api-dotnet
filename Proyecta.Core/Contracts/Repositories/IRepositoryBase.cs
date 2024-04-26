namespace Proyecta.Core.Contracts.Repositories;

public interface IRepositoryBase<in TEntity, in TEntityKey, TEntityListDto, TEntityDetailDto>
{
    Task<TEntityListDto> GetAll();
    Task<TEntityDetailDto?> GetById(TEntityKey id);
    Task Create(TEntity item);
    Task Update(TEntity item);
    Task Remove(TEntityKey id);
    Task<bool> ItemExists(TEntityKey id);
}