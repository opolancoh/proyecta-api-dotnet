namespace Proyecta.Core.Contracts.Services;

public interface IAuditableServiceBase<TResult, in TKey, in TCreateOrUpdate>
{
    Task<TResult> GetAll();
    Task<TResult> GetById(TKey id);
    Task<TResult> Create(TCreateOrUpdate item, string currentUserId);
    Task<TResult> Update(TKey id, TCreateOrUpdate item, string currentUserId);
    Task<TResult> Remove(TKey id, string currentUserId);
}