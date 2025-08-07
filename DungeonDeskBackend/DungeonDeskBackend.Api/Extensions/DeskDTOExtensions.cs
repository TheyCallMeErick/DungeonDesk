using DungeonDeskBackend.Api.DTOs.Requests;
using DungeonDeskBackend.Api.DTOs.Responses;
using DungeonDeskBackend.Application.DTOs.Inputs;
using DungeonDeskBackend.Application.DTOs.Inputs.Desk;
using DungeonDeskBackend.Application.DTOs.Outputs;
using DungeonDeskBackend.Application.DTOs.Outputs.Desk;

namespace DungeonDeskBackend.Api.Extensions;

public static class DeskDTOExtensions
{
    public static QueryInputDTO<GetDesksQueryDTO> ToQueryInputDTO(this GetDesksQueryRequestDTO request)
    {
        return new QueryInputDTO<GetDesksQueryDTO>
        {
            Pagination = new PaginationInputDTO
            {
                Page = request.page,
                PageSize = request.pageSize
            },
            Query = new GetDesksQueryDTO
            {
                Name = request.Name,
                Description = request.Description,
                TableStatus = request.TableStatus,
                MaxPlayers = request.MaxPlayers,
                IsFull = request.IsFull
            }
        };
    }

    public static ResponseDeskDTO ToResponseDTO(this DeskOutputDTO desk)
    {
        return new ResponseDeskDTO(
            Id: desk.Id,
            Name: desk.Name,
            Description: desk.Description,
            Status: desk.Status.ToString(),
            MaxPlayers: desk.MaxPlayers,
            AdventureId: desk.AdventureId
        );
    }

    public static PaginatedResponseDTO<ResponseDeskDTO> ToPaginatedResponseDTO(
        this OperationResultDTO<IEnumerable<DeskOutputDTO>> result)
    {
        return new PaginatedResponseDTO<ResponseDeskDTO>(
            Items: result.Data != null ? result.Data.ToList().ConvertAll(element => element.ToResponseDTO()) : new List<ResponseDeskDTO>(),
            Pagination: result.Pagination != null ? new PaginationResponseDTO
            (
                CurrentPage: result.Pagination.CurrentPage,
                PageSize: result.Pagination.PageSize,
                TotalCount: result.Pagination.TotalItems,
                TotalPages: result.Pagination.TotalPages
            ) : new PaginationResponseDTO(
                CurrentPage: 1,
                PageSize: 0,
                TotalCount: 0,
                TotalPages: 0
            )
        );
    }
    
    public static CreateDeskInputDTO ToCreateInputDto(this CreateDeskRequestDTO request)
    {
        return new CreateDeskInputDTO(
            Name: request.Name,
            Description: request.Description,
            MaxPlayers: request.MaxPlayers,
            AdventureId: request.AdventureId,
            MasterId: Guid.Empty
        );
    }
}
