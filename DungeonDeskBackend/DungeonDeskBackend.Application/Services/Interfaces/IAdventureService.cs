using DungeonDeskBackend.Application.DTOs.Outputs;
using DungeonDeskBackend.Domain.Models;

namespace DungeonDeskBackend.Application.Services.Interfaces;

public interface IAdventureService
{
    Task<OperationResultDTO<Adventure>> GetAdventureByIdAsync(Guid adventureId);
    Task<OperationResultDTO<IEnumerable<Adventure>>> GetAdventuresAsync();
    Task<OperationResultDTO<Adventure>> CreateAdventureAsync(Adventure adventure);
    Task<OperationResultDTO<Adventure>> UpdateAdventureAsync(Adventure adventure);
    Task<OperationResultDTO<Adventure>> DeleteAdventureAsync(Guid adventureId);
    Task<OperationResultDTO<IEnumerable<Desk>>> GetDesksUsingAdventureAsync(Guid adventureId);
}
