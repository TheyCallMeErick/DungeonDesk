using DungeonDeskBackend.Domain.Enums;
using DungeonDeskBackend.Domain.Models;
using DungeonDeskBackend.Domain.Validations.DTOs;
using DungeonDeskBackend.Domain.Validations.Interfaces;

namespace DungeonDeskBackend.Domain.Validations.Rules;

public class OnlyTheDeskMasterShoudAddOrUpdateChronicles : IValidationRule<OnlyTheDeskMasterShoudAddOrUpdateChroniclesValidatorDTO>
{
    public ValidationResult Validate(OnlyTheDeskMasterShoudAddOrUpdateChroniclesValidatorDTO dto)
    {
        var master = dto.Desk.PlayerDesks.FirstOrDefault(pd => pd.Role == EPlayerDeskRole.DeskMaster)?.PlayerId;
        if (master == null)
        {
            return ValidationResult.Failure("Desk does not have a master.");
        }
        if (master != dto.PlayerTryingUpdateId)
        {
            return ValidationResult.Failure("Only the desk master can add chronicles.");
        }

        return ValidationResult.Success();
    }
}
