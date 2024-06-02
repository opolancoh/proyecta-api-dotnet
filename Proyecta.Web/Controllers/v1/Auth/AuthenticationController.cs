using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecta.Core.Contracts.Services;
using Proyecta.Core.DTOs;
using Proyecta.Core.DTOs.ApiResponses;
using Proyecta.Core.DTOs.Auth;

namespace Proyecta.Web.Controllers.v1.Auth;

[ApiController]
[ApiVersion("1.0")]
[Route("api/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthService _service;

    public AuthenticationController(IAuthService service)
    {
        _service = service;
    }

    [HttpPost]
    [Route("register")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiBody<ApiGenericAddResponse<string>>))]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        var result = await _service.Register(registerDto);

        return StatusCode(result.Status, result.Body);
    }

    [HttpPost]
    [Route("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiBody<TokenDto>))]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var result = await _service.Login(loginDto);

        return StatusCode(result.Status, result.Body);
    }

    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
    public async Task<IActionResult> Logout(TokenDto tokenDto)
    {
        var result = await _service.Logout(tokenDto);

        return StatusCode(result.Status, result.Body);
    }

    [HttpPost("refresh-token")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiBody<RefreshTokenResponse>))]
    public async Task<IActionResult> RefreshToken(TokenDto tokenDto)
    {
        var result = await _service.RefreshToken(tokenDto);

        return StatusCode(result.Status, result.Body);
    }
}
