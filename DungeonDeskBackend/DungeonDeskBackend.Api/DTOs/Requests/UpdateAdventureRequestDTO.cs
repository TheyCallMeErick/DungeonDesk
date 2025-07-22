using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DungeonDeskBackend.Api.DTOs.Requests;

public record UpdateAdventureRequestDTO(
    [property: JsonPropertyName("title")]
    [property: Required(ErrorMessage = "Title is required.")]
    [property: MinLength(3, ErrorMessage = "Title must be at least 3 characters long.")]
    [property: MaxLength(100, ErrorMessage = "Title must not exceed 100 characters.")]
    string Title,
    [property: JsonPropertyName("description")]
    [property: MaxLength(500, ErrorMessage = "Description must not exceed 500 characters.")]
    [property: Required(ErrorMessage = "Description is required.")]
    string Description
);