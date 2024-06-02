using Proyecta.Core.DTOs.ApiResponses;
using Proyecta.Core.DTOs.Auth;

namespace Proyecta.Core.Contracts.Services;

public interface IApplicationUserService
{
    Task<ApiResponse> GetAll();
    Task<ApiResponse> GetById(string id);
    Task<ApiResponse> Add(ApplicationUserAddRequest item, string currentUserId);
    Task<ApiResponse> Update(string id, ApplicationUserUpdateRequest item, string currentUserId);
    Task<ApiResponse> Remove(string id, string currentUserId);
    Task<ApiResponse> AddRange(IEnumerable<ApplicationUserAddRequest> items, string currentUserId);
}
