using DungeonDeskBackend.Api.DTOs.Requests;
using DungeonDeskBackend.Api.DTOs.Responses;
using DungeonDeskBackend.Api.Extensions;
using DungeonDeskBackend.Application.DTOs.Inputs.Desk;
using DungeonDeskBackend.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DungeonDeskBackend.Api.Controllers;

[ApiController]
[Route("api/v1/desks")]
[Authorize]
public class DeskController : ControllerBase
{
    private readonly IDeskService _deskService;

    public DeskController(IDeskService deskService)
    {
        _deskService = deskService;
    }

    [HttpGet()]
    [ProducesResponseType(typeof(IEnumerable<PaginatedResponseDTO<ResponseDeskDTO>>), 200)]
    public async Task<IActionResult> GetDesks([FromQuery] GetDesksQueryRequestDTO queryParams)
    {
        var result = await _deskService.GetDesksAsync(queryParams.ToQueryInputDTO());
        if (!result.Success)
        {
            return BadRequest(result.Message);
        }
        return Ok(result.ToPaginatedResponseDTO());
    }

    [HttpGet("{deskId}")]
    [ProducesResponseType(typeof(ResponseDeskDTO), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetDeskById(Guid deskId)
    {
        var result = await _deskService.GetDeskByIdAsync(deskId);
        if (!result.Success)
        {
            return NotFound(result.Message);
        }
        return Ok(result.Data!.ToResponseDto());
    }

    [HttpPost]
    [ProducesResponseType(typeof(ResponseDeskDTO), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateDesk([FromBody] CreateDeskRequestDTO body)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var masterId = Guid.Parse(HttpContext.User.FindFirst("Id")?.Value ?? string.Empty);

        var dto = body.ToCreateInputDto();
        dto = dto with { MasterId = masterId };
        var result = await _deskService.CreateDeskAsync(dto);
        if (!result.Success)
        {
            return BadRequest(result.Message);
        }
        return Ok(result.Data!.ToResponseDto());
    }

    [HttpPatch("{deskId}")]
    [ProducesResponseType(typeof(ResponseDeskDTO), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateDesk(Guid deskId, [FromBody] UpdateDeskRequestDTO body)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var desk = new Domain.Models.Desk
        {
            Name = body.Name,
            Description = body.Description
        };
        var masterId = Guid.Parse(HttpContext.User.FindFirst("Id")?.Value ?? string.Empty);


        var result = await _deskService.UpdateDeskAsync(new UpdateDeskInputDTO
        (
            DeskId: deskId,
            Name: desk.Name,
            Description: desk.Description,
            MasterId: masterId
        ));
        if (!result.Success)
        {
            return NotFound(result.Message);
        }
        return Ok(result.Data);
    }
}
