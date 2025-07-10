namespace DungeonDeskBackend.Domain.Models; 

public class PlayerDesk
{
    public Guid PlayerId { get; set; }
    public Player Player { get; set; }

    public Guid DeskId { get; set; }
    public Desk Desk { get; set; }
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    public string Role { get; set; }
}
