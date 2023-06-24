using Proyecta.Core.Entities;
using Proyecta.Core.DTOs;

namespace Proyecta.Core.Contracts.Repositories;

public interface IApplicationUserRepository : IRepositoryBase<ApplicationUser, string>
{
    Task AddRange(IEnumerable<ApplicationUser> items);
    Task<IEnumerable<ApplicationUserListDto>> GetUsersWithRoles();
    Task<ApplicationUserDetailsDto?> GetByIdWithRoles(string id);
}