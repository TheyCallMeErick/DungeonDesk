namespace DungeonDeskBackend.Domain.Models;

public class Adventure : BaseModel
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid AuthorId { get; set; }
    public Player? Author { get; set; }

    public ICollection<Desk> DesksUsingThis { get; set; } = new List<Desk>();
    public ICollection<AdventureSheetTemplate> AdventureSheetTemplates { get; set; } = new List<AdventureSheetTemplate>();
}
