using System.Collections;
using DungeonDeskBackend.Domain.Enums;

namespace DungeonDeskBackend.Domain.Models;

public class AdventureSheetFieldValidations : BaseModel
{
    public ESheetFieldType? FieldType { get; set; }
    public string Rule { get; set; } = null!;
    public ICollection<AdventureSheetTemplateField>? AdventureSheetFields { get; set; }
}
