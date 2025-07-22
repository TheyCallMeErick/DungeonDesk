namespace DungeonDeskBackend.Api.DTOs.Requests; 

public record UpdateDeskRequestDTO(
    string Name,
    string Description
);