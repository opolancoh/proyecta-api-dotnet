using Proyecta.Core.DTOs.Auth;
using Proyecta.Core.Results;

namespace Proyecta.Core.Contracts.Services;

public interface IApplicationUserService
{
    Task<ApplicationResult> GetAll();
    Task<ApplicationResult> GetById(string id);
    Task<ApplicationResult> Create(ApplicationUserCreateOrUpdateDto item, string currentUserId);
    Task<ApplicationResult> Update(string id, ApplicationUserCreateOrUpdateDto item, string currentUserId);
    Task<ApplicationResult> Remove(string id, string currentUserId);
    Task<ApplicationResult> AddRange(IEnumerable<ApplicationUserCreateOrUpdateDto> items, string currentUserId);
}