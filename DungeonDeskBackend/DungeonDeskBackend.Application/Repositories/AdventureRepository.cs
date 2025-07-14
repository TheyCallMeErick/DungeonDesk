using DungeonDeskBackend.Application.Data;
using DungeonDeskBackend.Application.DTOs.Inputs;
using DungeonDeskBackend.Application.DTOs.Inputs.Adventure;
using DungeonDeskBackend.Application.Repositories.Interfaces;
using DungeonDeskBackend.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DungeonDeskBackend.Application.Repositories;

public class AdventureRepository : IAdventureRepository
{
    private readonly DungeonDeskDbContext DbContext;

    public AdventureRepository(DungeonDeskDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task<IEnumerable<Adventure>> GetAdventuresAsync(QueryInputDTO<GetAdventuresQueryDTO> queryInput)
    {
        var query = DbContext.Adventures
                         .AsNoTracking()
                         .AsQueryable();

        var q = queryInput.Query;

        if (!string.IsNullOrWhiteSpace(q?.Name))
        {
            var name = q.Name.Trim().ToLower();
            query = query.Where(a => a.Title.ToLower().Contains(name));
        }

        if (!string.IsNullOrWhiteSpace(q?.Description))
        {
            var desc = q.Description.Trim().ToLower();
            query = query.Where(a => a.Description.ToLower().Contains(desc));
        }

        if (q?.Author.HasValue == true)
        {
            query = query.Where(a => a.AuthorId == q.Author.Value);
        }

        if (queryInput.Pagination != null)
        {
            query = query
                .Skip(queryInput.Pagination.Skip)
                .Take(queryInput.Pagination.PageSize);
        }

        return await query.ToListAsync();
    }
}
