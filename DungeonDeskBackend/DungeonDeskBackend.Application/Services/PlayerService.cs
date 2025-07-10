using DungeonDeskBackend.Application.Data;
using DungeonDeskBackend.Application.Services.Interfaces;
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

    public async Task<List<Player>> GetPlayersAsync()
    {
        return await _context.Players.ToListAsync();
    }
    public async Task<Player> GetPlayerByIdAsync(Guid id)
    {
        return await _context.Players.FindAsync(id) ?? throw new KeyNotFoundException("Player not found");
    }

    public async Task<Player> CreatePlayerAsync(Player player)
    {
        _context.Players.Add(player);
        await _context.SaveChangesAsync();
        return player;
    }

    public async Task<Player> UpdatePlayerAsync(Guid id, Player player)
    {
        var existingPlayer = await _context.Players.FindAsync(id);
        if (existingPlayer == null)
        {
            throw new KeyNotFoundException("Player not found");
        }

        existingPlayer.Name = player.Name;
        existingPlayer.UpdatedAt = DateTime.UtcNow;

        _context.Players.Update(existingPlayer);
        await _context.SaveChangesAsync();
        return existingPlayer;
    }

    public async Task DeletePlayerAsync(Guid id)
    {
        var player = await _context.Players.FindAsync(id);
        if (player == null)
        {
            throw new KeyNotFoundException("Player not found");
        }

        _context.Players.Remove(player);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Desk>> GetDesksByPlayerIdAsync(Guid playerId)
    {
        var player = await _context.Players.Include(p => p.PlayerDesks).FirstOrDefaultAsync(p => p.Id == playerId);
        if (player == null)
        {
            throw new KeyNotFoundException("Player not found");
        }
        return player.PlayerDesks.Select(x=>x.Desk).ToList();
    }

    public Task<Desk> JoinDeskAsync(Guid playerId, Guid deskId)
    {
        var player = _context.Players.Find(playerId);
        if (player == null)
        {
            throw new KeyNotFoundException("Player not found");
        }

        var desk = _context.Desks.Find(deskId);
        if (desk == null)
        {
            throw new KeyNotFoundException("Desk not found");
        }

        if(desk.PlayerDesks.Count >= desk.MaxPlayers)
        {
            throw new InvalidOperationException("Desk is full");
        }
        player.PlayerDesks.Add(new PlayerDesk
        {
            PlayerId = player.Id,
            DeskId = desk.Id,
            Role = "Player",
            JoinedAt = DateTime.UtcNow
        });
        _context.Players.Update(player);
        _context.SaveChanges();

        return Task.FromResult(desk);
    }
}
