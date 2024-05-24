using Proyecta.Core.DTOs.ApiResponse;
using Proyecta.Core.DTOs.Auth;

namespace Proyecta.Core.Contracts.Services;

public interface IApplicationUserService
{
    Task<ApiResponse<IEnumerable<ApplicationUserListDto>>> GetAll();
    Task<ApiResponse<ApplicationUserDetailDto>> GetById(string id);
    Task<ApiResponse<ApiResponseGenericAdd<string>>> Add(ApplicationUserAddRequest item, string currentUserId);
    Task<ApiResponse> Update(string id, ApplicationUserUpdateRequest item, string currentUserId);
    Task<ApiResponse> Remove(string id, string currentUserId);

    Task<ApiResponse<IEnumerable<ApiResponseGenericAdd<string>>>> AddRange(
        IEnumerable<ApplicationUserAddRequest> items, string currentUserId);
}