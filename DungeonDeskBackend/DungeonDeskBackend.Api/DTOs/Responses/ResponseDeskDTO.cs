using System.Text.Json.Serialization;

namespace DungeonDeskBackend.Api.DTOs.Responses; 

public record ResponseDeskDTO(
    [property: JsonPropertyName("id")]
    Guid Id,
    [property: JsonPropertyName("name")]
    string Name,
    [property: JsonPropertyName("description")]
    string Description,
    [property: JsonPropertyName("status")]
    string Status,
    [property: JsonPropertyName("max_players")]
    int MaxPlayers,
    [property: JsonPropertyName("adventure_id")]
    Guid? AdventureId
);
