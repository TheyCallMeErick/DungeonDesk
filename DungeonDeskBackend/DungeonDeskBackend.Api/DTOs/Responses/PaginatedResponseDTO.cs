using System.Text.Json.Serialization;

namespace DungeonDeskBackend.Api.DTOs.Responses;

public record PaginatedResponseDTO<T>(
    [property: JsonPropertyName("items")]
    [property: JsonRequired]
    IEnumerable<T> Items,
    [property: JsonPropertyName("pagination")]
    [property: JsonRequired]
    PaginationResponseDTO Pagination
) where T : notnull;

