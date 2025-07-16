namespace DungeonDeskBackend.Application.DTOs.Inputs.Chronicle;

public record AddChronicleInputDTO
{

    public Guid SessionId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public Guid AuthorID { get; set; }
   
    public AddChronicleInputDTO(Guid sessionId, string title, string content, Guid authorId)
    {
        SessionId = sessionId;
        Title = title;
        Content = content;
        AuthorID = authorId;
    }
}
