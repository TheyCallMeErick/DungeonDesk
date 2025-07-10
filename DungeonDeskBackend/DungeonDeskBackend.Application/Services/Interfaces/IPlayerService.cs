using DungeonDeskBackend.Domain.Models;

namespace DungeonDeskBackend.Application.Services.Interfaces;

public interface IPlayerService
{
    public Task<List<Player>> GetPlayersAsync();
    public Task<Player> CreatePlayerAsync(Player player);
    public Task<Player> UpdatePlayerAsync(Guid id, Player player);
    public Task DeletePlayerAsync(Guid id);
    public Task<List<Desk>> GetDesksByPlayerIdAsync(Guid playerId);
    public Task<Desk> JoinDeskAsync(Guid playerId, Guid deskId);
}
