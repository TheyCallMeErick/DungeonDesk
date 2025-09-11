using System.Text.Json.Serialization;

namespace DungeonDeskBackend.Api.DTOs.Responses;

public record UserResponseDTO(
    [property: JsonPropertyName("id")]
    Guid Id,
    [property: JsonPropertyName("username")]
    string? Username,
    [property: JsonPropertyName("name")]
    string? Name,
    [property: JsonPropertyName("email")]
    string? Email,
    [property: JsonPropertyName("profile_picture_file_name")]
    string? ProfilePictureFileName
);
