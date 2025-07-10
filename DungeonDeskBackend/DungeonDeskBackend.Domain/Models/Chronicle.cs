namespace DungeonDeskBackend.Domain.Models;

public class Chronicle : BaseModel
{
    public string Title { get; set; }
    public string Content { get; set; }

    public Guid SessionId { get; set; }
    public Session Session { get; set; }

    public Guid AuthorId { get; set; }
    public Player Author { get; set; }
}
