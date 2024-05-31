using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecta.Core.Contracts.Services;
using Proyecta.Core.DTOs.ApiResponse;
using Proyecta.Core.DTOs.IdName;

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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<IEnumerable<IdNameListDto<Guid>>>))]
    public async Task<IActionResult> Get()
    {
        var result = await _service.GetAll();

        return StatusCode(result.Status, result.Body);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<IdNameDetailDto<Guid>>))]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetById(id);

        return StatusCode(result.Status, result.Body);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<ApiResponseGenericAdd<Guid>>))]
    public async Task<IActionResult> Create(IdNameAddOrUpdateDto item)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        var result = await _service.Create(item, userId);

        return StatusCode(result.Status, result.Body);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
    public async Task<IActionResult> Update(Guid id, IdNameAddOrUpdateDto item)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        var result = await _service.Update(id, item, userId);

        return StatusCode(result.Status, result.Body);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
    public async Task<IActionResult> Remove(Guid id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        var result = await _service.Remove(id, userId);

        return StatusCode(result.Status, result.Body);
    }

    [HttpPost]
    [Route("add-range")]
    [ProducesResponseType(StatusCodes.Status200OK,
        Type = typeof(ApiResponse<IEnumerable<ApiResponseGenericAdd<Guid>>>))]
    public async Task<IActionResult> AddRange(IEnumerable<IdNameAddOrUpdateDto> items)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        var result = await _service.AddRange(items, userId);

        return StatusCode(result.Status, result.Body);
    }
}