using System.Security.Claims;
using DungeonDeskBackend.Api.DTOs.Requests;
using DungeonDeskBackend.Api.DTOs.Responses;
using DungeonDeskBackend.Application.DTOs.Inputs.Auth;
using DungeonDeskBackend.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ObservatorioApi.Controllers;

[ApiController]
[Route("api/v1/auth")]
[Authorize]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet("check")]
    public IActionResult Check()
    {
        return Ok();
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ResponseLoginDTO), 200)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
    {
        var result = await _authService.ValidateUserCredentialsAsync(new UserCredentialsInputDTO
        (
            email: loginRequest.Email,
            password: loginRequest.Password,
            ip: HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty,
            deviceInfo: HttpContext.Request.Headers["User-Agent"].ToString()
        ));
        if (!result.Success)
        {
            return BadRequest(result.Message);
        }
        return Ok(new ResponseLoginDTO
        (
            AccessToken: result.Data!.AccessToken,
            RefreshToken: result.Data.RefreshToken
        ));
    }

    [HttpGet("refresh-access-token")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshAccessToken()
    {
        var refreshToken = HttpContext.Request.Headers["Refresh-Token"].ToString();
        if (string.IsNullOrEmpty(refreshToken))
        {
            return BadRequest("Refresh token is required.");
        }
        var result = await _authService.RefreshAccessTokenAsync(refreshToken);
        if (!result.Success)
        {
            return BadRequest(result.Message);
        }
        return Ok(result.Data);
    }

    [HttpGet("renew-tokens")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ResponseLoginDTO), 200)]
    public async Task<IActionResult> RenewTokens()
    {
        var refreshToken = HttpContext.Request.Headers["Refresh-Token"].ToString();
        if (string.IsNullOrEmpty(refreshToken))
        {
            return BadRequest("Refresh token is required.");
        }
        var result = await _authService.RefreshAccessTokenAsync(
            Guid.Parse(refreshToken),
            HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty,
            HttpContext.Request.Headers["User-Agent"].ToString()
        );
        if (!result.Success)
        {
            return BadRequest(result.Message);
        }
        return Ok(new ResponseLoginDTO
        (
            AccessToken: result.Data!.AccessToken,
            RefreshToken: result.Data.RefreshToken
        ));
    }

    [HttpGet("current-user")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        Console.WriteLine(userId);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User ID not found in token.");
        }
        var result = await _authService.GetCurrentUserAsync(userId);
        if (!result.Success)
        {
            return BadRequest(result.Message);
        }
        return Ok(result.Data);
    }
}
