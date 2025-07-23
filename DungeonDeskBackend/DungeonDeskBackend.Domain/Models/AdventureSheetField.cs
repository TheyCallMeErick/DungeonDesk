namespace DungeonDeskBackend.Domain.Models;

public class AdventureSheetField : BaseModel
{
    public string? Value { get; set; }
    public AdventureSheetTemplateField AdventureSheetTemplateField { get; set; } = null!;
    public Guid AdventureSheetTemplateFieldId { get; set; }
}
