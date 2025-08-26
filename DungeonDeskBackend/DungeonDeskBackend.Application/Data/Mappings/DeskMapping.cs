using DungeonDeskBackend.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DungeonDeskBackend.Application.Data.Mappings;

public class DeskMapping : IEntityTypeConfiguration<Desk>
{
    public void Configure(EntityTypeBuilder<Desk> builder)
    {
        builder.Property(e => e.Name)
              .IsRequired()
              .HasMaxLength(200);

        builder.Property(e => e.Description)
              .HasMaxLength(1000);

        builder.Property(e => e.Status)
              .IsRequired();

        builder.Property(e => e.MaxPlayers)
              .IsRequired();

        builder.HasMany(e => e.Sessions)
              .WithOne(s => s.Desk)
              .HasForeignKey(s => s.DeskId);

        builder.HasOne(e => e.Adventure)
              .WithMany(a => a.DesksUsingThis)
              .HasForeignKey(e => e.AdventureId);
    }
}
