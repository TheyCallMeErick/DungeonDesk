namespace DungeonDeskBackend.Domain.Models;

public class Player : BaseModel
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public ICollection<PlayerDesk> PlayerDesks { get; set; } = new List<PlayerDesk>();
}
