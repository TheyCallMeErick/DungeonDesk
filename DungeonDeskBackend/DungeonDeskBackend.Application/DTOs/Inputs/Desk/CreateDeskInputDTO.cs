namespace DungeonDeskBackend.Application.DTOs.Inputs.Desk; 

public record CreateDeskInputDTO(
    string Name,
    string Description,
    int MaxPlayers,
    Guid AdventureId,
    Guid MasterId
);