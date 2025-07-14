using DungeonDeskBackend.Application.Data;
using DungeonDeskBackend.Application.Services;
using DungeonDeskBackend.Application.Services.Interfaces;
using DungeonDeskBackend.Domain.Enums;
using DungeonDeskBackend.Domain.Models;
using DungeonDeskBackend.Tests.Fixtures.Fakers;
using Microsoft.EntityFrameworkCore;

namespace DungeonDeskBackend.Tests; 

public class ChronicleServiceTests : IDisposable
{
    private readonly DungeonDeskDbContext _dbContext;
    private readonly IChronicleService _chronicleService;
    public ChronicleServiceTests()
    {
        var options = new DbContextOptionsBuilder<DungeonDeskDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new DungeonDeskDbContext(options);
        _chronicleService = new ChronicleService(_dbContext);
    }

    [Fact]
    public async Task CreateChronicleAsync_ShouldAddChronicleToSession()
    {
        //Arrange
        var desk = DeskFaker.MakeOne();
        var player = PlayerFaker.MakeOne();
        var session = SessionFaker.MakeOne();
        var playerDesk = new PlayerDesk
        {
            PlayerId = player.Id,
            DeskId = desk.Id,
            Role = EPlayerDeskRole.DeskMaster
        };
        session.DeskId = desk.Id;
        desk.PlayerDesks.Add(playerDesk);
        _dbContext.Desks.Add(desk);
        _dbContext.Sessions.Add(session);
        await _dbContext.SaveChangesAsync();
        //Act
        var result = await _chronicleService.AddChronicleAsync(session.Id, "Title", "content", player.Id);
        // Assert
        Assert.True(result.Success);
        Assert.Single(_dbContext.Chronicles);
    }

    [Fact]
    public async Task CreateChronicleAsync_ShouldNotAddChronicleIfPlayerIsNotDeskMaster()
    {
        // Arrange
        var desk = DeskFaker.MakeOne();
        var player = PlayerFaker.MakeOne();
        var session = SessionFaker.MakeOne();
        var playerDesk = new PlayerDesk
        {
            PlayerId = player.Id,
            DeskId = desk.Id,
            Role = EPlayerDeskRole.Player
        };
        session.DeskId = desk.Id;
        desk.PlayerDesks.Add(playerDesk);
        _dbContext.Desks.Add(desk);
        _dbContext.Sessions.Add(session);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _chronicleService.AddChronicleAsync(session.Id, "Title", "content", player.Id);

        // Assert
        Assert.False(result.Success);
        Assert.Empty(_dbContext.Chronicles);
    }

    [Fact]
    public async Task UpdateChronicleAsync_ShouldUpdateChronicleIfPlayerIsDeskMaster()
    {
        // Arrange
        var desk = DeskFaker.MakeOne();
        var player = PlayerFaker.MakeOne();
        var session = SessionFaker.MakeOne();
        var playerDesk = new PlayerDesk
        {
            PlayerId = player.Id,
            DeskId = desk.Id,
            Role = EPlayerDeskRole.DeskMaster
        };
        session.DeskId = desk.Id;
        desk.PlayerDesks.Add(playerDesk);
        _dbContext.Desks.Add(desk);
        _dbContext.Sessions.Add(session);

        var chronicle = new Chronicle
        {
            SessionId = session.Id,
            Title = "Old Title",
            Content = "Old Content",
            AuthorId = player.Id
        };

        _dbContext.Chronicles.Add(chronicle);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _chronicleService.UpdateChronicleAsync(chronicle.Id, "New Title", "New Content", player.Id);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("New Title", chronicle.Title);
        Assert.Equal("New Content", chronicle.Content);
    }

    [Fact]
    public async Task UpdateChronicleAsync_ShouldNotUpdateChronicleIfPlayerIsNotDeskMaster()
    {
        // Arrange
        var desk = DeskFaker.MakeOne();
        var player = PlayerFaker.MakeOne();
        var session = SessionFaker.MakeOne();
        var playerDesk = new PlayerDesk
        {
            PlayerId = player.Id,
            DeskId = desk.Id,
            Role = EPlayerDeskRole.Player
        };
        session.DeskId = desk.Id;
        desk.PlayerDesks.Add(playerDesk);
        _dbContext.Desks.Add(desk);
        _dbContext.Sessions.Add(session);

        var chronicle = new Chronicle
        {
            SessionId = session.Id,
            Title = "Old Title",
            Content = "Old Content",
            AuthorId = player.Id
        };

        _dbContext.Chronicles.Add(chronicle);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _chronicleService.UpdateChronicleAsync(chronicle.Id, "New Title", "New Content", player.Id);

        // Assert
        chronicle = await _dbContext.Chronicles.FirstAsync(x=>x.Id == chronicle.Id);
        Assert.False(result.Success);
        Assert.Equal("Old Title", chronicle.Title);
        Assert.Equal("Old Content", chronicle.Content);
    }

