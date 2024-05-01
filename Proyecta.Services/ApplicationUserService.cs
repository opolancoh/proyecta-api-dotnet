using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Proyecta.Core.Contracts.Repositories;
using Proyecta.Core.Contracts.Services;
using Proyecta.Core.Entities.Auth;
using Proyecta.Core.DTOs.Auth;
using Proyecta.Core.Results;

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

    public async Task<ApplicationResult> GetAll()
    {
        var users = await _repository.GetAllWithRoles();

        return new ApplicationResult
        {
            Success = true,
            Code = "200",
            Data = users
        };
    }

    public async Task<ApplicationResult> GetById(string id)
    {
        var user = await _repository.GetByIdWithRoles(id);

        if (user == null)
        {
            return new ApplicationResult
            {
                Success = false,
                Code = "404",
                Message = $"The entity with id '{id}' doesn't exist in the database."
            };
        }

        return new ApplicationResult
        {
            Success = true,
            Code = "200",
            Data = user
        };
    }

    public async Task<ApplicationResult> Create(ApplicationUserCreateOrUpdateDto item, string currentUserId)
    {
        var newItem = MapDtoToEntity(item, currentUserId);

        var result = await _userManager.CreateAsync(newItem, item.Password!);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(error =>
            {
                _logger.LogInformation($@"User not created. Code:{error.Code} Description:{error.Description}");
                return new ApplicationResponseError { Code = error.Code, Description = error.Description };
            }).ToList();

            return new ApplicationResult
            {
                Success = false,
                Code = "400",
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

        return new ApplicationResult
        {
            Success = true,
            Code = "201",
            Message = "User created successfully.",
            Data = new { userId }
        };
    }

    public async Task<ApplicationResult> Update(string id, ApplicationUserCreateOrUpdateDto item, string currentUserId)
    {
        // Look for current user
        var currentUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (currentUser == null)
        {
            return new ApplicationResult
            {
                Success = false,
                Code = "404",
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
            var errors = result.Errors.Select(error =>
            {
                _logger.LogInformation($@"User not created. Code:{error.Code} Description:{error.Description}");
                return new ApplicationResponseError { Code = error.Code, Description = error.Description };
            }).ToList();

            return new ApplicationResult
            {
                Success = false,
                Code = "500",
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

        return new ApplicationResult
        {
            Success = true,
            Code = "200",
            Message = "User updated successfully.",
        };
    }

    public async Task<ApplicationResult> Remove(string id, string currentUserId)
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
            return new ApplicationResult
            {
                Success = false,
                Code = "404",
                Message = $"User Id '{id}' was not found.",
            };
        }

        var result = await _userManager.DeleteAsync(currentUser);
        if (result.Succeeded)
        {
            return new ApplicationResult
            {
                Success = true,
                Code = "200",
                Message = "User deleted successfully.",
            };
        }

        var errors = result.Errors.Select(error =>
        {
            _logger.LogInformation($@"User not created. Code:{error.Code} Description:{error.Description}");
            return new ApplicationResponseError { Code = error.Code, Description = error.Description };
        }).ToList();

        return new ApplicationResult
        {
            Success = false,
            Code = "500",
            Message = $"An error has occurred while deleting the User with Id '{id}'.",
            Errors = errors
        };
    }

    public async Task<ApplicationResult> AddRange(IEnumerable<ApplicationUserCreateOrUpdateDto> items,
        string currentUserId)
    {
        var data = new List<object>();
        var errors = new List<ApplicationResponseError>();
        var itemsToAdd = items.ToList();
        foreach (var item in itemsToAdd)
        {
            var result = await Create(item, currentUserId);
            if (result.Success)
            {
                data.Add(result.Data!);
            }
            else
            {
                var resultErrors = result.Errors?.Select(error =>
                {
                    _logger.LogInformation($@"User not created. Code:{error.Code} Description:{error.Description}");
                    return new ApplicationResponseError { Code = error.Code, Description = error.Description };
                }).ToList();
                if (resultErrors != null)
                    errors.AddRange(resultErrors);
            }
        }

        var isSuccess = errors.Count == 0;

        var response = new ApplicationResult
        {
            Success = isSuccess,
            Code = isSuccess ? "201" : "202",
            Message = isSuccess
                ? "All users were added successfully."
                : $"Not all users were added. Added:[{itemsToAdd.Count() - errors.Count}] Not Added:[{errors.Count}]",
            Data = data,
            Errors = errors
        };

        return response;
    }

    private ApplicationUser MapDtoToEntity(ApplicationUserCreateOrUpdateDto item, string currentUserId)
    {
        var now = DateTime.UtcNow;

        var entity = new ApplicationUser
        {
            FirstName = item.FirstName!,
            LastName = item.LastName!,
            DisplayName = item.DisplayName!,
            UserName = item.UserName,
            CreatedAt = now,
            CreatedById = currentUserId,
            UpdatedAt = now,
            UpdatedById = currentUserId
        };

        return entity;
    }
}