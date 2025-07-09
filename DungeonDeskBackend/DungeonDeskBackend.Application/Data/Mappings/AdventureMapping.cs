using DungeonDeskBackend.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DungeonDeskBackend.Application.Data.Mappings; 

public class AdventureMapping : IEntityTypeConfiguration<Adventure>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Adventure> builder)
    {
        throw new NotImplementedException();
    }
}
