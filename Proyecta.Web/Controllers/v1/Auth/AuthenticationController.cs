using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecta.Core.Contracts.Services;
using Proyecta.Core.DTOs;
using Proyecta.Core.DTOs.ApiResponse;
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
    [Authorize(Roles = "System,Administrator")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<ApiResponseGenericAdd<string>>))]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
        var result = await _service.Register(registerDto, userId);

        return new OkObjectResult(result);
    }

    [HttpPost]
    [Route("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<TokenDto>))]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var result = await _service.Login(loginDto);

        return new OkObjectResult(result);
    }
    
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
    public async Task<IActionResult> Logout(TokenDto tokenDto)
    {
        var result = await _service.Logout(tokenDto);

        return new OkObjectResult(result);
    }

    [HttpPost("refresh-token")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<RefreshTokenResponse>))]
    public async Task<IActionResult> RefreshToken(TokenDto tokenDto)
    {
        var result = await _service.RefreshToken(tokenDto);

        return new OkObjectResult(result);
    }
}