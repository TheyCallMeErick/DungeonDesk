namespace  DungeonDeskBackend.Domain.Models;

public class Player : BaseModel
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public ICollection<Desk> Desks { get; set; }

}
