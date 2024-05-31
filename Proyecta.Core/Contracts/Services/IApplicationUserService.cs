using Proyecta.Core.DTOs.ApiResponse;
using Proyecta.Core.DTOs.Auth;

namespace Proyecta.Core.Contracts.Services;

public interface IApplicationUserService
{
    Task<IApiResponse> GetAll();
    Task<IApiResponse> GetById(string id);
    Task<IApiResponse> Add(ApplicationUserAddRequest item, string currentUserId);
    Task<IApiResponse> Update(string id, ApplicationUserUpdateRequest item, string currentUserId);
    Task<IApiResponse> Remove(string id, string currentUserId);
    Task<IApiResponse> AddRange(IEnumerable<ApplicationUserAddRequest> items, string currentUserId);
}