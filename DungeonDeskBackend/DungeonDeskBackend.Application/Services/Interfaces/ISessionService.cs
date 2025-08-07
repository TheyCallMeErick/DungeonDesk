using DungeonDeskBackend.Application.DTOs.Inputs.Session;
using DungeonDeskBackend.Application.DTOs.Outputs;
using DungeonDeskBackend.Application.DTOs.Outputs.Session;
using DungeonDeskBackend.Domain.Models;

namespace DungeonDeskBackend.Application.Services.Interfaces;

public interface ISessionService
{
    Task<OperationResultDTO<Session>> CreateSessionAsync(CreateSessionInputDTO createSessionInputDTO);
    Task<OperationResultDTO<Session>> DeleteSessionAsync(Guid sessionId, Guid playerId);
    Task<OperationResultDTO<IEnumerable<SessionOutputDTO>>> GetSessionsByDeskIdAsync(Guid deskId);
}
