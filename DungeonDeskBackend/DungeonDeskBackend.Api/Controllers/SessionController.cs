using DungeonDeskBackend.Api.DTOs.Requests;
using DungeonDeskBackend.Api.Extensions;
using DungeonDeskBackend.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DungeonDeskBackend.Api.Controllers;

[Route("api/session")]
[ApiController]
[Authorize]
public class SessionController : ControllerBase
{
    private readonly ISessionService _sessionService;
    public SessionController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateSession([FromBody] CreateSessionRequestDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var userId = User?.Identity?.Name;
        var input = dto.ToCreateInputDto();
        input = input with { playerId = Guid.Parse(userId ?? Guid.Empty.ToString()) };

        var result = await _sessionService.CreateSessionAsync(input);
        if (!result.Success)
        {
            return BadRequest(result.Message);
        }

        return Ok(result.Data);
    }


    [HttpGet("desk/{deskId}")]
    public async Task<IActionResult> GetSessionsByDeskId(Guid deskId)
    {
        var result = await _sessionService.GetSessionsByDeskIdAsync(deskId);
        if (!result.Success)
        {
            return BadRequest(result.Message);
        }
        return Ok(result.Data?.Select(s => s.ToResponseDTO()));
    }
}
