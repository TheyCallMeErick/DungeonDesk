using DungeonDeskBackend.Api.DTOs.Requests;
using DungeonDeskBackend.Api.DTOs.Responses;
using DungeonDeskBackend.Application.DTOs.Inputs;
using DungeonDeskBackend.Application.DTOs.Inputs.Adventure;
using DungeonDeskBackend.Domain.Models;

namespace DungeonDeskBackend.Api.Extensions;

public static class AdventureDTOExtensions
{
    public static QueryInputDTO<GetAdventuresQueryDTO> ToQueryDto(this GetAdventuresQueryRequestDTO request)
    {
        return new QueryInputDTO<GetAdventuresQueryDTO>
        {
            Pagination = new PaginationInputDTO
            {
                Page = request.page,
                PageSize = request.pageSize
            },
            Query = new GetAdventuresQueryDTO
            {
                Name = request.name,
                Description = request.description,
                Author = Guid.TryParse(request.author, out var guid) ? guid : null
            }
        };
    }
    
    public static ResponseAdventureDTO ToResponseDto(this Adventure adventure)
    {
        return new ResponseAdventureDTO(
            adventure.Id,
            adventure.Title,
            adventure.Description,
            adventure.AuthorId,
            adventure.CreatedAt,
            adventure.UpdatedAt
        );
    }
}
