using Proyecta.Core.Entities.DTOs;
using Proyecta.Core.Models;
using Proyecta.Core.Models.Auth;

namespace Proyecta.Core.Contracts.Services;

public interface IAuthenticationService
{
    Task<ApplicationResponse> Register(ApplicationUserRegisterDto item);
    Task<ApplicationResponse> Authenticate(LoginInputModel item);
}