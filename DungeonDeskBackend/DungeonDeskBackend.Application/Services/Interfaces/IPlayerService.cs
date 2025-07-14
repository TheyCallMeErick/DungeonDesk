using DungeonDeskBackend.Application.DTOs.Outputs;
using DungeonDeskBackend.Domain.Models;

namespace DungeonDeskBackend.Application.Services.Interfaces;

public interface IPlayerService
{
    public Task<OperationResultDTO<IEnumerable<Player>>> GetPlayersAsync();
    public Task<OperationResultDTO<Player>> CreatePlayerAsync(Player player);
    public Task<OperationResultDTO<Player>> UpdatePlayerAsync(Guid id, Player player);
    public Task<OperationResultDTO<Player>> DeletePlayerAsync(Guid id);
    public Task<OperationResultDTO<IEnumerable<Desk>>> GetDesksByPlayerIdAsync(Guid playerId);
    public Task<OperationResultDTO<Desk>> JoinDeskAsync(Guid playerId, Guid deskId);
}
