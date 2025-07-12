using DungeonDeskBackend.Application.DTOs.Outputs;
using DungeonDeskBackend.Domain.Models;

namespace DungeonDeskBackend.Application.Services.Interfaces;

public interface ISessionService
{
    Task<OperationResultDTO> CreateSessionAsync(Guid deskId,DateTime ScheduledAt,string Notes , Guid playerId);
    Task<OperationResultDTO> DeleteSessionAsync(Guid sessionId, Guid playerId);
    Task<IEnumerable<Session>> GetSessionsByDeskIdAsync(Guid deskId);
}
