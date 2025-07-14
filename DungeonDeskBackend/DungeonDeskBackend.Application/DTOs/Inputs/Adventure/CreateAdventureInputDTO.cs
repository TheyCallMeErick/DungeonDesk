namespace DungeonDeskBackend.Application.DTOs.Inputs.Adventure;

public record CreateAdventureInputDTO
{
    public string Title { get; set; }
    public string Description { get; set; }
    public Guid AuthorId { get; set; }
    
    public CreateAdventureInputDTO(string title, string description, Guid authorId)
    {
        Title = title;
        Description = description;
        AuthorId = authorId;
    }
}
