namespace DungeonDeskBackend.Application.DTOs.Inputs;

public class PaginationInputDTO
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public PaginationInputDTO() { }

    public PaginationInputDTO(int page, int pageSize)
    {
        Page = page;
        PageSize = pageSize;
    }

    public int Skip => (Page - 1) * PageSize;
    public int Take => PageSize;
}
