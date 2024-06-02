using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecta.Core.Contracts.Services;
using Proyecta.Core.DTOs.ApiResponses;
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiBody<IEnumerable<RiskListDto>>))]
    public async Task<IActionResult> Get()
    {
        var result = await _service.GetAll();

        return StatusCode(result.Status, result.Body);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiBody<RiskDetailDto>))]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetById(id);

        return StatusCode(result.Status, result.Body);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiBody<ApiGenericAddResponse<Guid>>))]
    public async Task<IActionResult> Create(RiskAddOrUpdateDto item)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        var result = await _service.Create(item, userId);

        return StatusCode(result.Status, result.Body);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiBody))]
    public async Task<IActionResult> Update(Guid id, RiskAddOrUpdateDto item)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        var result = await _service.Update(id, item, userId);

        return StatusCode(result.Status, result.Body);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiBody))]
    public async Task<IActionResult> Remove(Guid id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        var result = await _service.Remove(id, userId);

        return StatusCode(result.Status, result.Body);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(ApiBody<IEnumerable<ApiGenericAddResponse<Guid>>>))]
    [Route("add-range")]
    public async Task<IActionResult> AddRange(IEnumerable<RiskAddOrUpdateDto> items)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        var result = await _service.AddRange(items, userId);

        return StatusCode(result.Status, result.Body);
    }
}
