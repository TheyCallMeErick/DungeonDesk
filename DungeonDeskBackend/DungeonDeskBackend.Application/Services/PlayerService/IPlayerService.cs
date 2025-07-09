using DungeonDeskBackend.Domain.Models;

namespace DungeonDeskBackend.Application.Services.PlayerService;

public interface IPlayerService
{
    public Task<List<Player>> GetPlayersAsync();
    public Task<Player> CreatePlayerAsync(Player player);
    public Task<Player> UpdatePlayerAsync(Guid id, Player player);
    public Task DeletePlayerAsync(Guid id);
    public Task<List<Desk>> GetDesksByPlayerIdAsync(Guid playerId);
}
