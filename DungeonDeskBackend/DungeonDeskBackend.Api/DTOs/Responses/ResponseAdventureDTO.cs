using System.Text.Json.Serialization;

namespace DungeonDeskBackend.Api.DTOs.Responses; 

public class ResponseAdventureDTO
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }
    [JsonPropertyName("name")]
    public string Name { get; init; }
    [JsonPropertyName("description")]
    public string Description { get; init; }
    [JsonPropertyName("author_id")]
    public Guid AuthorId { get; init; }
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; init; }
    [JsonPropertyName("updated_at")]
    public DateTime? UpdatedAt { get; init; }

    public ResponseAdventureDTO(Guid id, string name, string description, Guid authorId, DateTime createdAt, DateTime? updatedAt)
    {
        Id = id;
        Name = name;
        Description = description;
        AuthorId = authorId;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
}