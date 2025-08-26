using System.ComponentModel.DataAnnotations;

namespace DungeonDeskBackend.Api.DTOs.Requests; 

public record CreateSessionRequestDTO(
    [property: Required]
    Guid deskId,
    [property: Required]
    DateTime ScheduledAt,
    string Notes 
);
