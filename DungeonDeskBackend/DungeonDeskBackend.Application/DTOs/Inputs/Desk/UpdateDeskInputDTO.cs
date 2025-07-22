namespace DungeonDeskBackend.Application.DTOs.Inputs.Desk; 

public record UpdateDeskInputDTO(
    string Name,
    string Description,
    Guid MasterId,
    Guid DeskId
);
