using System.Text.Json.Serialization;

namespace DungeonDeskBackend.Api.DTOs.Responses; 

public record ResponseSessionDTO(
    [property: JsonPropertyName("id")]
    Guid Id,
    [property: JsonPropertyName("scheduled_at")]
    DateTime ScheduledAt,
    [property: JsonPropertyName("desk")]
    ResponseDeskDTO? Desk,
    [property: JsonPropertyName("started_at")]
    DateTime? StartedAt,
    [property: JsonPropertyName("ended_at")]
    DateTime? EndedAt,
    [property: JsonPropertyName("notes")]
    string Notes
);
