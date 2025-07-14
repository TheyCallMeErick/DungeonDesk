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
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
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
        Assert.NotNull(result.Data);
        Assert.Equal(adventures.Count(), result.Data.Count());
    }

    [Fact]
    public async Task GetAdventureByIdAsync_ShouldReturnAdventure_WhenExists()
    {
        // Arrange
        var adventure = AdventureFaker.MakeOne();
        _dbContext.Adventures.Add(adventure);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _adventureService.GetAdventureByIdAsync(adventure.Id);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Data);
        Assert.Equal(adventure.Id, result.Data.Id);
    }

    [Fact]
    public async Task GetAdventureByIdAsync_ShouldReturnError_WhenNotExists()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await _adventureService.GetAdventureByIdAsync(nonExistentId);

        // Assert
        Assert.False(result.Success);
        Assert.Equal($"Adventure with ID {nonExistentId} not found.", result.Message);
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task CreateAdventureAsync_ShouldAddAdventure()
    {
        // Arrange
        var adventure = AdventureFaker.MakeOne();

        // Act
        await _adventureService.CreateAdventureAsync(adventure);
        await _dbContext.SaveChangesAsync();

        // Assert
        var result = await _adventureService.GetAdventureByIdAsync(adventure.Id);
        Assert.NotNull(result);
        Assert.NotNull(result.Data);
        Assert.Equal(adventure.Id, result.Data.Id);
    }

    [Fact]
    public async Task UpdateAdventureAsync_ShouldUpdateAdventure_WhenExists()
    {
        // Arrange
        var adventure = AdventureFaker.MakeOne();
        _dbContext.Adventures.Add(adventure);
        await _dbContext.SaveChangesAsync();

        adventure.Title = "Updated Adventure";

        // Act
        await _adventureService.UpdateAdventureAsync(adventure);
        await _dbContext.SaveChangesAsync();

        // Assert
        var result = await _adventureService.GetAdventureByIdAsync(adventure.Id);
        Assert.NotNull(result);
        Assert.NotNull(result.Data);
        Assert.Equal(adventure.Title, result.Data.Title);
    }

    [Fact]
    public async Task UpdateAdventureAsync_ShouldReturnError_WhenNotExists()
    {
        // Arrange
        var adventure = AdventureFaker.MakeOne();

        // Act
        var result = await _adventureService.UpdateAdventureAsync(adventure);

        // Assert
        Assert.False(result.Success);
        Assert.Equal($"Adventure with ID {adventure.Id} not found.", result.Message);
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task DeleteAdventureAsync_ShouldRemoveAdventure_WhenExists()
    {
        // Arrange
        var adventure = AdventureFaker.MakeOne();
        _dbContext.Adventures.Add(adventure);
        await _dbContext.SaveChangesAsync();

        // Act
        await _adventureService.DeleteAdventureAsync(adventure.Id);
        await _dbContext.SaveChangesAsync();

        // Assert
        Assert.Null(_dbContext.Adventures.Find(adventure.Id));
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}
