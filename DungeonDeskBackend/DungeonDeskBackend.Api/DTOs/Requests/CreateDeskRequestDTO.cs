using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DungeonDeskBackend.Api.DTOs.Requests; 

public record CreateDeskRequestDTO(
    [property: JsonPropertyName("name")]
    [property: Required(ErrorMessage = "Name is required.")]
    [property: MinLength(3, ErrorMessage = "Name must be at least 3 characters long.")]
    [property: MaxLength(100, ErrorMessage = "Name must not exceed 100 characters.")]
    string Name,

    [property: JsonPropertyName("description")]
    [property: MaxLength(500, ErrorMessage = "Description must not exceed 500 characters.")]
    string Description,

    [property: JsonPropertyName("max_players")]
    [property: Range(1, 20, ErrorMessage = "Max players must be between 1 and 20.")]
    int MaxPlayers,

    [property: JsonPropertyName("adventure_id")]
    [property: Required(ErrorMessage = "Adventure ID is required.")]
    Guid AdventureId
);