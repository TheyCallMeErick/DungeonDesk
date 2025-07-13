using DungeonDeskBackend.Application.Data;
using DungeonDeskBackend.Application.Services;
using DungeonDeskBackend.Application.Services.Interfaces;
using DungeonDeskBackend.Domain.Enums;
using DungeonDeskBackend.Domain.Models;
using DungeonDeskBackend.Tests.Fixtures.Fakers;
using Microsoft.EntityFrameworkCore;

namespace DungeonDeskBackend.Tests;

public class SessionServiceTests
{
    private readonly DungeonDeskDbContext _dbContext;
    private readonly ISessionService _sessionService;
    public SessionServiceTests()
    {
        var options = new DbContextOptionsBuilder<DungeonDeskDbContext>()
           .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
           .Options;

        _dbContext = new DungeonDeskDbContext(options);
        _sessionService = new SessionService(_dbContext);
    }

    [Fact]
    public async Task GetSessionsByDeskIdAsync_ShouldReturnAllDeskSessions()
    {
        // Arrange
        var session1 = SessionFaker.MakeOne();
        var session2 = SessionFaker.MakeOne();
        var desk = DeskFaker.MakeOne();
        session1.DeskId = desk.Id;
        session2.DeskId = desk.Id;
        _dbContext.Desks.Add(desk);
        _dbContext.Sessions.AddRange(session1, session2);
        await _dbContext.SaveChangesAsync();

        // Act
        var sessions = await _sessionService.GetSessionsByDeskIdAsync(desk.Id);

        // Assert
        Assert.Equal(2, sessions.Count());
    }

    [Fact]
    public async Task CreateSessionAsync_ShouldAddSessionIfThePlayerIsTheMaster()
    {
        // Arrange
        var desk = DeskFaker.MakeOne();
        var master = PlayerFaker.MakeOne();
        var playerDesk = new PlayerDesk
        {
            PlayerId = master.Id,
            DeskId = desk.Id,
            Role = EPlayerDeskRole.DeskMaster
        };
        desk.PlayerDesks.Add(playerDesk);
        _dbContext.Desks.Add(desk);
        await _dbContext.SaveChangesAsync();

        // Act
        var createdSession = await _sessionService.CreateSessionAsync(desk.Id, new DateTime(), "", master.Id);

        // Assert
        Assert.True(createdSession.Success);
        Assert.Equal(1, _dbContext.Sessions.Count());
    }

    [Fact]
    public async Task CreateSessionAsync_ShouldNotAddSessionIfTheUserRolesIsntMaster()
    {
        // Arrange
        var desk = DeskFaker.MakeOne();
        var master = PlayerFaker.MakeOne();
        var playerDesk = new PlayerDesk
        {
            PlayerId = master.Id,
            DeskId = desk.Id,
            Role = EPlayerDeskRole.Player
        };
        desk.PlayerDesks.Add(playerDesk);
        _dbContext.Desks.Add(desk);
        await _dbContext.SaveChangesAsync();

        // Act
        var createdSession = await _sessionService.CreateSessionAsync(desk.Id, new DateTime(), "", master.Id);

        // Assert
        Assert.False(createdSession.Success);
        Assert.Equal(0, _dbContext.Sessions.Count());
    }

    [Fact]
    public async Task CreateSessionAsync_ShouldThrowExceptionIfDeskNotFound()
    {
        // Arrange
        var nonExistentDeskId = Guid.NewGuid();
        var master = PlayerFaker.MakeOne();

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            await _sessionService.CreateSessionAsync(nonExistentDeskId, new DateTime(), "", master.Id));
    }

    [Fact]
    public async Task CreateSessionAsync_ShoudCreateSessionIfPlayerIsMaster()
    {
        // Arrange
        var desk = DeskFaker.MakeOne();
        var master = PlayerFaker.MakeOne();
        var playerDesk = new PlayerDesk
        {
            PlayerId = master.Id,
            DeskId = desk.Id,
            Role = EPlayerDeskRole.DeskMaster
        };
        desk.PlayerDesks.Add(playerDesk);
        _dbContext.Desks.Add(desk);
        await _dbContext.SaveChangesAsync();

        // Act
        var createdSession = await _sessionService.CreateSessionAsync(desk.Id, new DateTime(), "", master.Id);

        // Assert
        Assert.True(createdSession.Success);
    }

    [Fact]
    public async Task CreateSessionAsync_ShouldNotCreateSessionIfPlayerIsNotMaster()
    {
        // Arrange
        var desk = DeskFaker.MakeOne();
        var player = PlayerFaker.MakeOne();
        var playerDesk = new PlayerDesk
        {
            PlayerId = player.Id,
            DeskId = desk.Id,
            Role = EPlayerDeskRole.Player
        };
        desk.PlayerDesks.Add(playerDesk);
        _dbContext.Desks.Add(desk);
        await _dbContext.SaveChangesAsync();

        // Act
        var createdSession = await _sessionService.CreateSessionAsync(desk.Id, new DateTime(), "", player.Id);

        // Assert
        Assert.False(createdSession.Success);
        Assert.Equal(0, _dbContext.Sessions.Count());
    }

    [Fact]
    public async Task DeleteSessionAsync_ShouldDeleteSessionIfExistsAndIfPlayerIsMaster()
    {
        // Arrange
        var session = SessionFaker.MakeOne();
        var desk = DeskFaker.MakeOne();
        var master = PlayerFaker.MakeOne();
        var playerDesk = new PlayerDesk
        {
            PlayerId = master.Id,
            DeskId = desk.Id,
            Role = EPlayerDeskRole.DeskMaster
        };
        session.DeskId = desk.Id;
        _dbContext.Desks.Add(desk);
        desk.PlayerDesks.Add(playerDesk);
        _dbContext.Sessions.Add(session);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _sessionService.DeleteSessionAsync(session.Id, master.Id);

        // Assert
        Assert.True(result.Success);
        Assert.Empty(_dbContext.Sessions);
    }

    [Fact]
    public async Task DeleteSessionAsync_ShouldNotDeleteSessionIfPlayerIsNotMaster()
    {
        // Arrange
        var session = SessionFaker.MakeOne();
        var desk = DeskFaker.MakeOne();
        var player = PlayerFaker.MakeOne();
        var playerDesk = new PlayerDesk
        {
            PlayerId = player.Id,
            DeskId = desk.Id,
            Role = EPlayerDeskRole.Player
        };
        session.DeskId = desk.Id;
        _dbContext.Desks.Add(desk);
        desk.PlayerDesks.Add(playerDesk);
        _dbContext.Sessions.Add(session);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _sessionService.DeleteSessionAsync(session.Id, player.Id);

        // Assert
        Assert.False(result.Success);
        Assert.Single(_dbContext.Sessions);
    }
}
