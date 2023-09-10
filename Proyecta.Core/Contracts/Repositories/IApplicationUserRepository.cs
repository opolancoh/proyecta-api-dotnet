using Proyecta.Core.Entities.Auth;
using Proyecta.Core.DTOs.Auth;

namespace Proyecta.Core.Contracts.Repositories;

public interface IApplicationUserRepository : IRepositoryBase<ApplicationUser, string>
{
    Task AddRange(IEnumerable<ApplicationUser> items);
    Task<IEnumerable<ApplicationUserListDto>> GetAllWithRoles();
    Task<ApplicationUserDetailsDto?> GetByIdWithRoles(string id);
}