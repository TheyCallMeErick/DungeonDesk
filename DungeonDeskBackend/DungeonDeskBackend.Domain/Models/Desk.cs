using DungeonDeskBackend.Domain.Enums;

namespace  DungeonDeskBackend.Domain.Models;

public class Desk : BaseModel
{
    public ICollection<Player> Players { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string MasterId { get; set; }
    public Player Master { get; set; }
    public ETableStatus Status { get; set; }
    public int MaxPlayers { get; set; }
    public ICollection<Session> Sessions { get; set; }
    public Adventure Adventure { get; set; }
    public Guid? AdventureId { get; set; }
}
