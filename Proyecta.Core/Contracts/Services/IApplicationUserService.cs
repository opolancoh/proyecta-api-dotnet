using Microsoft.AspNetCore.Identity;
using Proyecta.Core.Entities;
using Proyecta.Core.Entities.Auth;
using Proyecta.Core.Entities.DTOs;
using Proyecta.Core.Models;

namespace Proyecta.Core.Contracts.Services;

public interface IApplicationUserService
{
    Task<ApplicationResponse> GetAll();
    Task<ApplicationResponse> GetById(string id);
    Task<ApplicationResponse> Create(ApplicationUserCreateOrUpdateDto item);
    Task<ApplicationResponse> Update(string id, ApplicationUserCreateOrUpdateDto item);
    Task<ApplicationResponse> Remove(string id);
    Task<ApplicationResponse> AddRange(IEnumerable<ApplicationUserCreateOrUpdateDto> items);
}