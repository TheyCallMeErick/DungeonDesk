namespace DungeonDeskBackend.Domain.Models;

public class AdventureSheetFieldOptions : BaseModel
{
    public string? Value { get; set; }
    public string? Label { get; set; }
    public int Order { get; set; } = 0;
    public List<AdventureSheetTemplateField> AdventureSheetFields { get; set; } = null!;
}
