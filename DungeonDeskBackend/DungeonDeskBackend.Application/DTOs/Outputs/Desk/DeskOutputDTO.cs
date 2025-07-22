using DungeonDeskBackend.Domain.Enums;

namespace DungeonDeskBackend.Application.DTOs.Outputs.Desk;

public record DeskOutputDTO(
    Guid Id,
    string Name,
    string Description,
    ETableStatus Status,
    int MaxPlayers,
    Guid? AdventureId
);
