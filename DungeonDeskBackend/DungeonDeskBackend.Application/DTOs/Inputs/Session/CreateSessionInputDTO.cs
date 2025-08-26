namespace DungeonDeskBackend.Application.DTOs.Inputs.Session;

public record CreateSessionInputDTO
(
    Guid deskId,
    DateTime ScheduledAt,
    string Notes ,
    Guid playerId
);
