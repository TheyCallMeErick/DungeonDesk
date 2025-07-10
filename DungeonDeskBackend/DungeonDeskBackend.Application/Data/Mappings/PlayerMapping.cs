using DungeonDeskBackend.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DungeonDeskBackend.Application.Data.Mappings;

public class PlayerMapping : IEntityTypeConfiguration<Player>
{

    public void Configure(EntityTypeBuilder<Player> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Email)
              .IsRequired()
              .HasMaxLength(100);

        builder.Property(e => e.Password)
              .IsRequired();

        builder.Property(e => e.Name)
              .IsRequired()
              .HasMaxLength(100);
    }
}
