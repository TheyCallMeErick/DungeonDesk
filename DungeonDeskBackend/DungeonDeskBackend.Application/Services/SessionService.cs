using DungeonDeskBackend.Application.Data;
using DungeonDeskBackend.Application.DTOs.Outputs;
using DungeonDeskBackend.Application.Services.Interfaces;
using DungeonDeskBackend.Domain.Models;
using DungeonDeskBackend.Domain.Validations;
using DungeonDeskBackend.Domain.Validations.DTOs;
using DungeonDeskBackend.Domain.Validations.Rules;
using Microsoft.EntityFrameworkCore;

namespace DungeonDeskBackend.Application.Services; 

public class SessionService : ISessionService
{
    private readonly DungeonDeskDbContext _context;

    public SessionService(DungeonDeskDbContext context)
    {
        _context = context;
    }

    public async Task<OperationResultDTO> CreateSessionAsync(Guid deskId, DateTime ScheduledAt, string Notes, Guid playerId)
    {
        var desk = await _context.Desks.Include(x => x.PlayerDesks).FirstOrDefaultAsync(x => x.Id == deskId);
        if (desk == null)
        {
            throw new KeyNotFoundException("Desk not found");
        }

        var validationResult = Validator.Validate(
            dto: new OnlyTheDeskMasterShoudManageSessionsDTO
            {
                Desk = desk,
                PlayerTryingUpdateId = playerId
            },
            rule: new OnlyTheDeskMasterShoudManageSessions()
        );

        if (!validationResult.IsValid)
        {
            return OperationResultDTO.FailureResult(validationResult.Message);
        }
        var session = new Session
        {
            DeskId = deskId,
            ScheduledAt = ScheduledAt,
            Notes = Notes,
            Chronicles = new List<Chronicle>(),
            CreatedAt = DateTime.UtcNow
        };
        _context.Sessions.Add(session);
        await _context.SaveChangesAsync();
        return OperationResultDTO.SuccessResult("Session created successfully");
    }

    public async Task<OperationResultDTO> DeleteSessionAsync(Guid sessionId, Guid playerId)
    {
        var desk = await _context.Desks.Include(x => x.PlayerDesks).FirstOrDefaultAsync(x => x.Sessions.Any(x=>x.Id==sessionId));
        if (desk == null)
        {
            throw new KeyNotFoundException("Desk not found");
        }

        var validationResult = Validator.Validate(
            dto: new OnlyTheDeskMasterShoudManageSessionsDTO
            {
                Desk = desk,
                PlayerTryingUpdateId = playerId
            },
            rule: new OnlyTheDeskMasterShoudManageSessions()
        );

        if (!validationResult.IsValid)
        {
            return OperationResultDTO.FailureResult(validationResult.Message);
        }
        var session = await _context.Sessions.FindAsync(sessionId);
        if (session == null)
        {
            throw new KeyNotFoundException("Session not found");
        }
        _context.Sessions.Remove(session);
        await _context.SaveChangesAsync();
        return OperationResultDTO.SuccessResult("Session deleted successfully");
    }

    public async Task<IEnumerable<Session>> GetSessionsByDeskIdAsync(Guid deskId)
    {
        return await _context.Sessions
            .Where(x => x.DeskId == deskId)
            .Include(x => x.Chronicles)
            .ToListAsync();
    }
}
