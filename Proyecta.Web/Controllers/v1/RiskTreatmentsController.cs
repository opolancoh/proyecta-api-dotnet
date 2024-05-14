using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecta.Core.Contracts.Services;
using Proyecta.Core.DTOs.ApiResponse;
using Proyecta.Core.DTOs.IdName;

namespace Proyecta.Web.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/risk-treatments")]
[Route("api/v{version:apiVersion}/risk-treatments")]
[Authorize]
public class RiskTreatmentsController : ControllerBase
{
    private readonly IRiskTreatmentService _service;

    public RiskTreatmentsController(IRiskTreatmentService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<IEnumerable<IdNameListDto<Guid>>>))]
    public async Task<IActionResult> Get()
    {
        var result = await _service.GetAll();

        return new OkObjectResult(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<IdNameDetailDto<Guid>>))]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetById(id);

        return new OkObjectResult(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<ApiResponseGenericAdd<Guid>>))]
    public async Task<IActionResult> Create(IdNameAddOrUpdateDto item)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        var result = await _service.Create(item, userId);

        return new OkObjectResult(result);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
    public async Task<IActionResult> Update(Guid id, IdNameAddOrUpdateDto item)
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
    [Route("add-range")]
    [ProducesResponseType(StatusCodes.Status200OK,
        Type = typeof(ApiResponse<IEnumerable<ApiResponseGenericAdd<Guid>>>))]
    public async Task<IActionResult> AddRange(IEnumerable<IdNameAddOrUpdateDto> items)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        var result = await _service.AddRange(items, userId);

        return new OkObjectResult(result);
    }
}