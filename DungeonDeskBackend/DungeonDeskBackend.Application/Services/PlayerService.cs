using DungeonDeskBackend.Application.Data;
using DungeonDeskBackend.Application.DTOs.Outputs;
using DungeonDeskBackend.Application.Services.Interfaces;
using DungeonDeskBackend.Domain.Enums;
using DungeonDeskBackend.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DungeonDeskBackend.Application.Services;

public class PlayerService : IPlayerService
{
    private readonly DungeonDeskDbContext _context;

    public PlayerService(DungeonDeskDbContext context)
    {
        _context = context;
    }

    public async Task<OperationResultDTO<IEnumerable<Player>>> GetPlayersAsync()
    {
        var data = await _context.Players.ToListAsync();
        return OperationResultDTO<IEnumerable<Player>>
            .SuccessResult()
            .WithData(data)
            .WithMessage("Players retrieved successfully.");
    }
    public async Task<OperationResultDTO<Player>> GetPlayerByIdAsync(Guid id)
    {
        var data = await _context.Players.FirstOrDefaultAsync(x => x.Id == id);
        if (data == null)
        {
            return OperationResultDTO<Player>
                .FailureResult($"Player with ID {id} not found.");
        }
        return OperationResultDTO<Player>
            .SuccessResult()
            .WithData(data)
            .WithMessage("Player retrieved successfully.");
    }

    public async Task<OperationResultDTO<Player>> CreatePlayerAsync(Player player)
    {
        _context.Players.Add(player);
        await _context.SaveChangesAsync();
        return OperationResultDTO<Player>
            .SuccessResult()
            .WithData(player)
            .WithMessage("Player created successfully.");
    }

    public async Task<OperationResultDTO<Player>> UpdatePlayerAsync(Guid id, Player player)
    {
        var existingPlayer = await _context.Players.FindAsync(id);
        if (existingPlayer == null)
        {
            return OperationResultDTO<Player>
                .FailureResult($"Player with ID {id} not found.");
        }

        existingPlayer.Name = player.Name;
        existingPlayer.UpdatedAt = DateTime.UtcNow;

        _context.Players.Update(existingPlayer);
        await _context.SaveChangesAsync();
        return OperationResultDTO<Player>
            .SuccessResult()
            .WithData(existingPlayer)
            .WithMessage("Player updated successfully.");
    }

    public async Task<OperationResultDTO<Player>> DeletePlayerAsync(Guid id)
    {
        var player = await _context.Players.FirstOrDefaultAsync(x => x.Id == id);
        if (player == null)
        {
            return OperationResultDTO<Player>
                .FailureResult($"Player with ID {id} not found.");
        }

        _context.Players.Remove(player);
        await _context.SaveChangesAsync();
        return OperationResultDTO<Player>
            .SuccessResult()
            .WithMessage("Player deleted successfully.");
    }

    public async Task<OperationResultDTO<IEnumerable<Desk>>> GetDesksByPlayerIdAsync(Guid playerId)
    {
        var desks = await _context.Desks
            .Include(p => p.PlayerDesks)
                .Where(p => p.PlayerDesks.Any(
                        pd => pd.PlayerId == playerId
                        )
                    )
            .ToListAsync();
        if (!desks.Any())
        {
            return OperationResultDTO<IEnumerable<Desk>>
                .FailureResult($"No desks found for player with ID {playerId}.");
        }
        return OperationResultDTO<IEnumerable<Desk>>
            .SuccessResult()
            .WithData(desks)
            .WithMessage("Desks retrieved successfully.")
            .WithPagination(PaginationOutputDTO.Create(desks.Count, 1, 10));
    }

    public async Task<OperationResultDTO<Desk>> JoinDeskAsync(Guid playerId, Guid deskId)
    {
        var player = await _context.Players.FirstOrDefaultAsync(x=>x.Id == playerId);
        if (player == null)
        {
            return OperationResultDTO<Desk>
                .FailureResult($"Player with ID {playerId} not found.");
        }

        var desk = _context.Desks.Find(deskId);
        if (desk == null)
        {
            return OperationResultDTO<Desk>
                .FailureResult($"Desk with ID {deskId} not found.");
        }

        if (desk.PlayerDesks.Count >= desk.MaxPlayers)
        {
            return OperationResultDTO<Desk>
                .FailureResult($"Desk with ID {deskId} is full.");
        }
        if( player.PlayerDesks.Any(pd => pd.DeskId == deskId))
        {
            return OperationResultDTO<Desk>
                .FailureResult($"Player with ID {playerId} is already in desk with ID {deskId}.");
        }
        player.PlayerDesks.Add(new PlayerDesk
        {
            PlayerId = player.Id,
            DeskId = desk.Id,
            Role = EPlayerDeskRole.Player,
            JoinedAt = DateTime.UtcNow
        });
        _context.Players.Update(player);
        await _context.SaveChangesAsync();

        return OperationResultDTO<Desk>
            .SuccessResult()
            .WithData(desk)
            .WithMessage("Player joined desk successfully.");
    }
}
