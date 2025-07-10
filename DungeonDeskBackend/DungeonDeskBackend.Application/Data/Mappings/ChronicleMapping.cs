using DungeonDeskBackend.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DungeonDeskBackend.Application.Data.Mappings;

public class ChronicleMapping : IEntityTypeConfiguration<Chronicle>
{
    public void Configure(EntityTypeBuilder<Chronicle> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Title)
              .IsRequired()
              .HasMaxLength(200);

        builder.Property(e => e.Content)
              .IsRequired();

        builder.HasOne(e => e.Session)
              .WithMany(s => s.Chronicles)
              .HasForeignKey(e => e.SessionId)
              .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Author)
              .WithMany()
              .HasForeignKey(e => e.AuthorId)
              .OnDelete(DeleteBehavior.Restrict);
    }
}
