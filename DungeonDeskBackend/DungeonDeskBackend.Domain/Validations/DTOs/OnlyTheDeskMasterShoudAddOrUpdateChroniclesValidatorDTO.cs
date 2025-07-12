using DungeonDeskBackend.Domain.Models;

namespace DungeonDeskBackend.Domain.Validations.DTOs;

public record OnlyTheDeskMasterShoudAddOrUpdateChroniclesValidatorDTO
{
    public required Desk Desk { get; set; }
    public required Guid PlayerTryingUpdateId { get; set; }

}
