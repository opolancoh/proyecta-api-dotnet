using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecta.Core.Contracts.Services;
using Proyecta.Core.DTOs;
using Proyecta.Core.DTOs.Risk;

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
    public async Task<IActionResult> Get()
    {
        var result = await _service.GetAll();

        return StatusCode(StatusCodes.Status200OK, result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetById(id);

        return StatusCode(StatusCodes.Status200OK, result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(RiskOwnerCreateOrUpdateDto item)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        var result = await _service.Create(item, userId);

        return StatusCode(StatusCodes.Status200OK, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, RiskOwnerCreateOrUpdateDto item)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        var result = await _service.Update(id, item, userId);

        return StatusCode(StatusCodes.Status200OK, result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(Guid id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        var result = await _service.Remove(id, userId);

        return StatusCode(StatusCodes.Status200OK, result);
    }
    
    [HttpPost]
    [Route("add-range")]
    public async Task<IActionResult> AddRange(IEnumerable<RiskOwnerCreateOrUpdateDto> items)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        var result = await _service.AddRange(items, userId);

        return StatusCode(StatusCodes.Status200OK, result);
    }
}