using DungeonDeskBackend.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DungeonDeskBackend.Application.Data.Mappings; 

public class AdventureSheetInventoryMapping : IEntityTypeConfiguration<AdventureSheetInventory>
{
    public void Configure(EntityTypeBuilder<AdventureSheetInventory> builder)
    {
        builder.ToTable("AdventureSheetInventories");
        builder.HasMany(x => x.Items)
               .WithOne(i => i.AdventureSheetInventory)
               .HasForeignKey(i => i.AdventureSheetInventoryId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
