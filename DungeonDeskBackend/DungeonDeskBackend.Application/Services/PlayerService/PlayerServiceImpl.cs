using DungeonDeskBackend.Application.Data;
using DungeonDeskBackend.Application.Services.PlayerService;
using DungeonDeskBackend.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DungeonDeskBackend.Application.Services;

public class PlayerServiceImpl : IPlayerService
{
    private readonly DungeonDeskDbContext _context;

    public PlayerServiceImpl(DungeonDeskDbContext context)
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
        var player = await _context.Players.Include(p => p.Desks).FirstOrDefaultAsync(p => p.Id == playerId);
        if (player == null)
        {
            throw new KeyNotFoundException("Player not found");
        }
        return player.Desks.ToList();
    }
}
