using Proyecta.Core.DTOs.Auth;
using Proyecta.Core.Responses;

namespace Proyecta.Core.Contracts.Services;

public interface IApplicationUserService
{
    Task<ApiResponse<IEnumerable<ApplicationUserListDto>>> GetAll();
    Task<ApiResponse<ApplicationUserDetailDto>> GetById(string id);
    Task<ApiResponse<ApiCreateResponse<string>>> Create(ApplicationUserCreateOrUpdateDto item, string currentUserId);
    Task<ApiResponse> Update(string id, ApplicationUserCreateOrUpdateDto item, string currentUserId);
    Task<ApiResponse> Remove(string id, string currentUserId);

    Task<ApiResponse<IEnumerable<ApiCreateResponse<string>>>> AddRange(
        IEnumerable<ApplicationUserCreateOrUpdateDto> items, string currentUserId);
}