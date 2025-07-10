using DungeonDeskBackend.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DungeonDeskBackend.Application.Data.Mappings;

public class SessionMapping : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.ScheduledAt)
              .IsRequired();

        builder.Property(e => e.Notes)
              .HasMaxLength(2000);

        builder.HasOne(e => e.Desk)
              .WithMany(d => d.Sessions)
              .HasForeignKey(e => e.DeskId)
              .OnDelete(DeleteBehavior.Cascade);
    }
}
