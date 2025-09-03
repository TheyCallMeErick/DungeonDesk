using DungeonDeskBackend.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DungeonDeskBackend.Application.Data.Mappings;

public class AdventureMapping : IEntityTypeConfiguration<Adventure>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Adventure> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Title)
              .IsRequired()
              .HasMaxLength(255);

        builder.Property(e => e.Description)
              .HasMaxLength(1000);

        builder.HasOne(e => e.Author)
              .WithMany()
              .HasForeignKey(e => e.AuthorId)
              .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.DesksUsingThis)
              .WithOne(d => d.Adventure)
              .HasForeignKey(d => d.AdventureId);

        builder.HasMany(e => e.AdventureSheetTemplates)
              .WithOne(ast => ast.Adventure)
              .HasForeignKey(ast => ast.AdventureId);
    }
}
