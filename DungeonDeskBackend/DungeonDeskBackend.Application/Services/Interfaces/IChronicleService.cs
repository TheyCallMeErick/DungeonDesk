using DungeonDeskBackend.Application.DTOs.Outputs;
using DungeonDeskBackend.Domain.Models;

namespace DungeonDeskBackend.Application.Services.Interfaces;

public interface IChronicleService
{
    Task<OperationResultDTO> AddChronicleAsync(Guid sessionId, string title, string content, Guid authorId);
    Task<OperationResultDTO> UpdateChronicleAsync(Guid chronicleId, string title, string content, Guid authorId);
    Task<OperationResultDTO> DeleteChronicleAsync(Guid chronicleId, Guid authorId);
    Task<IEnumerable<Chronicle>> GetChroniclesBySessionIdAsync(Guid sessionId);
    Task<Chronicle?> GetChronicleByIdAsync(Guid chronicleId);
}
