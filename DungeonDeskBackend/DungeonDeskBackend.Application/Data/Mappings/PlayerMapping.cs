using DungeonDeskBackend.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DungeonDeskBackend.Application.Data.Mappings; 

public class PlayerMapping : IEntityTypeConfiguration<Player>
{

    public void Configure(EntityTypeBuilder<Player> builder)
    {
        throw new NotImplementedException();
    }
}
