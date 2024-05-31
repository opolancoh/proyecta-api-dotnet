using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Proyecta.Core.Contracts.Repositories;
using Proyecta.Core.Contracts.Services;
using Proyecta.Core.DTOs.ApiResponse;
using Proyecta.Core.DTOs.Auth;
using Proyecta.Core.Entities;
using Proyecta.Services.Helpers;

namespace Proyecta.Services;

public sealed class ApplicationUserService : IApplicationUserService
{
    private readonly ILogger<ApplicationUserService> _logger;
    private readonly IApplicationUserRepository _repository;
    private readonly UserManager<ApplicationUser> _userManager;

    public ApplicationUserService(
        ILogger<ApplicationUserService> logger,
        IApplicationUserRepository repository,
        UserManager<ApplicationUser> userManager
    )
    {
        _logger = logger;
        _repository = repository;
        _userManager = userManager;
    }

    public async Task<IApiResponse> GetAll()
    {
        var result = await _repository.GetAllWithRoles();

        return new ApiResponse
        {
            Status = ApiResponseStatus.Ok,
            Body = new ApiBody<IEnumerable<ApplicationUserListDto>>()
            {
                Data = result
            }
        };
    }

    public async Task<IApiResponse> GetById(string id)
    {
        var user = await _repository.GetByIdWithRoles(id);

        if (user == null)
        {
            return new ApiResponse
            {
                Status = ApiResponseStatus.NotFound,
                Body = new ApiBody
                {
                    Message =
                        $"The requested resource with ID '{id}' was not found, or you don't have permission to access it."
                }
            };
        }

        return new ApiResponse
        {
            Status = ApiResponseStatus.Ok,
            Body = new ApiBody<ApplicationUserDetailDto>
            {
                Data = user
            }
        };
    }

    public async Task<IApiResponse> Add(ApplicationUserAddRequest item, string currentUserId)
    {
        var newItem = MapDtoToEntity(item, currentUserId);

        var result = await _userManager.CreateAsync(newItem, item.Password!);

        if (!result.Succeeded)
        {
            _logger.LogError("Resource was not created");

            var errors = ErrorResponseHelper.GetIdentityErrors(result.Errors);

            return new ApiResponse
            {
                Status = ApiResponseStatus.BadRequest,
                Body = new ApiBody
                {
                    Message = "The resource could not be created, or you do not have permission to create it.",
                    Errors = errors
                }
            };
        }

        if (item.Roles != null)
        {
            await _userManager.AddToRolesAsync(newItem, item.Roles);
        }

        var userId = await _userManager.GetUserIdAsync(newItem);

        return new ApiResponse
        {
            Status = ApiResponseStatus.Created,
            Body = new ApiBody<ApiResponseGenericAdd<string>>
            {
                Message = "User created successfully.",
                Data = new ApiResponseGenericAdd<string> { Id = userId }
            }
        };
    }

    public async Task<IApiResponse> Update(string id, ApplicationUserUpdateRequest item, string currentUserId)
    {
        var currentUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (currentUser == null)
        {
            return new ApiResponse
            {
                Status = ApiResponseStatus.NotFound,
                Body = new ApiBody
                {
                    Message =
                        $"The requested resource with ID '{id}' was not found, or you don't have permission to access it."
                }
            };
        }

        // Update user data
        currentUser.UpdatedAt = DateTime.UtcNow;
        currentUser.UpdatedById = currentUserId;
        currentUser.FirstName = item.FirstName;
        currentUser.LastName = item.LastName;
        currentUser.DisplayName = item.DisplayName!;
        currentUser.UserName = item.UserName;

        var result = await _userManager.UpdateAsync(currentUser);
        if (!result.Succeeded)
        {
            _logger.LogError("Resource with ID '{ID}' was not updated", id);

            var errors = ErrorResponseHelper.GetIdentityErrors(result.Errors);

            return new ApiResponse
            {
                Status = ApiResponseStatus.Conflict,
                Body = new ApiBody
                {
                    Message =
                        $"The resource with ID '{id}' could not be updated, or you don't have permission to update it.",
                    Errors = errors
                }
            };
        }

        if (item.Roles != null)
        {
            var currentRoles = await _userManager.GetRolesAsync(currentUser);
            // Roles in item.Roles not in currentRoles
            var rolesToAdd = item.Roles.Except(currentRoles);
            // Roles in currentRoles not in item.Roles
            var rolesToRemove = currentRoles.Except(item.Roles);
            // Update user roles
            await _userManager.AddToRolesAsync(currentUser, rolesToAdd);
            await _userManager.RemoveFromRolesAsync(currentUser, rolesToRemove);
        }

        return new ApiResponse
        {
            Status = ApiResponseStatus.NoContent,
            Body = new ApiBody
            {
                Message = "The resource was updated successfully."
            }
        };
    }

