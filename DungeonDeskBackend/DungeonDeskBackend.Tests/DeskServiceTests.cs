using System.Threading.Tasks;
using DungeonDeskBackend.Application.Data;
using DungeonDeskBackend.Application.Services;
using DungeonDeskBackend.Application.Services.Interfaces;
using DungeonDeskBackend.Tests.Fixtures.Fakers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DungeonDeskBackend.Tests; 

public class DeskServiceTests : IDisposable
{
    private readonly IDeskService _deskService;
    private readonly DungeonDeskDbContext _dbContext;

    public DeskServiceTests()
    {
        var options = new DbContextOptionsBuilder<DungeonDeskDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new DungeonDeskDbContext(options);
        _deskService = new DeskService(_dbContext);
    }

    [Fact]
    public async Task CreateDeskAsync_ShouldAddDesk()
    {
        // Arrange
        var desk = DeskFaker.MakeOne();

        // Act
        var result = await _deskService.CreateDeskAsync(desk);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal(desk.Name, result.Data.Name);
        Assert.Equal(desk.Description, result.Data.Description);
        Assert.Contains(_dbContext.Desks, d => d.Name == desk.Name);
    }

    [Fact]
    public async Task GetDesksAsync_ShouldReturnAllDesks()
    {
        // Arrange
        var desk1 = DeskFaker.MakeOne();
        var desk2 = DeskFaker.MakeOne();

        _dbContext.Desks.AddRange(desk1, desk2);
        _dbContext.SaveChanges();

        // Act
        var result = await _deskService.GetDesksAsync();

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Contains(result.Data, d => d.Name == desk1.Name);
        Assert.Contains(result.Data, d => d.Name == desk2.Name);
    }

    [Fact]
    public async Task GetDeskByIdAsync_ShouldReturnDesk()
    {
        // Arrange
        var desk = DeskFaker.MakeOne();
        _dbContext.Desks.Add(desk);
        _dbContext.SaveChanges();

        // Act
        var result = await _deskService.GetDeskByIdAsync(desk.Id);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal(desk.Name, result.Data.Name);
    }

    [Fact]
    public async Task UpdateDeskAsync_ShouldUpdateDesk()
    {
        // Arrange
        var desk = DeskFaker.MakeOne();
        await _dbContext.Desks.AddAsync(desk);
        await _dbContext.SaveChangesAsync();
        desk.Name = "Updated Desk Name";

        // Act
        var result = await _deskService.UpdateDeskAsync(desk.Id, desk);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal("Updated Desk Name", result.Data.Name);
    }

    [Fact]
    public async Task DeleteDeskAsync_ShouldRemoveDesk()
    {
        // Arrange
        var desk = DeskFaker.MakeOne();
        _dbContext.Desks.Add(desk);
        _dbContext.SaveChanges();

        // Act
        var result = await _deskService.DeleteDeskAsync(desk.Id);

        // Assert
        Assert.True(result.Success);
        Assert.Empty(_dbContext.Desks);
        Assert.Null(result.Data);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}