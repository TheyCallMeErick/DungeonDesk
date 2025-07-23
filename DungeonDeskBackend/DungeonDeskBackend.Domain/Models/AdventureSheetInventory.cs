namespace DungeonDeskBackend.Domain.Models;

public class AdventureSheetInventory : BaseModel
{
    public Guid AdventureSheetId { get; set; }
    public Sheet AdventureSheet { get; set; } = null!;
    public List<InventoryItem> Items { get; set; } = new List<InventoryItem>();
}
