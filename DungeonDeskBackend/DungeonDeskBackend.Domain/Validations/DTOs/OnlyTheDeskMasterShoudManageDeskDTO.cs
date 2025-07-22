using DungeonDeskBackend.Domain.Models;

namespace DungeonDeskBackend.Domain.Validations.DTOs; 

public record OnlyTheDeskMasterShoudManageDeskDTO(
Desk Desk,
   Guid PlayerTryingUpdateId 
);
