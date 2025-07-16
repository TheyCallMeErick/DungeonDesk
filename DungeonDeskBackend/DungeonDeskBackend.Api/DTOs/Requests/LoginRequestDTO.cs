using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace DungeonDeskBackend.Api.DTOs.Requests; 

public record LoginRequestDTO(
    [property: JsonPropertyName("email")]
    string Email,
    [property: JsonPropertyName("password")]
    string Password
);
