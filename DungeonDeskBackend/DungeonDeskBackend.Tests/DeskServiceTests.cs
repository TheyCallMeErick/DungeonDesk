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
            .UseInMemoryDatabase(databaseName: "TestDb")
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
        var createdDesk = await _deskService.CreateDeskAsync(desk);

        // Assert
        Assert.NotNull(createdDesk);
        Assert.Equal(desk.Name, createdDesk.Name);
        Assert.Equal(desk.Description, createdDesk.Description);
        var desks = await _deskService.GetDesksAsync();
        Assert.Contains(desks, d => d.Name == desk.Name);
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
        var desks = await _deskService.GetDesksAsync();

        // Assert
        Assert.Equal(2, desks.Count);
        Assert.Contains(desks, d => d.Name == desk1.Name);
        Assert.Contains(desks, d => d.Name == desk2.Name);
    }

    [Fact]
    public async Task GetDeskByIdAsync_ShouldReturnDesk()
    {
        // Arrange
        var desk = DeskFaker.MakeOne();
        _dbContext.Desks.Add(desk);
        _dbContext.SaveChanges();

        // Act
        var retrievedDesk = await _deskService.GetDeskByIdAsync(desk.Id);

        // Assert
        Assert.NotNull(retrievedDesk);
        Assert.Equal(desk.Name, retrievedDesk.Name);
    }

    [Fact]
    public async Task UpdateDeskAsync_ShouldUpdateDesk()
    {
        // Arrange
        var desk = DeskFaker.MakeOne();
        _dbContext.Desks.Add(desk);
        _dbContext.SaveChanges();
        desk.Name = "Updated Desk Name";

        // Act
        var updatedDesk = await _deskService.UpdateDeskAsync(desk.Id, desk);

        // Assert
        Assert.NotNull(updatedDesk);
        Assert.Equal("Updated Desk Name", updatedDesk.Name);
    }

    [Fact]
    public async Task DeleteDeskAsync_ShouldRemoveDesk()
    {
        // Arrange
        var desk = DeskFaker.MakeOne();
        _dbContext.Desks.Add(desk);
        _dbContext.SaveChanges();

        // Act
        await _deskService.DeleteDeskAsync(desk.Id);

        // Assert
        var desks = await _deskService.GetDesksAsync();
        Assert.DoesNotContain(desks, d => d.Name == desk.Name);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}