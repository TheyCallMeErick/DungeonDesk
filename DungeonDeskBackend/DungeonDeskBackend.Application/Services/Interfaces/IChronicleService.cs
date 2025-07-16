using DungeonDeskBackend.Application.DTOs.Inputs.Chronicle;
using DungeonDeskBackend.Application.DTOs.Outputs;
using DungeonDeskBackend.Domain.Models;

namespace DungeonDeskBackend.Application.Services.Interfaces;

public interface IChronicleService
{
    Task<OperationResultDTO<Chronicle>> AddChronicleAsync(AddChronicleInputDTO dto);
    Task<OperationResultDTO<Chronicle>> UpdateChronicleAsync(UpdateChronicleInputDTO dto);
    Task<OperationResultDTO<Chronicle>> DeleteChronicleAsync(DeleteChronicleInputDTO dto);
    Task<OperationResultDTO<IEnumerable<Chronicle>>> GetChroniclesBySessionIdAsync(Guid sessionId);
    Task<OperationResultDTO<Chronicle?>> GetChronicleByIdAsync(Guid chronicleId);
}
