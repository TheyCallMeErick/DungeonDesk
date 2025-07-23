using DungeonDeskBackend.Domain.Enums;

namespace DungeonDeskBackend.Domain.Models;

public class AdventureSheetTemplateField : BaseModel
{
    public string? Name { get; set; }
    public string? Value { get; set; }
    public bool IsRequired { get; set; }
    public ESheetFieldType? FieldType { get; set; }
    public Guid AdventureSheetTemplateId { get; set; }
    public AdventureSheetTemplate? AdventureSheetTemplate { get; set; }
    public ICollection<AdventureSheetFieldValidations> Validations { get; set; } = new List<AdventureSheetFieldValidations>();
    public ICollection<AdventureSheetFieldOptions>? Options { get; set; } = new List<AdventureSheetFieldOptions>();
}
