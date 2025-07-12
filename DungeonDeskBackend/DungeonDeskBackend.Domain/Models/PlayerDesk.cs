using DungeonDeskBackend.Domain.Enums;

namespace DungeonDeskBackend.Domain.Models;

public class PlayerDesk
{
    public Guid PlayerId { get; set; }
    public Player Player { get; set; } = new Player();
    public Guid DeskId { get; set; }
    public Desk Desk { get; set; } = new Desk();
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    public EPlayerDeskRole Role { get; set; }
}
