namespace DungeonDeskBackend.Domain.Models;

public class BaseModel
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = null;
}
