namespace DungeonDeskBackend.Domain.Models;

public class AdventureSheetFieldValue : BaseModel
{
    public string? Value { get; set; }
    public AdventureSheetTemplateField AdventureSheetTemplateField { get; set; } = null!;
    public Guid AdventureSheetTemplateFieldId { get; set; }
    public Guid SheetId { get; set; }
    public Sheet Sheet { get; set; } = null!;
}
