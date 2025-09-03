namespace DungeonDeskBackend.Domain.Models;

public class Player : BaseModel
{
    public ICollection<PlayerDesk> PlayerDesks { get; set; } = new List<PlayerDesk>();
    public User User { get; set; }
    public Guid UserId { get; set; }
}
