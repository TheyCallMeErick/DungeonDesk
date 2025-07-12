using DungeonDeskBackend.Application.Data;
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

    public async Task<IEnumerable<Adventure>> GetAllAdventuresAsync()
    {
        return await _context.Adventures.ToListAsync();
    }
    public async Task<Adventure> GetAdventureByIdAsync(Guid adventureId)
    {
        var adventure = await _context.Adventures.FindAsync(adventureId);
        if (adventure == null)
        {
            throw new KeyNotFoundException($"Adventure with ID {adventureId} not found");
        }
        return adventure;
    }
    public Task<Adventure> CreateAdventureAsync(Adventure adventure)
    {
        if (adventure == null)
        {
            throw new ArgumentNullException(nameof(adventure), "Adventure cannot be null");
        }
        _context.Adventures.Add(adventure);
        return _context.SaveChangesAsync().ContinueWith(t => adventure);

    }

    public async Task<IEnumerable<Desk>> GetDesksUsingAdventureAsync(Guid adventureId)
    {
        var adventure = await _context.Adventures
            .Include(a => a.DesksUsingThis)
            .FirstOrDefaultAsync(a => a.Id == adventureId);

        if (adventure == null)
        {
            throw new KeyNotFoundException($"Adventure with ID {adventureId} not found");
        }

        return adventure.DesksUsingThis;
    }

    public async Task<Adventure> UpdateAdventureAsync(Adventure adventure)
    {
        if (adventure == null)
        {
            throw new ArgumentNullException(nameof(adventure), "Adventure cannot be null");
        }

        var existingAdventure = await _context.Adventures.FindAsync(adventure.Id);
        if (existingAdventure == null)
        {
            throw new KeyNotFoundException($"Adventure with ID {adventure.Id} not found");
        }

        existingAdventure.Title = adventure.Title;
        existingAdventure.Description = adventure.Description;
        existingAdventure.AuthorId = adventure.AuthorId;

        _context.Adventures.Update(existingAdventure);
        await _context.SaveChangesAsync();

        return existingAdventure;
    }

    public Task DeleteAdventureAsync(Guid adventureId)
    {
        var adventure = _context.Adventures.Find(adventureId);
        if (adventure == null)
        {
            throw new KeyNotFoundException($"Adventure with ID {adventureId} not found");
        }

        _context.Adventures.Remove(adventure);
        return _context.SaveChangesAsync();
    }
}
