using Microsoft.EntityFrameworkCore;

namespace DungeonDeskBackend.Application.Data;

public class DungeonDeskDbContext : DbContext
{
    public DungeonDeskDbContext(DbContextOptions<DungeonDeskDbContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DungeonDeskDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Domain.Models.Chronicle> Chronicles { get; set; }
    public DbSet<Domain.Models.Desk> Desks { get; set; }
    public DbSet<Domain.Models.Player> Players { get; set; }
    public DbSet<Domain.Models.Session> Sessions { get; set; }
    public DbSet<Domain.Models.Adventure> Adventures { get; set; }
    public DbSet<Domain.Models.PlayerDesk> PlayerDesks { get; set; }
    public DbSet<Domain.Models.User> Users { get; set; }
    public DbSet<Domain.Models.RefreshToken> RefreshTokens { get; set; }
}
