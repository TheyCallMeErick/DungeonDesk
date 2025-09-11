namespace DungeonDeskBackend.Application.DTOs.Outputs.User; 

public record UserOutputDTO(
    Guid Id,
    string? Username,
    string? Name,
    string? Email,
    string? ProfilePictureFileName
);
