using DungeonDeskBackend.Application.Data;
using DungeonDeskBackend.Application.Services.Interfaces;
using DungeonDeskBackend.Domain.Models;

namespace DungeonDeskBackend.Application.Services;

public class DeskService : IDeskService
{
    private readonly DungeonDeskDbContext _context;

    public DeskService(DungeonDeskDbContext context)
    {
        _context = context;
    }

    public Task<Desk> CreateDeskAsync(Desk desk)
    {
        _context.Desks.Add(desk);
        _context.SaveChanges();
        return Task.FromResult(desk);
    }

    public Task DeleteDeskAsync(Guid id)
    {
        var desk = _context.Desks.Find(id);
        if (desk == null)
        {
            throw new KeyNotFoundException("Desk not found");
        }

        _context.Desks.Remove(desk);
        _context.SaveChanges();
        return Task.CompletedTask;
    }

    public Task<Desk> GetDeskByIdAsync(Guid id)
    {
        var desk = _context.Desks.Find(id);
        if (desk == null)
        {
            throw new KeyNotFoundException("Desk not found");
        }

        return Task.FromResult(desk);
    }

    public Task<List<Desk>> GetDesksAsync()
    {
        return Task.FromResult(_context.Desks.ToList());
    }

    public Task<List<Player>> GetPlayersByDeskIdAsync(Guid deskId)
    {
        var players = _context.Players.Where(p => p.PlayerDesks.Any(d => d.DeskId == deskId)).ToList();
        return Task.FromResult(players);
    }

    public Task<Desk> UpdateDeskAsync(Guid id, Desk desk)
    {
        var existingDesk = _context.Desks.Find(id);
        if (existingDesk == null)
        {
            throw new KeyNotFoundException("Desk not found");
        }

        existingDesk.Name = desk.Name;
        existingDesk.Description = desk.Description;
        existingDesk.UpdatedAt = DateTime.UtcNow;

        _context.Desks.Update(existingDesk);
        _context.SaveChanges();
        return Task.FromResult(existingDesk);
    }
}
