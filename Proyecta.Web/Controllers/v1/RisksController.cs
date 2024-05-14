using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecta.Core.Contracts.Services;
using Proyecta.Core.DTOs.ApiResponse;
using Proyecta.Core.DTOs.Risk;

namespace Proyecta.Web.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/[controller]")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class RisksController : ControllerBase
{
    private readonly IRiskService _service;

    public RisksController(IRiskService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<IEnumerable<RiskListDto>>))]
    public async Task<IActionResult> Get()
    {
        var result = await _service.GetAll();

        return new OkObjectResult(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<RiskDetailDto>))]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetById(id);

        return new OkObjectResult(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<ApiResponseGenericAdd<Guid>>))]
    public async Task<IActionResult> Create(RiskAddOrUpdateDto item)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        var result = await _service.Create(item, userId);

        return new OkObjectResult(result);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
    public async Task<IActionResult> Update(Guid id, RiskAddOrUpdateDto item)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        var result = await _service.Update(id, item, userId);

        return new OkObjectResult(result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
    public async Task<IActionResult> Remove(Guid id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        var result = await _service.Remove(id, userId);

        return new OkObjectResult(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK,
        Type = typeof(ApiResponse<IEnumerable<ApiResponseGenericAdd<Guid>>>))]
    [Route("add-range")]
    public async Task<IActionResult> AddRange(IEnumerable<RiskAddOrUpdateDto> items)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        var result = await _service.AddRange(items, userId);

        return new OkObjectResult(result);
    }
}