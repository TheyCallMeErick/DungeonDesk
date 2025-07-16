namespace DungeonDeskBackend.Application.DTOs.Inputs.Chronicle;

public record UpdateChronicleInputDTO
{

    public Guid ChronicleId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public Guid AuthorId { get; set; }
    public UpdateChronicleInputDTO(Guid chronicleId, string title, string content, Guid authorId)
    {
        ChronicleId = chronicleId;
        Title = title;
        Content = content;
        AuthorId = authorId;
    }
}