    public async Task<IApiResponse> Remove(string id, string currentUserId)
    {
        var currentUser = await _userManager
            .Users
            .Select(x =>
                new ApplicationUser
                {
                    Id = x.Id,
                    ConcurrencyStamp = x.ConcurrencyStamp
                })
            .FirstOrDefaultAsync(x => x.Id == id);

        if (currentUser == null)
        {
            return new ApiResponse
            {
                Status = ApiResponseStatus.NotFound,
                Body = new ApiBody
                {
                    Message =
                        $"The requested resource with ID '{id}' was not found, or you don't have permission to access it."
                }
            };
        }

        var result = await _userManager.DeleteAsync(currentUser);
        if (!result.Succeeded)
        {
            _logger.LogError("Resource with ID '{ID}' was not deleted", id);

            var errors = ErrorResponseHelper.GetIdentityErrors(result.Errors);

            return new ApiResponse
            {
                Status = ApiResponseStatus.Conflict,
                Body = new ApiBody
                {
                    Message =
                        $"The resource with ID '{id}' could not be deleted, or you don't have permission to update it.",
                    Errors = errors
                }
            };
        }

        return new ApiResponse
        {
            Status = ApiResponseStatus.NoContent,
            Body = new ApiBody
            {
                Message = "The resource was deleted successfully."
            }
        };
    }

    public async Task<IApiResponse> AddRange(IEnumerable<ApplicationUserAddRequest> items, string currentUserId)
    {
        var data = new List<ApiResponseGenericAdd<string>>();
        var errors = new Dictionary<string, List<string>>();
        var errorsCount = 0;
        var itemsToAdd = items.ToList();
        foreach (var item in itemsToAdd)
        {
            var result = await Add(item, currentUserId);
            if (result.Status == ApiResponseStatus.Created)
            {
                var userCreationResult = result.Body as ApiBody<ApiResponseGenericAdd<string>>;
                data.Add(userCreationResult!.Data!);
            }
            else
            {
                foreach (var error in result.Body.Errors!)
                {
                    errors.Add($"{error.Key}_{++errorsCount}", error.Value);
                }
            }
        }

        var isSuccess = errors.Count == 0;

        var response = new ApiResponse
        {
            Status = isSuccess ? ApiResponseStatus.NoContent : ApiResponseStatus.MultiStatus,
            Body = new ApiBody<IEnumerable<ApiResponseGenericAdd<string>>>
            {
                Message = isSuccess
                    ? "All users were added successfully."
                    : $"Not all users were added. Added:[{itemsToAdd.Count() - errors.Count}] Not Added:[{errors.Count}]",
                Data = data
            }
        };

        if (!isSuccess) response.Body.Errors = errors;

        return response;
    }

    private ApplicationUser MapDtoToEntity(ApplicationUserAddRequest item, string? currentUserId)
    {
        var now = DateTime.UtcNow;

        var entity = new ApplicationUser
        {
            FirstName = item.FirstName,
            LastName = item.LastName,
            DisplayName = item.DisplayName!,
            UserName = item.UserName,
            CreatedAt = now,
            UpdatedAt = now,
        };

        if (currentUserId == null)
        {
            var id = Guid.NewGuid().ToString();
            entity.Id = id;
            entity.CreatedById = id;
            entity.UpdatedById = id;
        }
        else
        {
            entity.CreatedById = currentUserId;
            entity.UpdatedById = currentUserId;
        }

        return entity;
    }
}
