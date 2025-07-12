using DungeonDeskBackend.Domain.Models;

namespace DungeonDeskBackend.Application.Services.Interfaces;

public interface IAdventureService
{
    Task<Adventure> GetAdventureByIdAsync(Guid adventureId);
    Task<IEnumerable<Adventure>> GetAdventuresAsync();
    Task<Adventure> CreateAdventureAsync(Adventure adventure);
    Task<Adventure> UpdateAdventureAsync(Adventure adventure);
    Task DeleteAdventureAsync(Guid adventureId);
    Task<IEnumerable<Desk>> GetDesksUsingAdventureAsync(Guid adventureId);
}