    [Fact]
    public async Task UpdateChronicleAsync_ShouldNotUpdateIfChronicleDoesNotExist()
    {
        // Arrange
        var desk = DeskFaker.MakeOne();
        var player = PlayerFaker.MakeOne();
        var session = SessionFaker.MakeOne();
        var playerDesk = new PlayerDesk
        {
            PlayerId = player.Id,
            DeskId = desk.Id,
            Role = EPlayerDeskRole.DeskMaster
        };
        session.DeskId = desk.Id;
        desk.PlayerDesks.Add(playerDesk);
        _dbContext.Desks.Add(desk);
        _dbContext.Sessions.Add(session);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _chronicleService.UpdateChronicleAsync(Guid.NewGuid(), "New Title", "New Content", player.Id);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(0, _dbContext.Chronicles.Count());
    }

    [Fact]
    public async Task DeleteChronicleAsync_ShouldDeleteChronicleIfPlayerIsDeskMaster()
    {
        // Arrange
        var desk = DeskFaker.MakeOne();
        var player = PlayerFaker.MakeOne();
        var session = SessionFaker.MakeOne();
        var playerDesk = new PlayerDesk
        {
            PlayerId = player.Id,
            DeskId = desk.Id,
            Role = EPlayerDeskRole.DeskMaster
        };
        session.DeskId = desk.Id;
        desk.PlayerDesks.Add(playerDesk);
        _dbContext.Desks.Add(desk);
        _dbContext.Sessions.Add(session);

        var chronicle = new Chronicle
        {
            SessionId = session.Id,
            Title = "Title",
            Content = "Content",
            AuthorId = player.Id
        };

        _dbContext.Chronicles.Add(chronicle);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _chronicleService.DeleteChronicleAsync(chronicle.Id, player.Id);

        // Assert
        Assert.True(result.Success);
        Assert.Empty(_dbContext.Chronicles);
    }

    [Fact]
    public async Task DeleteChronicleAsync_ShouldNotDeleteChronicleIfPlayerIsNotDeskMaster()
    {
        // Arrange
        var desk = DeskFaker.MakeOne();
        var player = PlayerFaker.MakeOne();
        var session = SessionFaker.MakeOne();
        var playerDesk = new PlayerDesk
        {
            PlayerId = player.Id,
            DeskId = desk.Id,
            Role = EPlayerDeskRole.Player
        };
        session.DeskId = desk.Id;
        desk.PlayerDesks.Add(playerDesk);
        _dbContext.Desks.Add(desk);
        _dbContext.Sessions.Add(session);

        var chronicle = new Chronicle
        {
            SessionId = session.Id,
            Title = "Title",
            Content = "Content",
            AuthorId = player.Id
        };

        _dbContext.Chronicles.Add(chronicle);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _chronicleService.DeleteChronicleAsync(chronicle.Id, player.Id);

        // Assert
        Assert.False(result.Success);
        Assert.Single(_dbContext.Chronicles);
    }

    [Fact]
    public async Task DeleteChronicleAsync_ShouldNotDeleteIfChronicleDoesNotExist()
    {
        // Arrange
        var desk = DeskFaker.MakeOne();
        var player = PlayerFaker.MakeOne();
        var session = SessionFaker.MakeOne();
        var playerDesk = new PlayerDesk
        {
            PlayerId = player.Id,
            DeskId = desk.Id,
            Role = EPlayerDeskRole.DeskMaster
        };
        session.DeskId = desk.Id;
        desk.PlayerDesks.Add(playerDesk);
        _dbContext.Desks.Add(desk);
        _dbContext.Sessions.Add(session);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _chronicleService.DeleteChronicleAsync(Guid.NewGuid(), player.Id);

        // Assert
        Assert.False(result.Success);
        Assert.Empty(_dbContext.Chronicles);
    }

    [Fact]
    public async Task GetChronicleById_Async_ShouldReturnChronicleIfExists()
    {
        // Arrange
        var chronicle = ChronicleFaker.MakeOne();

        _dbContext.Chronicles.Add(chronicle);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _chronicleService.GetChronicleByIdAsync(chronicle.Id);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Data);
        Assert.Equal(chronicle.Title, result.Data.Title);
    }

    [Fact]
    public async Task GetChronicleById_Async_ShouldReturnNullIfChronicleDoesNotExist()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await _chronicleService.GetChronicleByIdAsync(nonExistentId);

        // Assert
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task GetChroniclesBySessionIdAsync_ShouldReturnChroniclesForSession()
    {
        // Arrange
        var session = SessionFaker.MakeOne();
        var chronicle1 = ChronicleFaker.MakeOne();
        var chronicle2 = ChronicleFaker.MakeOne();

        session.Chronicles.Add(chronicle1);
        session.Chronicles.Add(chronicle2);

        _dbContext.Sessions.Add(session);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _chronicleService.GetChroniclesBySessionIdAsync(session.Id);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Data);
        Assert.Equal(2, result.Data.Count());
        Assert.Contains(result.Data, c => c.Id == chronicle1.Id);
        Assert.Contains(result.Data, c => c.Id == chronicle2.Id);
    }


    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

}
