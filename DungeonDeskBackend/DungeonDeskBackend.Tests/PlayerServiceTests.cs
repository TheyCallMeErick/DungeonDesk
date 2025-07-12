using DungeonDeskBackend.Application.Data;
using DungeonDeskBackend.Application.Services;
using DungeonDeskBackend.Application.Services.Interfaces;
using DungeonDeskBackend.Domain.Enums;
using DungeonDeskBackend.Domain.Models;
using DungeonDeskBackend.Tests.Fixtures.Fakers;
using Microsoft.EntityFrameworkCore;

namespace DungeonDeskBackend.Tests;

public class PlayerServiceTests : IDisposable
{
    private readonly IPlayerService _playerService;
    private readonly DungeonDeskDbContext _dbContext;

    public PlayerServiceTests()
    {
        var options = new DbContextOptionsBuilder<DungeonDeskDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        _dbContext = new DungeonDeskDbContext(options);
        _playerService = new PlayerService(_dbContext);
    }

    [Fact]
    public async Task GetPlayersAsync_ShouldReturnAllPlayers()
    {
        // Arrange
        var player1 = PlayerFaker.MakeOne();
        var player2 = PlayerFaker.MakeOne();

        _dbContext.Players.AddRange(player1, player2);
        await _dbContext.SaveChangesAsync();

        // Act
        var players = await _playerService.GetPlayersAsync();

        // Assert
        Assert.Equal(2, players.Count);
        Assert.Contains(players, p => p.Name == player1.Name);
        Assert.Contains(players, p => p.Name == player2.Name);
    }

    [Fact]
    public async Task CreatePlayerAsync_ShouldAddPlayer()
    {
        // Arrange
        var player = PlayerFaker.MakeOne();

        // Act
        var createdPlayer = await _playerService.CreatePlayerAsync(player);

        // Assert
        Assert.NotNull(createdPlayer);
        Assert.Equal(player.Name, createdPlayer.Name);
        var players = await _playerService.GetPlayersAsync();
        Assert.Contains(players, p => p.Name == player.Name);
    }

    [Fact]
    public async Task UpdatePlayerAsync_ShouldUpdatePlayer()
    {
        // Arrange
        var player = PlayerFaker.MakeOne();
        await _playerService.CreatePlayerAsync(player);
        player.Name = "Updated Name";

        // Act
        var updatedPlayer = await _playerService.UpdatePlayerAsync(player.Id, player);

        // Assert
        Assert.NotNull(updatedPlayer);
        Assert.Equal("Updated Name", updatedPlayer.Name);
    }

    [Fact]
    public async Task DeletePlayerAsync_ShouldRemovePlayer()
    {
        // Arrange
        var player = PlayerFaker.MakeOne();
        await _playerService.CreatePlayerAsync(player);

        // Act
        await _playerService.DeletePlayerAsync(player.Id);

        // Assert
        var players = await _playerService.GetPlayersAsync();
        Assert.DoesNotContain(players, p => p.Name == "Player to Delete");
    }

    [Fact]
    public async Task GetDesksByPlayerIdAsync_ShouldReturnPlayerDesks()
    {
        // Arrange
        var player = PlayerFaker.MakeOne();
        var desk1 = DeskFaker.MakeOne();
        desk1.PlayerDesks.Add(new PlayerDesk { PlayerId = player.Id, DeskId = desk1.Id, Role = EPlayerDeskRole.Player, JoinedAt = DateTime.UtcNow });
        var desk2 = DeskFaker.MakeOne();
        desk2.PlayerDesks.Add(new PlayerDesk { PlayerId = player.Id, DeskId = desk2.Id, Role = EPlayerDeskRole.Player, JoinedAt = DateTime.UtcNow });
        await _playerService.CreatePlayerAsync(player);
        await _dbContext.Desks.AddRangeAsync(desk1, desk2);
        await _dbContext.SaveChangesAsync();

        // Act
        var desks = await _playerService.GetDesksByPlayerIdAsync(player.Id);

        // Assert
        Assert.Equal(2, desks.Count);
        Assert.Contains(desks, d => d.Name == desk1.Name);
        Assert.Contains(desks, d => d.Name == desk2.Name);
    }

    [Fact]
    public async Task JoinDeskAsync_ShouldAddPlayerToDesk()
    {
        // Arrange
        var player = PlayerFaker.MakeOne();
        var desk = DeskFaker.MakeOne();
        await _playerService.CreatePlayerAsync(player);
        await _dbContext.Desks.AddAsync(desk);
        await _dbContext.SaveChangesAsync();

        // Act
        var joinedDesk = await _playerService.JoinDeskAsync(player.Id, desk.Id);

        // Assert
        Assert.NotNull(joinedDesk);
        Assert.Equal(desk.Id, joinedDesk.Id);
        Assert.Contains(joinedDesk.PlayerDesks.Select(x => x.PlayerId), p => p == player.Id);
    }

    [Fact]
    public async Task JoinDeskAsync_ShouldThrowExceptionIfDeskIsFull()
    {
        // Arrange
        var player1 = PlayerFaker.MakeOne();
        var player2 = PlayerFaker.MakeOne();
        var desk = DeskFaker.MakeOne();
        desk.MaxPlayers = 1;

        await _playerService.CreatePlayerAsync(player1);
        await _playerService.CreatePlayerAsync(player2);
        await _dbContext.Desks.AddAsync(desk);
        await _dbContext.SaveChangesAsync();
        await _playerService.JoinDeskAsync(player1.Id, desk.Id);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _playerService.JoinDeskAsync(player2.Id, desk.Id));
    }

    [Fact]
    public async Task JoinDeskAsync_ShouldThrowExceptionIfPlayerOrDeskNotFound()
    {
        // Arrange
        var playerId = Guid.NewGuid();
        var deskId = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _playerService.JoinDeskAsync(playerId, deskId));
    }

    [Fact]
    public async Task JoinDeskAsync_ShouldThrowExceptionIfPlayerAlreadyInDesk()
    {
        // Arrange
        var player = PlayerFaker.MakeOne();
        var desk = DeskFaker.MakeOne();
        desk.PlayerDesks.Add(new PlayerDesk { PlayerId = player.Id, DeskId = desk.Id, Role = EPlayerDeskRole.Player, JoinedAt = DateTime.UtcNow });
        await _playerService.CreatePlayerAsync(player);
        await _dbContext.Desks.AddAsync(desk);
        await _dbContext.SaveChangesAsync();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _playerService.JoinDeskAsync(player.Id, desk.Id));
    }

    [Fact]
    public async Task JoinDeskAsync_ShouldThrowExceptionIfDeskNotFound()
    {
        // Arrange
        var player = PlayerFaker.MakeOne();
        await _playerService.CreatePlayerAsync(player);
        var nonexistentDeskId = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _playerService.JoinDeskAsync(player.Id, nonexistentDeskId));
    }

    [Fact]
    public async Task JoinDeskAsync_ShouldThrowExceptionIfPlayerNotFound()
    {
        // Arrange
        var desk = DeskFaker.MakeOne();
        await _dbContext.Desks.AddAsync(desk);
        await _dbContext.SaveChangesAsync();
        var nonexistentPlayerId = Guid.NewGuid();

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _playerService.JoinDeskAsync(nonexistentPlayerId, desk.Id));
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}
