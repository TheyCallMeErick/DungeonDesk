using DungeonDeskBackend.Application.Data;
using DungeonDeskBackend.Application.Services;
using DungeonDeskBackend.Application.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

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

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}