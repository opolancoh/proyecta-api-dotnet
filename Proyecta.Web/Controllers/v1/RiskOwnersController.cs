using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecta.Core.Contracts.Services;
using Proyecta.Core.DTOs;

namespace Proyecta.Web.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/[controller]")]
[Route("api/v{version:apiVersion}/[controller]")]
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
        var result = await _service.Create(item);

        return StatusCode(StatusCodes.Status200OK, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, RiskOwnerCreateOrUpdateDto item)
    {
        var result = await _service.Update(id, item);

        return StatusCode(StatusCodes.Status200OK, result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(Guid id)
    {
        var result = await _service.Remove(id);

        return StatusCode(StatusCodes.Status200OK, result);
    }
    
    [HttpPost]
    [Route("add-range")]
    public async Task<IActionResult> AddRange(IEnumerable<RiskOwnerCreateOrUpdateDto> items)
    {
        var result = await _service.AddRange(items);

        return StatusCode(StatusCodes.Status200OK, result);
    }
}