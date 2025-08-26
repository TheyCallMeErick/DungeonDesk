namespace DungeonDeskBackend.Domain.Models;

public class Sheet : BaseModel
{
    public PlayerDesk PlayerDesk { get; set; } = null!;
    public Guid PlayerDeskId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public AdventureSheetTemplate AdventureSheetTemplate { get; set; } = null!;
    public Guid AdventureSheetTemplateId { get; set; }
    public ICollection<AdventureSheetFieldValue> Fields { get; set; } = new List<AdventureSheetFieldValue>();
}
