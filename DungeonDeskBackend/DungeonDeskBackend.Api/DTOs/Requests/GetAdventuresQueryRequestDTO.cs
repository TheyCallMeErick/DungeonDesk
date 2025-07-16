using Microsoft.AspNetCore.Mvc;

namespace DungeonDeskBackend.Api.DTOs.Requests;

public record GetAdventuresQueryRequestDTO(
    [property: FromQuery(Name = "name")]
    string? name,
    [property: FromQuery(Name = "description")]
    string? description,
    [property: FromQuery(Name = "author_id")]
    string? author,
    [property: FromQuery(Name = "page")]
    int page = 1,
    [property: FromQuery(Name = "page_size")]
    int pageSize = 10
);
