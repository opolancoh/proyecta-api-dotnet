using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecta.Core.Contracts.Services;
using Proyecta.Core.DTOs.ApiResponse;
using Proyecta.Core.DTOs.Auth;

namespace Proyecta.Web.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/[controller]")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(Roles = "System, Administrator")]
public class UsersController : ControllerBase
{
    private readonly IApplicationUserService _service;

    public UsersController(IApplicationUserService service)
    {
        _service = service;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<IEnumerable<ApplicationUserListDto>>))]
    public async Task<IActionResult> Get()
    {
        var result = await _service.GetAll();

        return new OkObjectResult(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<ApplicationUserDetailDto>))]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _service.GetById(id);

        return new OkObjectResult(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<ApiResponseGenericAdd<string>>))]
    public async Task<IActionResult> Create(ApplicationUserAddRequest item)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        var result = await _service.Add(item, userId);

        return new OkObjectResult(result);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
    public async Task<IActionResult> Update(string id, ApplicationUserUpdateRequest item)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        var result = await _service.Update(id, item, userId);

        return new OkObjectResult(result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
    public async Task<IActionResult> Remove(string id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        var result = await _service.Remove(id, userId);

        return new OkObjectResult(result);
    }

    [HttpPost]
    [Route("add-range")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<IEnumerable<ApiResponseGenericAdd<string>>>))]
    public async Task<IActionResult> AddRange(IEnumerable<ApplicationUserAddRequest> items)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        var result = await _service.AddRange(items, userId);

        return new OkObjectResult(result);
    }
}