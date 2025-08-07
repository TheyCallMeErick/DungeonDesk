namespace DungeonDeskBackend.Application.DTOs.Inputs.SheetTemplate; 

public record CreateSheetTemplateDTO(
    string Title,
    string Description,
    Guid AdventureId
);
