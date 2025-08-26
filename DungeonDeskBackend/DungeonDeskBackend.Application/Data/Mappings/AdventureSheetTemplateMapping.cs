using DungeonDeskBackend.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DungeonDeskBackend.Application.Data.Mappings; 

public class AdventureSheetTemplateMapping : IEntityTypeConfiguration<AdventureSheetTemplate>
{
    public void Configure(EntityTypeBuilder<AdventureSheetTemplate> builder)
    {
        builder.ToTable("AdventureSheetTemplates");

        builder.Property(x => x.Title).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Description).HasMaxLength(500);
        builder.Property(x => x.Version).IsRowVersion();
        builder.HasMany(ast => ast.Fields)
               .WithOne(f => f.AdventureSheetTemplate)
               .HasForeignKey(f => f.AdventureSheetTemplateId)
               .OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(ast => ast.Sheets)
               .WithOne(s => s.AdventureSheetTemplate)
               .HasForeignKey(s => s.AdventureSheetTemplateId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
