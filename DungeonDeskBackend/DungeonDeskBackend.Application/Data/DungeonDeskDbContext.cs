using DungeonDeskBackend.Domain.Models;
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

    public DbSet<Chronicle> Chronicles { get; set; }
    public DbSet<Desk> Desks { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<Adventure> Adventures { get; set; }
    public DbSet<PlayerDesk> PlayerDesks { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Sheet> Sheets { get; set; }
    public DbSet<AdventureSheetTemplate> AdventureSheetTemplates { get; set; }
    public DbSet<AdventureSheetFieldValue> AdventureSheetFields { get; set; }
    public DbSet<AdventureSheetFieldValidations> AdventureSheetFieldValidations { get; set; }
    public DbSet<AdventureSheetFieldOptions> AdventureSheetFieldOptions { get; set; }
    public DbSet<AdventureSheetInventory> AdventureSheetInventories { get; set; }
    public DbSet<Item> Items { get; set; }

}
