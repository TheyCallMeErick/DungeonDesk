namespace DungeonDeskBackend.Domain.Models;

public class Sheet : BaseModel
{
    public Player Player { get; set; } = null!;
    public Guid PlayerId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public AdventureSheetTemplate AdventureSheetTemplate { get; set; } = null!;
    public Guid AdventureSheetTemplateId { get; set; }
    public ICollection<AdventureSheetField> Fields { get; set; } = new List<AdventureSheetField>();
}
