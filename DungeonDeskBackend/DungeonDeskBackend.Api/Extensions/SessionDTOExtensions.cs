using DungeonDeskBackend.Api.DTOs.Requests;
using DungeonDeskBackend.Api.DTOs.Responses;
using DungeonDeskBackend.Application.DTOs.Inputs.Session;
using DungeonDeskBackend.Application.DTOs.Outputs.Session;

namespace DungeonDeskBackend.Api.Extensions;

public static class SessionDTOExtensions
{
    public static CreateSessionInputDTO ToCreateInputDto(this CreateSessionRequestDTO request)
    {
        return new CreateSessionInputDTO(
            request.deskId,
            request.ScheduledAt,
            request.Notes,
            Guid.Empty
        );
    }

    public static ResponseSessionDTO ToResponseDTO(this SessionOutputDTO dto)
    {
        return new ResponseSessionDTO(
            dto.Id,
            dto.ScheduledAt,
            dto.Desk?.ToResponseDTO(),
            dto.StartedAt,
            dto.EndedAt,
            dto.Notes
        );
    }
}
