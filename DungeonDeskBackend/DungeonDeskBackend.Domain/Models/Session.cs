namespace DungeonDeskBackend.Domain.Models;

public class Session : BaseModel
{
    public DateTime ScheduledAt { get; set; }

    public Guid DeskId { get; set; }
    public Desk? Desk { get; set; }

    public DateTime? StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }

    public string Notes { get; set; } = string.Empty;
    public ICollection<Chronicle> Chronicles { get; set; } = new List<Chronicle>();
}
