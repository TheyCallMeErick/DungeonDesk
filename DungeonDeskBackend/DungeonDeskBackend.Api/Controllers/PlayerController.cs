using DungeonDeskBackend.Api.DTOs.Requests;
using DungeonDeskBackend.Application.DTOs.Inputs.Player;
using DungeonDeskBackend.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DungeonDeskBackend.Api.Controllers;

[ApiController]
[Route("api/player")]
[Authorize]
public class PlayerController : ControllerBase
{
    private readonly IPlayerService _playerService;

    public PlayerController(IPlayerService playerService)
    {
        _playerService = playerService;
    }

    [HttpPost("create")]
    [AllowAnonymous]
    public async Task<IActionResult> CreatePlayer([FromBody] CreatePlayerRequestDTO request)
    {
        var result = await _playerService.CreatePlayerAsync(new CreatePlayerInputDTO
        (
            Name : request.Name,
            Email : request.Email ?? string.Empty,
            Username : request.Username ?? string.Empty, 
            Password : request.Password
        ));
        if (!result.Success)
        {
            return BadRequest(result.Message);
        }
        return Ok(result.Data);
    }

    [HttpPatch("join-desk/{deskId}")]
    public async Task<IActionResult> JoinDesk([FromRoute] Guid deskId)
    {
        if(deskId == Guid.Empty)
        {
            return BadRequest("Desk ID cannot be empty.");
        }
        var playerId = User.FindFirst("id")?.Value;
        if (string.IsNullOrEmpty(playerId))
        {
            return Unauthorized("Player ID not found in token.");
        }
        var result = await _playerService.JoinDeskAsync(deskId, Guid.Parse(playerId));
        if (!result.Success)
        {
            return BadRequest(result.Message);
        }
        return Ok(result.Data);
    }
}
