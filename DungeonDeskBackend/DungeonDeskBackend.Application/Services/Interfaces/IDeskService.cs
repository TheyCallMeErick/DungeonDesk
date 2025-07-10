using DungeonDeskBackend.Domain.Models;

namespace DungeonDeskBackend.Application.Services.Interfaces;

public interface IDeskService
{
    Task<List<Desk>> GetDesksAsync();
    Task<Desk> GetDeskByIdAsync(Guid id);
    Task<Desk> CreateDeskAsync(Desk desk);
    Task<Desk> UpdateDeskAsync(Guid id, Desk desk);
    Task DeleteDeskAsync(Guid id);
    Task<List<Player>> GetPlayersByDeskIdAsync(Guid deskId);
}
