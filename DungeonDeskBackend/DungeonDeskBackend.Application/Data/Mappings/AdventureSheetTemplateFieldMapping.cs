using DungeonDeskBackend.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DungeonDeskBackend.Application.Data.Mappings; 

public class AdventureSheetTemplateFieldMapping : IEntityTypeConfiguration<AdventureSheetTemplateField>
{

    public void Configure(EntityTypeBuilder<AdventureSheetTemplateField> builder)
    {
        builder.ToTable("AdventureSheetTemplateFields");

        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        builder.Property(x => x.IsRequired).IsRequired();
        builder.Property(x => x.FieldType).IsRequired().HasConversion<int>();

        builder.HasMany(x => x.Validations)
               .WithMany(v => v.AdventureSheetFields);
               
    }
}
