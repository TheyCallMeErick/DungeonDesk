using DungeonDeskBackend.Application.DTOs.Inputs;
using DungeonDeskBackend.Application.DTOs.Inputs.Desk;
using DungeonDeskBackend.Application.DTOs.Outputs;
using DungeonDeskBackend.Domain.Models;

namespace DungeonDeskBackend.Application.Services.Interfaces;

public interface IDeskService
{
    Task<OperationResultDTO<IEnumerable<Desk>>> GetDesksAsync(QueryInputDTO<GetDesksQueryDTO> dto);
    Task<OperationResultDTO<Desk>> GetDeskByIdAsync(Guid id);
    Task<OperationResultDTO<Desk>> CreateDeskAsync(Desk desk);
    Task<OperationResultDTO<Desk>> UpdateDeskAsync(Guid id, Desk desk);
    Task<OperationResultDTO<Desk>> DeleteDeskAsync(Guid id);
    Task<OperationResultDTO<IEnumerable<Player>>> GetPlayersByDeskIdAsync(Guid deskId);
}
