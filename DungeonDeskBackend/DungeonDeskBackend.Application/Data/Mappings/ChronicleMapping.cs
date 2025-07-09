using DungeonDeskBackend.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DungeonDeskBackend.Application.Data.Mappings;

public class ChronicleMapping : IEntityTypeConfiguration<Chronicle>
{
    public void Configure(EntityTypeBuilder<Chronicle> builder)
    {
        throw new NotImplementedException();
    }
}
