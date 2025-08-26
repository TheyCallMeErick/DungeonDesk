using DungeonDeskBackend.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DungeonDeskBackend.Application.Data.Mappings;

public class PlayerDeskMapping : IEntityTypeConfiguration<PlayerDesk>
{
    public void Configure(EntityTypeBuilder<PlayerDesk> builder)
    {
        builder.HasOne(pd => pd.Player)
               .WithMany(p => p.PlayerDesks)
               .HasForeignKey(pd => pd.PlayerId);

        builder.HasOne(pd => pd.Desk)
               .WithMany(d => d.PlayerDesks)
               .HasForeignKey(pd => pd.DeskId);

        builder.Property(pd => pd.JoinedAt)
               .IsRequired();

        builder.Property(pd => pd.Role)
               .HasMaxLength(50);
    }
}
