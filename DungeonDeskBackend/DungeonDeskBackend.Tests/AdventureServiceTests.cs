using DungeonDeskBackend.Application.Data;
using DungeonDeskBackend.Application.Services;
using DungeonDeskBackend.Application.Services.Interfaces;
using DungeonDeskBackend.Tests.Fixtures.Fakers;
using Microsoft.EntityFrameworkCore;

namespace DungeonDeskBackend.Tests; 

public class AdventureServiceTests  : IDisposable
{
    private readonly IAdventureService _adventureService;
    private readonly DungeonDeskDbContext _dbContext;

    public AdventureServiceTests()
    {
        var options = new DbContextOptionsBuilder<DungeonDeskDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        _dbContext = new DungeonDeskDbContext(options);
        _adventureService = new AdventureService(_dbContext);
    }

    [Fact]
    public async Task GetAdventuresAsync_ShouldReturnAllAdventures()
    {
        // Arrange
        var adventures =  AdventureFaker.MakeMany(4);

        _dbContext.Adventures.AddRange(adventures);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _adventureService.GetAdventuresAsync();

        // Assert
        Assert.Equal(adventures.Count(), result.Count());
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}
