using Microsoft.AspNetCore.Mvc;
using Proyecta.Core.Contracts.Services;
using Proyecta.Core.Entities;
using Proyecta.Core.Entities.DTOs;
using Proyecta.Core.Exceptions;

namespace Proyecta.Web.Controllers.v1;


[ApiController]
[ApiVersion("1.0")]
[Route("api/[controller]")]
[Route("api/v{version:apiVersion}/[controller]")]
public class RisksController : ControllerBase
{
    private readonly IRiskService _service;

    public RisksController(IRiskService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IEnumerable<Risk>> Get()
    {
        return await _service.GetAll();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Risk>> GetById(Guid id)
    {
        var item = await _service.GetById(id);

        if (item == null)
        {
            return NotFound();
        }

        return item;
    }

    [HttpPost]
    public async Task<IActionResult> Create(RiskCreateOrUpdateDto item)
    {
        var newItemId = await _service.Create(item);

        return CreatedAtAction(nameof(GetById), new { id = newItemId }, new { });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, RiskCreateOrUpdateDto item)
    {
        try
        {
            await _service.Update(id, item);
        }
        catch (EntityNotFoundException<Guid>)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(Guid id)
    {
        try
        {
            await _service.Remove(id);
        }
        catch (EntityNotFoundException<Guid>)
        {
            return NotFound();
        }

        return NoContent();
    }
    
    [HttpPost]
    [Route("add-range")]
    public async Task<IActionResult> AddRange(IEnumerable<RiskCreateOrUpdateDto> items)
    {
        await _service.AddRange(items);

        return NoContent();
    }
}