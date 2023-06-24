using Proyecta.Core.Entities;
using Proyecta.Core.Models;
using Proyecta.Core.Models.Auth;

namespace Proyecta.Core.Contracts.Services;

public interface IAuthenticationService
{
    Task<ApplicationResult> Register(RegisterInputModel item);
    Task<ApplicationResult> Login(LoginInputModel item);
    Task<ApplicationResult> RefreshToken(AuthTokenInputModel item);
    Task<ApplicationResult> RevokeToken(string username);
}