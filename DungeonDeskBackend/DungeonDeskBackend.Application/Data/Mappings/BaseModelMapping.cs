using DungeonDeskBackend.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DungeonDeskBackend.Application.Data.Mappings; 

public class BaseModelMapping <T> : IEntityTypeConfiguration<T> where T : BaseModel
{

    public void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedOnAdd();
        builder.Property(e => e.CreatedAt).IsRequired().ValueGeneratedOnAdd().HasDefaultValue(DateTime.Now);
        builder.Property(e => e.UpdatedAt).IsRequired().ValueGeneratedOnAddOrUpdate().HasDefaultValue(DateTime.Now);
    }
}
