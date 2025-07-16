using DungeonDeskBackend.Application.DTOs.Inputs;
using DungeonDeskBackend.Application.DTOs.Inputs.Desk;
using DungeonDeskBackend.Domain.Models;

namespace DungeonDeskBackend.Application.Repositories.Interfaces;

public interface IDeskRepository
{
    Task<IEnumerable<Desk>> GetDesksAsync(QueryInputDTO<GetDesksQueryDTO> queryInput);
}
