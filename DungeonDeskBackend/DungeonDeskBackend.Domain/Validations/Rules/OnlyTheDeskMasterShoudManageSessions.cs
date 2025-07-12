using DungeonDeskBackend.Domain.Enums;
using DungeonDeskBackend.Domain.Validations.DTOs;
using DungeonDeskBackend.Domain.Validations.Interfaces;

namespace DungeonDeskBackend.Domain.Validations.Rules; 

public class OnlyTheDeskMasterShoudManageSessions : IValidationRule<OnlyTheDeskMasterShoudManageSessionsDTO>
{
    public ValidationResult Validate(OnlyTheDeskMasterShoudManageSessionsDTO dto)
    {
        var master = dto.Desk.PlayerDesks.FirstOrDefault(pd => pd.Role == EPlayerDeskRole.DeskMaster)?.PlayerId;
        if (master == null)
        {
            return ValidationResult.Failure("Desk does not have a master.");
        }
        if (master != dto.PlayerTryingUpdateId)
        {
            return ValidationResult.Failure("Only the desk master can manage sessions.");
        }

        return ValidationResult.Success();
    }
}
