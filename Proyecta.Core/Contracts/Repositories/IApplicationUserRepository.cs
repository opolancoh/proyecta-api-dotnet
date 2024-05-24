using Proyecta.Core.DTOs;
using Proyecta.Core.Entities.Auth;
using Proyecta.Core.DTOs.Auth;
using Proyecta.Core.DTOs.IdName;

namespace Proyecta.Core.Contracts.Repositories;

public interface IApplicationUserRepository
{
    Task<IEnumerable<ApplicationUserListDto>> GetAllWithRoles();
    Task<ApplicationUserDetailDto?> GetByIdWithRoles(string id);
}