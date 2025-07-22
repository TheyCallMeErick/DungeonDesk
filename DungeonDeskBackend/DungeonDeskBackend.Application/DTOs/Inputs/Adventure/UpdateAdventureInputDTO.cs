namespace DungeonDeskBackend.Application.DTOs.Inputs.Adventure;

public record UpdateAdventureInputDTO(Guid AdventureId, string Title, string Description, Guid UserId);

