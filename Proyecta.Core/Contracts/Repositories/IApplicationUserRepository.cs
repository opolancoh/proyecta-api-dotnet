using Proyecta.Core.DTOs;
using Proyecta.Core.Entities.Auth;
using Proyecta.Core.DTOs.Auth;
using Proyecta.Core.DTOs.IdName;

namespace Proyecta.Core.Contracts.Repositories;

public interface
    IApplicationUserRepository : IRepositoryBase<ApplicationUser, string, IEnumerable<IdNameDto<string>>, IdNameAuditableDto<string>>
{
    Task AddRange(IEnumerable<ApplicationUser> items);
    Task<IEnumerable<ApplicationUserListDto>> GetAllWithRoles();
    Task<ApplicationUserDetailDto?> GetByIdWithRoles(string id);
}