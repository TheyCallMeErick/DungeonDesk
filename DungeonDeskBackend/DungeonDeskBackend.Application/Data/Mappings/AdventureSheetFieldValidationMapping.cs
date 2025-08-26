using DungeonDeskBackend.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DungeonDeskBackend.Application.Data.Mappings;

public class AdventureSheetFieldValidationMapping : IEntityTypeConfiguration<AdventureSheetFieldValidations>
{
    public void Configure(EntityTypeBuilder<AdventureSheetFieldValidations> builder)
    {
        builder.Property(x => x.FieldType).IsRequired().HasConversion<int>();

        builder.Property(x => x.Rule).IsRequired(true).HasMaxLength(100);
    }
}
