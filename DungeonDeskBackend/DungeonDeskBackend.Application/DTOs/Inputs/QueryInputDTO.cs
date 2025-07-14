namespace DungeonDeskBackend.Application.DTOs.Inputs;

public record QueryInputDTO<T>
{
    public T Query { get; set; }
    public PaginationInputDTO Pagination { get; set; }

    public QueryInputDTO(T query, PaginationInputDTO pagination)
    {
        Query = query;
        Pagination = pagination;
    }
    public QueryInputDTO()
    {
        Query = default!;
        Pagination = new PaginationInputDTO();
    }

}
