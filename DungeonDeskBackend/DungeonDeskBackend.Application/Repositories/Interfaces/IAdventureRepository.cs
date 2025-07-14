using DungeonDeskBackend.Application.DTOs.Inputs;
using DungeonDeskBackend.Application.DTOs.Inputs.Adventure;
using DungeonDeskBackend.Domain.Models;

namespace DungeonDeskBackend.Application.Repositories.Interfaces; 

public interface IAdventureRepository
{
    Task<IEnumerable<Adventure>> GetAdventuresAsync(QueryInputDTO<GetAdventuresQueryDTO> queryInput);
}
