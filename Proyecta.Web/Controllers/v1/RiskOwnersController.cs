using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecta.Core.Contracts.Services;
using Proyecta.Core.DTOs.ApiResponses;
using Proyecta.Core.DTOs.IdName;

namespace Proyecta.Web.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/risk-owners")]
[Route("api/v{version:apiVersion}/risk-owners")]
[Authorize]
public class RiskOwnersController : ControllerBase
{
    private readonly IRiskOwnerService _service;

    public RiskOwnersController(IRiskOwnerService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiBody<IEnumerable<IdNameListDto<Guid>>>))]
    public async Task<IActionResult> Get()
    {
        var result = await _service.GetAll();

        return StatusCode(result.Status, result.Body);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiBody<IdNameDetailDto<Guid>>))]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetById(id);

        return StatusCode(result.Status, result.Body);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiBody<ApiGenericAddResponse<Guid>>))]
    public async Task<IActionResult> Create(IdNameAddOrUpdateDto item)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        var result = await _service.Add(item, userId);

        return StatusCode(result.Status, result.Body);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiBody))]
    public async Task<IActionResult> Update(Guid id, IdNameAddOrUpdateDto item)
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
    [Route("add-range")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiBody<IEnumerable<ApiGenericAddResponse<Guid>>>))]
    public async Task<IActionResult> AddRange(IEnumerable<IdNameAddOrUpdateDto> items)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        var result = await _service.AddRange(items, userId);

        return StatusCode(result.Status, result.Body);
    }
}
