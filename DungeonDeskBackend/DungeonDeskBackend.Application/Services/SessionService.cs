using DungeonDeskBackend.Application.Data;
using DungeonDeskBackend.Application.DTOs.Inputs.Session;
using DungeonDeskBackend.Application.DTOs.Outputs;
using DungeonDeskBackend.Application.DTOs.Outputs.Desk;
using DungeonDeskBackend.Application.DTOs.Outputs.Session;
using DungeonDeskBackend.Application.Extensions;
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

    public async Task<OperationResultDTO<Session>> CreateSessionAsync(CreateSessionInputDTO dto)
    {
        var desk = await _context.Desks.Include(x => x.PlayerDesks).FirstOrDefaultAsync(x => x.Id == dto.deskId);
        if (desk == null)
        {
            throw new KeyNotFoundException("Desk not found");
        }

        var validationResult = Validator.Validate(
            dto: new OnlyTheDeskMasterShoudManageSessionsDTO
            {
                Desk = desk,
                PlayerTryingUpdateId = dto.playerId
            },
            rule: new OnlyTheDeskMasterShoudManageSessions()
        );

        if (!validationResult.IsValid)
        {
            return OperationResultDTO<Session>.FailureResult(validationResult.Message);
        }
        var session = new Session
        {
            DeskId = dto.deskId,
            ScheduledAt = dto.ScheduledAt,
            Notes = dto.Notes,
            Chronicles = new List<Chronicle>(),
            CreatedAt = DateTime.UtcNow
        };
        _context.Sessions.Add(session);
        await _context.SaveChangesAsync();
        return OperationResultDTO<Session>.SuccessResult().
            WithData(session).
            WithMessage("Session created successfully.");
    }

    public async Task<OperationResultDTO<Session>> DeleteSessionAsync(Guid sessionId, Guid playerId)
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
            return OperationResultDTO<Session>.FailureResult(validationResult.Message);
        }
        var session = await _context.Sessions.FindAsync(sessionId);
        if (session == null)
        {
            throw new KeyNotFoundException("Session not found");
        }
        _context.Sessions.Remove(session);
        await _context.SaveChangesAsync();
        return OperationResultDTO<Session>.SuccessResult().
            WithData(session).
            WithMessage("Session deleted successfully.");
    }

    public async Task<OperationResultDTO<IEnumerable<SessionOutputDTO>>> GetSessionsByDeskIdAsync(Guid deskId)
    {
        var sessions = await _context.Sessions
            .Where(x => x.DeskId == deskId)
            .Include(x => x.Chronicles)
            .ToListAsync();
        if (sessions == null || !sessions.Any())
        {
            return OperationResultDTO<IEnumerable<SessionOutputDTO>>.FailureResult($"No sessions found for desk with ID {deskId}.");
        }
        return OperationResultDTO<IEnumerable<SessionOutputDTO>>.SuccessResult()
            .WithData(sessions.ConvertAll(x=> new SessionOutputDTO
            (
                Id : x.Id,
                ScheduledAt : x.ScheduledAt,
                Notes : x.Notes,
                StartedAt : x.StartedAt,
                EndedAt : x.EndedAt,
                Desk: x.Desk?.ToOutputDTO()
            )))
            .WithMessage("Sessions retrieved successfully.")
            .WithPagination(PaginationOutputDTO.Create(sessions.Count, 1, 10));
    }
}
