using DungeonDeskBackend.Application.DTOs.Outputs;
using DungeonDeskBackend.Domain.Models;

namespace DungeonDeskBackend.Application.Services.Interfaces;

public interface ISessionService
{
    Task<OperationResultDTO<Session>> CreateSessionAsync(Guid deskId,DateTime ScheduledAt,string Notes , Guid playerId);
    Task<OperationResultDTO<Session>> DeleteSessionAsync(Guid sessionId, Guid playerId);
    Task<OperationResultDTO<IEnumerable<Session>>> GetSessionsByDeskIdAsync(Guid deskId);
}
