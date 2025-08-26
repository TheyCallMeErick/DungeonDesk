using DungeonDeskBackend.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DungeonDeskBackend.Application.Data.Mappings; 

public class InventoryItemMapping : IEntityTypeConfiguration<InventoryItem>
{
    public void Configure(EntityTypeBuilder<InventoryItem> builder)
    {
        builder.HasOne(x => x.AdventureSheetInventory)
               .WithMany(i => i.Items)
               .HasForeignKey(x => x.AdventureSheetInventoryId)
               .OnDelete(DeleteBehavior.Cascade);
        builder.Property(x => x.Quantity).IsRequired();

        builder.HasOne(x => x.Item)
               .WithMany(i => i.InventoryItems)
               .HasForeignKey(x => x.ItemId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
