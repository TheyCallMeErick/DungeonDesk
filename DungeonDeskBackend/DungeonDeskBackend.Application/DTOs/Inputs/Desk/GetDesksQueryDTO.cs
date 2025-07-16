using DungeonDeskBackend.Domain.Enums;

namespace DungeonDeskBackend.Application.DTOs.Inputs.Desk; 

public record GetDesksQueryDTO(
    string? Name = null,
    string? Description = null,
    ETableStatus? TableStatus = null,
    int? MaxPlayers = null,
    bool IsFull = false
);