using DungeonDeskBackend.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace DungeonDeskBackend.Api.DTOs.Requests; 

public record GetDesksQueryRequestDTO(
    [property: FromQuery(Name = "name")]
    string? Name = null,
    [property: FromQuery(Name = "description")]
    string? Description = null,
    [property: FromQuery(Name = "table_status")]
    ETableStatus? TableStatus = null,
    [property: FromQuery(Name = "max_players")]
    int? MaxPlayers = null,
    [property: FromQuery(Name = "is_full")]
    bool IsFull = false,
    [property: FromQuery(Name = "page")]
    int page = 1,
    [property: FromQuery(Name = "page_size")]
    int pageSize = 10
);
