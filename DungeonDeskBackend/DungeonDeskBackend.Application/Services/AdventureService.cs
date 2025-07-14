using DungeonDeskBackend.Application.Data;
using DungeonDeskBackend.Application.DTOs.Outputs;
using DungeonDeskBackend.Application.Services.Interfaces;
using DungeonDeskBackend.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DungeonDeskBackend.Application.Services;

public class AdventureService : IAdventureService
{
    private readonly DungeonDeskDbContext _context;

    public AdventureService(DungeonDeskDbContext context)
    {
        _context = context;
    }

    public async Task<OperationResultDTO<IEnumerable<Adventure>>> GetAdventuresAsync()
    {
        var data = await _context.Adventures.ToListAsync();
        if (data == null || !data.Any())
        {
            return OperationResultDTO<IEnumerable<Adventure>>
                .FailureResult("No adventures found.");
        }
        return OperationResultDTO<IEnumerable<Adventure>>
            .SuccessResult()
            .WithData(data)
            .WithMessage("Adventures retrieved successfully.")
            .WithPagination(PaginationOutputDTO.Create(data.Count, 1, 10));
    }
    public async Task<OperationResultDTO<Adventure>> GetAdventureByIdAsync(Guid adventureId)
    {
        var adventure = await _context.Adventures.FindAsync(adventureId);
        if (adventure == null)
        {
            return OperationResultDTO<Adventure>
                .FailureResult($"Adventure with ID {adventureId} not found.");
        }
        return OperationResultDTO<Adventure>
            .SuccessResult()
            .WithData(adventure)
            .WithMessage("Adventure retrieved successfully.");
    }
    public async Task<OperationResultDTO<Adventure>> CreateAdventureAsync(Adventure adventure)
    {
        if (adventure == null)
        {
            return OperationResultDTO<Adventure>
                .FailureResult("Adventure cannot be null");
        }
        await _context.Adventures.AddAsync(adventure);
        await _context.SaveChangesAsync();
        return OperationResultDTO<Adventure>
            .SuccessResult()
            .WithData(adventure)
            .WithMessage("Adventure created successfully.");
    }

    public async Task<OperationResultDTO<IEnumerable<Desk>>> GetDesksUsingAdventureAsync(Guid adventureId)
    {
        var adventure = await _context.Adventures
            .Include(a => a.DesksUsingThis)
            .FirstOrDefaultAsync(a => a.Id == adventureId);

        if (adventure == null)
        {
            return OperationResultDTO<IEnumerable<Desk>>
                .FailureResult($"Adventure with ID {adventureId} not found.");
        }

        return OperationResultDTO<IEnumerable<Desk>>
            .SuccessResult()
            .WithData(adventure.DesksUsingThis)
            .WithMessage("Desks using this adventure retrieved successfully.")
            .WithPagination(PaginationOutputDTO.Create(adventure.DesksUsingThis.Count, 1, 10));
    }

    public async Task<OperationResultDTO<Adventure>> UpdateAdventureAsync(Adventure adventure)
    {
        if (adventure == null)
        {
            return OperationResultDTO<Adventure>
                .FailureResult("Adventure cannot be null");
        }

        var existingAdventure = await _context.Adventures.FindAsync(adventure.Id);

        if (existingAdventure == null)
        {
            return OperationResultDTO<Adventure>
                .FailureResult($"Adventure with ID {adventure.Id} not found.");
        }

        existingAdventure.Title = adventure.Title;
        existingAdventure.Description = adventure.Description;
        existingAdventure.AuthorId = adventure.AuthorId;

        _context.Adventures.Update(existingAdventure);
        await _context.SaveChangesAsync();

        return OperationResultDTO<Adventure>
            .SuccessResult()
            .WithData(existingAdventure)
            .WithMessage("Adventure updated successfully.")
            .WithPagination(PaginationOutputDTO.Create(1, 1, 10));
    }

    public Task<OperationResultDTO<Adventure>> DeleteAdventureAsync(Guid adventureId)
    {
        var adventure = _context.Adventures.FirstOrDefault(x => x.Id == adventureId);
        if (adventure == null)
        {
            return Task.FromResult(OperationResultDTO<Adventure>
                .FailureResult($"Adventure with ID {adventureId} not found."));
        }

        _context.Adventures.Remove(adventure);
        _context.SaveChangesAsync();
        return Task.FromResult(OperationResultDTO<Adventure>
            .SuccessResult()
            .WithMessage("Adventure deleted successfully.")
            .WithData(adventure));
    }
}
