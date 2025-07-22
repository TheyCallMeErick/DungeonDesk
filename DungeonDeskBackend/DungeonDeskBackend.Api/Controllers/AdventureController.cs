using System.Net;
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
    public async Task<IActionResult> Get([FromQuery] GetAdventuresQueryRequestDTO queryParams)
    {
        var result = await _adventureService.GetAdventuresAsync(queryParams.ToQueryDto());
        if (!result.Success)
        {
            return BadRequest(result.Message);
        }
        return Ok(result.Data!.Select(adventure => adventure.ToResponseDto()).ToList());
    }

    [HttpGet("{adventureId}")]
    [ProducesResponseType(typeof(ResponseAdventureDTO), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(Guid adventureId)
    {
        var result = await _adventureService.GetAdventureByIdAsync(adventureId);
        if (!result.Success)
        {
            return NotFound(result.Message);
        }
        return Ok(result.Data!.ToResponseDto());
    }

    [HttpPost]
    [ProducesResponseType(typeof(ResponseAdventureDTO), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Create([FromBody] CreateAdventureRequestDTO body)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var dto = body.ToInputDto();
        dto.AuthorId = Guid.Parse(HttpContext.User.FindFirst("Id")?.Value ?? string.Empty);
        var result = await _adventureService.CreateAdventureAsync(dto);
        if (!result.Success)
        {
            return BadRequest(result.Message);
        }

        return StatusCode((int)HttpStatusCode.Created, result.Data!.ToResponseDto());
    }

    [HttpPatch("{adventureId}")]
    [ProducesResponseType(typeof(ResponseAdventureDTO), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Update(Guid adventureId, [FromBody] UpdateAdventureRequestDTO body)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var dto = new UpdateAdventureInputDTO
        (
            AdventureId: adventureId,
            Title: body.Title,
            Description: body.Description,
            UserId: Guid.Parse(HttpContext.User.FindFirst("Id")?.Value ?? string.Empty)
        );

        var result = await _adventureService.UpdateAdventureAsync(dto);
        if (!result.Success)
        {
            return BadRequest(result.Message);
        }
        return Ok(result.Data!.ToResponseDto());
    }

    [HttpDelete("{adventureId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(Guid adventureId)
    {
        var result = await _adventureService.DeleteAdventureAsync(adventureId);
        if (!result.Success)
        {
            return NotFound(result.Message);
        }
        return NoContent();
    }
}
