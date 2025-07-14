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
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
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
        var result = await _playerService.GetPlayersAsync();

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal(2, result.Data.Count());
        Assert.Contains(_dbContext.Players, p => p.Name == player1.Name);
        Assert.Contains(_dbContext.Players, p => p.Name == player2.Name);
    }

    [Fact]
    public async Task CreatePlayerAsync_ShouldAddPlayer()
    {
        // Arrange
        var player = PlayerFaker.MakeOne();

        // Act
        var result = await _playerService.CreatePlayerAsync(player);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal(player.Name, result.Data.Name);
        Assert.Single(_dbContext.Players);
        Assert.Contains(_dbContext.Players, p => p.Name == player.Name);
    }

    [Fact]
    public async Task UpdatePlayerAsync_ShouldUpdatePlayer()
    {
        // Arrange
        var player = PlayerFaker.MakeOne();
        await _playerService.CreatePlayerAsync(player);
        player.Name = "Updated Name";

        // Act
        var result = await _playerService.UpdatePlayerAsync(player.Id, player);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal("Updated Name", result.Data.Name);
    }

    [Fact]
    public async Task DeletePlayerAsync_ShouldRemovePlayer()
    {
        // Arrange
        var player = PlayerFaker.MakeOne();
        await _playerService.CreatePlayerAsync(player);

        // Act
        var result = await _playerService.DeletePlayerAsync(player.Id);

        // Assert
        Assert.True(result.Success);
        Assert.Null(result.Data);
        Assert.Empty(_dbContext.Players);
        Assert.DoesNotContain(_dbContext.Players, p => p.Name == player.Name);
        Assert.DoesNotContain(_dbContext.Players, p => p.Name == "Player to Delete");
    }

    [Fact]
    public async Task GetDesksByPlayerIdAsync_ShouldReturnPlayerDesks()
    {
        // Arrange
        var player = PlayerFaker.MakeOne();
        await _dbContext.Players.AddAsync(player);
        await _dbContext.SaveChangesAsync();

        var desk1 = DeskFaker.MakeOne();
        desk1.PlayerDesks.Add(
            new PlayerDesk
            {
                PlayerId = player.Id,
                DeskId = desk1.Id,
                Role = EPlayerDeskRole.Player,
                JoinedAt = DateTime.UtcNow
            }
        );
        var desk2 = DeskFaker.MakeOne();
        desk2.PlayerDesks.Add(
            new PlayerDesk
            {
                PlayerId = player.Id,
                DeskId = desk2.Id,
                Role = EPlayerDeskRole.Player,
                JoinedAt = DateTime.UtcNow
            }
        );
        await _dbContext.Desks.AddRangeAsync(desk1, desk2);
        await _dbContext.SaveChangesAsync();
        // Act
        var result = await _playerService.GetDesksByPlayerIdAsync(player.Id);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal(2, result.Data.Count());
        Assert.Contains(_dbContext.Desks, d => d.Name == desk1.Name);
        Assert.Contains(_dbContext.Desks, d => d.Name == desk2.Name);
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
        var result = await _playerService.JoinDeskAsync(player.Id, desk.Id);

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal(desk.Id, result.Data.Id);
        Assert.Contains(_dbContext.PlayerDesks.Select(x => x.PlayerId), p => p == player.Id);
    }

    [Fact]
    public async Task JoinDeskAsync_ShouldReturnErrorExceptionIfDeskIsFull()
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

        // Act
        var result = await _playerService.JoinDeskAsync(player2.Id, desk.Id);

        //Assert
        Assert.False(result.Success);
        Assert.Equal($"Desk with ID {desk.Id} is full.", result.Message);
        Assert.Null(result.Data);
        Assert.DoesNotContain(_dbContext.PlayerDesks, pd => pd.PlayerId == player2.Id && pd.DeskId == desk.Id);
    }

    [Fact]
    public async Task JoinDeskAsync_ShouldReturnErrorIfPlayerOrDeskNotFound()
    {
        // Arrange
        var playerId = Guid.NewGuid();
        var deskId = Guid.NewGuid();

        // Act 
        var result = await _playerService.JoinDeskAsync(playerId, deskId);

        // Assert
        Assert.False(result.Success);
        Assert.Equal($"Player with ID {playerId} not found.", result.Message);
    }

    [Fact]
    public async Task JoinDeskAsync_ShouldReturnErrorIfPlayerAlreadyInDesk()
    {
        // Arrange
        var player = PlayerFaker.MakeOne();
        var desk = DeskFaker.MakeOne();
        desk.PlayerDesks.Add(new PlayerDesk { PlayerId = player.Id, DeskId = desk.Id, Role = EPlayerDeskRole.Player, JoinedAt = DateTime.UtcNow });
        await _playerService.CreatePlayerAsync(player);
        await _dbContext.Desks.AddAsync(desk);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _playerService.JoinDeskAsync(player.Id, desk.Id);

        // Assert
        Assert.False(result.Success);
        Assert.Equal($"Player with ID {player.Id} is already in desk with ID {desk.Id}.", result.Message);
    }

    [Fact]
    public async Task JoinDeskAsync_ShouldReturnErrorIfDeskNotFound()
    {
        // Arrange
        var player = PlayerFaker.MakeOne();
        await _playerService.CreatePlayerAsync(player);
        var nonexistentDeskId = Guid.NewGuid();

        // Act
        var result = await _playerService.JoinDeskAsync(player.Id, nonexistentDeskId);

        // Assert
        Assert.False(result.Success);
        Assert.Equal($"Desk with ID {nonexistentDeskId} not found.", result.Message);
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task JoinDeskAsync_ShouldReturnErrorIfPlayerNotFound()
    {
        // Arrange
        var desk = DeskFaker.MakeOne();
        await _dbContext.Desks.AddAsync(desk);
        await _dbContext.SaveChangesAsync();
        var nonexistentPlayerId = Guid.NewGuid();

        // Act
        var result = await _playerService.JoinDeskAsync(nonexistentPlayerId, desk.Id);

        // Assert
        Assert.False(result.Success);
        Assert.Equal($"Player with ID {nonexistentPlayerId} not found.", result.Message);
        Assert.Null(result.Data);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}
