using Proyecta.Core.Entities.DTOs;
using Proyecta.Core.Models;
using Proyecta.Core.Models.Auth;

namespace Proyecta.Core.Contracts.Services;

public interface IAuthenticationService
{
    Task<ApplicationResult> Register(RegisterInputModel item);
    Task<ApplicationResult> Authenticate(LoginInputModel item);
}