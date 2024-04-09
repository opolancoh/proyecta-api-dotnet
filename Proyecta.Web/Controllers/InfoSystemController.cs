using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Authorization;
using Proyecta.Core.Results;
using Proyecta.Web.Utils;

namespace Proyecta.Web.Controllers;

[ApiController]
[Route("info-system")]
[Authorize(Roles = "System")]
public class InfoSystemController : ControllerBase
{
    [HttpGet]
    public Task<IActionResult> Get()
    {
        var envVariables = Environment.GetEnvironmentVariables();

        var info = new
        {
            envVariables
        };

        return Task.FromResult<IActionResult>(StatusCode(StatusCodes.Status200OK, new ApplicationResult
        {
            Status = StatusCodes.Status200OK,
            D = info
        }));
    }
}