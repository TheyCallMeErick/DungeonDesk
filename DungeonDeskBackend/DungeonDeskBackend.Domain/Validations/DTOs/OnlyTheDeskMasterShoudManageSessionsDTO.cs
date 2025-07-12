using DungeonDeskBackend.Domain.Models;

namespace DungeonDeskBackend.Domain.Validations.DTOs; 

public record OnlyTheDeskMasterShoudManageSessionsDTO
{
    public required Desk Desk { get; set; }
    public required Guid PlayerTryingUpdateId { get; set; }
}
