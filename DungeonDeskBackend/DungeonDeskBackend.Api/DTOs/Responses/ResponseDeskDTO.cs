namespace DungeonDeskBackend.Api.DTOs.Responses; 

public record ResponseDeskDTO(
    Guid Id,
    string Name,
    string Description,
    string Status,
    int MaxPlayers,
    Guid? AdventureId
);
