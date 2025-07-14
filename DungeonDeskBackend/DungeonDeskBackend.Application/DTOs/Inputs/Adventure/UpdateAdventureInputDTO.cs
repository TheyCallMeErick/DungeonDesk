namespace DungeonDeskBackend.Application.DTOs.Inputs.Adventure;

public class UpdateAdventureInputDTO
{
    public string Title { get; set; }
    public string Description { get; set; }
    
    public UpdateAdventureInputDTO(string title, string description)
    {
        Title = title;
        Description = description;
    }
}
