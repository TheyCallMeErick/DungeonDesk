namespace DungeonDeskBackend.Application.DTOs.Inputs.Player;

public record CreatePlayerInputDTO(
    string Name,
    string Email,
    string Username,
    string Password
);