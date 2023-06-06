using Proyecta.Core.Entities.Auth;
using Proyecta.Core.Entities.DTOs;

namespace Proyecta.Core.Contracts.Repositories;

public interface IApplicationUserRepository : IRepositoryBase<ApplicationUser, string>
{
    Task AddRange(IEnumerable<ApplicationUser> items);
    Task<IEnumerable<ApplicationUserListDto>> GetUsersWithRoles();
    Task<ApplicationUserDetailsDto?> GetByIdWithRoles(string id);
}