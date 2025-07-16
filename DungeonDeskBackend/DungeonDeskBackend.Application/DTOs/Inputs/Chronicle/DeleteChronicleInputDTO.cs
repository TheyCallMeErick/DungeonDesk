namespace DungeonDeskBackend.Application.DTOs.Inputs.Chronicle;

public record DeleteChronicleInputDTO
{
    public Guid ChronicleId { get; set; }
    public Guid AuthorId { get; set; }
    
    public DeleteChronicleInputDTO(Guid chronicleId, Guid authorId)
    {
        ChronicleId = chronicleId;
        AuthorId = authorId;
    }
}
