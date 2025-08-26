using DungeonDeskBackend.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DungeonDeskBackend.Application.Data.Mappings;


public class AdventureSheetMapping : IEntityTypeConfiguration<Sheet>
{

    public void Configure(EntityTypeBuilder<Sheet> builder)
    {
        builder.HasOne(x => x.PlayerDesk)
               .WithOne(pd => pd.ActiveSheet)
               .HasForeignKey<Sheet>(s => s.PlayerDeskId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.Name)
               .HasMaxLength(255);
        builder.Property(x => x.Description)
               .HasMaxLength(1000);
        builder.HasMany(x => x.Fields)
               .WithOne(f => f.Sheet)
               .HasForeignKey(f => f.SheetId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
