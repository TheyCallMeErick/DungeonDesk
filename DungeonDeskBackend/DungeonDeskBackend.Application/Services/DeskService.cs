using DungeonDeskBackend.Application.Data;
using DungeonDeskBackend.Application.DTOs.Outputs;
using DungeonDeskBackend.Application.Services.Interfaces;
using DungeonDeskBackend.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DungeonDeskBackend.Application.Services;

public class DeskService : IDeskService
{
    private readonly DungeonDeskDbContext _context;

    public DeskService(DungeonDeskDbContext context)
    {
        _context = context;
    }

    public async Task<OperationResultDTO<Desk>> CreateDeskAsync(Desk desk)
    {
        await _context.Desks.AddAsync(desk);
        await _context.SaveChangesAsync();
        return OperationResultDTO<Desk>
            .SuccessResult()
            .WithData(desk)
            .WithMessage("Desk created successfully.");
    }

    public async Task<OperationResultDTO<Desk>> DeleteDeskAsync(Guid id)
    {
        var desk = _context.Desks.Find(id);
        if (desk == null)
        {
            return OperationResultDTO<Desk>
                .FailureResult($"Desk with ID {id} not found.");
        }

        _context.Desks.Remove(desk);
        await  _context.SaveChangesAsync();
        return OperationResultDTO<Desk>
            .SuccessResult()
            .WithMessage("Desk deleted successfully.");
    }

    public async Task<OperationResultDTO<Desk>> GetDeskByIdAsync(Guid id)
    {
        var desk = await _context.Desks.FirstOrDefaultAsync(x=> x.Id == id);
        if (desk == null)
        {
            return OperationResultDTO<Desk>
                .FailureResult($"Desk with ID {id} not found.");
        }

        return OperationResultDTO<Desk>
            .SuccessResult()
            .WithData(desk)
            .WithMessage("Desk retrieved successfully.");
    }

    public Task<OperationResultDTO<IEnumerable<Desk>>> GetDesksAsync()
    {
        var data = _context.Desks.ToList();
        if (data == null || !data.Any())
        {
            return Task.FromResult(OperationResultDTO<IEnumerable<Desk>>
                .FailureResult("No desks found."));
        }
        return Task.FromResult(OperationResultDTO<IEnumerable<Desk>>
            .SuccessResult()
            .WithData(data)
            .WithMessage("Desks retrieved successfully.")
            .WithPagination(PaginationOutputDTO.Create(data.Count, 1, 10)));
    }

    public async Task<OperationResultDTO<IEnumerable<Player>>> GetPlayersByDeskIdAsync(Guid deskId)
    {
        var players = await _context
            .Players
            .Where(p => p.PlayerDesks.Any(d => d.DeskId == deskId))
            .ToListAsync();
        if (players == null || !players.Any())
        {
            return OperationResultDTO<IEnumerable<Player>>
                .FailureResult($"No players found for desk with ID {deskId}.");
        }
        return OperationResultDTO<IEnumerable<Player>>
            .SuccessResult()
            .WithData(players)
            .WithMessage("Players retrieved successfully.")
            .WithPagination(PaginationOutputDTO.Create(players.Count, 1, 10));
    }

    public async Task<OperationResultDTO<Desk>> UpdateDeskAsync(Guid id, Desk desk)
    {
        var existingDesk = await _context.Desks.FirstOrDefaultAsync(x => x.Id == id);
        if (existingDesk == null)
        {
            return OperationResultDTO<Desk>
                .FailureResult($"Desk with ID {id} not found.");
        }

        existingDesk.Name = desk.Name;
        existingDesk.Description = desk.Description;
        existingDesk.UpdatedAt = DateTime.UtcNow;

        _context.Desks.Update(existingDesk);
        await _context.SaveChangesAsync();
        return OperationResultDTO<Desk>
            .SuccessResult()
            .WithData(existingDesk)
            .WithMessage("Desk updated successfully.");
    }
}
