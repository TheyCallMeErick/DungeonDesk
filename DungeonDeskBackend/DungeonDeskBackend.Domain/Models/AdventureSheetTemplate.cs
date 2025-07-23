namespace DungeonDeskBackend.Domain.Models; 

public class AdventureSheetTemplate : BaseModel
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public Guid AdventureId { get; set; }
    public Adventure Adventure { get; set; }
    public int Version { get; set; } = 1;
    public ICollection<AdventureSheetTemplateField> Fields { get; set; } = new List<AdventureSheetTemplateField>();
}
