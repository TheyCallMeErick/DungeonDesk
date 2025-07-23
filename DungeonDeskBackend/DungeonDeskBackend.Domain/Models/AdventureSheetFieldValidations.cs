using DungeonDeskBackend.Domain.Enums;

namespace DungeonDeskBackend.Domain.Models;

public class AdventureSheetFieldValidations : BaseModel
{
    public ESheetFieldType? FieldType { get; set; }
    public string? ValidationValue { get; set; }
    public ICollection<AdventureSheetTemplateField>? AdventureSheetFields { get; set; }
}
