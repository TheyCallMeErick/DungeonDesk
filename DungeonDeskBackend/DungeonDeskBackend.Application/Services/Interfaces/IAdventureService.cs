using DungeonDeskBackend.Application.DTOs.Inputs;
using DungeonDeskBackend.Application.DTOs.Inputs.Adventure;
using DungeonDeskBackend.Application.DTOs.Outputs;
using DungeonDeskBackend.Domain.Models;

namespace DungeonDeskBackend.Application.Services.Interfaces;

public interface IAdventureService
{
    Task<OperationResultDTO<Adventure>> GetAdventureByIdAsync(Guid adventureId);
    Task<OperationResultDTO<IEnumerable<Adventure>>> GetAdventuresAsync(QueryInputDTO<GetAdventuresQueryDTO> queryInput);
    Task<OperationResultDTO<Adventure>> CreateAdventureAsync(CreateAdventureInputDTO adventure);
    Task<OperationResultDTO<Adventure>> UpdateAdventureAsync(UpdateAdventureInputDTO adventure);
    Task<OperationResultDTO<Adventure>> DeleteAdventureAsync(Guid adventureId);
    Task<OperationResultDTO<IEnumerable<Desk>>> GetDesksUsingAdventureAsync(Guid adventureId);
}
