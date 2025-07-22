using System.Threading.Tasks;
using DungeonDeskBackend.Application.Data;
using DungeonDeskBackend.Application.DTOs.Inputs;
using DungeonDeskBackend.Application.DTOs.Inputs.Desk;
using DungeonDeskBackend.Application.Repositories;
using DungeonDeskBackend.Application.Services;
using DungeonDeskBackend.Application.Services.Interfaces;
using DungeonDeskBackend.Domain.Enums;
using DungeonDeskBackend.Domain.Models;
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
        var deskRepository = new DeskRepository(_dbContext);
        _deskService = new DeskService(_dbContext,deskRepository);
    }

    [Fact]
    public async Task CreateDeskAsync_ShouldAddDesk()
    {
        // Arrange
        var desk = DeskFaker.MakeOne();
        var master = PlayerFaker.MakeOne();
        var adventure = AdventureFaker.MakeOne();
        _dbContext.Players.Add(master);
        _dbContext.Adventures.Add(adventure);
        _dbContext.SaveChanges();

        // Act
        var result = await _deskService.CreateDeskAsync(new CreateDeskInputDTO(
            Name: desk.Name,
            Description: desk.Description,
            MaxPlayers: desk.MaxPlayers,
            AdventureId: adventure.Id,
            MasterId: master.Id
        ));

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal(desk.Name, result.Data.Name);
        Assert.Equal(desk.Description, result.Data.Description);
        Assert.Contains(_dbContext.Desks, d => d.Name == desk.Name);
    }

    [Fact]
    public async Task GetDesksAsync_ShouldReturnDesks()
    {
        // Arrange
        var desk1 = DeskFaker.MakeOne();
        var desk2 = DeskFaker.MakeOne();

        _dbContext.Desks.AddRange(desk1, desk2);
        _dbContext.SaveChanges();

        // Act
        var result = await _deskService.GetDesksAsync(
            new QueryInputDTO<GetDesksQueryDTO>
            {
                Query = new GetDesksQueryDTO()
            });

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Contains(result.Data, d => d.Name == desk1.Name);
        Assert.Contains(result.Data, d => d.Name == desk2.Name);
    }

    [Fact]
    public async Task GetDesksAsync_ShouldReturnEmpty_WhenNoDesks()
    {
        // Act
        var result = await _deskService.GetDesksAsync(
            new QueryInputDTO<GetDesksQueryDTO>
            {
                Query = new GetDesksQueryDTO()
            });

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Empty(result.Data);
    }
   
    [Fact]
    public async Task GetDesksAsync_ShouldReturnFilteredDesks()
    {
        // Arrange
        var desk1 = DeskFaker.MakeOne();
        desk1.Name = "Test Desk 1";
        desk1.Description = "A test desk for unit testing";
        var desk2 = DeskFaker.MakeOne();
        desk2.Name = "Test Desk 2";
        desk2.Description = "Another test desk for unit testing";

        _dbContext.Desks.AddRange(desk1, desk2);
        _dbContext.SaveChanges();

        // Act
        var result = await _deskService.GetDesksAsync(
            new QueryInputDTO<GetDesksQueryDTO>
            {
                Query = new GetDesksQueryDTO(Name: "Test Desk 1")
            });

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Single(result.Data);
        Assert.Equal("Test Desk 1", result.Data.First().Name);
    }
    
    [Fact]
    public async Task GetDesksAsync_ShouldReturnFilteredDesksByStatus()
    {
        // Arrange
        var desk1 = DeskFaker.MakeOne();
        desk1.Status = ETableStatus.Closed;
        var desk2 = DeskFaker.MakeOne();
        desk2.Status = ETableStatus.Open;

        _dbContext.Desks.AddRange(desk1, desk2);
        _dbContext.SaveChanges();

        // Act
        var result = await _deskService.GetDesksAsync(
            new QueryInputDTO<GetDesksQueryDTO>
            {
                Query = new GetDesksQueryDTO(TableStatus: ETableStatus.Open)
            });

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Single(result.Data);
        Assert.Equal(ETableStatus.Open, result.Data.First().Status);
    }
    
    [Fact]
    public async Task GetDesksAsync_ShouldReturnFilteredDesksByMaxPlayers()
    {
        // Arrange
        var desk1 = DeskFaker.MakeOne();
        desk1.MaxPlayers = 4;
        var desk2 = DeskFaker.MakeOne();
        desk2.MaxPlayers = 6;

        _dbContext.Desks.AddRange(desk1, desk2);
        _dbContext.SaveChanges();

        // Act
        var result = await _deskService.GetDesksAsync(
            new QueryInputDTO<GetDesksQueryDTO>
            {
                Query = new GetDesksQueryDTO(MaxPlayers: 5)
            });

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Single(result.Data);
        Assert.Equal(4, result.Data.First().MaxPlayers);
    }

    [Fact]
    public async Task GetDesksAsync_ShouldReturnFilteredDesksByFullStatus()
    {
        // Arrange
        var desk1 = DeskFaker.MakeOne();
        desk1.MaxPlayers = 4;

        var playerDesk1 = PlayerDeskFaker.MakeOne();
        playerDesk1.Role = EPlayerDeskRole.Player;
        playerDesk1.Player = PlayerFaker.MakeOne();

        var playerDesk2 = PlayerDeskFaker.MakeOne();
        playerDesk2.Role = EPlayerDeskRole.Player;
        playerDesk2.Player = PlayerFaker.MakeOne();

        var playerDesk3 = PlayerDeskFaker.MakeOne();
        playerDesk3.Role = EPlayerDeskRole.Player;
        playerDesk3.Player = PlayerFaker.MakeOne();

        var playerDesk4 = PlayerDeskFaker.MakeOne();
        playerDesk4.Role = EPlayerDeskRole.Player;
        playerDesk3.Player = PlayerFaker.MakeOne();

        desk1.PlayerDesks.Add(playerDesk1);
        desk1.PlayerDesks.Add(playerDesk2);
        desk1.PlayerDesks.Add(playerDesk3);
        desk1.PlayerDesks.Add(playerDesk4);


        var desk2 = DeskFaker.MakeOne();
        desk2.MaxPlayers = 6;

        _dbContext.Desks.AddRange(desk1, desk2);
        _dbContext.SaveChanges();

        // Act
        var result = await _deskService.GetDesksAsync(
            new QueryInputDTO<GetDesksQueryDTO>
            {
                Query = new GetDesksQueryDTO(IsFull: true)
            });

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Single(result.Data);
        Assert.Equal(desk1.Id, result.Data.First().Id);
    }

    [Fact]
    public async Task GetDesksAsync_ShouldReturnFilteredDesksByDescription()
    {
        // Arrange
        var desk1 = DeskFaker.MakeOne();
        desk1.Description = "A desk for testing purposes";
        var desk2 = DeskFaker.MakeOne();
        desk2.Description = "Another desk for testing purposes";

        _dbContext.Desks.AddRange(desk1, desk2);
        _dbContext.SaveChanges();

        // Act
        var result = await _deskService.GetDesksAsync(
            new QueryInputDTO<GetDesksQueryDTO>
            {
                Query = new GetDesksQueryDTO(Description: "testing")
            });

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal(2, result.Data.Count());
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
        var master = PlayerFaker.MakeOne();
        desk.PlayerDesks.Add(new PlayerDesk
        {
            Player = master,
            Role = EPlayerDeskRole.DeskMaster
        });
        await _dbContext.Desks.AddAsync(desk);
        await _dbContext.SaveChangesAsync();
        desk.Name = "Updated Desk Name";

        // Act
        var result = await _deskService.UpdateDeskAsync(new UpdateDeskInputDTO(
            Name: desk.Name,
            Description: desk.Description,
            MasterId: master.Id,
            DeskId: desk.Id
        ));

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