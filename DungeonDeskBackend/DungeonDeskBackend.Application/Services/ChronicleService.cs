using DungeonDeskBackend.Application.Data;
using DungeonDeskBackend.Application.DTOs.Inputs.Chronicle;
using DungeonDeskBackend.Application.DTOs.Outputs;
using DungeonDeskBackend.Application.Services.Interfaces;
using DungeonDeskBackend.Domain.Models;
using DungeonDeskBackend.Domain.Validations;
using DungeonDeskBackend.Domain.Validations.DTOs;
using DungeonDeskBackend.Domain.Validations.Rules;
using Microsoft.EntityFrameworkCore;

namespace DungeonDeskBackend.Application.Services;

public class ChronicleService : IChronicleService
{
    private readonly DungeonDeskDbContext context;

    public ChronicleService(DungeonDeskDbContext context)
    {
        this.context = context;
    }

    public async Task<OperationResultDTO<Chronicle>> AddChronicleAsync(AddChronicleInputDTO dto)
    {
        var session = await context
            .Sessions
                .Include(x => x.Desk)
                .ThenInclude(x => x.PlayerDesks)
            .FirstOrDefaultAsync(x => x.Id == dto.SessionId);

        if (session == null)
        {
            return OperationResultDTO<Chronicle>.FailureResult($"Session with ID {dto.SessionId} not found.");
        }
        if (session.Desk == null)
        {
            return OperationResultDTO<Chronicle>.FailureResult($"Session with ID {dto.SessionId} does not have a desk associated.");
        }

        var validationResult = Validator.Validate(
            dto: new OnlyTheDeskMasterShoudAddOrUpdateChroniclesValidatorDTO
            {
                Desk = session.Desk,
                PlayerTryingUpdateId = dto.AuthorID
            },
            rule: new OnlyTheDeskMasterShoudAddOrUpdateChronicles()
        );

        if (!validationResult.IsValid)
        {
            return OperationResultDTO<Chronicle>.FailureResult(validationResult.Message);
        }

        var chronicle = new Chronicle
        {
            SessionId = dto.SessionId,
            Title = dto.Title,
            Content = dto.Content,
            AuthorId = dto.AuthorID
        };

        context.Chronicles.Add(chronicle);
        await context.SaveChangesAsync();
        return OperationResultDTO<Chronicle>.SuccessResult()
            .WithData(chronicle)
            .WithMessage("Chronicle added successfully.")
            .WithPagination(PaginationOutputDTO.Create(1, 1, 10));
    }

    public async Task<OperationResultDTO<Chronicle>> UpdateChronicleAsync(UpdateChronicleInputDTO dto)
    {
        var chronicle = await context
            .Chronicles
                .Include(x => x.Session)
                .ThenInclude(x => x.Desk)
                .ThenInclude(x => x.PlayerDesks)
            .FirstOrDefaultAsync(x => x.Id == dto.ChronicleId);

        if (chronicle == null || chronicle.Session == null || chronicle.Session.Desk == null)
        {
            return OperationResultDTO<Chronicle>.FailureResult($"Chronicle with ID {dto.ChronicleId} not found or does not have an associated session or desk.");
        }

        var validationResult = Validator.Validate(
            dto: new OnlyTheDeskMasterShoudAddOrUpdateChroniclesValidatorDTO
            {
                Desk = chronicle.Session.Desk,
                PlayerTryingUpdateId = dto.AuthorId
            },
            rule: new OnlyTheDeskMasterShoudAddOrUpdateChronicles()
        );

        if (!validationResult.IsValid)
        {
            return OperationResultDTO<Chronicle>.FailureResult(validationResult.Message);
        }
        chronicle.Title = dto.Title;
        chronicle.Content = dto.Content;
        context.Chronicles.Update(chronicle);
        await context.SaveChangesAsync();
        return OperationResultDTO<Chronicle>.SuccessResult()
            .WithData(chronicle)
            .WithMessage("Chronicle updated successfully.")
            .WithPagination(PaginationOutputDTO.Create(1, 1, 10));
    }

    public async Task<OperationResultDTO<Chronicle>> DeleteChronicleAsync(DeleteChronicleInputDTO dto)
    {
        var chronicle = await context.Chronicles
        .Include(c => c.Session)
        .ThenInclude(s => s.Desk)
        .ThenInclude(d => d.PlayerDesks)
        .FirstOrDefaultAsync(c => c.Id == dto.ChronicleId);

        if (chronicle == null || chronicle.Session?.Desk == null)
        {
            return OperationResultDTO<Chronicle>.FailureResult($"Chronicle with ID {dto.ChronicleId} not found or does not have an associated session or desk.");
        }

        var validationResult = Validator.Validate(
            new OnlyTheDeskMasterShoudAddOrUpdateChroniclesValidatorDTO
            {
                Desk = chronicle.Session.Desk,
                PlayerTryingUpdateId = dto.AuthorId
            },
            new OnlyTheDeskMasterShoudAddOrUpdateChronicles()
        );

        if (!validationResult.IsValid)
        {
            return OperationResultDTO<Chronicle>.FailureResult(validationResult.Message);
        }

        context.Chronicles.Remove(chronicle);
        await context.SaveChangesAsync();
        return OperationResultDTO<Chronicle>.SuccessResult()
            .WithMessage("Chronicle deleted successfully.");
    }

    public async Task<OperationResultDTO<Chronicle?>> GetChronicleByIdAsync(Guid chronicleId)
    {
        var chronicle = await context.Chronicles
            .FirstOrDefaultAsync(c => c.Id == chronicleId);

        if (chronicle == null)
        {
            return OperationResultDTO<Chronicle?>.FailureResult($"Chronicle with ID {chronicleId} not found.");
        }

        return OperationResultDTO<Chronicle?>.SuccessResult()
            .WithData(chronicle)
            .WithMessage("Chronicle retrieved successfully.");
    }

    public async Task<OperationResultDTO<IEnumerable<Chronicle>>> GetChroniclesBySessionIdAsync(Guid sessionId)
    {
        var chronicles = await context.Chronicles
            .Where(c => c.SessionId == sessionId)
            .ToListAsync();
        if (chronicles == null || !chronicles.Any())
        {
            return OperationResultDTO<IEnumerable<Chronicle>>.FailureResult($"No chronicles found for session with ID {sessionId}.");
        }
        return OperationResultDTO<IEnumerable<Chronicle>>.SuccessResult()
            .WithData(chronicles)
            .WithMessage("Chronicles retrieved successfully.")
            .WithPagination(PaginationOutputDTO.Create(chronicles.Count, 1, 10));
    }
}
