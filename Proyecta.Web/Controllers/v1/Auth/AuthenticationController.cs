using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecta.Core.Contracts.Services;
using Proyecta.Core.Models.Auth;

namespace Proyecta.Web.Controllers.v1.Auth;

[ApiController]
[ApiVersion("1.0")]
[Route("api/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _service;

    public AuthenticationController(IAuthenticationService service)
    {
        _service = service;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(RegisterInputModel item)
    {
        var result = await _service.Register(item);

        return StatusCode(StatusCodes.Status200OK, result);
    }
    
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginInputModel item)
    {
        var result = await _service.Login(item);

        return StatusCode(StatusCodes.Status200OK, result);
    }
    
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(AuthTokenInputModel item)
    {
        var result = await _service.RefreshToken(item);

        return StatusCode(StatusCodes.Status200OK, result);
    }
    
    [Authorize]
    [HttpPost("revoke-token")]
    public async Task<IActionResult> RevokeToken(string username)
    {
        var result = await _service.RevokeToken(username);

        return StatusCode(StatusCodes.Status200OK, result);
    }
}