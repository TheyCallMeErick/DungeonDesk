using DungeonDeskBackend.Domain.Enums;

namespace DungeonDeskBackend.Domain.Models;

public class Desk : BaseModel
{
    public ICollection<PlayerDesk> PlayerDesks { get; set; } = new List<PlayerDesk>();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ETableStatus Status { get; set; }
    public int MaxPlayers { get; set; }
    public ICollection<Session> Sessions { get; set; } = new List<Session>();
    public Adventure Adventure { get; set; } = new Adventure();
    public Guid? AdventureId { get; set; }
}
