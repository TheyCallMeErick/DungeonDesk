namespace DungeonDeskBackend.Domain.Models;

public class InventoryItem
{
    public Guid AdventureSheetInventoryId { get; set; }
    public AdventureSheetInventory AdventureSheetInventory { get; set; } = null!;
    public Guid ItemId { get; set; }
    public Item Item { get; set; } = null!;
    public int Quantity { get; set; }
}
