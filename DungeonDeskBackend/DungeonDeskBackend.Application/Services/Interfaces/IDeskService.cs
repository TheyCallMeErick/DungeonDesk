using DungeonDeskBackend.Application.DTOs.Inputs;
using DungeonDeskBackend.Application.DTOs.Inputs.Desk;
using DungeonDeskBackend.Application.DTOs.Outputs;
using DungeonDeskBackend.Application.DTOs.Outputs.Desk;
using DungeonDeskBackend.Domain.Models;

namespace DungeonDeskBackend.Application.Services.Interfaces;

public interface IDeskService
{
    Task<OperationResultDTO<IEnumerable<DeskOutputDTO>>> GetDesksAsync(QueryInputDTO<GetDesksQueryDTO> dto);
    Task<OperationResultDTO<DeskOutputDTO>> GetDeskByIdAsync(Guid id);
    Task<OperationResultDTO<DeskOutputDTO>> CreateDeskAsync(CreateDeskInputDTO desk);
    Task<OperationResultDTO<Desk>> UpdateDeskAsync(UpdateDeskInputDTO dto);
    Task<OperationResultDTO<Desk>> DeleteDeskAsync(Guid id);
    Task<OperationResultDTO<IEnumerable<Player>>> GetPlayersByDeskIdAsync(Guid deskId);
}
