using DungeonDeskBackend.Api.DTOs.Requests;
using DungeonDeskBackend.Api.DTOs.Responses;
using DungeonDeskBackend.Api.Extensions;
using DungeonDeskBackend.Application.DTOs.Inputs;
using DungeonDeskBackend.Application.DTOs.Inputs.Adventure;
using DungeonDeskBackend.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DungeonDeskBackend.Api.Controllers;

[Route("api/v1/adventure")]
[ApiController]
[Authorize]
public class AdventureController : ControllerBase
{
    private readonly IAdventureService _adventureService;

    public AdventureController(IAdventureService adventureService)
    {
        _adventureService = adventureService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ResponseAdventureDTO>), 200)]
    public async Task<IActionResult> GetAdventures([FromQuery] GetAdventuresQueryRequestDTO queryParams)
    {
        var result = await _adventureService.GetAdventuresAsync(queryParams.ToQueryDto());
        if (!result.Success)
        {
            return BadRequest(result.Message);
        }
        return Ok(result.Data!.Select(adventure => adventure.ToResponseDto()).ToList());
    }
}
