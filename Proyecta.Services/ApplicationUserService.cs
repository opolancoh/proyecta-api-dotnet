using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Proyecta.Core.Contracts.Repositories;
using Proyecta.Core.Contracts.Services;
using Proyecta.Core.DTOs.ApiResponse;
using Proyecta.Core.Entities.Auth;
using Proyecta.Core.DTOs.Auth;
using Proyecta.Core.Entities;

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

    public async Task<ApiResponse<IEnumerable<ApplicationUserListDto>>> GetAll()
    {
        var users = await _repository.GetAllWithRoles();

        return new ApiResponse<IEnumerable<ApplicationUserListDto>>
        {
            Success = true,
            Code = ApiResponseCode.Ok,
            Data = users
        };
    }

    public async Task<ApiResponse<ApplicationUserDetailDto>> GetById(string id)
    {
        var user = await _repository.GetByIdWithRoles(id);

        if (user == null)
        {
            return new ApiResponse<ApplicationUserDetailDto>
            {
                Success = false,
                Code = ApiResponseCode.NotFound,
                Message = $"The entity with id '{id}' doesn't exist in the database."
            };
        }

        return new ApiResponse<ApplicationUserDetailDto>
        {
            Success = true,
            Code = ApiResponseCode.Ok,
            Data = user
        };
    }

    public async Task<ApiResponse<ApiResponseGenericAdd<string>>> Add(ApplicationUserAddRequest item,
        string currentUserId)
    {
        var newItem = MapDtoToEntity(item, currentUserId);

        var result = await _userManager.CreateAsync(newItem, item.Password!);

        if (!result.Succeeded)
        {
            var errors = new Dictionary<string, List<string>>();
            foreach (var error in result.Errors)
            {
                _logger.LogError("User not created. Code: {Code}, Description: {Description}", error.Code,
                    error.Description);

                if (errors.ContainsKey(error.Code))
                {
                    errors[error.Code].Add(error.Description);
                }
                else
                {
                    errors.Add(error.Code, new List<string> { error.Description });
                }
            }

            return new ApiResponse<ApiResponseGenericAdd<string>>
            {
                Success = false,
                Code = ApiResponseCode.BadRequest,
                Message = "An error has occurred while creating the User.",
                Errors = errors
            };
        }

        _logger.LogInformation("User created a new account with password.");

        if (item.Roles != null)
        {
            await _userManager.AddToRolesAsync(newItem, item.Roles);
            _logger.LogInformation("Roles added successfully.");
        }

        var userId = await _userManager.GetUserIdAsync(newItem);

        return new ApiResponse<ApiResponseGenericAdd<string>>
        {
            Success = true,
            Code = ApiResponseCode.Created,
            Message = "User created successfully.",
            Data = new ApiResponseGenericAdd<string> { Id = userId }
        };
    }

    public async Task<ApiResponse> Update(string id, ApplicationUserUpdateRequest item, string currentUserId)
    {
        // Look for current user
        var currentUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (currentUser == null)
        {
            return new ApiResponse
            {
                Success = false,
                Code = ApiResponseCode.NotFound,
                Message = $"User Id '{id}' was not found.",
            };
        }

        // Update user data
        currentUser.UpdatedAt = DateTime.UtcNow;
        currentUser.UpdatedById = currentUserId;
        currentUser.FirstName = item.FirstName!;
        currentUser.LastName = item.LastName!;
        currentUser.DisplayName = item.DisplayName!;
        currentUser.UserName = item.UserName;

        var result = await _userManager.UpdateAsync(currentUser);
        if (!result.Succeeded)
        {
            var errors = new Dictionary<string, List<string>>();
            foreach (var error in result.Errors)
            {
                _logger.LogError("User not created. Code: {Code}, Description: {Description}", error.Code,
                    error.Description);

                if (errors.ContainsKey(error.Code))
                {
                    errors[error.Code].Add(error.Description);
                }
                else
                {
                    errors.Add(error.Code, new List<string> { error.Description });
                }
            }

            return new ApiResponse
            {
                Success = false,
                Code = ApiResponseCode.InternalServerError,
                Message = "An error has occurred while updating the Application User.",
                Errors = errors
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
            _logger.LogInformation("Roles updated successfully.");
        }

        _logger.LogInformation("User updated successfully.");

        return new ApiResponse
        {
            Success = true,
            Code = ApiResponseCode.NoContent,
            Message = "User updated successfully.",
        };
    }

    public async Task<ApiResponse> Remove(string id, string currentUserId)
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
                Success = false,
                Code = ApiResponseCode.NotFound,
                Message = $"User Id '{id}' was not found.",
            };
        }

        var result = await _userManager.DeleteAsync(currentUser);
        if (result.Succeeded)
        {
            return new ApiResponse
            {
                Success = true,
                Code = ApiResponseCode.NoContent,
                Message = "User deleted successfully.",
            };
        }

        var errors = new Dictionary<string, List<string>>();
        foreach (var error in result.Errors)
        {
            _logger.LogError("User not created. Code: {Code}, Description: {Description}", error.Code,
                error.Description);

            if (errors.ContainsKey(error.Code))
            {
                errors[error.Code].Add(error.Description);
            }
            else
            {
                errors.Add(error.Code, new List<string> { error.Description });
            }
        }

        return new ApiResponse
        {
            Success = false,
            Code = ApiResponseCode.InternalServerError,
            Message = $"An error has occurred while deleting the User with Id '{id}'.",
            Errors = errors
        };
    }

    public async Task<ApiResponse<IEnumerable<ApiResponseGenericAdd<string>>>> AddRange(
        IEnumerable<ApplicationUserAddRequest> items,
        string currentUserId)
    {
        var data = new List<ApiResponseGenericAdd<string>>();
        var errors = new Dictionary<string, List<string>>();
        var errorsCount = 0;
        var itemsToAdd = items.ToList();
        foreach (var item in itemsToAdd)
        {
            var result = await Add(item, currentUserId);
            if (result.Success)
            {
                data.Add(result.Data!);
            }
            else
            {
                foreach (var error in result.Errors!)
                {
                    errors.Add($"{error.Key}_{++errorsCount}", error.Value);
                }
            }
        }

        var isSuccess = errors.Count == 0;

        var response = new ApiResponse<IEnumerable<ApiResponseGenericAdd<string>>>
        {
            Success = isSuccess,
            Code = isSuccess ? ApiResponseCode.NoContent : ApiResponseCode.Accepted,
            Message = isSuccess
                ? "All users were added successfully."
                : $"Not all users were added. Added:[{itemsToAdd.Count() - errors.Count}] Not Added:[{errors.Count}]",
            Data = data
        };

        if (!isSuccess) response.Errors = errors;

        return response;
    }

    private ApplicationUser MapDtoToEntity(ApplicationUserAddRequest item, string? currentUserId)
    {
        var now = DateTime.UtcNow;

        var entity = new ApplicationUser
        {
            FirstName = item.FirstName!,
            LastName = item.LastName!,
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