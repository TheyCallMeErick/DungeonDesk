using DungeonDeskBackend.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DungeonDeskBackend.Application.Data.Mappings; 

public class AdventureSheetFieldValueMapping : IEntityTypeConfiguration<AdventureSheetFieldValue>
{

    public void Configure(EntityTypeBuilder<AdventureSheetFieldValue> builder)
    {
        builder.Property(x => x.Value).IsRequired();

        builder.HasOne(x => x.AdventureSheetTemplateField)
               .WithMany()
               .HasForeignKey(x => x.AdventureSheetTemplateFieldId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Sheet)
               .WithMany(s => s.Fields)
               .HasForeignKey(x => x.SheetId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
