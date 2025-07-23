namespace DungeonDeskBackend.Domain.Models;

public class AdventureSheetFieldOptions : BaseModel
{
    public string? Value { get; set; }
    public string? Label { get; set; }
    public int Order { get; set; } = 0;
    public Guid AdventureSheetFieldId { get; set; }
    public AdventureSheetTemplateField AdventureSheetField { get; set; } = null!;
}
