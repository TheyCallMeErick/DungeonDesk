using DungeonDeskBackend.Application.DTOs.Outputs.Desk;
using DungeonDeskBackend.Domain.Models;

namespace DungeonDeskBackend.Application.Extensions;

public static class DeskExtensions
{
    public static DeskOutputDTO ToOutputDTO(this Desk desk)
    {
        return new DeskOutputDTO(
            desk.Id,
            desk.Name,
            desk.Description,
            desk.Status,
            desk.MaxPlayers,
            desk.AdventureId
        );
    }
}
