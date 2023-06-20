using Proyecta.Core.Entities.DTOs;
using Proyecta.Core.Models;

namespace Proyecta.Core.Contracts.Services;

public interface IApplicationUserService
{
    Task<ApplicationResult> GetAll();
    Task<ApplicationResult> GetById(string id);
    Task<ApplicationResult> Create(ApplicationUserCreateOrUpdateDto item);
    Task<ApplicationResult> Update(string id, ApplicationUserCreateOrUpdateDto item);
    Task<ApplicationResult> Remove(string id);
    Task<ApplicationResult> AddRange(IEnumerable<ApplicationUserCreateOrUpdateDto> items);
}