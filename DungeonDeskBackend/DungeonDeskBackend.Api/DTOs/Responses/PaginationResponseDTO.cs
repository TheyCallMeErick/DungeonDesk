using System.Text.Json.Serialization;

namespace DungeonDeskBackend.Api.DTOs.Responses; 

public record PaginationResponseDTO(
    [property: JsonPropertyName("total_count")]
    int TotalCount,
    [property: JsonPropertyName("page_size")]
    int PageSize,
    [property: JsonPropertyName("current_page")]
    int CurrentPage,
    [property: JsonPropertyName("total_pages")]
    int TotalPages
);
