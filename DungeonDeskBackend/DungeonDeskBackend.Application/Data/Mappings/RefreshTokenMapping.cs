using DungeonDeskBackend.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DungeonDeskBackend.Application.Data.Mappings; 

public class RefreshTokenMapping : IEntityTypeConfiguration<RefreshToken>
{

    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Token)
              .IsRequired()
              .HasMaxLength(500);

        builder.Property(e => e.ExpiresAt)
              .IsRequired();

        builder.Property(e => e.IsRevoked)
              .IsRequired();

        builder.Property(e => e.RevokedAt)
              .IsRequired(false);

        builder.Property(e => e.CreatedByIp)
              .HasMaxLength(45);

        builder.Property(e => e.DeviceInfo)
              .HasMaxLength(200);
    }

}
