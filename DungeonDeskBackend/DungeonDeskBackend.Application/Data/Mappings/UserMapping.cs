using DungeonDeskBackend.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DungeonDeskBackend.Application.Data.Mappings;

public class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(e => e.Username)
              .HasMaxLength(50);

        builder.Property(e => e.Email)
              .HasMaxLength(100);

        builder.Property(e => e.Name)
          .HasMaxLength(255);

        builder.Property(e => e.Password)
              .IsRequired();

        builder.Property(e => e.ProfilePictureFileName)
              .HasMaxLength(1024);

        builder.HasMany(e => e.RefreshTokens)
               .WithOne(e => e.User)
               .HasForeignKey(rt => rt.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Player)
               .WithOne(p => p.User)
               .HasForeignKey<User>(u => u.PlayerId);
    }
}
