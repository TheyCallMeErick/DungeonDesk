using System.Text.Json.Serialization;

namespace DungeonDeskBackend.Api.DTOs.Responses; 

public record ResponseLoginDTO(
    [property: JsonPropertyName("access_token")]
    string AccessToken,
    [property: JsonPropertyName("refresh_token")]
    string RefreshToken
);
