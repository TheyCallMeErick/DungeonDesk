namespace DungeonDeskBackend.Domain.Models;

public class Adventure : BaseModel
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid AuthorId { get; set; }
    public Player Author { get; set; } = new Player();

    public ICollection<Desk> DesksUsingThis { get; set; } = new List<Desk>();
}