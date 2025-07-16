using DungeonDeskBackend.Application.Data;
using DungeonDeskBackend.Application.DTOs.Inputs;
using DungeonDeskBackend.Application.DTOs.Inputs.Desk;
using DungeonDeskBackend.Application.Repositories.Interfaces;
using DungeonDeskBackend.Domain.Enums;
using DungeonDeskBackend.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DungeonDeskBackend.Application.Repositories; 

public class DeskRepository : IDeskRepository
{
    private readonly DungeonDeskDbContext DbContext;

    public DeskRepository(DungeonDeskDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<IEnumerable<Desk>> GetDesksAsync(QueryInputDTO<GetDesksQueryDTO> queryInput)
    {
        var query = DbContext.Desks.AsNoTracking().AsQueryable();
        var q = queryInput.Query;
        if (!string.IsNullOrEmpty(queryInput.Query.Name))
        {
            query = query.Where(d => d.Name.Contains(queryInput.Query.Name));
        }

        if (!string.IsNullOrEmpty(queryInput.Query.Description))
        {
            query = query.Where(d => d.Description.Contains(queryInput.Query.Description));
        }

        if (queryInput.Query.TableStatus.HasValue)
        {
            query = query.Where(d => d.Status == queryInput.Query.TableStatus.Value);
        }

        if (queryInput.Query.MaxPlayers.HasValue)
        {
            query = query.Where(d => d.MaxPlayers <= queryInput.Query.MaxPlayers.Value);
        }

        if (queryInput.Query.IsFull)
        {
            query = query.Where(d => d.PlayerDesks.Count(pd => pd.Role == EPlayerDeskRole.Player) >= d.MaxPlayers);
        }

        return await query
            .Skip((queryInput.Pagination.Page - 1) * queryInput.Pagination.PageSize)
            .Take(queryInput.Pagination.PageSize)
            .ToListAsync();
    }
}
