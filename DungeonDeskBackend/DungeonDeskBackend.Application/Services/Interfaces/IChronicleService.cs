using DungeonDeskBackend.Application.DTOs.Outputs;
using DungeonDeskBackend.Domain.Models;

namespace DungeonDeskBackend.Application.Services.Interfaces;

public interface IChronicleService
{
    Task<OperationResultDTO<Chronicle>> AddChronicleAsync(Guid sessionId, string title, string content, Guid authorId);
    Task<OperationResultDTO<Chronicle>> UpdateChronicleAsync(Guid chronicleId, string title, string content, Guid authorId);
    Task<OperationResultDTO<Chronicle>> DeleteChronicleAsync(Guid chronicleId, Guid authorId);
    Task<OperationResultDTO<IEnumerable<Chronicle>>> GetChroniclesBySessionIdAsync(Guid sessionId);
    Task<OperationResultDTO<Chronicle?>> GetChronicleByIdAsync(Guid chronicleId);
}
