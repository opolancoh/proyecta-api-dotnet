using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecta.Core.Contracts.Services;
using Proyecta.Core.DTOs;
using Proyecta.Web.Filters;

namespace Proyecta.Web.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/risk-categories")]
[Route("api/v{version:apiVersion}/risk-categories")]
[Authorize]
public class RiskCategoriesController : ControllerBase
{
    private readonly IRiskCategoryService _service;

    public RiskCategoriesController(IRiskCategoryService service)
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
    [CustomValidation("PropertyNameToValidate")]
    public async Task<IActionResult> Create(RiskCategoryCreateOrUpdateDto item)
    {
        var result = await _service.Create(item);

        return StatusCode(StatusCodes.Status200OK, result);
    }

    [HttpPut("{id}")]
    [InputModelValidationFilter]
    public async Task<IActionResult> Update(Guid id, RiskCategoryCreateOrUpdateDto item)
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
    public async Task<IActionResult> AddRange(IEnumerable<RiskCategoryCreateOrUpdateDto> items)
    {
        var result = await _service.AddRange(items);

        return StatusCode(StatusCodes.Status200OK, result);
    }
}