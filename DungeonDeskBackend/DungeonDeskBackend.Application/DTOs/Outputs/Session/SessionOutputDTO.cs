using DungeonDeskBackend.Application.DTOs.Outputs.Desk;

namespace DungeonDeskBackend.Application.DTOs.Outputs.Session; 

public record SessionOutputDTO(
    Guid Id,
    DateTime ScheduledAt,
    DeskOutputDTO? Desk,
    DateTime? StartedAt,
    DateTime? EndedAt,
    string Notes
);
