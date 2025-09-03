namespace DungeonDeskBackend.Domain.Models;

public class User : BaseModel
{
    public string? Username { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? ProfilePictureFileName { get; set; }

    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public Player? Player { get; set; }
    public Guid? PlayerId { get; set; }
}
